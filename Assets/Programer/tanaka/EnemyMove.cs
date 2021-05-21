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

    [SerializeField, Range(0, 7)] public int StartPoint;                                                                         //メビウス上の点の番号

    GameObject player;

    private Transform target;
    private Vector3 distanceTarget = new Vector3(22f, 0f, 0f);
    public float angle;
    float anglesave;
    [SerializeField] private float rotateSpeed = 180f;
    public float NormalSpeed = 1.3f;
    private float Speed;
    bool MobiusCol;
    public float StanTime = 1;
    float StanTimeCount;
    bool Stan;
    public float InsideSpeed = 2;

    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用


    GameObject ball;//子オブジェクトのトゲなし
    GameObject toge;//子オブジェクトのトゲあり
    bool TogeFlg;//トゲのフラグ

    void Start()
    {
        Mobius = GameObject.FindGameObjectsWithTag("Mobius");//メビウスの輪の総数分配列生成


        player = GameObject.Find("Player");

        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード

        TogeFlg = false;

        ball = transform.GetChild(0).gameObject;
        toge = transform.GetChild(1).gameObject;

        SaveMobius = -1;

        //初期位置設定
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;
        this.gameObject.transform.position = new Vector3(MobiusPos.x, MobiusPos.y, 0);
        this.gameObject.transform.position += new Vector3(0, 50.0f, 0);


        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 50;//内側までの距離
        }
        else//外側
        {
            InsideLength = 0;
        }

        SideCnt = 2;

        if (RotateLeftFlg)//メビウスの輪の世界線調整
        {
            SideCnt = 2;
        }
        else
        {
            SideCnt = 1;
        }

        angle = 360 - (StartPoint * 45);//始まりの位置を求める
        anglesave = angle;
        Speed = NormalSpeed;
        MobiusCol = false;
        Stan = false;
        StanTimeCount = 0;

        target = Mobius[NowMobius].transform;
        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 - InsideLength;// メビウスの輪の円の半径を取得
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);

    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        TogeFlg = rythm.rythmCheckFlag;

        if (TogeFlg)
        {
            ball.SetActive(false);
            toge.SetActive(true);
        }
        else
        {
            ball.SetActive(true);
            toge.SetActive(false);

        }

        //Debug.Log(angle);
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2) - InsideLength;// メビウスの輪の円の半径を取得
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
            if (InsideFlg)//内側
            {
                Speed = NormalSpeed * InsideSpeed;
            }
            else
            {
                Speed = NormalSpeed;
            }

            //移動
            if (RotateLeftFlg)
            {
                angle += (rotateSpeed * Speed) * Time.deltaTime;
            }
            else
            {
                angle -= (rotateSpeed * Speed) * Time.deltaTime;
            }

        }


        //角度の範囲を指定(0～360)
        if (angle > 360)
        {
            angle = angle - 360;
        }
        if (angle < 0)
        {
            angle = angle + 360;
        }


        if (NowMobius == player.GetComponent<PlayerMove>().GetNowMobiusNum())
        {
            //踏まれたかどうか
            bool HipDropCollision = player.GetComponent<PlayerMove>().HipDropCollision(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2);

            if (HipDropCollision)//プレイヤーに踏まれたら
            {
                StanOn();
            }
        }



        if (MobiusCol)
        {
            counter += Time.deltaTime;

            //移ったときに元のメビウスの輪に戻らないようにカウントする
            if (counter > 0.2)//移り変わりを制御
            {
                if (angle > anglesave + 90 || angle < anglesave - 90)
                {
                    //移り変わることができるようにする
                    SaveMobius = NowMobius;
                    counter = 0;
                    MobiusCol = false;
                }
            }


        }
        else
        {
            CollisonMobius();//移り先のメビウスの輪を探す
        }


    }//void Update()

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(1, 0, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ
    }




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
            VecLength += PlayerHankei - 10;//メビウス同士の当たり判定の長さを調整

            if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定
            {
                Vec.x = Vec.x / VecLength;
                Vec.y = Vec.y / VecLength;

                Vector2 CollisonPos;//当たっている場所を計算(接点)
                CollisonPos.x = NowMobiusPos.x + Vec.x * hankei;
                CollisonPos.y = NowMobiusPos.y + Vec.y * hankei;

                NextVec = CollisonPos - PlayerPos;//メビウスの輪同士の接点とプレイヤーの位置のベクトルを計算
                NextLength = Mathf.Sqrt(NextVec.x * NextVec.x + NextVec.y * NextVec.y);//メビウスの輪同士の接点とプレイヤーの位置の長さ計算

                //メビウス同士の成す角度を求める
                float CollisonAngle = Get2PointAngle(NowMobiusPos, NextMobiusPos);
                //メビウスとプレイヤーの成す角度を求める
                float NowAngle = Get2PointAngle(NowMobiusPos, PlayerPos);


                if (CollisonAngle < NowAngle + 5 && CollisonAngle > NowAngle - 5)//瞬間移動バグを修正
                //if ((PlayerHankei / 2) + InsideLength > NextLength)//プレイヤーと移り先のメビウスの輪が当たった
                {
                    //transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, 180);
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
                    anglesave = angle;

                    if (anglesave > 360)
                    {
                        anglesave = anglesave - 360;
                    }
                    if (anglesave < 0)
                    {
                        anglesave = anglesave + 360;
                    }

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
                            InsideLength = 50;//内側までの距離
                            InsideFlg = true;
                            //Debug.Log("内側");
                        }


                        SideCnt = 0;
                    }//if (SideCnt>=2)//2回切り替えると

                    //メビウスの輪を切り替えると左右移動を反転させる
                    if (RotateLeftFlg)
                    {
                        RotateLeftFlg = false;

                        if (!InsideFlg)//イレギュラー処理内側から外側に行くとずれる
                        {
                            angle -= 40;
                        }
                        else
                        {
                            angle += 40;
                            //Debug.Log("1");
                        }
                    }
                    else
                    {
                        RotateLeftFlg = true;
                        if (!InsideFlg)//イレギュラー処理内側から外側に行くとずれる
                        {

                            //Debug.Log("2");
                        }
                        else
                        {
                            angle += 20;
                            //Debug.Log("3");
                        }
                    }

                    //Debug.Log("メビウスの輪を切り替えた");
                    SideCnt++;
                    MobiusCol = true;
                    break;
                }//if ((hankei/3)+(InsideLength/2) > NextLength)//プレイヤーと移り先のメビウスの輪が当たった


            }//if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定

        }//for (int i = 0; i < Mobius.Length; i++)

    }//private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定


    private float Get2PointAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }
        return degree;
    }

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

    public void StanOn()
    {
        StanTimeCount = 0;
        Stan = true;
    }


    private void OnTriggerEnter(Collider other)
    {

    }

}
