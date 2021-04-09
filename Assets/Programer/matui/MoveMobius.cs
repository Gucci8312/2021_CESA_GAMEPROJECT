using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    private float MovePower = 500.0f;                                        //移動力
    public float MoveBairitu = 15;                                           //移動に掛ける倍率
    public float Gensokuritu = 50;                                           //速度を減速させる用（０にするとずっと無限に移動する）

    public bool PlayerMoveFlg;                                               // プレイヤーによる移動判定用
    GameObject player;

    public Vector2 StickInput;                                               //スティック入力時の値を取得用(-1～1)
    public Vector2 FlickVec;                                                 //弾いた時のベクトル格納用
    public bool FlickMoveFlag = false;                                       //弾き移動をさせるかどうか
    bool OneFlickFlag = false;                                               //スティック入力を連続でさせない用

    public List<GameObject> Line = new List<GameObject>();                   //線のオブジェクト
    List<CrossLine> cl = new List<CrossLine>();                              //CrossLineスクリプト
    private Rigidbody Rb;
    private Vector3 MovePos;                                                 //移動する位置
    private Vector3 MoveVec;
    private bool MobiusColFlag;                                              //メビウスの当たり判定
    Vector3 ColPos;                                                          //メビウスが当たった座標（具体的には自分と相手の座標の中点）
    Vector3 StartMovePos;                                                    //移動開始点

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    MobiusColor Mc;                                                          //MobiusColorスクリプト

    bool MobiusStripFlag;                                                    //メビウスの輪になっているかどうか
    GameObject ColMobiusObj;                                                 //当たった相手メビウス格納用

    void Start()
    {
        player = GameObject.Find("Player");
        RigitBodyInit();

        TimingInput = false;
        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード

        Mc = this.GetComponent<MobiusColor>();

        StartMovePos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MobiusColFlag = false;

        TimingInput = this.rythm.checkMoviusMove;//ノーツに合わせられたかを取得
        MobiusStrip();//メビウスの輪になっているかを調べる

        // プレイヤーが乗っているとき
        if (PlayerMoveFlg || Mc.GetColorSameFlag())
        {           
            CrossPosMove();
            
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

    private void CrossPosMove()//交点へ移動する処理
    {
        if (!FlickMoveFlag)
        {
            Rb.velocity = Vector3.zero;//勢いを止める
            Rb.isKinematic = true;//物理的な動きをなしにする
            StartMovePos = this.transform.position;

            if (StickFlickInputFlag() && TimingInput)//キー入力またはコントローラー入力されていたら　かつ　リズムが合えば
            {
                LineCol();//自分の中心と線がはみ出てないか調べる

                Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), //Rayを飛ばす発射位置
                    new Vector3(FlickVec.x * 1, FlickVec.y * 1, 0));//飛ばす方向

                //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 100, false);//レイが見えるようになる（デバッグの時のみ）
                bool CrossLineFlag = false;//交点へ移動できるかどうか

                List<GameObject> CrossLine = new List<GameObject>();//入力した方向にある近い交点を持つオブジェクトを格納するリスト

                //貫通レイキャスト
                foreach (RaycastHit hit in Physics.RaycastAll(ray, 1000))
                {
                   // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

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
                    CrossLine NearCl = NearObjSearch(CrossLine, this.transform.position).GetComponent<CrossLine>();//CrossLineゲットコンポーネントを取得

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

                    if (CrossLineFlag)//交点へ移動できるなら
                    {
                        Rb.isKinematic = false;//物理的な動きをありにする

                        //最終的に入力した方向にある線に沿って交点へ移動
                        MovePos = NearCl.CanMovePosition(this.transform.position);//移動できる交点を取得
                        float Radius = Mathf.Atan2(MovePos.y - this.transform.position.y, MovePos.x - this.transform.position.x); //自分と指定した座標とのラジアンを求める
                        MoveVec = new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);
                        Rb.AddForce(MoveVec * (MovePower * MoveBairitu), ForceMode.VelocityChange);//瞬間的に加速させる（要調整）
                        FlickMoveFlag = true;

                        this.rythm.checkMoviusMove = false;
                    }
                }

            }
        }

        else//移動処理
        {
            Rb.AddForce(-Rb.velocity * (Gensokuritu * 0.1f), ForceMode.Acceleration);//減速させる（要調整）

            if (!HighSpeedCol())//何も当たらなければ
            {
                if (Rb.velocity.magnitude < (MovePower / 10) + MoveBairitu) //勢いが一定以下になったら
                {
                    ZeroVelo();

                    FlickVec.x = 0;
                    FlickVec.y = 0;
                }

                if (MovingStop(MovePos))  //指定した座標を通れば
                {
                    ZeroVelo();
                    this.transform.position = MovePos;

                    FlickVec.x = 0;
                    FlickVec.y = 0;
                }
            }
           
        }
    }

    public bool StickFlickInputFlag()
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

    private GameObject NearObjSearch(List<GameObject> _obj,Vector3 NearPos)//レイで当たったオブジェクトで一番近いものを取得
    {
        if (_obj.Count == 0) { Debug.Log("リストがない"); }

        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < _obj.Count; i++)
        {
            distance.Add((NearPos - _obj[i].transform.position).magnitude);

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
                if (SameObjListSearch(Line, other.gameObject))//Lineリストの中に当たったものがなければ
                {
                    Line.Add(other.gameObject);//Lineリストに当たったものを追加
                    cl.Add(other.GetComponent<CrossLine>());
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

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Mobius")
        //{
        //    ColMobiusObj = other.gameObject;//ぶつかったメビウスを格納
        //    MobiusStripFlag = true;
        //}
    }


    private void MobiusCol(GameObject col)//メビウス同士が当たった時の処理
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
            float disdis = distance - SocialDistance;//差分を詰めるための変数
            disdis=disdis*1.5f;//もう少し詰める

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

            //Debug.Log("差を詰めた");
        }
    }

    private bool HighSpeedCol()//速すぎて当たり判定をすり抜けた時の対策
    {
        Ray ray;

        float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
        float distance = (this.transform.position - StartMovePos).magnitude + (ThisR*1.2f);      //レイを飛ばす長さ

        List<GameObject> ColObj = new List<GameObject>();               //すり抜けたメビウスオブジェクトを格納するリスト

        //3本のレイを飛ばしてオブヘクトを検索
        for (int i = -1; i < 2; i++)
        {
            if (FlickVec.x != 0 && FlickVec.y == 0)//横移動なら
            {
                ray = new Ray(new Vector3(StartMovePos.x, StartMovePos.y + (ThisR * i), StartMovePos.z),    //Rayを飛ばす発射位置
                 new Vector3(FlickVec.x * 1, FlickVec.y * 1, 0));                             //飛ばす方向
            }
            else//縦移動なら
            {
                ray = new Ray(new Vector3(StartMovePos.x + (ThisR * i), StartMovePos.y, StartMovePos.z),    //Rayを飛ばす発射位置
                 new Vector3(FlickVec.x * 1, FlickVec.y * 1, 0));                             //飛ばす方向
            }

            //貫通のレイキャスト
            foreach (RaycastHit hit in Physics.RaycastAll(ray, distance))
            {
                // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト
                GameObject hitObj=null;

                //レイが当たったオブジェクト
                switch (hit.collider.gameObject.tag)
                {
                    case "Mobius":
                        if (!hit.collider.gameObject.GetComponent<MoveMobius>().GetFlickMoveFlag())//動いていないメビウスなら
                        {
                            //ColObj.Add(hit.collider.gameObject);//レイで当たったオブジェクトをリストに格納
                            hitObj = hit.collider.gameObject;
                        }
                        break;
                }


                if (ColObj.Count == 0 && hitObj != null) //レイが当たったオブジェクトがあれば　かつ　リストが空なら
                {
                    ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                }
                else if (ColObj.Count != 0 && hitObj != null)
                {
                    if (SameObjListSearch(ColObj, hitObj))//ColObjリストの中に当たったものがなければ
                    {
                        ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                    }
                }
            }//foreach
        }//for

        if (ColObj.Count != 0)//リストの中に要素があれば
        {
            
            GameObject otherObj = NearObjSearch(ColObj, StartMovePos);//リストの中から始点に近いオブジェクトを取得
            Vector3 Scale=Vector3.zero;//当たったオブジェクトの大きさ（範囲内にあるか調べる用）

            //オブジェクトごとに大きさを取得
            switch (otherObj.tag)
            {
                case "Mobius":
                    float otherR = (otherObj.GetComponent<SphereCollider>().bounds.size.x + otherObj.GetComponent<SphereCollider>().bounds.size.y) / 2;// 相手メビウスの輪の円の直径を取得
                    Scale = new Vector3(otherR, otherR, otherR);
                    break;
            }

            if (TwoPosInObjFlag(otherObj.transform.position,Scale))//範囲内にあれば
            {
               // Debug.Log("すり抜けた" + otherObj.name + " と当たり判定を実行");
                Collision(otherObj);//当たり判定時の処理を実行
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
           //Debug.Log("すり抜けてない");
            return false;
        }
    }

    private void MobiusStrip()
    {
        if (MobiusStripFlag)
        {
            float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
            float ColR = (ColMobiusObj.GetComponent<SphereCollider>().bounds.size.x + ColMobiusObj.GetComponent<SphereCollider>().bounds.size.y) / 4;// 相手メビウスの輪の円の半径を取得
            float SocialDistance = ThisR + ColR + 15;//お互いの半径分と少しだけ離す

            float distance = (this.transform.position - ColMobiusObj.transform.position).magnitude;//相手と自分の距離

            if (SocialDistance < distance)//離れ過ぎたら 
            {
                MobiusStripFlag = false;
            }
        }
    }

    private void ZeroVelo() //動きを止める
    {
        Rb.velocity = Vector3.zero;//勢いを止める
        FlickMoveFlag = false;
        Rb.isKinematic = true;
    }

    private bool MovingStop(Vector3 StopPos)//メビウスが指定した座標に着いたかどうか
    {
        if (FlickVec.x > 0)//右移動の時
        {
            if (this.transform.position.x > MovePos.x)
            {
                Debug.Log("右を通った");
                return true;
            }
        }
        else if (FlickVec.x < 0)//左移動の時
        {
            if (this.transform.position.x < MovePos.x)
            {
                Debug.Log("左を通った");
                return true;
            }
        }
        else if (FlickVec.y > 0)//上移動の時
        {
            if (this.transform.position.y > MovePos.y)
            {
                Debug.Log("上を通った");
                return true;
            }
        }
        else if (FlickVec.y < 0)//下移動の時
        {
            if (this.transform.position.y < MovePos.y)
            {
                Debug.Log("下を通った");
                return true;
            }
        }

        return false;
    }

    private void Collision(GameObject otherObj)//当たり判定時の処理
    {
        // メビウスの輪同士がぶつかったとき
        if (otherObj.tag == "Mobius")
        {
            if (FlickMoveFlag)//自身が勢いがあるとき　
            {
                Debug.Log("メビウスの輪同士がぶつかった" + this.gameObject.name);
                this.transform.position = StartMovePos;//一度、移動前の座標にする
                MobiusCol(otherObj);

                if (!otherObj.GetComponent<MoveMobius>().GetFlickMoveFlag()) //相手が動いてないとき
                {
                    if (!MobiusStripFlag)//メビウスの輪になっていないときに
                    {
                        MobiusColFlag = true;
                    }
                    ColPos = (this.transform.position + otherObj.transform.position) / 2;//自分と相手の座標の中点を代入（見た目的に当たった場所）
                    ZeroVelo();
                    
                    //メビウスの輪にするための情報をセット
                    SetMobiusStrip(otherObj);
                    otherObj.GetComponent<MoveMobius>().SetMobiusStrip(this.gameObject);
                }
            }
        }
    }

    private bool TwoPosInObjFlag(Vector3 SearchPos,Vector3 Scale)//始点と終点の二つの座標の間にあるかどうか
    {
        Debug.Log(Scale);

        if (FlickVec.x > 0)//右移動の時
        {
            if (StartMovePos.x <= SearchPos.x + Scale.x &&  MovePos.x >= SearchPos.x - Scale.x ) 
            {
                return true;
            }
        }
        else if (FlickVec.x < 0)//左移動の時
        {
            if (MovePos.x <= SearchPos.x + Scale.x && StartMovePos.x >= SearchPos.x - Scale.x)
            {
                return true;
            }
        }
        else if (FlickVec.y > 0)//上移動の時
        {
            if (StartMovePos.y <= SearchPos.y + Scale.y && MovePos.y >= SearchPos.y - Scale.y)
            {
                return true;
            }
        }
        else if (FlickVec.y < 0)//下移動の時
        {
            if (MovePos.y <= SearchPos.y + Scale.y && StartMovePos.y >= SearchPos.y - Scale.y)
            {
                return true;
            }
        }

        return false;
    }
    private bool SameObjListSearch(List<GameObject> ListObj,GameObject SearchObj)//オブジェクトのリストの中の要素を検索する
    {
        int Count = 0;
        for (int i = 0; i < ListObj.Count; i++)
        {
            if (ListObj[i] != SearchObj)//リストの要素と検索したいオブジェクトが違うなら
            {
                Count++;
            }
        }
        if (ListObj.Count == Count)//リストの中に検索したいオブジェクトが無ければ
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetMobiusStrip(GameObject _obj)//メビウスの輪にする為の値をセット
    {
        ColMobiusObj = _obj;//ぶつかったメビウスを格納
        MobiusStripFlag = true;
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

    public bool GetMobiusColFlg()
    {
        return MobiusColFlag;
    } 
    public Vector3 GetColPos()
    {
        return ColPos;
    }
    public bool GetMobiusStripFlag()
    {
        return MobiusStripFlag;
    }
    public GameObject GetColMobiusObj()
    {
        return ColMobiusObj;
    }
}
