using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultEnemy : EnemyMove
{
    [SerializeField] GameObject LarvaeEnemyObj;
    protected override void Awake()
    {
        base.Awake();
        type = (int)EnemyType.Adult;
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
        }
    }

    

    //生成した時の状態をセット
    public void SetMakeState(bool rotateleftflg,int nowmobius,bool insideflg ,float nowangle,int sidecnt)
    {
        SideCnt = sidecnt - 1;
        if (SideCnt < 0) SideCnt = 2;
        RotateLeftFlg = rotateleftflg;
        NowMobius = nowmobius;
        InsideFlg = insideflg;
        angle = nowangle;

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

    public void SplitOn()
    {
        GameObject NewLarvaeEnemy1 = Instantiate(LarvaeEnemyObj);
        GameObject NewLarvaeEnemy2 = Instantiate(LarvaeEnemyObj);

        //敵を右方向に生成
        NewLarvaeEnemy1.GetComponent<LarvaeEnemy>().SetMakeState(true, NowMobius, InsideFlg, angle+30,SideCnt);
        NewLarvaeEnemy1.GetComponent<LarvaeEnemy>().SetAdultRotateLeftFlg(RotateLeftFlg);
        //敵を左方向に生成
        NewLarvaeEnemy2.GetComponent<LarvaeEnemy>().SetMakeState(false, NowMobius, InsideFlg, angle-30,SideCnt-1);
        NewLarvaeEnemy2.GetComponent<LarvaeEnemy>().SetAdultRotateLeftFlg(RotateLeftFlg);

        Destroy(this.gameObject);
    }

}
