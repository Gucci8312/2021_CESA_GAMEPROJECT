using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum EnemyType
{
    Normal,
    Adult,
    Larvae,
}

public class EnemyMove : MobiusOnObj
{
    
    public int type;

    protected GameObject player;                             //プレイヤーオブジェクト

    public float StanTime = 1;                               //スタン時の時間格納
    float StanTimeCount;                                     //スタン時のカウント
    protected bool Stan;                                               //スタン中かどうか
    GameObject ball;                                         //子オブジェクトのトゲなし
    GameObject toge;                                         //子オブジェクトのトゲあり
    protected bool AlertCollision;                           //アラートが出る範囲にいる

    protected override void Awake()
    {
        InLength = 35;
        OutLength = 0;
        base.Awake();
        player = GameObject.Find("Player");                  //プレイヤーオブジェクト取得
        ball = transform.GetChild(0).gameObject;             //ボールオブジェクト取得
        toge = transform.GetChild(1).gameObject;             //トゲオブジェクト取得
        type = (int)EnemyType.Normal;
    }

    protected override void Start()
    {
        base.Start();
        Stan = false;
        StanTimeCount = 0;
        AlertCollision = false;
    }


    // Update is called once per frame  
    void Update()
    {
        if (!Pause)
        {
            ball.SetActive(false);
            toge.SetActive(true);
            
            PositionSum();

            if (Stan)//スタン中
            {
                ball.SetActive(true);
                toge.SetActive(false);
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

            angle=AngleRangeSum(angle);


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
    }

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(1, 0, 0, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ
    }



    //プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    protected void CollisonMobius()
    {
        bool MobiusCollision = false;

        for (int i = 0; i < Mobius.Length; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            //メビウス同士当たっているかどうか
            MobiusCollision = CollisionSphere(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                 // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                     // 次ののメビウスの輪の位置を取得
                                  Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 -2);  // 円の半径を取得

            if (MobiusCollision)
            {
                //切り替えることができたか
                bool switching = MobiusSwitch(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center,                                                 // 現在のメビウスの輪の位置を取得
                                  Mobius[i].GetComponent<SphereCollider>().bounds.center,                                                                     // 次ののメビウスの輪の位置を取得
                                  this.GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2);                  // プレイヤーの半径を取得

                if (switching)
                {
                    //切り替えたステータスをセット
                    SwitchingSetStatus(i);
                    break;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (other.GetComponent<PlayerMove>().GetNowMobiusNum() == NowMobius)//同じメビウスか
            {
                if (!other.GetComponent<PlayerMove>().GetJumpNow())//ジャンプしているかどうか
                {
                    if (!Stan)//スタンしていないか
                    {
                        if (other.GetComponent<PlayerMove>().GetInsideFlg() == InsideFlg)//外側か内側か
                        {
                            other.GetComponent<PlayerMove>().SetCollisionState();
                        }
                    }
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
    
    public void SetAlert(bool flg)
    {
        AlertCollision = flg;
    }

    //アラートの範囲にいるかどうかを返す
    public bool GetAlertCollision()
    {
        return AlertCollision;
    }
}
