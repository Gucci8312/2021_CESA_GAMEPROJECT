using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    //private float MovePower = 500.0f;                                        //移動力
    //public float MoveBairitu = 15;                                           //移動に掛ける倍率
    //public float Gensokuritu = 50;                                           //速度を減速させる用（０にするとずっと無限に移動する）

    public float GoalMovetime = 0.05f;                                       //目的地へ到達するまでの時間（秒）
    float Nowtime = 0;                                                       //移動時間（秒）

    public bool PlayerMoveFlg;                                               // プレイヤーによる移動判定用
    GameObject player;
    PlayerMove pm;                                                           //PlayerMoveスクリプト

    public bool EnemyMoveFlag;                                               //エネミーによる移動判定用
    private bool GetEnemyBeatFlag = false;
    public GameObject[] Enemy = new GameObject[2];

    private Vector2 StickInput;                                               //スティック入力時の値を取得用(-1～1)
    private Vector2 FlickVec;                                                 //弾いた時のベクトル格納用
    public bool FlickMoveFlag = false;                                       //弾き移動をさせるかどうか
    bool OneFlickFlag = false;                                               //スティック入力を連続でさせない用

    public List<GameObject> Line = new List<GameObject>();                   //線のオブジェクト
    List<CrossLine> cl = new List<CrossLine>();                              //CrossLineスクリプト
    private Rigidbody Rb;
    public Vector3 MovePos;                                                 //移動する位置
    private Vector3 MoveVec;
    private bool MobiusColFlag;                                              //メビウスの当たり判定
    public Vector3 ColPos;                                                          //メビウスが当たった座標（具体的には自分と相手の座標の中点）
    Vector3 StartMovePos;                                                    //移動開始点
    Vector3 OldPos;                                                          //前回の座標
    Vector3 MoyoriPos;

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    MobiusColor Mc;                                                          //MobiusColorスクリプト
    //MobiusAttachPos MaPos;                                                   //MobiusAttachPosスクリプト

    bool MobiusStripFlag;                                                    //メビウスの輪になっているかどうか
    GameObject ColMobiusObj;                                                 //当たった相手メビウス格納用

    void Start()
    {
        player = GameObject.Find("Player");
        pm = player.GetComponent<PlayerMove>();
        RigitBodyInit();

        TimingInput = false;
        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード

        Mc = this.GetComponent<MobiusColor>();  //MobiusColor取得
        //MaPos = GameObject.Find("mebiusu").GetComponent<MobiusAttachPos>();

        StartMovePos = this.transform.position;
        OldPos = this.transform.position;
        MoyoriPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MobiusColFlag = false;

        TimingInput = this.rythm.checkMoviusMove;//ノーツに合わせられたかを取得
        //MobiusStrip();//メビウスの輪になっているかを調べる

        // プレイヤーが乗っているとき
        if (PlayerMoveFlg || EnemyMoveFlag || Mc.GetColorSameFlag())
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
        EnemyMoveFlag = false;

        int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得

        if (this.name == "Mobius (" + num + ")")//自分が対象のメビウスの輪なら
        {
            //Debug.Log("Mobius (" + num + "):対象のメビウスの輪");
            PlayerMoveFlg = true;
        }

        for (int i = 0; i < Enemy.Length; i++)
        {
            num = Enemy[i].GetComponent<EnemyMove>().GetNowMobiusNum();
            if (this.name == "Mobius (" + num + ")" && !PlayerMoveFlg)//エネミーが乗っていたら
            {
                EnemyMoveFlag = true;
            }
        }
        //OldPos = this.transform.position;
        MobiusStrip();//メビウスの輪になっているかを調べる

    }

    private void CrossPosMove()//交点へ移動する処理
    {
        if (!FlickMoveFlag)
        {
            Rb.velocity = Vector3.zero;//勢いを止める
            Rb.isKinematic = true;//物理的な動きをなしにする
            StartMovePos = this.transform.position;
            OldPos = this.transform.position;
            Nowtime = 0;

            //if (StickFlickInputFlag() && TimingInput)//キー入力またはコントローラー入力されていたら　かつ　リズムが合えば
            if (PlayerHipDropMoveFlag() || (GetEnemyBeatFlag&&EnemyMoveFlag))//
            {

                if (LineVecFlag())//自分の中心と線がはみ出てないか調べる
                {

                    //Ray (飛ばす発射位置、飛ばす方向）
                    Ray ray = new Ray(new Vector3(this.transform.position.x + (FlickVec.x * 10), this.transform.position.y + (FlickVec.y * 10), this.transform.position.z),
                        new Vector3(FlickVec.x * 1, FlickVec.y * 1, 0));

                    //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 100, false);//レイが見えるようになる（デバッグの時のみ）
                    bool CrossLineFlag = false;//交点へ移動できるかどうか

                    List<GameObject> CrossLineObj = new List<GameObject>();//入力した方向にある近い交点を持つオブジェクトを格納するリスト
                    List<Vector3> HitPos = new List<Vector3>();            //レイがあった座標を格納するリスト

                    //貫通レイキャスト
                    foreach (RaycastHit hit in Physics.RaycastAll(ray, 1000))
                    {
                        // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

                        if (hit.collider.gameObject.CompareTag("Line"))
                        {
                            if (SameObjListSearch(Line, hit.collider.gameObject))//Lineリストの中にレイが当たったオブジェクトがなければ
                            {
                                CrossLineObj.Add(hit.collider.gameObject);//CrossLineObjリストの中に追加
                                CrossLineObj[CrossLineObj.Count - 1].GetComponent<CrossLine>().RayHitPos = hit.point;
                                HitPos.Add(hit.point);
                            }
                        }
                    }


                    if (CrossLineObj.Count != 0)//レイを使って探した交点があれば
                    {
                        CrossLine NearCl = NearObjSearch(CrossLineObj, HitPos, this.transform.position).GetComponent<CrossLine>();//CrossLineゲットコンポーネントを取得

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
                        // Debug.Log("ベクトル入手" + CrossLineFlag);
                        if (CrossLineFlag)//交点へ移動できるなら
                        {

                            //Debug.Log(NearObjSearch(CrossLineObj,HitPos, this.transform.position));
                            //Rb.isKinematic = false;//物理的な動きをありにする

                            //最終的に入力した方向にある線に沿って交点へ移動
                            MovePos = cl[0].NearCrossPos(NearCl.RayHitPos);//移動できる交点を取得
                            MoveVec = SearchVector(this.transform.position, MovePos);

                            //進む方向の線の上に乗っているかどうか調べる
                            float distance = (FlickVec - new Vector2(MoveVec.x, MoveVec.y)).magnitude;
                            if (distance < 0.15f) //二つベクトルに誤差が無ければ
                            {
                                FlickMoveFlag = true;

                                this.rythm.checkMoviusMove = false;
                            }
                        }
                    }

                }
            }
        }

        else//移動処理
        {
            // Rb.AddForce(-Rb.velocity * (Gensokuritu * 0.1f), ForceMode.Acceleration);//減速させる（要調整）

            if (!HighSpeedCol())//何も当たらなければ
            {

                if (Nowtime >= GoalMovetime)//到着したら
                {
                    ZeroVelo();
                    this.transform.position = MovePos;
                    MoyoriPos = MovePos;

                    FlickVec.x = 0;
                    FlickVec.y = 0;

                }
                else
                {
                    OldPos = this.transform.position;

                    //線形補間による移動
                    this.transform.position = SenkeiHokan(StartMovePos, MovePos, Nowtime, 0, GoalMovetime);
                    Nowtime += Time.deltaTime;
                }
            }
        }
    }

    //	P0：始点　,P1：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private Vector2 SenkeiHokan(Vector2 P0, Vector2 P1, float t, float t0, float t1)
    {
        Vector2 pos = Vector2.zero;
        pos.x = P0.x + (P1.x - P0.x) * (t - t0) / (t1 - t0);
        pos.y = P0.y + (P1.y - P0.y) * (t - t0) / (t1 - t0);

        return pos;
    }

    public Vector3 SearchVector(Vector3 pos1, Vector3 pos2)//引数pos1からpos2までのベクトルを取得
    {
        float Radius = Mathf.Atan2(pos2.y - pos1.y, pos2.x - pos1.x); //自分と指定した座標とのラジアンを求める
        return new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);
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
                    ////斜めにレイを飛ばさない用
                    //if (stickmax.x > stickmax.y) { StickInput.y = 0; }
                    //else { StickInput.x = 0; }

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

    private bool PlayerHipDropMoveFlag()//プレイヤーのヒップドロップによる移動フラグ
    {
        if (pm.JumpOk && PlayerMoveFlg) 
        {
            FlickVec = SearchVector(this.transform.position, player.transform.position);//プレイヤーへのベクトルを取得
            if (!pm.GetInsideFlg()) { FlickVec = -FlickVec; }//外側に居れば反転させる

            pm.JumpOk = false;//一応こっちでfalseしとく
            return true;
        }
        return false;
    }

    public bool EnemyOnMoveFlag(bool Beatflag, Vector2 vec)//
    {
        if (Beatflag)
        {
            FlickVec = vec;
            GetEnemyBeatFlag = true;
            return true;
        }

        GetEnemyBeatFlag = false;
        return false;
    }

    private GameObject NearObjSearch(List<GameObject> Serchobj, List<Vector3> Searchpos, Vector3 NearPos)//オブジェクトのリストからで一番近いものを取得
    {
        //if (Serchobj.Count == 0) { Debug.Log("リストがない"); }

        List<float> distance = new List<float>();//引数の座標と調べ対座標との差
        float Min = 10000;//最小値

        for (int i = 0; i < Searchpos.Count; i++)
        {
            distance.Add((NearPos - Searchpos[i]).magnitude);

            if (distance[i] <= Min)//取得している最小の値より小さければ
            {
                Min = distance[i];//差が最小の値を取得
            }
        }

        for (int i = 0; i < distance.Count; i++)
        {
            if (distance[i] == Min)//差が最小の値を持った要素であれば
            {
                return Serchobj[i].gameObject;
            }
        }

        return null;
    }

    private void RigitBodyInit()//Rigidbodyの初期化
    {
        Rb = this.gameObject.GetComponent<Rigidbody>();
        Rb.constraints = RigidbodyConstraints.FreezePositionZ;
        Rb.freezeRotation = true;
    }
    public void ZeroVelo() //動きを止める
    {
        Rb.velocity = Vector3.zero;//勢いを止める
        FlickMoveFlag = false;
        Rb.isKinematic = true;
    }

    private bool LineVecFlag()//メビウスの輪が線上に乗っているかどうか調べる（縦か横の線のみ）
    {
        bool flag = false;
        GameObject MoveLine = null;

        for (int i = 0; i < Line.Count; i++)
        {
            if (cl[i].CanInputMoveVec(FlickVec, out FlickVec))
            {
                flag = true;
                MoveLine = Line[i];
                break;

            }
        }

        if (flag)
        {
            Line.Clear();
            Line.Add(MoveLine);
            cl.Clear();
            cl.Add(MoveLine.GetComponent<CrossLine>());
        }
        return flag;
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
                // Debug.Log("メビウスの輪同士が離れた" + this.gameObject.name);
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


    public void MobiusCol(GameObject col, Vector3 DistanceVec)//メビウスがオブジェクトに当たった時の処理
    {
        float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
        //float ColR = (col.GetComponent<SphereCollider>().bounds.size.x + col.GetComponent<SphereCollider>().bounds.size.y) / 4;// 相手メビウスの輪の円の半径を取得
        //float SocialDistance = ThisR + ColR + 8;//お互いの半径分と少しだけ離す

        this.transform.position = new Vector3(this.transform.position.x + ((ThisR + 4) * -DistanceVec.x), this.transform.position.y + ((ThisR + 4) * -DistanceVec.y),
            this.transform.position.z);

    }

    private bool HighSpeedCol()//速すぎて当たり判定をすり抜けた時の対策
    {
        Vector3 Pos = Vector3.zero;//レイを飛ばしすぎないようにするもの
        if (Nowtime < GoalMovetime)
        {
            Pos = SenkeiHokan(StartMovePos, MovePos, Nowtime, 0, GoalMovetime);
        }
        else
        {
            Pos = MovePos;
        }

        Ray ray;
        float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
        float distance = (Pos - OldPos).magnitude/* + (ThisR/2)*/;     //レイを飛ばす長さ

        List<GameObject> ColObj = new List<GameObject>();               //すり抜けたメビウスオブジェクトを格納するリスト
        List<Vector3> HitPos = new List<Vector3>();                     //ヒットした座標

        ray = new Ray(new Vector3(OldPos.x /*- (MoveVec.x * ThisR)*/, OldPos.y/* - (MoveVec.y * ThisR)*/, OldPos.z),    //Rayを飛ばす発射位置
         new Vector3(MoveVec.x, MoveVec.y, 0));                             //飛ばす方向

        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green, 1000, false);
        //貫通のレイキャスト
        foreach (RaycastHit hit in Physics.SphereCastAll(ray, ThisR, distance))
        {
            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト
            GameObject hitObj = null;

            //レイが当たったオブジェクト
            switch (hit.collider.gameObject.tag)
            {
                case "Mobius":
                    //if (!hit.collider.gameObject.GetComponent<MoveMobius>().GetFlickMoveFlag())//動いていないメビウスなら
                    //{
                    //ColObj.Add(hit.collider.gameObject);//レイで当たったオブジェクトをリストに格納
                    hitObj = hit.collider.gameObject;
                    //}
                    break;
            }

            if (hitObj == this.gameObject || hit.point == Vector3.zero) { hitObj = null; }//ヒットした中に自身が含まれないようにする


            if (ColObj.Count == 0 && hitObj != null) //レイが当たったオブジェクトがあれば　かつ　リストが空なら
            {
                ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                HitPos.Add(hit.point); Debug.Log("HitPos" + hit.point);
            }
            else if (ColObj.Count != 0 && hitObj != null)
            {
                if (SameObjListSearch(ColObj, hitObj))//ColObjリストの中に当たったものがなければ
                {
                    ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                    HitPos.Add(hit.point); Debug.Log("HitPos" + hit.point);
                }
            }
        }//foreach

        if (ColObj.Count != 0)//リストの中に要素があれば
        {
            GameObject otherObj = NearObjSearch(ColObj, HitPos, StartMovePos);//リストの中から始点に近いオブジェクトを取得

            this.transform.position = HitPos[ListNumberSearch(ColObj, otherObj)];
            Collision(otherObj);//当たり判定時の処理を実行

            Debug.Log(otherObj.name + "とぶつかった~～");
            return true;
        }
        else
        {
            //Debug.Log("すり抜けてない");
            return false;
        }
    }

    private void MobiusStrip()//メビウスの輪になっているときの処理
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
                ColMobiusObj = null;

                if (!PlayerMoveFlg && !EnemyMoveFlag)
                {
                    this.transform.position = MoyoriPos;
                }
            }
        }
    }

    private void Collision(GameObject otherObj)//当たり判定時の処理
    {
        // メビウスの輪同士がぶつかったとき
        if (otherObj.tag == "Mobius")
        {
            if (FlickMoveFlag)//自身が勢いがあるとき　
            {
                //Debug.Log("メビウスの輪同士がぶつかった" + this.gameObject.name);

                //自分と指定した座標とのラジアンを求める
                Vector3 DisVec = SearchVector(this.transform.position, otherObj.transform.position);

                bool SameFlag = false;//前回当たったオブジェクトと同じかどうか

                if (ColMobiusObj == null || ColMobiusObj != otherObj)//まだぶつかってない　または　違うものとぶつかったら
                {
                    if (otherObj.GetComponent<MoveMobius>().GetFlickMoveFlag())//相手が動いていたら
                    {
                        otherObj.transform.position = this.transform.position;
                        otherObj.GetComponent<MoveMobius>().MobiusCol(this.gameObject, -DisVec);
                        otherObj.GetComponent<MoveMobius>().ZeroVelo();
                    }
                    MobiusCol(otherObj, DisVec);//メビウス同士がぶつかった時の処理を実行

                }
                else if (ColMobiusObj == otherObj)//さっきと同じものとぶつかったら
                {
                    this.transform.position = OldPos;
                    SameFlag = true;
                }

                //if (!otherObj.GetComponent<MoveMobius>().GetFlickMoveFlag()) //相手が動いてないときなら止める
                //{
                if (!MobiusStripFlag)//メビウスの輪になっていないときに
                {
                    MobiusColFlag = true;
                }
                ColPos = (this.transform.position + otherObj.transform.position) / 2;//自分と相手の座標の中点を代入（見た目的に当たった場所）
                ZeroVelo();

                if (!SameFlag)//同じじゃなければ
                {
                    //メビウスの輪にするための情報をセット
                    SetMobiusStrip(otherObj);
                    otherObj.GetComponent<MoveMobius>().SetMobiusStrip(this.gameObject);
                }
                //}
            }
        }
    }

    private int ListNumberSearch(List<GameObject> ListObj, GameObject SearchObj)//特定のリストの要素数を調べる
    {
        for (int i = 0; i < ListObj.Count; i++)
        {
            if (ListObj[i] == SearchObj)//要素が見つかれば
            {
                //Debug.Log(i);
                return i; //見つかった要素数を返す
            }
        }

        return 10000;//false的なやつ
    }

    private bool SameObjListSearch(List<GameObject> ListObj, GameObject SearchObj)//オブジェクトのリストの中の要素を検索する
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
    public List<CrossLine> Getcl()
    {
        return cl;
    }
}
