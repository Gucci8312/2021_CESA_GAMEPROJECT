using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MobiusOnObj
{
    
    GameObject player;
    
    
    [SerializeField] private float rotateSpeed = 180f;
    
    public float StanTime = 1;              //スタン時の時間格納
    float StanTimeCount;                    //スタン時のカウント
    bool Stan;                              //スタン中かどうか
    GameObject RythmObj;                    //リズムオブジェクト
    Rythm rythm;                            //リズムスクリプト取得用
    GameObject ball;                        //子オブジェクトのトゲなし
    GameObject toge;                        //子オブジェクトのトゲあり
    bool TogeFlg;                           //トゲのフラグ

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Player");                                                         //プレイヤーオブジェクト取得
        RythmObj = GameObject.Find("rythm_circle");                                                 //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                //リズムのコード
        ball = transform.GetChild(0).gameObject;                                                    //ボールオブジェクト取得
        toge = transform.GetChild(1).gameObject;                                                    //トゲオブジェクト取得
    }

    protected override void Start()
    {
        base.Start();
        TogeFlg = false;
        Stan = false;
        StanTimeCount = 0;
    }


    // Update is called once per frame  
    void Update()
    {
        TogeFlg = rythm.rythmCheckFlag;

        //リズムに合わせてトゲを出し入れする
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
        
        PositionSum();

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
            //外内で速度調整
            if (InsideFlg)
            {
                Speed = NormalSpeed * InsideSpeed;
            }
            else
            {
                Speed = NormalSpeed;
            }

            //移動計算
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
            if (counter > 0.2)
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
            CollisonMobius();//移り先のメビウスの輪を探す
        }


    }//void Update()

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(1, 0, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ
    }



    //プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    private void CollisonMobius()
    {
        bool sts = false;

        for (int i = 0; i < Mobius.Length; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            //メビウス同士当たっているかどうか
            sts = CollisionSphere(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                           // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                   // 次ののメビウスの輪の位置を取得
                                  Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 +10);// 円の半径を取得

            if (sts)
            {
                //切り替えることができたか
                bool switching = MobiusSwitch(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                               // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                   // 次ののメビウスの輪の位置を取得
                                  this.GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2+10);                // プレイヤーの半径を取得

                if (switching)
                {
                    //切り替えたステータスをセット
                    SwitchingSetStatus(i);
                    break;
                }
            }
        }

    }

    //スタンしているかどうか
    public bool GetStanFlg()
    {
        return Stan;
    }

    //スタン状態にする
    public void StanOn()
    {
        StanTimeCount = 0;
        Stan = true;
    }
    
}
