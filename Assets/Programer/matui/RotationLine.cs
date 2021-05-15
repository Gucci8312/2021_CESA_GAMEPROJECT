using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLine : MonoBehaviour
{
    public enum RotationPosition//回転する位置
    {
        LeftPosition=0,
        RightPosition,
    }
    public RotationPosition RotationPos;

    public enum RotationVector//回転する方向
    {
        LefRotation = 0,
        RightRotation,
    }
    public RotationVector RotationVec;

    [Range(0,360)] public float GoalAngle;                                  //目標角度
    float InitAngle;                                                        //初期角度
    float NowAngle;

    public float GoalAngleTime;                                             //目標角度へ到達するまでの時間（秒）
    float NowAngletime;                                                     //移動時間（秒）
    public bool RotationFlag;                                               //回転角度
    bool OuhukuFlag = true;                                                 //true:行き　false:帰り

    private bool BeatFlag;                                                   //ビートが指定した回数になったかどうか
    public int MaxBeatNum = 5;                                               //ビート最大数指定
    float BeatCount = 0;

    Vector3 InitRotation;                                                    //初期回転値

    Vector2 LPos;
    Vector2 RPos;

    List<GameObject> PutOnMobius = new List<GameObject>();            //線上に乗っているメビウスオブジェクト
    List<MoveMobius> Mm = new List<MoveMobius>();
    List<LinePutMobius> Lpm = new List<LinePutMobius>();

    CrossLine Cl;                                                            //CrossLineスクリプト格納用

    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    Vector3 OldPos;
    // Start is called before the first frame update
    void Start()
    {
        Cl = this.gameObject.GetComponent<CrossLine>();
        InitAngle = this.transform.localEulerAngles.z;
        InitRotation = this.transform.localEulerAngles;

        LPos = RotationfromPosition(this.transform.position, this.transform.localScale, this.transform.localEulerAngles.z, 1);//自分の左端の回転を含めた座標を取得
        RPos = RotationfromPosition(this.transform.position, this.transform.localScale, this.transform.localEulerAngles.z, 0);//自分の右端の回転を含めた座標を取得

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
    }

    // Update is called once per frame
    void Update()
    {
        Cl.MoveLineFlag = RotationFlag;

        BeatCounter();
        OuhukuRotation();

        OldPos = this.transform.position;

    }

    //指定したビート数に達したら回転（往復）

    private void OuhukuRotation()
    {
        if (BeatFlag)//指定したビート数に達したら
        {
            //回転開始
            RotationFlag = true;
            NowAngletime = 0;
        }

        if (RotationFlag)//回転させる
        {
            bool EndFlag = false;//移動が終わったかどうか

            if (NowAngletime >= GoalAngleTime)//時間になったら
            {
                NowAngletime = GoalAngleTime;
                EndFlag = true;
            }


            if (OuhukuFlag)//行き
            {
                NowAngle = SenkeiHokan(0, GoalAngle, NowAngletime, 0, GoalAngleTime);
            }
            else//帰り
            {
                NowAngle = SenkeiHokan(GoalAngle, 0, NowAngletime, 0, GoalAngleTime);
            }
            RotationUpdate(NowAngle);

            if (PutOnMobius.Count != 0)
            {
                Vector3 pos = this.transform.position;
                Vector3 AddPos = pos - OldPos;//線が移動したときの変化量を取得

                float distance=0;//距離に応じて移動量を変化させる用
                for (int i = 0; i < PutOnMobius.Count; i++)
                {
                    switch (RotationPos)
                    {
                        case RotationPosition.LeftPosition:
                            distance = (new Vector3(LPos.x, LPos.y, 0) - PutOnMobius[i].transform.position).magnitude;
                            break;

                        case RotationPosition.RightPosition:
                            distance = (new Vector3(RPos.x, RPos.y, 0) - PutOnMobius[i].transform.position).magnitude;
                            break;
                    }

                    //メビウスに移動した変化量を加える
                    PutOnMobius[i].transform.position += AddPos *(distance*0.005f);//0.005fで移動量を調整
                    Mm[i].MovePos += AddPos;
                    Mm[i].StartMovePos += AddPos;
                    Mm[i].OldPos += AddPos;
                }
            }

            NowAngletime += Time.deltaTime;

            if (EndFlag)
            {
                //移動終了
                OuhukuFlag = !OuhukuFlag;
                RotationFlag = false;

            }
        }
        else
        {
            if (PutOnMobius.Count != 0)
            {
                for (int i = 0; i < PutOnMobius.Count; i++)
                {
                    PutOnMobius[i].GetComponent<MoveMobius>().MoveLineObj = null;
                }
                PutOnMobius.Clear();
                Mm.Clear();
                Lpm.Clear();
            }
        }
    }

    //ビートをカウントする
    private void BeatCounter()
    {
        if (BeatCount >= MaxBeatNum)
        {
            BeatFlag = true;
            BeatCount = 0;
        }
        else
        {
            BeatFlag = false;

        }

        if (this.rythm.m_EmobiusBeatFlag)//ビートを刻んだら
        {
            //time += Time.deltaTime;
            BeatCount++;

        }
    }

    //回転したときの座標を更新(引数に回転させたい角度を代入)
    private void RotationUpdate(float _Angle)
    {
        float Angle;
        float AddAngle = _Angle;
        Vector3 Rot = InitRotation;

        switch (RotationVec)
        {
            case RotationVector.LefRotation:
                if (RotationPos == RotationPosition.LeftPosition)
                {
                    AddAngle = -AddAngle;
                }
                break;
            case RotationVector.RightRotation:
                if (RotationPos == RotationPosition.RightPosition)
                {
                    AddAngle = -AddAngle;
                }
                break;
        }

        switch (RotationPos)
        {
            case RotationPosition.LeftPosition:
                Angle = InitAngle - AddAngle;
                this.transform.position = RotationfromPosition(LPos, this.transform.localScale, Angle, 0);
                Rot.z -= AddAngle;
                break;

            case RotationPosition.RightPosition:
                Angle = InitAngle + AddAngle;
                this.transform.position = RotationfromPosition(RPos, this.transform.localScale, Angle, 1);
                Rot.z += AddAngle;
                break;
        }

        this.transform.localEulerAngles = Rot;

    }

    //回転したときの座標を求める（横長の線を基準に回転）
    private Vector2 RotationfromPosition(Vector2 pos, Vector2 scale, float Angle, int tyouten)
    {
        scale.y = 0;//横長の棒の先端に点を置くために縦軸を0にする

        float theta = Mathf.Atan((scale.y / 2) / (scale.x / 2)) * 180 / 3.14f;
        float sha = Mathf.Sqrt((scale.x / 2) * (scale.x / 2) + (scale.y / 2) * (scale.y / 2));


        float deg = 0;

        switch (tyouten)
        {
            case 0:
                deg = theta + Angle;
                break;
            case 1:
                deg = 180 - theta + Angle;
                break;
        }

        Vector2 outpos;
        outpos.x = pos.x + sha * Mathf.Cos(deg * 3.14f / 180);
        outpos.y = pos.y + sha * Mathf.Sin(deg * 3.14f / 180);


        return outpos;
    }

    //	R0：始点　,R1：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private float SenkeiHokan(float R0, float R1, float t, float t0, float t1)
    {
        float Angle;
        Angle = R0 + (R1 - R0) * (t - t0) / (t1 - t0);
        return Angle;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mobius"))
        {
            MoveMobius otherMm = other.gameObject.GetComponent<MoveMobius>();

            if (!RotationFlag)//線が動いていないとき
            {
                if (otherMm.MoveLineObj == null)//当たったメビウスがまだどの動く線にくっつくか決めてなければ
                {
                    otherMm.MoveLineObj = this.gameObject;
                    PutOnMobius.Add(other.gameObject);
                    Mm.Add(otherMm);
                    Lpm.Add(other.GetComponent<LinePutMobius>());
                }

            }
        }
    }

}
