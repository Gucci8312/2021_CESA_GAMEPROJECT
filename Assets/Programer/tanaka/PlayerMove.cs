using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// プレイヤーの挙動
public class PlayerMove : MobiusOnObj
{
    // Start is called before the first frame update
    public int NowMobiusColor;                                                                      //松井君に渡す用　MobiusuColorから取得した変数格納

    public float UpSpeed;
    bool JumpFlg;
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

    [SerializeField] private float rotateSpeed = 180f;//回転速度
    

    bool SpacePress;//スピードアップボタンの判定
    bool SpeedUpMashing;//スピードアップが連打されているかどうか
    bool JumpMashing;//ジャンプボタンが連打されているか
    public bool HipDrop;//ヒップドロップ中

    bool SpeedUpFlg;//スピードアップしているか

    bool RythmSaveFlg;//リズムの切り替わりで判定させる
    bool RythmFlg;//リズムが来ているかどうか

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

    [SerializeField]
    GameObject HipDropCollisionObj;

    bool ClearOne;

    protected override void Awake()
    {
        base.Awake();

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
        PlayerAnimation = GameObject.Find("PlayerModel").GetComponent<AnimaterControl>();
        DushEffect = transform.GetChild(1).gameObject;
        SmokeEffect = transform.GetChild(2).gameObject;
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
        Stop = false;

        jumpmove = 0;
        jumpmovesave = jumpmove;
        jumpcount = 0;

        SpacePress = false;

        SpeedUpFlg = false;
        GameOver = false;

        RythmFlg = this.rythm.rythmCheckFlag;
        RythmSaveFlg = RythmFlg;


        cam = Camera.main;
        camerashake = cam.GetComponent<CameraShake>();
    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {


        NowMobiusColor = Mobius[NowMobius].GetComponent<MobiusColor>().GetNowColorNum();//現在のメビウスの色を取得

        if (Time.timeScale != 0)
        {

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

                    SpeedUpInput();//スピードアップ入力
                    JumpInput();//ジャンプ入力


                    RythmSaveFlg = RythmFlg;//リズムセーブ
                }


                if (JumpFlg)
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
                                JumpFlg = false;
                                SmokeEffect.SetActive(true);
                                HipDropCollisionObj.GetComponent<HipDropCol>().HipDropFlg = false;
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
                                HipDropCollisionObj.GetComponent<HipDropCol>().HipDropFlg = true;
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
                                JumpFlg = false;
                                SmokeEffect.SetActive(true);
                                HipDropCollisionObj.GetComponent<HipDropCol>().HipDropFlg = false;
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
                                HipDropCollisionObj.GetComponent<HipDropCol>().HipDropFlg = true;
                            }
                        }//else

                    }//else//外側



                }//if (JumpFlg)
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

                    if (SwitchMobius)
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
                                SwitchMobius = false;
                            }
                        }

                    }
                    else
                    {
                        if (jumpmove == 0)
                        {
                            CollisonMobius();//移り先のメビウスの輪を探す
                        }
                    }
                }//else

            }
            else
            {
                if (!Stop && !CollisionState)
                {
                    ClearMove();
                }
            }

        }

    }//void Update()

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(0, 1, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ

        Gizmos.color = new Vector4(0, 0, 1, 0.5f); //色指定
        Gizmos.DrawSphere(HipDropCollisionObj.GetComponent<SphereCollider>().center, HipDropCollisionObj.GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ
    }

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

    private void SpeedUpInput()
    {

        if (RythmFlg)//リズムのタイミングが来た
        {
            if (RythmSaveFlg != RythmFlg)//タイミングがtrueになった瞬間
            {
                SpacePress = false;
                SpeedUpMashing = false;
                SmokeEffect.SetActive(false);
            }

            if (Controler.GetRythmButtonFlg())//スピードアップのキー入力
            {
                if (!SpeedUpMashing && !SpacePress)
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
                if (SpacePress)//スピードアップ入力があった
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
        SpacePress = true;
        Speed = UpSpeed;
        SpeedUpFlg = true;

        PlayerAnimation.Run();
        DushEffect.SetActive(true);

    }

    //スピードを普通にセット
    private void SetSpeedNormal()
    {
        SpacePress = false;
        Speed = NormalSpeed;
        SpeedUpFlg = false;
        SpeedUpMashing = true;

        PlayerAnimation.Walk();
        DushEffect.SetActive(false);

    }

    protected override void PositionSum()
    {
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength+jumpmove;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);

        //ヒップドロップの当たる場所計算
        float SumNum = 0;

        Vector3 len = new Vector3(0, 0, 0);
        //メビウスの輪の中心とプレイヤーの距離を求める
        len.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength + jumpmove - SumNum;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        HipDropCollisionPos = target.position + Quaternion.Euler(0f, 0f, angle) * len;
    }




    private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    {
        bool sts = false;

        for (int i = 0; i < Mobius.Length; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            //メビウス同士当たっているかどうか
            sts = CollisionSphere(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                           // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                   // 次ののメビウスの輪の位置を取得
                                  Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2+10);// 円の半径を取得

            if (sts)
            {
                bool switching = MobiusSwitch(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                               // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                   // 次ののメビウスの輪の位置を取得
                                  this.GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2);                // プレイヤーの半径を取得

                if (switching)
                {
                    SwitchingSetStatus(i);
                }
            }
        }

    }



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
                RotateLeftFlg = false;
                PositionSum();
                HipDrop = true;
                transform.position = new Vector3(0, 100, -485);

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
            transform.position = new Vector3(0, y, -485);

            if (y < 7.5f)
            {
                Stop = true;


                PlayerAnimation.GameClearRightVer();

            }
        }

        DushEffect.SetActive(false);
        SmokeEffect.SetActive(false);

    }

    
    // 衝突時
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            if (!Clear && !StartFlg && jumpmove == 0)
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
    

    public bool GetCollisionState()//敵と当たっているかどうかを返す
    {
        return CollisionState;
    }
    public int GetNowMobiusColor()//松井君に渡すための関数
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
            if (InsideFlg)
            {
                ans = (angle / 2f) + 180f;
            }
            else
            {
                ans = (angle / 2f) + 180f;
            }
        }
        else
        {
            if (InsideFlg)
            {
                ans = (angle / 2f) - 180f;
            }
            else
            {
                ans = (angle / 2f) - 180f;
            }
        }

        return ans;
    }

    public bool GetHipDropNow()//ヒップドロップが終わった
    {
        return JumpOk;
    }

    public bool GetRotateLeftFlg()
    {
        return RotateLeftFlg;
    }

    public bool GetHipDrop()//ヒップドロップ中かどうか
    {
        return HipDrop;
    }


    public bool GetJumpNow()//ジャンプしているかどうか
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
}
