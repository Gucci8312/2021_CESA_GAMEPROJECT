using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovePower = 100.0f;                 // 移動力
    public float Bairitu = 5;
    public float Gensokuritu = 50;                  //速度を減速させる用（０にするとずっと無限に移動する）

    private bool PlayerMoveFlg;                           // プレイヤーによる移動判定用

    GameObject player;

    public Vector2 StickInput;          //スティック入力時の値を取得用(-1～1)
    public Vector2 FlickVec;            //弾いた時のベクトル格納用
    bool FlickMoveFlag = false;         //弾き移動をさせるかどうか
    bool OneFlickFlag = false;          //スティック入力を連続でさせない用

    public List<GameObject> Line = new List<GameObject>();         //線のオブジェクト
    List<CrossLine> cl = new List<CrossLine>();                    //CrossLineスクリプト
    private Rigidbody Rb;
    private Vector3 MovePos;                                       //移動する位置
    //private float Radius;                                        //移動する際に使うラジアン
    private Vector3 MoveVec;
    private bool MobiusColFlag;                                    //メビウスの当たり判定

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    MobiusColor Mc;         //MobiusColorスクリプト

    void Start()
    {
        player = GameObject.Find("Player");
        RigitBodyInit();

        TimingInput = false;
        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード

        Mc = this.GetComponent<MobiusColor>();
    }

    // Update is called once per frame
    void Update()
    {
        TimingInput = this.rythm.checkPlayerMove;//ノーツに合わせられたかを取得


        // プレイヤーが乗っているとき
        if (PlayerMoveFlg || Mc.GetColorSameFlag())
        {
            //StickFlick();
            if (Line.Count != 0)
            {
                CrossPosMove();
            }
            Rb.isKinematic = false;//物理的な動きをありにする

        }
        else
        {
            ZeroVelo();
            FlickVec.x = 0;
            FlickVec.y = 0;
        }


        PlayerMoveFlg = false;

        int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得

        if (this.name == "Mobius (" + num + ")" )//自分が対象のメビウスの輪かプレイヤーが触れているメビウスと同じ色なら
        {
            //Debug.Log("Mobius (" + num + "):対象のメビウスの輪");
            PlayerMoveFlg = true;
        }
    }

    //スティックの弾き移動処理
    private void StickFlick()
    {
        StickInput.x = Input.GetAxis("Horizontal");
        StickInput.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W)) { StickInput.y = 1; }
        if (Input.GetKey(KeyCode.S)) { StickInput.y = -1; }
        if (Input.GetKey(KeyCode.D)) { StickInput.x = 1; }
        if (Input.GetKey(KeyCode.A)) { StickInput.x = -1; }

        if (!FlickMoveFlag)
        {
            Rb.velocity = Vector3.zero;//勢いを止めておく

            if (StickInput.x != 0 || StickInput.y != 0)//スティック入力されていたら
            {
                Vector2 stickmax = StickInput;//スティックを端まで倒したときの値を格納用

                //計算をしやすくするために正数に変化
                if (stickmax.x < 0) { stickmax.x = -stickmax.x; }
                if (stickmax.y < 0) { stickmax.y = -stickmax.y; }

                if (stickmax.x + stickmax.y >= 1)//スティックを端まで倒した場合
                {
                    FlickVec = -StickInput; ;//倒した方向の逆方向の値を代入
                }
            }
            else//スティックを倒していなければ（離したとき）
            {
                if (FlickVec.x != 0 || FlickVec.y != 0)//端まで倒したときのベクトルを持っていれば
                {
                    FlickMoveFlag = true;

                    Rb.AddForce(FlickVec * MovePower * 5, ForceMode.Impulse);//瞬間的に加速させる（要調整）
                }
            }
        }

        else
        {
            //this.gameObject.transform.position = new Vector3(
            //    transform.position.x + FlickVec.x * Speed, transform.position.y + FlickVec.y * Speed, 0f); //弾いたほうへ移動

            // Speed -= Time.deltaTime;//減速させる

            Rb.AddForce(-Rb.velocity / 20 * MovePower);//減速させる（要調整）

            if (Rb.velocity.magnitude < 10) //勢いが一定以下になったら
            {
                Rb.velocity = Vector3.zero;//勢いを止める

                FlickVec.x = 0;
                FlickVec.y = 0;
                FlickMoveFlag = false;
            }
        }
    }

    private void CrossPosMove()//交点へ移動する処理
    {
        //FlickVec = Vector2.zero;

        //if (Input.GetKeyDown(KeyCode.W)) { FlickVec.y = 1; }
        //if (Input.GetKeyDown(KeyCode.S)) { FlickVec.y = -1; }
        //if (Input.GetKeyDown(KeyCode.D)) { FlickVec.x = 1; }
        //if (Input.GetKeyDown(KeyCode.A)) { FlickVec.x = -1; }

        if (!FlickMoveFlag)
        {
            Rb.velocity = Vector3.zero;//勢いを止める
            Rb.isKinematic = true;//物理的な動きをなしにする

            if (StickFlickInputFlag())//キー入力またはコントローラー入力されていたら
            {
                LineCol();


                Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), //Rayを飛ばす発射位置
                    new Vector3(FlickVec.x * 1, FlickVec.y * 1, 0));//飛ばす方向

                //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 100, false);//レイが見えるようになる（デバッグの時のみ）


                bool CrossLineFlag = false;//交点へ移動できるかどうか

                List<GameObject> CrossLine = new List<GameObject>();//入力した方向にある近い交点を持つオブジェクトを格納するリスト

                foreach (RaycastHit hit in Physics.RaycastAll(ray, 1000))//貫通のレイキャスト
                {
                    Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

                    if (hit.collider.gameObject.CompareTag("Line"))
                    {
                        for (int i = 0; i < Line.Count; i++)
                        {
                            if (hit.collider.gameObject != Line[i])
                            {
                                CrossLine.Add(hit.collider.gameObject);//レイで当たった
                            }
                        }
                        //break;
                    }
                }


                if (CrossLine.Count != 0)//レイを使って探した交点があれば
                {
                    CrossLine NearCl = CanMovePosition(CrossLine).GetComponent<CrossLine>();//CrossLineゲットコンポーネントを取得

                    for (int i = 0; i < cl.Count; i++)
                    {
                        for (int j = 0; j < cl[i].GetCrossPos().Count; j++)
                        {
                            if (NearCl.SameCrossPos(cl[i].GetCrossPos()[j]))//一番近い線の交点とメビウスの輪が触れている線が持つ交点と同じ座標があれば
                            {
                                CrossLineFlag = true;
                                break;
                            }
                        }
                    }

                    if (CrossLineFlag && TimingInput)
                    {
                        Rb.isKinematic = false;//物理的な動きをありにする

                        Debug.Log("近い交点" + CanMovePosition(CrossLine).name);

                        //最終的に入力した方向にある線に沿って交点へ移動

                        MovePos = NearCl.CanMovePosition(this.transform.position);//移動できる交点を取得
                        float Radius = Mathf.Atan2(MovePos.y - this.transform.position.y, MovePos.x - this.transform.position.x); //自分と指定した座標とのラジアンを求める
                        MoveVec = new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);

                        float distance = (this.transform.position - MovePos).magnitude;//自分の座標と移動したい座標との差
                        Rb.AddForce(MoveVec * (MovePower + (distance * Bairitu)), ForceMode.VelocityChange);//瞬間的に加速させる（要調整）
                        FlickMoveFlag = true;

                        this.rythm.checkMoviusMove = false;
                        this.rythm.rythmCheckFlag = false;

                    }
                }

            }
        }

        else//移動処理
        {
            Rb.AddForce(-Rb.velocity * (Gensokuritu * 0.1f), ForceMode.Acceleration);//減速させる（要調整）
            float distance = (this.transform.position - MovePos).magnitude;

            if (Rb.velocity.magnitude < (MovePower / 10) + Bairitu) //勢いが一定以下になったら
            {
                ZeroVelo();

                FlickVec.x = 0;
                FlickVec.y = 0;
            }
            else if (distance < (MovePower / 10) + Bairitu)  //ほぼ近ければ
            {
                ZeroVelo();
                this.transform.position = MovePos;

                FlickVec.x = 0;
                FlickVec.y = 0;
            }
        }
    }

    private bool StickFlickInputFlag()
    {
        StickInput.x = Input.GetAxis("Horizontal");
        StickInput.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W)) { StickInput.y = 1; }
        if (Input.GetKey(KeyCode.S)) { StickInput.y = -1; }
        if (Input.GetKey(KeyCode.D)) { StickInput.x = 1; }
        if (Input.GetKey(KeyCode.A)) { StickInput.x = -1; }


        Vector2 stickmax = StickInput;//スティックを端まで倒したときの値を格納用

        //計算をしやすくするために正数に変化
        if (stickmax.x < 0) { stickmax.x = -stickmax.x; }
        if (stickmax.y < 0) { stickmax.y = -stickmax.y; }


        if (!OneFlickFlag)
        {
            if (StickInput.x != 0 || StickInput.y != 0)//スティック入力されていたら
            {
                if (stickmax.x + stickmax.y >= 1)//スティックを端まで倒した場合
                {
                    //斜めにレイを飛ばさない用
                    if (stickmax.x > stickmax.y) { StickInput.y = 0; }
                    else { StickInput.x = 0; }

                    FlickVec = StickInput; //倒した方向の値を代入
                    OneFlickFlag = true;
                    return true;
                }
            }
        }

        else
        {
            if (stickmax.x + stickmax.y < 1)//スティックを端まで倒していない場合
            {
                OneFlickFlag = false;
                return false;
            }
        }

        return false;
    }

    private GameObject CanMovePosition(List<GameObject> _obj)//レイで当たったオブジェクトで一番近いものを取得
    {
        if (_obj.Count == 0) { Debug.Log("リストがない"); }

        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < _obj.Count; i++)
        {
            distance.Add((this.transform.position - _obj[i].transform.position).magnitude);

            if (distance[i] <= Min)//取得している最小の値より小さければ
            {
                Min = distance[i];//差が最小の値を取得
            }
        }

        for (int i = 0; i < distance.Count; i++)
        {
            if (distance[i] == Min)//差が最小の値を持った要素であれば
            {
                return _obj[i].gameObject;
            }
        }

        Debug.Log("null");
        return null;
    }

    private void RigitBodyInit()//Rigidbodyの初期化
    {
        Rb = this.gameObject.GetComponent<Rigidbody>();
        Rb.constraints = RigidbodyConstraints.FreezePositionZ;
        Rb.freezeRotation = true;
    }

    private void LineCol()//メビウスの輪が線上に乗っているかどうか調べる（縦か横の線のみ）
    {
        float Gosa = 5;
        for (int i = 0; i < Line.Count; i++)
        {
            if ((this.transform.position.x <= Line[i].transform.position.x + Gosa && this.transform.position.x >= Line[i].transform.position.x - Gosa)
                || (this.transform.position.y <= Line[i].transform.position.y + Gosa && this.transform.position.y >= Line[i].transform.position.y - Gosa))
            {
                continue;
            }
            else
            {
                cl.Remove(Line[i].GetComponent<CrossLine>());
                Line.Remove(Line[i].gameObject);//登録したLineリストの中に該当する要素を削除する

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            if (Line.Count == 0)//Lineリストに要素が無ければ
            {
                Line.Add(other.gameObject);//Lineリストに当たったものを追加
                cl.Add(other.GetComponent<CrossLine>());
            }
            else//Lineリストに要素があれば
            {
                int Count = 0;
                for (int i = 0; i < Line.Count; i++)
                {
                    if (Line[i] != other.gameObject)//Lineリストの要素と当たったオブジェクトが違うなら
                    {
                        Count++;
                    }
                }
                if (Line.Count == Count)//Lineリストの中に当たったオブジェクトが無ければ
                {
                    Line.Add(other.gameObject);//Lineリストに当たったものを追加
                    cl.Add(other.GetComponent<CrossLine>());
                }
            }
        }

        // メビウスの輪同士がぶつかったとき
        if (other.gameObject.tag == "Mobius")
        {
            //MoveFlg = false;
            if (FlickMoveFlag)//自身が勢いがあるとき　
            {
                Debug.Log("メビウスの輪同士がぶつかった" + this.gameObject.name);

                if (!other.GetComponent<MoveMobius>().GetFlickMoveFlag()) //相手が動いてないとき
                {
                    MobiusCol(other.gameObject);
                    MobiusColFlag = true;

                    ZeroVelo();
                }
                else
                {
                    if (Rb.velocity.magnitude > other.GetComponent<MoveMobius>().Velocty())//自身の勢いが強ければ
                    {
                        MobiusCol(other.gameObject);
                    }
                }

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            Line.Remove(other.gameObject);//登録したLineリストの中に該当する要素を削除する
            cl.Remove(other.GetComponent<CrossLine>());
        }

        // メビウスの輪同士が離れたとき
        if (other.gameObject.tag == "Mobius")
        {
            if (PlayerMoveFlg)
            {
                Debug.Log("メビウスの輪同士が離れた" + this.gameObject.name);
                MobiusColFlag = false;
            }
        }

    }

    private void MobiusCol(GameObject col)
    {
        float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
        float ColR = (col.GetComponent<SphereCollider>().bounds.size.x + col.GetComponent<SphereCollider>().bounds.size.y) / 4;// 相手メビウスの輪の円の半径を取得
        float SocialDistance = ThisR+ ColR + 8;//お互いの半径分と少しだけ離す


        if (FlickVec.x > 0)//右移動の時
        {
            this.transform.position = new Vector3(col.transform.position.x - SocialDistance, this.transform.position.y, this.transform.position.z);
        }
        else if(FlickVec.x < 0)//左移動の時
        {
            this.transform.position = new Vector3(col.transform.position.x + SocialDistance, this.transform.position.y, this.transform.position.z);
        }
        else if(FlickVec.y > 0)//上移動の時
        {
            this.transform.position = new Vector3(this.transform.position.x, col.transform.position.y - SocialDistance, this.transform.position.z);
        }
        else if (FlickVec.y < 0)//下移動の時
        {
            this.transform.position = new Vector3(this.transform.position.x, col.transform.position.y + SocialDistance, this.transform.position.z);
        }


        float distance = (this.transform.position - col.transform.position).magnitude;

        if (SocialDistance < distance)//引き離したとき離れ過ぎたら 
        {
            Debug.Log(SocialDistance);
            Debug.Log(distance);


            float disdis = distance - SocialDistance;//差分を詰めるための変数

            disdis=disdis*1.3f;//もう少し詰める

            //差分を詰める
            if (FlickVec.x > 0)//右移動の時
            {
                this.transform.position = new Vector3(this.transform.position.x + disdis, this.transform.position.y, this.transform.position.z);
            }
            else if (FlickVec.x < 0)//左移動の時
            {
                this.transform.position = new Vector3(this.transform.position.x - disdis, this.transform.position.y, this.transform.position.z);
            }
            else if (FlickVec.y > 0)//上移動の時
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + disdis, this.transform.position.z);
            }
            else if (FlickVec.y < 0)//下移動の時
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - disdis, this.transform.position.z);
            }

            Debug.Log("差を詰めた");
        }
    }

    private void ZeroVelo() //動きを止める
    {
        Rb.velocity = Vector3.zero;//勢いを止める
        FlickMoveFlag = false;
        Rb.isKinematic = true;
    }

    public bool GetFlickMoveFlag()
    {
        return FlickMoveFlag;
    }

    public bool GetPlayerMoveFlg()
    {
        return PlayerMoveFlg;
    }

    public float Velocty()
    {
        return Rb.velocity.magnitude;
    }


    //// 離れた時
    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = true;
    //    }
    //}
}
