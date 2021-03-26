using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovePower = 50.0f;                 // 移動力
    bool MoveFlg;                           // 移動判定用
    GameObject player;

    public Vector2 StickInput;          //スティック入力時の値を取得用(-1～1)
    public Vector2 FlickVec;            //弾いた時のベクトル格納用
    bool FlickMoveFlag = false;           //弾き移動をさせるかどうか
    float Speed;

    public List<GameObject> Line = new List<GameObject>();  //線のオブジェクト
    List<CrossLine> cl = new List<CrossLine>();             //CrossLineスクリプト
    Rigidbody Rb;
    Vector3 MovePos;                                        //移動する位置

    void Start()
    {
        player = GameObject.Find("Player");

        Rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // プレイヤーが乗っているとき
        if (MoveFlg)
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
            Rb.velocity = Vector3.zero;//勢いを止める
            Rb.isKinematic = true;//物理的な動きをなしにする

            FlickVec.x = 0;
            FlickVec.y = 0;
            FlickMoveFlag = false;//弾き移動を止める
        }


        MoveFlg = false;

        int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得

        if (this.name == "Mobius (" + num + ")")//自分が対象のメビウスの輪なら
        {
            //Debug.Log("Mobius (" + num + "):対象のメビウスの輪");
            MoveFlg = true;
        }
        //else
        //{
        //    MoveFlg = false;
        //}

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
                    Speed = MovePower;
                    FlickMoveFlag = true;

                    Rb.AddForce(FlickVec * MovePower*5, ForceMode.Impulse);//瞬間的に加速させる（要調整）
                }
            }
        }

        else
        {
            //this.gameObject.transform.position = new Vector3(
            //    transform.position.x + FlickVec.x * Speed, transform.position.y + FlickVec.y * Speed, 0f); //弾いたほうへ移動

           // Speed -= Time.deltaTime;//減速させる

            Rb.AddForce(-Rb.velocity / 20 * MovePower);//減速させる（要調整）

            if (Rb.velocity.magnitude < 5) //勢いが一定以下になったら
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
            if (StickFlickInputFlag())//キー入力またはコントローラー入力されていたら
            {
                Ray ray = new Ray(new Vector3(this.transform.position.x + FlickVec.x * 15, this.transform.position.y + FlickVec.y * 15, this.transform.position.z), //Rayを飛ばす発射位置
                    new Vector3(FlickVec.x * 1, FlickVec.y * 1, 0));//飛ばす方向

                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 100, false);


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
                            if (NearCl.SameCrossPos(cl[i].GetCrossPos()[j]))
                            {
                                CrossLineFlag = true;
                                break;
                            }
                        }
                    }

                    if (CrossLineFlag)
                    {
                        Debug.Log("近い交点" + CanMovePosition(CrossLine).name);

                        //最終的に入力した方向にある線に沿って交点へ移動
                        //this.transform.position = NearCl.CanMovePosition(this.transform.position);
                        MovePos = NearCl.CanMovePosition(this.transform.position);

                        //Rb.AddForce(MovePos/1000 * MovePower * 5, ForceMode.Impulse);//瞬間的に加速させる（要調整）
                        Rb.AddForce(FlickVec * MovePower, ForceMode.Impulse);//瞬間的に加速させる（要調整））

                    }
                }

            }
        }

        else//移動処理
        {
            Rb.AddForce(FlickVec * MovePower/2 );

            float distance = (this.transform.position - MovePos).magnitude;
            if (distance<7) //ほぼ近ければ
            {
                this.transform.position = MovePos;

                Rb.velocity = Vector3.zero;//勢いを止める

                FlickVec.x = 0;
                FlickVec.y = 0;
                FlickMoveFlag = false;
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

        if (!FlickMoveFlag)
        {
            if (StickInput.x != 0 || StickInput.y != 0)//スティック入力されていたら
            {
                Vector2 stickmax = StickInput;//スティックを端まで倒したときの値を格納用

                //計算をしやすくするために正数に変化
                if (stickmax.x < 0) { stickmax.x = -stickmax.x; }
                if (stickmax.y < 0) { stickmax.y = -stickmax.y; }

                if (stickmax.x + stickmax.y >= 1)//スティックを端まで倒した場合
                {
                    FlickVec = StickInput; ;//倒した方向の値を代入
                    FlickMoveFlag = true;
                    return true;
                }
            }
        }

        return false;
    }

    private GameObject CanMovePosition(List<GameObject> _obj)//メビウスが移動できる座標を与える
    {
        if (_obj.Count == 0) { Debug.Log("リストがない");}

        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < _obj.Count; i++)
        {
            distance.Add((this.transform.position - _obj[i].transform.position).magnitude);

            //if (distance[i] == 0)//差がない（同じ座標）場合
            //{
            //    distance[i] = 10000;//適当に大きい値を入れて最小の値として取得させないようにする
            //}

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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            Line.Add(other.gameObject);//Lineリストに当たったものを追加
            cl.Add(other.GetComponent<CrossLine>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            Line.Remove(other.gameObject);//登録したLineリストの中に該当する要素を削除する
            cl.Remove(other.GetComponent<CrossLine>());
        }
    }


    //// 衝突時
    //private void OnCollisionEnter(Collision other)
    //{
    //    // プレイヤーに当たった時
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = true;
    //    }

    //    // メビウスの輪同士がぶつかったとき
    //    if (other.gameObject.tag == "Mobius")
    //    {
    //        MoveFlg = false;
    //        Debug.Log("メビウスの輪同士がぶつかった");
    //    }
    //}

    //// 離れた時
    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = true;
    //    }
    //}
}
