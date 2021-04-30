using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    GameObject[] Mobius;                                                        // メビウスの輪
    public int NowMobius;                                                                           //現在のメビウスの添え字　初期のメビウスの輪
    int SaveMobius;                                                                                 //１つ前にいたメビウスの添え字
    float InsideLength;                                                                             //内側に入ったときの位置を調整する値
    public bool RotateLeftFlg;                                                                      //回転方向が左右のどちらかを判定　true:左　false:右
    public bool InsideFlg;                                                                          //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    int SideCnt;                                                                                    //メビウスの輪に沿った動きにするためメビウスの輪を何回切り替えたかをカウント  2以上で外側内側入れ替える
    float counter;                                                                                  //乗り移るとき、元のメビウスの輪に戻らないようにカウントする値
    Vector2 MobiusSavePos;                                                                          //移動前のメビウスの位置を保存する変数
    Vector2 MoveMobiusSum;                                                                          //移動前と移動後の差分を格納する計算用変数
    Vector2 vecX, vecY;

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    [SerializeField, Range(0, 7)] public int StartPoint;                                                                         //メビウス上の点の番号
    float MoveAngle;                                                                                //移動量
    int MobiusPointNum;                                                                             //メビウス上の点の総数　今後、点の数を増やす場合publicにする
    bool StartFlg;                                                                                  //初期位置設定用フラグ　最初の一回だけ通る

    GameObject RythmObj;
    Rythm rythm;
    bool RythmOneFlg;


    GameObject player;

    private Transform target;
    private Vector3 distanceTarget = new Vector3(22f, 0f, 0f);
    public float angle;
    [SerializeField] private float rotateSpeed = 180f;
    public float NormalSpeed = 1.3f;
    private float Speed;
    bool MobiusCol;
    public float StanTime;
    float StanTimeCount;
    bool Stan;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納

        Mobius = GameObject.FindGameObjectsWithTag("Mobius");//メビウスの輪の総数分配列生成


        player = GameObject.Find("Player");

        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }
        RythmObj = GameObject.Find("rythm_circle");
        this.rythm = RythmObj.GetComponent<Rythm>();

        RythmOneFlg = true;

        MobiusPointNum = 8;
        MoveAngle = 360.0f / MobiusPointNum;

        SideCnt = 2;
        SaveMobius = -1;
        TimingInput = false;
        StartFlg = true;

        //初期位置設定
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;
        this.gameObject.transform.position = new Vector3(MobiusPos.x, MobiusPos.y, 0);
        this.gameObject.transform.position += new Vector3(0, 50.0f, 0);


        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 22;//内側までの距離
        }
        else//外側
        {
            InsideLength = 0;
        }
        angle = 360 - (StartPoint * 45);
        Speed = NormalSpeed;
        MobiusCol = false;
        Stan = false;
        StanTimeCount = 0;
    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 - InsideLength;// メビウスの輪の円の半径を取得
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);

        if (Stan)//スタン中
        {
            StanTimeCount += Time.deltaTime;
            if (StanTimeCount > StanTime)
            {
                StanTimeCount = 0;
                Stan = false;
            }
        }
        else//通常時
        {
            //移動
            if (RotateLeftFlg)
            {
                angle += (rotateSpeed * Speed) * Time.deltaTime;
            }
            else
            {
                angle -= (rotateSpeed * Speed) * Time.deltaTime;
            }

            if (NowMobius == player.GetComponent<PlayerMove>().NowMobius)
            {
                if (player.GetComponent<PlayerMove>().jump)
                {
                    //Debug.Log("ヒップドロップ中");
                    if(angle+5>player.GetComponent<PlayerMove>().angle-5&& angle - 5 < player.GetComponent<PlayerMove>().angle + 5)
                    {
                        Vector3 playpos = player.transform.position;
                        Vector3 hedlength = distanceTarget;
                        hedlength.y = hedlength.y + 10;
                        Vector3 enemyhedpos = target.position + Quaternion.Euler(0f, 0f, angle) * hedlength;

                        //Debug.Log("プレイヤーと当たった");

                        if (player.GetComponent<PlayerMove>().GetPlayerLength() > hedlength.y)
                        {
                            StanTimeCount = 0;
                            Stan = true;
                            Debug.Log("ヒップドロップ成功");
                        }
                    }
                    

                }
            }
        }


        //角度の範囲を指定(0～360)
        //angle = Mathf.Repeat(angle, 360f);
        if (angle > 360)
        {
            angle = angle - 360;
        }
        if (angle < 0)
        {
            angle = angle + 360;
        }


        if (MobiusCol)
        {
            counter += Time.deltaTime;
            //移ったときに元のメビウスの輪に戻らないようにカウントする
            if (counter > 1)//
            {
                //移り変わることができるようにする
                SaveMobius = NowMobius;
                counter = 0;
                MobiusCol = false;
            }
        }
        else
        {
            CollisonMobius();//移り先のメビウスの輪を探す
        }

        //Debug.Log(this.name + "UPDATE");
        //if (StartFlg)
        //{
        //    ApproachMobius();//対象のメビウスの輪に近づける

        //    for (int i = 0; i < StartPoint; i++)//入力されたポイントの場所に移動させる
        //    {
        //        transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, MoveAngle);//右移動
        //    }

        //    StartFlg = false;
        //}
        //else
        //{
        //    if (!Mobius[NowMobius].GetComponent<MoveMobius>().GetFlickMoveFlag())//メビウスが止まっているとき
        //    {
        //        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得

        //        MoveMobiusSum = MobiusPos - MobiusSavePos;

        //        MobiusSavePos = MobiusPos;


        //        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // メビウスの輪の円の半径を取得
        //        GetComponent<SphereCollider>().bounds.size.x / 2;
        //        this.gameObject.transform.position = new Vector3(MobiusPos.x, MobiusPos.y, 0);
        //        this.gameObject.transform.position += new Vector3(0, (hankei - InsideLength), 0);

        //        for (int i = 0; i < StartPoint; i++)//ポイントの場所に移動させる
        //        {
        //            transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, MoveAngle);//右移動
        //        }


        //        TimingInput = this.rythm.rythmSendCheckFlag;

        //        if (!TimingInput && !RythmOneFlg) RythmOneFlg = true;

        //        if (TimingInput && RythmOneFlg)//テンポのタイミングで入力されたら
        //        {
        //            counter++;
        //            //Debug.Log(this.name+":TimindInputOn");
        //        }





        //        if (Mobius[NowMobius] != null)
        //        {

        //            if (TimingInput && RythmOneFlg)//キー入力あり
        //            {
        //                //ApproachMobius();//軌道に乗せる

        //                if (RotateLeftFlg)
        //                {
        //                    //transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, this.transform.forward, MoveAngle);//左移動
        //                    StartPoint--;
        //                    if (StartPoint < 0)
        //                    {
        //                        StartPoint = 7;
        //                    }
        //                }
        //                else
        //                {
        //                    //transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, MoveAngle);//右移動
        //                    StartPoint++;
        //                    if (StartPoint > 7)
        //                    {
        //                        StartPoint = 0;
        //                    }
        //                }


        //                player.GetComponent<PlayerMove>().EnemyUpdateCountUp();//敵の更新処理が何体目まで実行されたかカウントアップする　最後の敵が更新されればrythmSendCheckFlagをfalseにする

        //                RythmOneFlg = false;
        //                //Debug.Log("移動");
        //                //Debug.Log(StartPoint);
        //            }//if (TimingInput)


        //            this.gameObject.transform.position = new Vector3(this.transform.position.x + MoveMobiusSum.x, this.transform.position.y + MoveMobiusSum.y, 0);         //メビウスの動きについていく


        //            if (!Mobius[NowMobius].GetComponent<MoveMobius>().GetFlickMoveFlag()) CollisonMobius();//移り先のメビウスの輪を探す

        //            //移ったときに元のメビウスの輪に戻らないようにカウントする
        //            if (counter > 1)//
        //            {
        //                //移り変わることができるようにする
        //                SaveMobius = NowMobius;
        //                counter = 0;

        //            }//if (counter > 2)


        //        }//if (Mobius[NowMobius] != null)
        //    }
        //}//else if(StartFlg)
    }//void Update()

    private void ApproachMobius()//対象のメビウスの輪に近づける
    {
        //対象のメビウスの輪を元にプレイヤーの3軸直交単位ベクトルを求める
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得
        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // メビウスの輪の円の半径を取得
            GetComponent<SphereCollider>().bounds.size.x / 2;

        MoveMobiusSum = MobiusPos - MobiusSavePos;

        MobiusSavePos = MobiusPos;

        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得
        float PlayHankei = this.GetComponent<SphereCollider>().bounds.size.x / 2 +                     // プレイヤーの円の半径を取得
            GetComponent<SphereCollider>().bounds.size.x / 2;

        vecY = PlayerPos - MobiusPos;                                                              //プレイヤーの位置から対象のメビウスへのベクトルを求める(Y軸ベクトル)

        vecX.x = 0.0f * 0.0f - 1.0f * vecY.y;                                                              //Y軸とZ軸からX軸を求める
        vecX.y = 1.0f * vecY.x - 0.0f * 0.0f;

        float veclength = Mathf.Sqrt(vecY.x * vecY.x + vecY.y * vecY.y);                                   //Yベクトルの長さ計算
                                                                                                           //単位ベクトルにする
        vecY.x = vecY.x / veclength;
        vecY.y = vecY.y / veclength;

        veclength = Mathf.Sqrt(vecX.x * vecX.x + vecX.y * vecX.y);                                         //Xベクトルの長さ計算
                                                                                                           //単位ベクトルにする
        vecX.x = vecX.x / veclength;
        vecX.y = vecX.y / veclength;


        //対象のメビウスの軌道に乗せる

        this.gameObject.transform.position = new Vector3(MobiusPos.x + (vecY.x * (hankei - InsideLength)), MobiusPos.y + (vecY.y * (hankei - InsideLength)), 0);     //対象のメビウスに近づける



    }//private void ApproachMobius()//対象のメビウスの輪に近づける


    private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    {

        Vector2 NowMobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // 現在のメビウスの輪の位置を取得
        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得

        Vector2 NowVec = NowMobiusPos - PlayerPos;
        float NowLength = Mathf.Sqrt(NowVec.x * NowVec.x + NowVec.y * NowVec.y);

        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;
        float PlayerHankei = this.GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

        Vector2 NextMobiusPos;//次のメビウスの場所
        Vector2 NextVec;
        float NextLength = 0;
        for (int i = 0; i < Mobius.Length; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            NextMobiusPos = Mobius[i].GetComponent<SphereCollider>().bounds.center;

            Vector2 Vec = NextMobiusPos - NowMobiusPos;//対象のメビウスの輪と
            float VecLength = Mathf.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y);
            if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定
            {
                Vec.x = Vec.x / VecLength;
                Vec.y = Vec.y / VecLength;

                Vector2 CollisonPos;//当たっている場所を計算(接点)
                CollisonPos.x = NowMobiusPos.x + Vec.x * hankei;
                CollisonPos.y = NowMobiusPos.y + Vec.y * hankei;

                NextVec = CollisonPos - PlayerPos;//メビウスの輪同士の接点とプレイヤーの位置のベクトルを計算
                NextLength = Mathf.Sqrt(NextVec.x * NextVec.x + NextVec.y * NextVec.y);//メビウスの輪同士の接点とプレイヤーの位置の長さ計算


                if ((PlayerHankei / 2) + InsideLength > NextLength)//プレイヤーと移り先のメビウスの輪が当たった
                {
                    transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, 180);
                    if (StartPoint >= 4)
                    {
                        StartPoint -= 4;
                    }
                    else
                    {
                        StartPoint += 4;
                    }
                    //Debug.Log(StartPoint);
                    SaveMobius = NowMobius;
                    NowMobius = i;
                    counter = 0;
                    angle += 180;

                    if (SideCnt >= 2)//2回切り替えると
                    {
                        //内側と外側を反転させる
                        if (InsideFlg)
                        {
                            InsideFlg = false;
                            InsideLength = 0;//内側までの距離

                            //Debug.Log("外側");
                        }
                        else
                        {
                            InsideLength = 22;//内側までの距離
                            InsideFlg = true;
                            //Debug.Log("内側");
                        }


                        SideCnt = 0;
                    }//if (SideCnt>=2)//2回切り替えると
                    ApproachMobius();

                    //メビウスの輪を切り替えると左右移動を反転させる
                    if (RotateLeftFlg)
                    {
                        RotateLeftFlg = false;
                    }
                    else
                    {
                        RotateLeftFlg = true;
                    }

                    //Debug.Log("メビウスの輪を切り替えた");
                    SideCnt++;
                    MobiusCol = true;
                    break;
                }//if ((hankei/3)+(InsideLength/2) > NextLength)//プレイヤーと移り先のメビウスの輪が当たった


            }//if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定

        }//for (int i = 0; i <  Mobius.Length; i++)


    }//private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定


    public int GetNowMobiusNum()
    {
        return NowMobius;
    }

    public bool GetInsideFlg()
    {
        return InsideFlg;
    }

    public bool GetStanFlg()
    {
        return Stan;
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
