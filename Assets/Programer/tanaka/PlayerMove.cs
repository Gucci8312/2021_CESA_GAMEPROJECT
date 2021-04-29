using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// プレイヤーの挙動
public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Mobius;                                                        // メビウスの輪
    public int NowMobiusColor;                                                                      //松井君に渡す用　MobiusuColorから取得した変数格納
    public int NowMobius;                                                                           //現在のメビウスの添え字　初期のメビウスの輪
    int SaveMobius;                                                                                 //１つ前にいたメビウスの添え字
    float InsideLength;                                                                             //内側に入ったときの位置を調整する値
    public bool RotateLeftFlg;                                                                      //回転方向が左右のどちらかを判定　true:左　false:右
    public bool InsideFlg;                                                                          //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    public float NormalSpeed;
    public float UpSpeed;
    public float InsideSpeed = 2;//内側のスピードを調整するための変数
    private float Speed;
    int SideCnt;                                                                                    //メビウスの輪に沿った動きにするためメビウスの輪を何回切り替えたかをカウント  2以上で外側内側入れ替える
    float counter;                                                                                  //乗り移るとき、元のメビウスの輪に戻らないようにカウントする値
    Vector2 MobiusSavePos;                                                                          //移動前のメビウスの位置を保存する変数
    Vector2 MoveMobiusSum;                                                                          //移動前と移動後の差分を格納する計算用変数
    Vector2 vecX, vecY;

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    [SerializeField, Range(0, 7)] public int StartPoint;                                            //メビウス上の点の番号
    float MoveAngle;                                                                                //移動量
    int MobiusPointNum;                                                                             //メビウス上の点の総数　今後、点の数を増やす場合publicにする
    bool StartFlg;                                                                                  //初期位置設定用フラグ　最初の一回だけ通る
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用


    public bool CollisionState;                                                                             //当たり判定を外部に渡す変数　treu:当たっている　false:当たっていない

    int EnemyMax;
    int EnemyUpdateCount;


    float jumpmove;//ジャンプの位置
    float jumpmovesave;

    float jumpmove_prev;
    float pow;

    float jumpcount;//ジャンプ処理の時間
    public bool JumpOk;//ヒップドロップが完了したかどうか　松井君に渡す用

    [SerializeField] float jumppow = 10;//ジャンプ力
    [SerializeField] float HipDropSpeed = 10;//ヒップドロップスピード

    public bool CollisionOn = true;//敵との当たり判定

    Vector2 Lopos;

    private Transform target;//現在のメビウスのトランスフォーム
    public float angle;//現在のメビウスからのプレイヤーの角度
    [SerializeField] private float rotateSpeed = 180f;//回転速度
    private Vector3 distanceTarget = new Vector3(0f, 0f, 0f);//メビウスからの距離
    bool MobiusCol;//メビウス同士の当たり判定


    bool SpacePress;//スピードアップボタンの判定
    bool SpeedUpMashing;//スピードアップが連打されているかどうか
    bool JumpMashing;//ジャンプボタンが連打されているか
    public bool HipDrop;//ヒップドロップ中

    private GameObject hipcol;
    bool SpeedUpFlg;//スピードアップしているか

    bool RythmSaveFlg;//リズムの切り替わりで判定させる
    bool RythmFlg;//リズムが来ているかどうか

    public float effectangle;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納

        Mobius = GameObject.FindGameObjectsWithTag("Mobius");

        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード


        MobiusPointNum = 8;
        MoveAngle = 360.0f / MobiusPointNum;


        SideCnt = 2;
        SaveMobius = -1;
        TimingInput = false;
        StartFlg = true;
        counter = -1;
        CollisionState = false;
        SpeedUpMashing = false;
        JumpMashing = false;

        EnemyMax = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemyUpdateCount = 0;

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


        angle = 360 - (StartPoint * 45);//始まりの位置を求める
        MobiusCol = false;
        jumpmove = 0;
        jumpmovesave = jumpmove;
        jumpcount = 0;

        Speed = NormalSpeed;
        SpacePress = false;



        SpeedUpFlg = false;

        RythmFlg = this.rythm.rythmCheckFlag;
        RythmSaveFlg = RythmFlg;

        PositionSum();

    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        if (StartFlg)
        {
            StartFlg = false;
        }

        NowMobiusColor = Mobius[NowMobius].GetComponent<MobiusColor>().GetNowColorNum();//松井君のスクリプトから変数取得

        PositionSum();//場所を求める

        //TimingInput = this.rythm.rythmCheckFlag;//ノーツに合わせられたかを取得
        //TimingInput = Input.GetKeyDown("joystick button 0");//ノーツに合わせられたかを取得

        //Debug.Log(TimingInput);

        RythmFlg = this.rythm.rythmCheckFlag;//リズム取得

        if (RythmFlg)//リズムのタイミングが来た
        {

            if (Controler.GetJumpButtonFlg() && !TimingInput)//ジャンプ
            {
                if (!JumpMashing)
                {
                    TimingInput = true;

                    jumpmove = 0;
                    jumpmovesave = 0;
                    jumpmove_prev = 0;

                    if (InsideFlg)//ジャンプの力をセット
                    {
                        pow = -jumppow;
                    }
                    else
                    {
                        pow = jumppow;
                    }
                }
            }

            if (RythmSaveFlg != RythmFlg)//タイミングがtrueになった瞬間
            {
                SpacePress = false;
            }

            if (Controler.GetRythmButtonFlg())//スピードアップのキー入力
            {
                if (!SpeedUpMashing)
                {
                    if (!SpacePress)//１回目のボタン入力
                    {
                        SpacePress = true;
                        Speed = UpSpeed;
                        SpeedUpFlg = true;
                    }
                    else
                    {
                        SpeedUpMashing = true;
                        SpacePress = false;
                        Speed = NormalSpeed;
                        SpeedUpFlg = false;
                    }
                }
            }



        }
        else
        {
            if (RythmSaveFlg != RythmFlg)//タイミングがfalseになった瞬間
            {

                JumpMashing = false;
                SpeedUpMashing = false;

                if (SpacePress)//キー入力があった
                {
                    Speed = UpSpeed;
                    SpeedUpFlg = true;
                }
                else//キー入力がなかった
                {
                    SpeedUpFlg = false;
                    Speed = NormalSpeed;
                }

            }

            if (Controler.GetRythmButtonFlg())//スピードアップのキー入力
            {
                SpacePress = false;
                Speed = NormalSpeed;
                SpeedUpFlg = false;
                SpeedUpMashing = true;
            }

            if (Controler.GetJumpButtonFlg())
            {
                JumpMashing = true;
            }


        }

        RythmSaveFlg = RythmFlg;//リズムセーブ

        if (TimingInput)
        {
            if (InsideFlg)//内側
            {

                if (HipDrop)
                {
                    //jumpmovesave = jumpmove;
                    //jumpmove += (jumpmove - jumpmove_prev) + pow;
                    //jumpmove_prev = jumpmovesave;

                    jumpmove += (HipDropSpeed * 100f) * Time.deltaTime;

                    if (jumpmove > 0)
                    {
                        jumpmove = 0;
                        this.rythm.checkPlayerMove = false;
                        jumpcount = 0;
                        JumpOk = true;
                        HipDrop = false;
                        TimingInput = false;
                    }
                }//if (HipDrop)
                else
                {
                    jumpmovesave = jumpmove;
                    jumpmove += (jumpmove - jumpmove_prev) + pow;
                    jumpmove_prev = jumpmovesave;

                    pow = 1;

                    if (jumpmove > jumpmovesave)
                    {
                        HipDrop = true;
                    }
                }//else

            }//if (InsideFlg)//内側
            else//外側
            {

                if (HipDrop)
                {
                    //pow = -HipDropSpeed;
                    //jumpmovesave = jumpmove;
                    //jumpmove = jumpmove + ((jumpmove - jumpmove_prev) + pow);
                    //jumpmove_prev = jumpmovesave;

                    jumpmove -= (HipDropSpeed * 100f) * Time.deltaTime;

                    if (jumpmove < 0)
                    {
                        jumpmove = 0;
                        this.rythm.checkPlayerMove = false;
                        jumpcount = 0;
                        JumpOk = true;
                        HipDrop = false;
                        TimingInput = false;
                    }
                }//if (HipDrop)
                else
                {
                    jumpmovesave = jumpmove;
                    jumpmove = jumpmove + ((jumpmove - jumpmove_prev) + pow);
                    jumpmove_prev = jumpmovesave;

                    pow = -1;

                    if (jumpmove < jumpmovesave)
                    {
                        HipDrop = true;
                    }
                }//else

            }//else//外側

        }//if (TimingInput)
        else
        {

            if (InsideFlg)//内側
            {

                if (SpeedUpFlg)
                {
                    Speed = UpSpeed * InsideSpeed;
                }
                else
                {
                    Speed = NormalSpeed * InsideSpeed;
                }

            }

            //プレイヤーの移動
            if (RotateLeftFlg)
            {
                angle += (rotateSpeed * Speed) * Time.deltaTime;
            }
            else
            {
                angle -= (rotateSpeed * Speed) * Time.deltaTime;
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







            if (MobiusCol)
            {
                counter += Time.deltaTime;
                //移ったときに元のメビウスの輪に戻らないようにカウントする
                if (counter > 1)//移り変わり制御
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
        }//else


        //Debug.Log(angle);
    }//void Update()

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(0, 1, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ

    }

    private void PositionSum()//メビウスの輪からの場所を計算
    {
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2) - InsideLength + jumpmove;// メビウスの輪の円の半径を取得
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);

    }

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

        Vector2 NowMobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;             // 現在のメビウスの輪の位置を取得
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



    public int GetNowMobiusNum()//現在の乗っているメビウスの輪の数字を返す
    {
        return NowMobius;
    }

    // 衝突時
    // private void OnTriggerEnter(Collider other)
    // private void OnCollisionEnter(Collision other)
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {

            if (!StartFlg)
            {
                if (jumpmove == 0)//ジャンプしていなければ
                {
                    if (other.GetComponent<EnemyMove>().GetNowMobiusNum() == NowMobius)//同じメビウスか
                    {
                        if (!other.GetComponent<EnemyMove>().GetStanFlg())//スタンしていないか
                        {
                            if (other.GetComponent<EnemyMove>().GetInsideFlg() == InsideFlg)//外側か内側か
                            {
                                if (CollisionOn)
                                {
                                    CollisionState = true;
                                    Debug.Log("敵と当たった");
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    // 離れた時
    //  private void OnTriggerEnter(Collider other)
    //private void OnCollisionExit(Collision other)
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            CollisionState = false;
            //Debug.Log("敵と離れた");
        }
    }



    public bool GetCollisionState()//敵と当たっているかどうかを返す
    {
        return CollisionState;
    }
    public int GetNowMobiusColor()//松井君に渡すための関数
    {
        return NowMobiusColor;
    }


    public bool GetStartFlg()
    {
        return StartFlg;
    }

    public int GetStartPoint()
    {
        return StartPoint;
    }

    public bool GetInsideFlg()
    {
        return InsideFlg;
    }

    public float GetAngle()//現在の角度を渡す
    {
        float ans = 0;
        if (RotateLeftFlg)//イレギュラー処理
        {
            
            if (InsideFlg)
            {
                ans = (angle / 2f) - 270f+100;
                //Debug.Log("1");
            }
            else
            {
                ans = (angle / 2f) - 270f+100;
                //Debug.Log("2");
            }
        }
        else
        {
            

            if (InsideFlg)
            {
                ans = (angle / 2f) - 180f;
                //Debug.Log("3");
            }
            else
            {
                ans = (angle / 2f) - 180f;
                //Debug.Log("4");
            }
        }

       

        //ans = effectangle;
        Debug.Log(ans);
        return ans;
    }

    public bool GetHipDropNow()//ヒップドロップをしたかどうか
    {

        return JumpOk;
    }

    public float GetPlayerLength()
    {
        return distanceTarget.y;
    }

    public bool GetSpeedUp()//スピードアップしているか
    {
        return SpeedUpFlg;
    }

    public bool GetJumpNow()//ジャンプしているかどうか
    {
        if (jumpmove == 0)
        {
            return false;
        }

        return true;
    }
}
