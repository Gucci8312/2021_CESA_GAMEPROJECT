using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    public float GoalMovetime = 0.05f;                                       //目的地へ到達するまでの時間（秒）
    float Nowtime = 0;                                                       //移動時間（秒）

    [HideInInspector] public bool PlayerMoveFlg;                             // プレイヤーによる移動判定用
    GameObject player;
    PlayerMove pm;                                                           //PlayerMoveスクリプト

    [HideInInspector] public bool EnemyMoveFlag;                             //エネミーによる移動判定用
    private bool GetEnemyBeatFlag = false;
    public GameObject[] Enemy = new GameObject[2];

    private Vector2 StickInput;                                               //スティック入力時の値を取得用(-1～1)
    private Vector2 FlickVec;                                                 //弾いた時のベクトル格納用
    private bool FlickMoveFlag = false;                                       //弾き移動をさせるかどうか
    bool OneFlickFlag = false;                                                //スティック入力を連続でさせない用

    public List<GameObject> Line = new List<GameObject>();                          //線のオブジェクト
    List<CrossLine> cl = new List<CrossLine>();                              //CrossLineスクリプト
    [HideInInspector] public GameObject MoveLineObj;                         //動く線のオブジェクト格納用（MoveLineが操作する）
    int MobiusMoveCrossPosNum;                                               //メビウスが移動する交点の要素番号

    private Rigidbody Rb;
    [HideInInspector] public Vector3 MovePos;                                //移動する位置
    [HideInInspector] public Vector3 OldMovePos;                             //前回の移動する位置
    private Vector3 MoveVec;
    private bool MobiusColFlag;                                              //メビウスの当たり判定
    public Vector3 ColPos;                                                   //メビウスが当たった座標（具体的には自分と相手の座標の中点）
    [HideInInspector] public Vector3 StartMovePos;                           //移動開始点
    [HideInInspector] public Vector3 OldPos;                                 //前回の座標
    //Vector3 MoyoriPos;                                                     //最寄りの駅

    private float ThisR;                                                     //半径

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    MobiusColor Mc;                                                          //MobiusColorスクリプト
    //MobiusAttachPos MaPos;                                                   //MobiusAttachPosスクリプト

    bool ColObjAttachFlag;                                                   //メビウス以外のオブジェクトに当たっているかどうか
    bool MobiusStripFlag;                                                    //メビウスの輪になっているかどうか
    GameObject ColMobiusObj;                                                 //当たった相手メビウス格納用

    ShakeMobius Sm;

    static bool StopFlag = false;//true:止める　false:動く

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
        OldMovePos = this.transform.position;
        //MoyoriPos = this.transform.position;

        this.gameObject.AddComponent<LinePutMobius>();

        Sm = this.GetComponent<ShakeMobius>();

        ThisR= (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
    }

    // Update is called once per frame
    void Update()
    {
        if (!StopFlag)
        {
            MoveMobiusUpdate();
        }

    }

    //MoveMobiusの更新
    private void MoveMobiusUpdate()
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
        BlockCheak();
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

            if (PlayerHipDropMoveFlag() || //プレイヤーがヒップドロップしたら
                (GetEnemyBeatFlag && EnemyMoveFlag))//EnemyMobius側で指定したビート数に達したら
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
                            CrossLine HitCL = hit.collider.gameObject.GetComponent<CrossLine>();

                            if (!HitCL.MoveFlag && !HitCL.SameLRvec(cl[0].GetLvec(), cl[0].GetRvec()))//動いていない　かつ　線の方向が同じじゃなければ
                            {
                                if (SameObjListSearch(Line, hit.collider.gameObject))//Lineリストの中にレイが当たったオブジェクトがなければ
                                {
                                    CrossLineObj.Add(hit.collider.gameObject);//CrossLineObjリストの中に追加
                                    CrossLineObj[CrossLineObj.Count - 1].GetComponent<CrossLine>().RayHitPos = hit.point;
                                    HitPos.Add(hit.point);
                                }
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
                            MovePos = cl[0].NearCrossPos(NearCl.RayHitPos, out MobiusMoveCrossPosNum);//移動できる交点を取得
                            MoveVec = SearchVector(this.transform.position, MovePos);


                            OldMovePos = this.transform.position;

                            //進む方向の線の上に乗っているかどうか調べる
                            float distance = (FlickVec - new Vector2(MoveVec.x, MoveVec.y)).magnitude;
                            if (distance < 0.15f) //二つベクトルに誤差が無ければ
                            {
                                FlickMoveFlag = true;

                                this.rythm.checkMoviusMove = false;
                            }
                        }
                        //else//移動できなければ
                        //{
                        //    Sm.ShakeOn();//失敗時の振動させる
                        //}
                    }
                    //else//移動できなければ
                    //{
                    //    Sm.ShakeOn();//失敗時の振動させる
                    //}
                }
                else//移動できなければ
                {
                    Sm.ShakeOn();//失敗時の振動させる
                }
            }
        }

        else//移動処理
        {
            // Rb.AddForce(-Rb.velocity * (Gensokuritu * 0.1f), ForceMode.Acceleration);//減速させる（要調整）

            //MovePos = cl[0].GetCrossPos()[MobiusMoveCrossPosNum];

            if (!HighSpeedRayCol())//何も当たらなければ
            {

                if (Nowtime >= GoalMovetime)//到着したら
                {
                    if (!cl[0].GetGotoLineFlag() && PlayerMoveFlg)//まだ通ったことのない線だったら
                    {
                        cl[0].SetGotoLineFlag(true);//線を通った
                    }

                    ZeroVelo();
                    this.transform.position = MovePos;
                    //MoyoriPos = MovePos;

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
            FlickVec = SearchVector(this.transform.position, pm.GetHipDropPos());//プレイヤーへのベクトルを取得
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

    public GameObject NearObjSearch(List<GameObject> Serchobj, List<Vector3> Searchpos, Vector3 NearPos)//オブジェクトのリストからで一番近いものを取得
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

    private bool LineVecFlag()//メビウスの輪が線上に乗っているかどうか
    {
        bool Seachflag = false;
        GameObject MoveLine = null;
        Vector2 Vec;//FlickVecに代入用

        for (int i = 0; i < Line.Count; i++)
        {
            if (cl[i].CanInputMoveVec(FlickVec, out Vec))
            {
                if (cl[i].GetRvec() == Vec)
                {
                    if (!cl[i].NearEndRCrossPosFlag(this.transform.position))//自身の座標が右端になければ
                    {

                        FlickVec = Vec;
                        Seachflag = true;
                        MoveLine = Line[i];
                        break;

                    }
                }
                else if (cl[i].GetLvec() == Vec)
                {
                    if (!cl[i].NearEndLCrossPosFlag(this.transform.position))
                    {

                        FlickVec = Vec;
                        Seachflag = true;
                        MoveLine = Line[i];
                        break;

                    }
                }
            }

        }


        if (Seachflag)
        {
            //リストの中の余分なものを削除
            Line.Clear();
            Line.Add(MoveLine);
            cl.Clear();
            cl.Add(MoveLine.GetComponent<CrossLine>());
        }

        //Debug.Log("移動" + Line[0].name);

        return Seachflag;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            GameObject ColLine = null;
            if (Line.Count == 0)//Lineリストに要素が無ければ
            {
                Line.Add(other.gameObject);//Lineリストに当たったものを追加
                cl.Add(other.GetComponent<CrossLine>());
                ColLine = other.gameObject;
            }
            else//Lineリストに要素があれば
            {
                if (SameObjListSearch(Line, other.gameObject))//Lineリストの中に当たったものがなければ
                {
                    Line.Add(other.gameObject);//Lineリストに当たったものを追加
                    cl.Add(other.GetComponent<CrossLine>());
                    ColLine = other.gameObject;
                }
            }

            //if (ColLine != null)
            //{
                //if (other.GetComponent<CrossLine>().MoveLineFlag && !other.GetComponent<CrossLine>().MoveFlag)
                //{
                //    if (MoveLineObj == null)
                //    {
                //        other.GetComponent<MoveLine>().PutMobiusOnOff(true, this.gameObject);
                //    }
                //}
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            Line.Remove(other.gameObject);//登録したLineリストの中に該当する要素を削除する
            cl.Remove(other.GetComponent<CrossLine>());

            //if (other.GetComponent<CrossLine>().MoveLineFlag && !other.GetComponent<CrossLine>().MoveFlag)
            //{
            //    if (MoveLineObj == other.gameObject)
            //    {
            //        other.GetComponent<MoveLine>().PutMobiusOnOff(false, this.gameObject);
            //        //Debug.Log(other.gameObject.name + "から離れた");
            //    }
            //}            
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


    public void MobiusCol(float distance, Vector3 DistanceVec)//メビウスがオブジェクトに当たった時の処理
    {
        //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
        //float ColR = (col.GetComponent<SphereCollider>().bounds.size.x + col.GetComponent<SphereCollider>().bounds.size.y) / 4;// 相手メビウスの輪の円の半径を取得
        //float SocialDistance = ThisR + ColR + 8;//お互いの半径分と少しだけ離す

        this.transform.position = new Vector3(this.transform.position.x + (distance * -DistanceVec.x), this.transform.position.y + (distance * -DistanceVec.y),
            this.transform.position.z);

    }

    private bool HighSpeedRayCol()//速すぎて当たり判定をすり抜けた時の対策
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
        //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
        float distance = (Pos - OldPos).magnitude/* + (ThisR/2)*/;     //レイを飛ばす長さ

        List<GameObject> ColObj = new List<GameObject>();               //すり抜けたメビウスオブジェクトを格納するリスト
        List<Vector3> HitPos = new List<Vector3>();                     //ヒットした座標

        ray = new Ray(new Vector3(OldPos.x, OldPos.y, OldPos.z),    //Rayを飛ばす発射位置
         new Vector3(MoveVec.x, MoveVec.y, 0));                             //飛ばす方向

        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1000, false);

        //貫通のレイキャスト
        foreach (RaycastHit hit in Physics.SphereCastAll(ray, ThisR, distance))
        {
            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト
            GameObject hitObj = null;

            //レイが当たったオブジェクト
            switch (hit.collider.gameObject.tag)
            {
                case "Mobius":
                    hitObj = hit.collider.gameObject;
                    break;

                case "Block":
                    hitObj = hit.collider.gameObject;
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

            this.transform.position = HitPos[ListNumberSearch(ColObj, otherObj)];//メビウスの座標を例が当たった座標にする（計算をしやすくするため）
            //this.transform.position = otherObj.transform.position;
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
            //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
            float ColR = (ColMobiusObj.GetComponent<MoveMobius>().GetThisR());// 相手メビウスの輪の円の半径を取得
            float SocialDistance = ThisR + ColR + 15;//お互いの半径分と少しだけ離す

            float distance = (this.transform.position - ColMobiusObj.transform.position).magnitude;//相手と自分の距離

            if (SocialDistance < distance)//離れ過ぎたら 
            {
                MobiusStripFlag = false;
                ColMobiusObj = null;
            }
        }
    }

    private void BlockCheak()
    {
        if (ColObjAttachFlag)
        {
            //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
            float ColScale = (ColMobiusObj.GetComponent<BoxCollider>().bounds.size.x + ColMobiusObj.GetComponent<BoxCollider>().bounds.size.y) / 4;
            float SocialDistance = ThisR + ColScale + 15;//お互いの半径分と少しだけ離す

            float distance = (this.transform.position - ColMobiusObj.transform.position).magnitude;//相手と自分の距離

            if (SocialDistance < distance)//離れ過ぎたら 
            {
                GimicObjNull();
            }
        }
    }

    public void Collision(GameObject otherObj)//当たり判定時の処理
    {
        if (FlickMoveFlag)//自身が勢いがあるとき　
        {
            Vector3 DisVec = SearchVector(this.transform.position,otherObj.transform.position);
            bool SameFlag = false;//前回当たったオブジェクトと同じかどうか

            //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
            float PosDistance = (StartMovePos - otherObj.transform.position).magnitude;//相手と始点の距離

            switch (otherObj.tag)
            {
                case "Mobius":
                    {
                        float ColR = otherObj.GetComponent<MoveMobius>().GetThisR();
                        float ScaleDistance = ThisR + ColR + 15;//お互いの半径分と少しだけ離す

                        if (!otherObj.GetComponent<LinePutMobius>().MoveLineFlag)
                        {
                            if (ScaleDistance < PosDistance)//離れているところから移動してぶつかったなら
                            {
                                //if (otherObj.GetComponent<MoveMobius>().GetFlickMoveFlag())//相手が動いていたら
                                //{
                                //    ////相手の動きを止める
                                //    otherObj.transform.position = this.transform.position;
                                //    otherObj.GetComponent<MoveMobius>().MobiusCol(ColR + 4, -DisVec);
                                //    otherObj.GetComponent<MoveMobius>().ZeroVelo();
                                //}
                                otherObj.GetComponent<MoveMobius>().ZeroVelo();
                                float dis = (this.transform.position - otherObj.transform.position).magnitude;//自分と相手の距離を求める
                                MobiusCol(ThisR + dis / 6, DisVec);//メビウス同士がぶつかった時の処理を実行(dis/6は差を埋める)


                                SoundManager.PlaySeName("メビウス_sin");//SEを呼ぶ
                            }
                            else//近いところでぶつかったなら
                            {
                                this.transform.position = OldPos;
                                SameFlag = true;
                            }
                        }
                        else
                        {
                            this.transform.position = OldPos;
                        }


                        if (!MobiusStripFlag)//メビウスの輪になっていないときに
                        {
                            MobiusColFlag = true;

                        }
                        if (!SameFlag)//同じじゃなければ
                        {
                            //メビウスの輪にするための情報をセット
                            SetMobiusStrip(otherObj);
                            otherObj.GetComponent<MoveMobius>().SetMobiusStrip(this.gameObject);
                        }


                        break;
                    }

                case "Block":
                    {
                        this.transform.position = otherObj.transform.position;//計算をしやすくするために中心に移動

                        float ColScale = (otherObj.GetComponent<BoxCollider>().bounds.size.x + otherObj.GetComponent<BoxCollider>().bounds.size.y) / 4;

                        float ScaleDistance = ThisR + ColScale + 15;//お互いの大きさと少しだけ離す

                        if (ScaleDistance < PosDistance)//離れているところから移動してぶつかったなら 
                        {
                            float dis = (this.transform.position - otherObj.transform.position).magnitude;//自分と相手の距離を求める
                            MobiusCol(ScaleDistance-1, DisVec);//メビウスがぶつかった時の処理を実行

                            otherObj.GetComponent<Block>().Collision(this.gameObject);
                        }
                        else//近いところでぶつかったなら
                        {
                            this.transform.position = OldPos;
                        }

                        break;
                    }
            }

            ColPos = (this.transform.position + otherObj.transform.position) / 2;//自分と相手の座標の中点を代入（見た目的に当たった場所）
            ZeroVelo();

        }
    }

    public int ListNumberSearch(List<GameObject> ListObj, GameObject SearchObj)//特定のリストの要素数を調べる
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

    public bool SameObjListSearch(List<GameObject> ListObj, GameObject SearchObj)//オブジェクトのリストの中の要素を検索する
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

    static public void StopFlagSet(bool flag)
    {
        StopFlag = flag;
    }

    public void SetMobiusStrip(GameObject _obj)//メビウスの輪にする為の値をセット
    {
        ColMobiusObj = _obj;//ぶつかったメビウスを格納
        MobiusStripFlag = true;
    }
    public void GimicObjNull()
    {
        ColObjAttachFlag = false;
        ColMobiusObj = null;
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
    public List<GameObject> GetLine()
    {
        return Line;
    }

    public float GetThisR()
    {
        return ThisR;
    }
}
