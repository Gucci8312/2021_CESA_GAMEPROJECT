using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// プレイヤーの挙動
public class PlayerMove : MobiusOnObj
{
    public int NowMobiusColor;                                                                      //現在のメビウスの色を返す

    public float UpSpeed;                                                                           //スピードアップ時のスピード格納
    bool JumpFlg;                                                                                   //ジャンプしているかどうか
    bool StartFlg;                                                                                  //初期位置設定用フラグ　最初の一回だけ通る
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用
    public bool CollisionState;                                                                     //当たり判定を外部に渡す変数　treu:当たっている　false:当たっていない

    float jumpmove;                                                                                 //ジャンプの位置
    float jumpmovesave;                                                                             //前の処理時のジャンプの位置を保存
    float jumpmove_prev;
    float pow;                                                                                      //ジャンプ力を計算
    public bool JumpOk;                                                                             //ヒップドロップが完了したかどうか　

    [SerializeField] float jumppow = 10;                                                            //ジャンプ力
    [SerializeField] float HipDropSpeed = 10;                                                       //ヒップドロップスピード

    public bool CollisionOn = true;                                                                 //敵との当たり判定

    [SerializeField] private float rotateSpeed = 180f;                                              //回転速度


    bool SpeedPress;                                                                                //スピードアップボタンの判定
    bool SpeedUpMashing;                                                                            //スピードアップが連打されているかどうか
    bool JumpMashing;                                                                               //ジャンプボタンが連打されているか
    public bool HipDrop;                                                                            //ヒップドロップ中

    bool SpeedUpFlg;                                                                                //スピードアップしているか

    bool RythmSaveFlg;                                                                              //リズムの切り替わりで判定させる
    bool RythmFlg;                                                                                  //リズムが来ているかどうか

    Vector3 HipDropCollisionPos;                                                                    //ヒップドロップの当たり判定場所
    Vector3 HipDropPos;                                                                             //ヒップドロップを行っている場所　移動バグ修正用

    AnimaterControl PlayerAnimation;                                                                //アニメーションのコントローラー

    bool Clear;                                                                                     //クリアしたかどうか
    bool Stop;                                                                                      //停止

    GameObject DushEffect;                                                                          //ダッシュした時のエフェクト
    GameObject SmokeEffect;                                                                         //ヒップドロップ時のエフェクト


    Camera cam;                                                                                     //カメラ
    CameraShake camerashake;                                                                        //カメラを揺らすスクリプト

    [SerializeField]
    GameObject missPrefab;                                                                          //リズムに合わなかった時のUI

    [SerializeField]
    GameObject successPrefab;                                                                       //リズムに合った時のUI

    [SerializeField]
    GameObject HipDropCollisionObj;                                                                 //ヒップドロップの当たり判定

    [SerializeField]
    Vector3 ClearPosition;                                                                          //クリア時の最終的な位置

    bool ClearOne;//クリア時一度だけ通るフラグ　アニメーション調整してセットする用



    protected override void Awake()
    {
        base.Awake();

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
        PlayerAnimation = GameObject.Find("PlayerModel").GetComponent<AnimaterControl>();             //アニメーションコントローラ取得
        DushEffect = transform.GetChild(1).gameObject;                                                //ダッシュエフェクト取得
        SmokeEffect = transform.GetChild(2).gameObject;                                               //煙エフェクト取得
        cam = Camera.main;                                                                            //カメラ取得
        camerashake = cam.GetComponent<CameraShake>();                                                //カメラを揺らす取得
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

        HipDropSpeed = HipDropSpeed * 100f;

        SpeedPress = false;

        SpeedUpFlg = false;

        RythmFlg = this.rythm.rythmCheckFlag;
        RythmSaveFlg = RythmFlg;
    }

    // Update is called once per frame
    void Update()
    {

        NowMobiusColor = Mobius[NowMobius].GetComponent<MobiusColor>().GetNowColorNum();//現在のメビウスの色を取得

        if (!Pause)
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
                    RythmFlg = this.rythm.rythmCheckFlag;                        //リズム取得
                    SpeedUpInput();                                              //スピードアップ入力
                    JumpInput();                                                 //ジャンプ入力
                    RythmSaveFlg = RythmFlg;                                     //リズムセーブ
                }

                HipDropCollisionObj.GetComponent<BoxCollider>().enabled = false; //ヒップドロップの当たり判定消す

                if (JumpFlg)
                {
                    if (InsideFlg)//内側
                    {
                        if (HipDrop)
                        {
                            HipDropCollisionObj.GetComponent<BoxCollider>().enabled = true;
                            jumpmove += HipDropSpeed * Time.deltaTime;

                            camerashake.OnShake();

                            if (jumpmove > 0)
                            {
                                jumpmove = 0;
                                this.rythm.checkPlayerMove = false;
                                JumpOk = true;
                                HipDrop = false;
                                JumpFlg = false;
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
                            HipDropCollisionObj.GetComponent<BoxCollider>().enabled = true;
                            jumpmove -= HipDropSpeed * Time.deltaTime;

                            camerashake.OnShake();

                            if (jumpmove < 0)
                            {
                                jumpmove = 0;
                                this.rythm.checkPlayerMove = false;
                                JumpOk = true;
                                HipDrop = false;
                                JumpFlg = false;
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

                    AngleRangeSum(angle);

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
        //本体の当たり判定
        Gizmos.color = new Vector4(0, 1, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ

        //ヒップドロップの当たり判定
        Gizmos.color = new Vector4(0, 0, 1, 0.5f); //色指定
        Gizmos.DrawCube(HipDropCollisionObj.GetComponent<BoxCollider>().center, HipDropCollisionObj.GetComponent<BoxCollider>().size); //中心点とサイズ
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

    //スピードアップのキー入力
    private void SpeedUpInput()
    {

        if (RythmFlg)//リズムのタイミングが来た
        {
            if (RythmSaveFlg != RythmFlg)//タイミングがtrueになった瞬間
            {
                SpeedPress = false;
                SpeedUpMashing = false;
                SmokeEffect.SetActive(false);
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

    //ジャンプの移動
    private void JumpMoveSum()
    {
        jumpmovesave = jumpmove;
        jumpmove = jumpmove + ((jumpmove - jumpmove_prev) + pow);
        jumpmove_prev = jumpmovesave;
        pow = -1;
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

        //ヒップドロップの当たる場所計算
        float SumNum = 0;

        Vector3 len = new Vector3(0, 0, 0);
        //メビウスの輪の中心とプレイヤーの距離を求める
        len.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength + jumpmove - SumNum;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        HipDropCollisionPos = target.position + Quaternion.Euler(0f, 0f, angle) * len;
    }



    //プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    private void CollisonMobius()
    {
        bool MobiusCollision = false;

        for (int i = 0; i < Mobius.Length; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            //メビウス同士当たっているかどうか
            MobiusCollision = CollisionSphere(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                  // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                      // 次ののメビウスの輪の位置を取得
                                  Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10);   // 円の半径を取得

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
                //アニメーションをセット
                PlayerAnimation.HipDrop();
                ClearOne = true;
            }

            float ClearHipDropSpeed = 15.0f;
            float y = transform.position.y;
            y -= (HipDropSpeed * ClearHipDropSpeed) * Time.deltaTime;
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
