﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// プレイヤーの挙動
public class PlayerMove : MobiusOnObj
{
    public int NowMobiusColor;                              //現在のメビウスの色を返す

    public float UpSpeed;                                   //スピードアップ時のスピード格納
    bool JumpFlg;                                           //ジャンプしているかどうか
    bool StartFlg;                                          //初期位置設定用フラグ　最初の一回だけ通る
    GameObject RythmObj;                                    //リズムオブジェクト
    Rythm rythm;                                            //リズムスクリプト取得用
    public bool CollisionState;                             //当たり判定を外部に渡す変数　treu:当たっている　false:当たっていない

    float jumpmove;                                         //ジャンプの位置
    float jumpmovesave;                                     //前の処理時のジャンプの位置を保存
    float jumpmove_prev;
    float pow;                                              //ジャンプ力を計算
    public bool JumpOk;                                     //ヒップドロップが完了したかどうか　

    [SerializeField] float jumppow = 10;                    //ジャンプ力
    [SerializeField] float HipDropSpeed = 10;               //ヒップドロップスピード

    bool SpeedPress;                                        //スピードアップボタンの判定
    bool SpeedUpMashing;                                    //スピードアップが連打されているかどうか
    bool JumpMashing;                                       //ジャンプボタンが連打されているか
    public bool HipDrop;                                    //ヒップドロップ中

    bool SpeedUpFlg;                                        //スピードアップしているか

    bool RythmSaveFlg;                                      //リズムの切り替わりで判定させる
    bool RythmFlg;                                          //リズムが来ているかどうか

    Vector3 HipDropCollisionPos;                            //ヒップドロップの当たり判定場所
    Vector3 HipDropPos;                                     //ヒップドロップを行っている場所　移動バグ修正用

    AnimaterControl PlayerAnimation;                        //アニメーションのコントローラー

    bool Clear;                                             //クリアしたかどうか
    bool Stop;                                              //停止

    GameObject DushEffect;                                  //ダッシュした時のエフェクト
    GameObject SmokeEffect;                                 //ヒップドロップ時のエフェクト

    CameraShake camerashake;                                //カメラを揺らすスクリプト

    [SerializeField] GameObject missPrefab;                 //リズムに合わなかった時のUI

    [SerializeField] GameObject successPrefab;              //リズムに合った時のUI

    [SerializeField] GameObject HipDropCollisionObj;        //ヒップドロップの当たり判定

    [SerializeField] Vector3 ClearPosition;                 //クリア時の最終的な位置

    bool ClearOne;                                          //クリア時一度だけ通るフラグ　アニメーション調整してセットする用

    [SerializeField] GameObject Menu;
    [SerializeField] Vector3 PausePos;                      //ポーズ中の位置
    Quaternion InitRot;                                     //初期の回転数値
    bool MenuOnOne;                                         //メニューが呼ばれて１回だけ処理する
    bool MenuOffOne;                                        //メニューが消えたとき1回だけ処理する

    bool SaveInsideFlg;
    bool SaveRotateFlg;

    float AngleY;
    float HipDropEffectTime;
    bool Clear2Motion;//２回目のモーション
    [SerializeField] Vector3 ClearLastPos;
    float Clear2MotionTime;

    protected override void Awake()
    {
        InitRot = transform.rotation;
        InLength = 50;
        OutLength = 0;
        HipDropEffectTime = 0;
        base.Awake();

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
        PlayerAnimation = GameObject.Find("PlayerModel").GetComponent<AnimaterControl>();             //アニメーションコントローラ取得
        DushEffect = transform.GetChild(1).gameObject;                                                //ダッシュエフェクト取得
        SmokeEffect = transform.GetChild(2).gameObject;                                               //煙エフェクト取得
        camerashake = Camera.main.GetComponent<CameraShake>();                                        //カメラを揺らす取得
    }

