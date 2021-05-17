﻿using System.Collections;
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

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    [SerializeField, Range(0, 7)] public int StartPoint;                                            //メビウス上の点の番号

    bool StartFlg;                                                                                  //初期位置設定用フラグ　最初の一回だけ通る
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用


    public bool CollisionState;                                                                             //当たり判定を外部に渡す変数　treu:当たっている　false:当たっていない


    float jumpmove;//ジャンプの位置
    float jumpmovesave;

    float jumpmove_prev;
    float pow;//jumppowをを保存した状態で使う変数

    float jumpcount;//ジャンプ処理の時間
    public bool JumpOk;//ヒップドロップが完了したかどうか　松井君に渡す用

    [SerializeField] float jumppow = 10;//ジャンプ力
    [SerializeField] float HipDropSpeed = 10;//ヒップドロップスピード

    public bool CollisionOn = true;//敵との当たり判定

    private Transform target;//現在のメビウスのトランスフォーム
    public float angle;//現在のメビウスからのプレイヤーの角度
    float saveangle;
    [SerializeField] private float rotateSpeed = 180f;//回転速度
    private Vector3 distanceTarget = new Vector3(0f, 0f, 0f);//メビウスからの距離
    bool MobiusCol;//メビウス同士の当たり判定


    bool SpacePress;//スピードアップボタンの判定
    bool SpeedUpMashing;//スピードアップが連打されているかどうか
    bool JumpMashing;//ジャンプボタンが連打されているか
    public bool HipDrop;//ヒップドロップ中

    bool SpeedUpFlg;//スピードアップしているか

    bool RythmSaveFlg;//リズムの切り替わりで判定させる
    bool RythmFlg;//リズムが来ているかどうか

    public float HipDropColPos = 10;//ヒップドロップの当たり判定位置の調整用
    public float HipDropColLength = 10;//ヒップドロップの当たり判定の半径
    Vector3 HipDropCollisionPos;//ヒップドロップの当たり判定場所
    Vector3 HipDropPos;//ヒップドロップを行っている場所　松井君の移動バグ修正用

    AnimaterControl PlayerAnimation;//アニメーションのコントローラー

    bool Clear;//クリアしたかどうか
    bool Stop;//停止

    bool GameOver;//ゲームオーバー

    GameObject DushEffect;
    GameObject SmokeEffect;


    Camera cam;
    CameraShake camerashake;

    [SerializeField]
    GameObject missPrefab;

    [SerializeField]
    GameObject successPrefab;

    bool ClearOne;

    private void OnValidate()
    {
        HipDropCollisionPos = new Vector3(this.transform.position.x, this.transform.position.y - HipDropColPos, this.transform.position.z);
    }


    void Start()
    {
        Mobius = GameObject.FindGameObjectsWithTag("Mobius");

        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }


        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード

        PlayerAnimation = GameObject.Find("PlayerModel").GetComponent<AnimaterControl>();

        DushEffect = transform.GetChild(1).gameObject;
        SmokeEffect = transform.GetChild(2).gameObject;

        DushEffect.SetActive(false);
        SmokeEffect.SetActive(false);

        SideCnt = 2;
        SaveMobius = -1;
        TimingInput = false;
        StartFlg = true;
        counter = -1;
        CollisionState = false;
        SpeedUpMashing = false;
        JumpMashing = false;
        Clear = false;
        ClearOne = false;
        Stop = false;

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
        MobiusCol = false;
        jumpmove = 0;
        jumpmovesave = jumpmove;
        jumpcount = 0;

        Speed = NormalSpeed;
        SpacePress = false;

        SpeedUpFlg = false;
        GameOver = false;

        RythmFlg = this.rythm.rythmCheckFlag;
        RythmSaveFlg = RythmFlg;

        PositionSum();

        cam = Camera.main;
        camerashake = cam.GetComponent<CameraShake>();

    }



    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {


        NowMobiusColor = Mobius[NowMobius].GetComponent<MobiusColor>().GetNowColorNum();//松井君のスクリプトから変数取得

        if (!Clear)
        {
            if (StartFlg)
            {
                StartFlg = false;

            }

            PositionSum();//場所を求める

            if (!CollisionState)
            {


                RythmFlg = this.rythm.rythmCheckFlag;//リズム取得

                if (RythmFlg)//リズムのタイミングが来た
                {

                    if (RythmSaveFlg != RythmFlg)//タイミングがtrueになった瞬間
                    {
                        SpacePress = false;
                        SmokeEffect.SetActive(false);
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

                                PlayerAnimation.Run();
                                Instantiate(successPrefab);
                            }
                            else
                            {

                                SpacePress = false;
                                Speed = NormalSpeed;
                                SpeedUpFlg = false;

                                SpeedUpMashing = true;
                                //PlayerAnimation.Walk();

                            }
                        }
                        else
                        {
                            Instantiate(missPrefab);
                        }
                    }

                    if (Controler.GetJumpButtonFlg() && !TimingInput)//ジャンプ
                    {
                        if (!JumpMashing)
                        {

                            TimingInput = true;

                            jumpmove = 0;
                            jumpmovesave = 0;
                            jumpmove_prev = 0;

                            PlayerAnimation.HipDrop();

                            Instantiate(successPrefab);

                            if (InsideFlg)//ジャンプの力をセット
                            {
                                pow = -jumppow;
                            }
                            else
                            {
                                pow = jumppow;
                            }
                        }
                        else
                        {
                            Instantiate(missPrefab);
                        }
                    }

                }
                else
                {
                    if (RythmSaveFlg != RythmFlg)//タイミングがfalseになった瞬間
                    {

                        JumpMashing = false;
                        SpeedUpMashing = false;

                        if (SpacePress)//スピードアップ入力があった
                        {
                            Speed = UpSpeed;
                            SpeedUpFlg = true;
                            DushEffect.SetActive(true);
                        }
                        else//スピードアップ入力なし
                        {
                            SpeedUpFlg = false;
                            Speed = NormalSpeed;
                            PlayerAnimation.Walk();
                            DushEffect.SetActive(false);

                        }


                    }

                    if (Controler.GetRythmButtonFlg())//スピードアップのキー入力
                    {
                        SpacePress = false;
                        Speed = NormalSpeed;
                        SpeedUpFlg = false;
                        SpeedUpMashing = true;

                        PlayerAnimation.Walk();
                        DushEffect.SetActive(false);
                        Instantiate(missPrefab);

                    }

                    if (Controler.GetJumpButtonFlg())
                    {
                        JumpMashing = true;
                        Instantiate(missPrefab);
                    }


                }

                RythmSaveFlg = RythmFlg;//リズムセーブ
            }


            if (TimingInput)
            {
                if (InsideFlg)//内側
                {

                    if (HipDrop)
                    {

                        jumpmove += (HipDropSpeed * 100f) * Time.deltaTime;

                        camerashake.OnShake();

                        if (jumpmove > 0)
                        {
                            jumpmove = 0;
                            this.rythm.checkPlayerMove = false;
                            jumpcount = 0;
                            JumpOk = true;
                            HipDrop = false;
                            TimingInput = false;
                            SmokeEffect.SetActive(true);
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

                        jumpmove -= (HipDropSpeed * 100f) * Time.deltaTime;

                        camerashake.OnShake();

                        if (jumpmove < 0)
                        {
                            jumpmove = 0;
                            this.rythm.checkPlayerMove = false;
                            jumpcount = 0;
                            JumpOk = true;
                            HipDrop = false;
                            TimingInput = false;
                            SmokeEffect.SetActive(true);
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

                if (jumpmove == 0)//ヒップドロップの場所
                {
                    HipDropPos = this.transform.position;
                }

                //プレイヤーの移動
                if (!Stop)
                {
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



                if (MobiusCol)
                {
                    counter += Time.deltaTime;
                    //移ったときに元のメビウスの輪に戻らないようにカウントする
                    if (counter > 0.2)//移り変わり制御
                    {
                        if (angle > saveangle + 90 || angle < saveangle - 90)
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
            }//else

        }
        else
        {
            if (!Stop)
            {
                ClearMove();
            }
        }



    }//void Update()

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(0, 1, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ

        Gizmos.color = new Vector4(0, 0, 1, 0.5f); //色指定
        Gizmos.DrawSphere(HipDropCollisionPos, HipDropColLength); //中心点とサイズ
    }

    private void PositionSum()//メビウスの輪からの場所を計算
    {
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength + jumpmove;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);


        //ヒップドロップの当たる場所計算
        float SumNum = 0;
        if (InsideFlg)
        {
            SumNum = -HipDropColPos;
        }
        else
        {
            SumNum = HipDropColPos;
        }
        Vector3 len = new Vector3(0, 0, 0);
        //メビウスの輪の中心とプレイヤーの距離を求める
        len.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength + jumpmove - SumNum;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        HipDropCollisionPos = target.position + Quaternion.Euler(0f, 0f, angle) * len;
    }


    private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    {

        Vector2 NowMobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;             // 現在のメビウスの輪の位置を取得
        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得

        Vector2 NowVec = NowMobiusPos - PlayerPos;
        float NowLength = Mathf.Sqrt(NowVec.x * NowVec.x + NowVec.y * NowVec.y);

        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;
        float PlayerHankei = this.GetComponent<SphereCollider>().bounds.size.x / 2 +               // プレイヤーの半径を取得
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


                if (CollisonAngle < NowAngle + 10 && CollisonAngle > NowAngle - 10)//瞬間移動バグを修正
                {
                    SaveMobius = NowMobius;
                    NowMobius = i;
                    counter = 0;
                    angle += 180;
                    //角度の範囲を指定(0～360)
                    if (angle > 360)
                    {
                        angle = angle - 360;

                    }
                    if (angle < 0)
                    {
                        angle = angle + 360;

                    }


                    if (SideCnt >= 2)//2回切り替えると
                    {
                        //内側と外側を反転させる
                        if (InsideFlg)
                        {
                            InsideFlg = false;
                            InsideLength = 0;//内側までの距離

                        }
                        else
                        {
                            InsideLength = 50;//内側までの距離
                            InsideFlg = true;
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
                    saveangle = angle;

                    //角度の範囲を指定(0～360)
                    if (saveangle > 360)
                    {
                        saveangle = saveangle - 360;

                    }
                    if (angle < 0)
                    {
                        saveangle = saveangle + 360;

                    }
                    SideCnt++;
                    MobiusCol = true;
                    break;
                }//if ((hankei/3)+(InsideLength/2) > NextLength)//プレイヤーと移り先のメビウスの輪が当たった


            }//if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定

        }//for (int i = 0; i < Mobius.Length; i++)

    }//private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定


    private void ClearMove()
    {
        if (!HipDrop)//ジャンプ中
        {
            jumpmovesave = jumpmove;
            jumpmove = jumpmove + ((jumpmove - jumpmove_prev) + pow);
            jumpmove_prev = jumpmovesave;

            pow = -1;

            PositionSum();

            if (jumpmove < jumpmovesave)
            {
                angle = 0;
                InsideFlg = false;
                PositionSum();
                HipDrop = true;
                transform.position = new Vector3(0, 100, -445);
                
            }
        }
        else//ヒップドロップ中
        {

            if (!ClearOne)
            {
                PlayerAnimation.HipDrop();
                ClearOne = true;
            }


            float y = transform.position.y;
            y -= (HipDropSpeed * 15f) * Time.deltaTime;
            transform.position = new Vector3(0, y, -445);
            
            if (y < 0)
            {
                Stop = true;
                

                PlayerAnimation.GameClearRightVer();

            }
        }

        DushEffect.SetActive(false);
        SmokeEffect.SetActive(false);
        //transform.position = new Vector3(0, 0, -445);
        //this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //Stop = true;
    }

    public bool HipDropCollision(Vector3 pos, float collength)
    {
        if (jumpmove == 0)
        {
            return false;
        }


        float x, y, z;

        x = Mathf.Pow(pos.x - HipDropCollisionPos.x, 2);
        y = Mathf.Pow(pos.y - HipDropCollisionPos.y, 2);
        z = Mathf.Pow(pos.z - HipDropCollisionPos.z, 2);

        if (x + y + z <= Mathf.Pow(HipDropColLength + collength, 2))//当たっていたら
        {
            return true;
        }

        return false;
    }

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
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            if (!Clear)
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

                                        DushEffect.SetActive(false);
                                        SmokeEffect.SetActive(false);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
    }

    // 離れた時
    private void OnTriggerExit(Collider other)
    {

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
                ans = (angle / 2f) + 180f;
                //Debug.Log("1");
            }
            else
            {
                ans = (angle / 2f) + 180f;
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

    public void ClearOn()
    {
        if (!Clear)
        {
            jumppow = 40;
            if (InsideFlg)//ジャンプの力をセット
            {
                pow = -jumppow;
            }
            else
            {
                pow = jumppow;
            }
            PlayerAnimation.HipDrop();

            saveangle = angle;
            Clear = true;
        }
    }

    public bool GetStop()
    {
        return Stop;
    }

    public Vector3 GetHipDropPos()
    {
        return HipDropPos;
    }
}
