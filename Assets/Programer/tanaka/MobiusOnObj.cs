using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobiusOnObj : MonoBehaviour
{
    protected GameObject[] Mobius;                                              //メビウスの輪
    [SerializeField] protected int NowMobius;                                   //現在のメビウスの添え字　初期のメビウスの輪
    protected int SaveMobius;                                                   //１つ前にいたメビウスの添え字
    protected float InsideLength;                                               //内側に入ったときの位置を調整する値
    [SerializeField] protected bool RotateLeftFlg;                              //回転方向が左右のどちらかを判定　true:左　false:右
    [SerializeField] protected bool InsideFlg;                                  //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    [SerializeField] protected float NormalSpeed;                               //普通のスピードを格納している定数

    [SerializeField] protected float rotateSpeed = 180f;                        //回転速度
    protected float Speed;                                                      //現在のスピード
    [SerializeField] protected float InsideSpeed = 2;                           //内側のスピードを調整するための変数
    protected int SideCnt;                                                      //メビウスの輪に沿った動きにするためメビウスの輪を何回切り替えたかをカウント  2以上で外側内側入れ替える
    protected float counter;                                                    //乗り移るとき、元のメビウスの輪に戻らないようにカウントする値
    [SerializeField, Range(0, 7)] public int StartPoint;                        //メビウス上の点の番号

    [SerializeField] protected float angle;                                     //現在のメビウスからのプレイヤーの角度
    protected float saveangle;                                                  //切り替えのための角度を保存

    protected Transform target;                                                 //現在のメビウスのトランスフォーム
    protected Vector3 distanceTarget = new Vector3(0f, 0f, 0f);                 //メビウスからの距離

    protected bool SwitchMobius;                                                //メビウスを切り替えたときすぐに戻らないようにする

    static protected bool Pause;                                                //ポーズ用フラグ

    protected virtual void Awake()
    {
        Mobius = GameObject.FindGameObjectsWithTag("Mobius");
        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                  //全てのメビウス取得
        }

        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 50;//内側までの距離
        }
        else//外側
        {
            InsideLength = 0;
        }

        //初期位置設定
        float MobiusDistance = 50.0f;//メビウスの輪からの位置の長さ
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;
        this.gameObject.transform.position = new Vector3(MobiusPos.x, MobiusPos.y, 0);
        this.gameObject.transform.position += new Vector3(0, MobiusDistance, 0);

        angle = 360 - (StartPoint * 45);//始まりの位置を求める

        PositionSum();
    }
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (RotateLeftFlg)//メビウスの輪の外内の切り替え調整
        {
            SideCnt = 2;
        }
        else
        {
            SideCnt = 1;
        }
        
        SaveMobius = -1;
        counter = -1;
        Speed = NormalSpeed;
        SwitchMobius = false;
        
    }

    //メビウスの輪からの場所を計算
    protected virtual void PositionSum()
    {
        target = Mobius[NowMobius].transform;

        //メビウスの輪の中心とプレイヤーの距離を求める
        distanceTarget.y = (Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 + GetComponent<SphereCollider>().bounds.size.x / 2 + 10.0f) - InsideLength ;
        //プレイヤーの位置をメビウスの位置・メビウスから見たプレイヤーの角度・距離から求める
        transform.position = target.position + Quaternion.Euler(0f, 0f, angle) * distanceTarget;
        //プレイヤーの角度をメビウスから見た角度を計算し、設定する
        transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, target.position.y, transform.position.z), -Vector3.forward);
        
    }

    //メビウスの輪同士が当てっているかどうか
    protected virtual bool CollisionSphere(Vector3 NowMobiusPos, Vector3 NextMobiusPos, float MobiusRadius)
    {
        Vector3 Vec = NextMobiusPos - NowMobiusPos;
        float VecLength = Mathf.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y + Vec.z * Vec.z);

        if (MobiusRadius + MobiusRadius > VecLength)
        {
            return true;
        }

        return false;
    }
    
    //メビウスの輪を切り替える為の計算
    protected virtual bool MobiusSwitch(Vector3 NowMobiusPos, Vector3 NextMobiusPos, float MyRadius)
    {
        //対象のメビウスの輪との距離を計算
        Vector3 Vec = NextMobiusPos - NowMobiusPos;
        float VecLength = Mathf.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y);
        Vec.x = Vec.x / VecLength;
        Vec.y = Vec.y / VecLength;
        Vec.z = Vec.z / VecLength;

        //当たっている場所を計算(接点)
        Vector3 CollisonPos;
        CollisonPos.x = NowMobiusPos.x + Vec.x * MyRadius;
        CollisonPos.y = NowMobiusPos.y + Vec.y * MyRadius;
        CollisonPos.z = NowMobiusPos.z + Vec.z * MyRadius;

        Vector3 NextVec = CollisonPos - this.GetComponent<SphereCollider>().bounds.center;                //メビウスの輪同士の接点とプレイヤーの位置のベクトルを計算
        float NextLength = Mathf.Sqrt(NextVec.x * NextVec.x + NextVec.y * NextVec.y);                     //メビウスの輪同士の接点とプレイヤーの位置の長さ計算

        //メビウス同士の成す角度を求める
        float CollisonAngle = Get2PointAngle(NowMobiusPos, NextMobiusPos);
        //メビウスとプレイヤーの成す角度を求める
        float NowAngle = Get2PointAngle(NowMobiusPos, this.GetComponent<SphereCollider>().bounds.center);

        const int AngleRange = 10;//メビウスの輪が切り替わる範囲
        if (CollisonAngle < NowAngle + AngleRange && CollisonAngle > NowAngle - AngleRange)               //切り替える場所に来た
        {
            return true;
        }

        return false;
    }

    //メビウスの輪を切り替えたときの数値をセット
    protected virtual void SwitchingSetStatus(int NextMobiusNum)
    {
        SaveMobius = NowMobius;
        NowMobius = NextMobiusNum;
        counter = 0;
        angle += 180;

        angle=AngleRangeSum(angle);

        saveangle = angle;
        saveangle=AngleRangeSum(saveangle);


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
        }

        if (RotateLeftFlg)
        {
            RotateLeftFlg = false;
        }
        else
        {
            RotateLeftFlg = true;
        }

        SideCnt++;
        SwitchMobius = true;

    }


    //二つの場所から角度を求める
    protected virtual float Get2PointAngle(Vector2 start, Vector2 target)
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

    //角度を1～360の範囲に計算する
    protected virtual float AngleRangeSum(float anglenum)
    {
        if (anglenum > 360)
        {
            anglenum = anglenum - 360;

        }
        if (anglenum < 0)
        {
            anglenum = anglenum + 360;
        }
        return anglenum;
    }

    public virtual float GetMoveAngle()
    {
        return angle;
    }

    //現在のメビウスの数字を返す
    public virtual int GetNowMobiusNum()
    {
        return NowMobius;
    }

    //外内のどちらかを返す
    public virtual bool GetInsideFlg()
    {
        return InsideFlg;
    }

    public virtual int GetSideCnt()
    {
        return SideCnt;
    }

    //ポーズをオンにする
    public static void PauseOn()
    {
        Pause = true;
    }

    //ポーズにオフにする
    public static void PauseOff()
    {
        Pause = false;
    }
}