    protected override void Start()
    {

        base.Start();

        DushEffect.SetActive(false);
        SmokeEffect.SetActive(false);

        JumpFlg = false;
        StartFlg = true;
        CollisionState = false;
        SpeedUpMashing = false;
        JumpMashing = false;
        Clear = false;
        ClearOne = false;
        Clear2Motion = false;
        Clear2MotionTime = 2.5f;

        Stop = false;
        jumpmove = 0;
        jumpmovesave = 0;
        jumpmove_prev = 0;
        HipDropSpeed = HipDropSpeed * 100f;
        SpeedPress = false;
        SpeedUpFlg = false;
        MenuOnOne = false;
        MenuOffOne = true;

        RythmFlg = this.rythm.rythmCheckFlag;
        RythmSaveFlg = RythmFlg;
        PlayerAnimation.Walk();
        NormalModel();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearOn();
        }

        NowMobiusColor = Mobius[NowMobius].GetComponent<MobiusColor>().GetNowColorNum();//現在のメビウスの色を取得

        //メニュー移動処理
        if (Menu.activeSelf == true && !Clear)
        {
            MenuOffOne = false;
            if (!Stop) PauseMove();
        }
        else
        {
            if (!MenuOffOne)
            {
                Stop = false;
                HipDrop = false;
                MenuOffOne = true;
            }

            MenuOnOne = false;
        }


        if (!Pause)
        {
            if (!Clear)
            {
                if (StartFlg)
                {
                    StartFlg = false;
                }


                if (JumpFlg)
                {
                    HipDropSum();
                }
                else
                {
                    //スピード決定
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

                    angle = AngleRangeSum(angle);


                    if (SwitchMobius)
                    {
                        float MaxCounter = 0.2f;//切り替えることができる時間

                        counter += Time.deltaTime;

                        //移ったときに元のメビウスの輪に戻らないようにカウントする
                        if (counter > MaxCounter)
                        {
                            float AngleMoveWide = 90;//移動の範囲
                            if (angle > saveangle + AngleMoveWide || angle < saveangle - AngleMoveWide)//９０度以上移動したかどうか
                            {
                                //移り変わることができるようにする
                                SaveMobius = NowMobius;
                                counter = 0;
                                SwitchMobius = false;
                            }
                        }
                    }
                    else
                    {

                        CollisonMobius();//移り先のメビウスの輪を探す

                    }
                }
                PositionSum();//場所を求める
                NormalModel();
                HipDropEffectDraw();
            }
        }
        else
        {
            DushEffect.SetActive(false);
            SmokeEffect.SetActive(false);
        }


