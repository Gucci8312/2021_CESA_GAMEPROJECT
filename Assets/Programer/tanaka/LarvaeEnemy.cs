using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaeEnemy : EnemyMove
{

    bool Deth;
    bool AdultRotateLeftFlg;
    float InvincibilityTime ;
    [SerializeField] GameObject AdultEnemyObj;


    protected override void Awake()
    {
        base.Awake();
        Deth = false;
        type = (int)EnemyType.Larvae;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Pause)
        {
            
            PositionSum();

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

            InvincibilityTime -= Time.deltaTime;
            if (InvincibilityTime < 0)
            {
                InvincibilityTime = 0;
            }

        }
    }

    public void SetLeftDirection(bool direction)
    {
        RotateLeftFlg = direction;
    }

    public void DethOn()//破棄
    {
        Deth = true;
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (InvincibilityTime == 0)
        {

            if (other.gameObject.tag == "Player")
            {

                if (other.GetComponent<PlayerMove>().GetNowMobiusNum() == NowMobius)//同じメビウスか
                {
                    if (!other.GetComponent<PlayerMove>().GetJumpNow())//ジャンプしているかどうか
                    {
                        if (other.GetComponent<PlayerMove>().GetInsideFlg() == InsideFlg)//外側か内側か
                        {
                            other.GetComponent<PlayerMove>().SetCollisionState();
                        }
                    }
                }

            }


            if (other.gameObject.tag == "Enemy")
            {
                if (other.GetComponent<EnemyMove>().type == (int)EnemyType.Larvae)
                {
                    if (other.GetComponent<LarvaeEnemy>().GetRotateFlg() ==true)
                    {
                        Destroy(other.gameObject);
                        GameObject NewAdultEnemy = Instantiate(AdultEnemyObj);
                        NewAdultEnemy.GetComponent<AdultEnemy>().SetMakeState(AdultRotateLeftFlg, NowMobius, InsideFlg, angle,SideCnt);

                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }


    public void SetMakeState(bool rotateleftflg,int nowmobius,bool insideflg,float nowangle,int sidecnt)
    {
        SideCnt = sidecnt - 1;
        if (SideCnt < 0) SideCnt = 2;
         RotateLeftFlg = rotateleftflg;
        NowMobius = nowmobius;
        
        angle = nowangle;
        InvincibilityTime = 0.5f;
        //外内で速度調整
        if (InsideFlg)
        {
            Speed = NormalSpeed * InsideSpeed;
        }
        else
        {
            Speed = NormalSpeed;
        }

        InsideFlg = insideflg;
        InsideFlg = !InsideFlg;
        if (InsideFlg)
        {
            InsideFlg = false;
            InsideLength = OutLength;//内側までの距離

        }
        else
        {
            InsideLength = InLength;//内側までの距離
            InsideFlg = true;
        }
    }

    public void SetAdultRotateLeftFlg(bool rotateleftflg)
    {
        AdultRotateLeftFlg = rotateleftflg;
    }

    
}