        //クリアの動き
        if (!Stop && !CollisionState && Clear)
        {
            ClearMove();
            SmokeEffect.SetActive(false);
        }

    }

    private void Update()
    {
        if (!Pause)
        {
            if (!Clear)
            {
                if (!CollisionState)
                {
                    RythmFlg = this.rythm.rythmCheckFlag;                        //リズム取得

                    SpeedUpInput();                                              //スピードアップ入力

                    JumpInput();                                                 //ジャンプ入力

                    RythmSaveFlg = RythmFlg;                                     //リズムセーブ
                }
            }
        }

    }

    void OnDrawGizmos()//当たり判定描画
    {
        //本体の当たり判定
        Gizmos.color = new Vector4(0, 1, 0, 0.8f); //色指定
        Gizmos.DrawSphere(transform.position + transform.GetComponent<SphereCollider>().center, GetComponent<SphereCollider>().bounds.size.x / 2);

    }

    //ジャンプキー入力
    private void JumpInput()
    {
        if (RythmFlg)//リズムのタイミングが来た
        {
            if (Controler.GetJumpButtonFlg() && !JumpFlg)//ジャンプ
            {
                if (!JumpMashing)
                {

                    JumpFlg = true;

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
            }

            if (Controler.GetJumpButtonFlg())
            {
                JumpMashing = true;
                Instantiate(missPrefab);
            }

        }
    }

    //ヒップドロップの計算
    private void HipDropSum()
    {

        if (InsideFlg)//内側
        {
            if (HipDrop)
            {
                jumpmove += HipDropSpeed * Time.deltaTime;

                if (jumpmove > 0)
                {
                    HipDropEndSetState();
                }
            }
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
            }

        }
        else//外側
        {
            if (HipDrop)
            {
                jumpmove -= HipDropSpeed * Time.deltaTime;

                if (jumpmove < 0)
                {
                    HipDropEndSetState();
                }
            }
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
            }
        }
    }

    //スピードアップのキー入力
    private void SpeedUpInput()
    {

        if (RythmFlg)//リズムのタイミングが来た
        {
            if (RythmSaveFlg != RythmFlg)//タイミングがtrueになった瞬間
            {
                SpeedPress = false;
                SpeedUpMashing = false;
            }

            if (Controler.GetRythmButtonFlg())//スピードアップのキー入力
            {
                if (!SpeedUpMashing && !SpeedPress)
                {
                    //ダッシュ成功時
                    SetSpeedUp();
                    SoundManager.PlaySeName("hit");
                    Instantiate(successPrefab);
                }
                else
                {
                    //ダッシュミス時
                    SetSpeedNormal();
                    SoundManager.PlaySeName("dash_miss");
                    Instantiate(missPrefab);
                }
            }
        }
        else//リズムが合っていないとき
        {
            if (RythmSaveFlg != RythmFlg)//タイミングがfalseになった瞬間
            {
                //スピードが反映されないバグを回避用
                if (SpeedPress)//スピードアップ入力があった
                {
                    SetSpeedUp();
                }
                else//スピードアップ入力なし
                {
                    SetSpeedNormal();
                }
            }

            if (Controler.GetRythmButtonFlg())//スピードアップのキー入力
            {
                SetSpeedNormal();
                SoundManager.PlaySeName("dash_miss");
                Instantiate(missPrefab);
            }
        }
    }

    //スピードアップをセット
    private void SetSpeedUp()
    {
        SpeedPress = true;
        Speed = UpSpeed;
        SpeedUpFlg = true;

        PlayerAnimation.Run();
        DushEffect.SetActive(true);

    }

    //スピードを普通にセット
    private void SetSpeedNormal()
    {
        SpeedPress = false;
        Speed = NormalSpeed;
        SpeedUpFlg = false;
        SpeedUpMashing = true;
        PlayerAnimation.Walk();
        DushEffect.SetActive(false);

    }

    //ヒップドロップが終わったときのステータスセット
    private void HipDropEndSetState()
    {
        HipDropCollisionObj.GetComponent<HipDropCol>().EnemyStanOn();
        jumpmove = 0;
        this.rythm.checkPlayerMove = false;
        JumpOk = true;
        HipDrop = false;
        JumpFlg = false;
        SmokeEffect.SetActive(true);
        HipDropEffectTime = 1;
        camerashake.OnShake();
        SoundManager.PlaySeName("hipdrop");
    }


    //現在のメビウスから位置を計算
    protected override void PositionSum()
    {
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength + jumpmove;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);


    }



    //プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    private void CollisonMobius()
    {
        bool MobiusCollision = false;

        for (int i = 0; i < Mobius.Length; i++)
        {
            if (i == NowMobius) continue;   //現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;  //前のメビウスを飛ばす

            //メビウス同士当たっているかどうか
            MobiusCollision = CollisionSphere(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                  // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                      // 次ののメビウスの輪の位置を取得
                                  Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2);      // 円の半径を取得

            if (MobiusCollision)
            {
                bool switching = MobiusSwitch(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                  // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                      // 次ののメビウスの輪の位置を取得
                                  this.GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2);                   // プレイヤーの半径を取得

                if (switching)
                {
                    //切り替えたときのステータスをセット
                    SwitchingSetStatus(i);
                }
            }
        }

    }


    //クリア時の移動処理
    private void ClearMove()
    {

        if (!Clear2Motion)//ヒップドロップ処理
        {
            Clear2MotionTime -= Time.deltaTime;
            if (Clear2MotionTime < 0f)
            {
                Clear2MotionTime = -1;
                Clear2Motion = true;
            }

            if (!HipDrop)//移動させる
            {
                angle = 0;
                InsideFlg = false;
                RotateLeftFlg = false;
                PositionSum();
                HipDrop = true;
                transform.position = new Vector3(0, 100, ClearPosition.z);
                NormalModel();
            }
            else//ヒップドロップ中
            {
                if (!ClearOne)
                {
                    //アニメーションをセット
                    PlayerAnimation.HipDrop();
                    ClearOne = true;

                }


                Vector3 vec = ClearPosition - transform.position;
                vec = vec.normalized;
                if (vec.y < 0)
                {
                    transform.position += vec * 5.0f;
                }
                if (Clear2MotionTime < 2.0f)
                {
                    PlayerAnimation.GameClearRightVer();
                }
            }
        }
        else//左に移動処理
        {
            Vector3 vec = ClearLastPos - this.transform.position;
            vec = vec.normalized;


            if (vec.x < 0)//移動中
            {
                this.transform.position += new Vector3(vec.x, 0, 0);
                PlayerAnimation.Wait();
                this.transform.Rotate(0, 5, 0);
            }
            else//移動が終われば
            {
                Stop = true;


            }
        }
        DushEffect.SetActive(false);
        SmokeEffect.SetActive(false);

    }

    private void PauseMove()
    {

        PauseModel();
        if (!HipDrop)//移動させる
        {
            HipDrop = true;
            transform.position = new Vector3(PausePos.x, PausePos.y+50, PausePos.z);
        }
        else//ヒップドロップ中
        {
            Vector3 vec = PausePos - transform.position;
            vec = vec.normalized;

            transform.position += vec * 5.0f;
            if (vec.y < 0)
            {
                Stop = true;
                PlayerAnimation.Wait();
            }
        }

    }


    //敵と当たっているかどうかを返す
    public bool GetCollisionState()
    {
        return CollisionState;
    }

    public void SetCollisionState()
    {
        CollisionState = true;
    }

    //色のラベルを返す
    public int GetNowMobiusColor()
    {
        return NowMobiusColor;
    }


    public float GetModelAngle()
    {
        return angle;
    }

    //エフェクト用の角度
    public float GetAngle()
    {
        float ans = 0;
        if (RotateLeftFlg)//イレギュラー処理　角度調整
        {
            ans = (angle / 2f) + 180f;
        }
        else
        {
            ans = (angle / 2f) - 180f;
        }

        return ans;
    }

    //ヒップドロップが終わった
    public bool GetHipDropNow()
    {
        return JumpOk;
    }

    public bool GetRotateLeftFlg()
    {
        return RotateLeftFlg;
    }
    //ヒップドロップ中かどうか
    public bool GetHipDrop()
    {
        return HipDrop;
    }

    //ジャンプしているかどうか
    public bool GetJumpNow()
    {
        if (jumpmove == 0)
        {
            return false;
        }

        return true;
    }

    //クリアの処理を行う
    public void ClearOn()
    {

        HipDropCollisionObj.GetComponent<HipDropCol>().SetClear();
        if (!Clear)
        {
            PlayerAnimation.HipDrop();
            Clear = true;
        }
    }

    //クリアの動きが終わったかどうか
    public bool GetStop()
    {
        return Stop;
    }

    //ヒップドロップ中の位置
    public Vector3 GetHipDropPos()
    {
        return HipDropPos;
    }

    public bool GetPause()
    {
        return Pause;
    }


    void NormalModel()
    {
        float InsideAngleSum = 0f;
        if (GetInsideFlg())
        {
            InsideAngleSum = 180f;
        }
        else
        {
            InsideAngleSum = 0f;
        }

        if (GetRotateLeftFlg())
        {
            this.transform.eulerAngles = new Vector3(0, 180, 360f - GetModelAngle() + InsideAngleSum);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, GetModelAngle() + InsideAngleSum);
        }

        if (AngleY < InsideAngleSum)
        {
            AngleY += 10;
        }
        else if (AngleY > InsideAngleSum)
        {
            AngleY -= 10;
        }

        this.transform.Rotate(0, InsideAngleSum, 0);
    }

    void PauseModel()
    {
        transform.eulerAngles = new Vector3(0f, 90f, 0f);
    }

    void HipDropEffectDraw()
    {
        if (HipDropEffectTime > 0)
        {
            HipDropEffectTime += Time.deltaTime;

            if (HipDropEffectTime > 1.5f)
            {
                SmokeEffect.SetActive(false);
                HipDropEffectTime = 0;
            }
        }
    }
}
