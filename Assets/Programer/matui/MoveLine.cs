using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//動く線の処理
public class MoveLine : MonoBehaviour
{
    public enum MoveVector
    {
        Up = 0,
        Down,
        Left,
        Right,
    }
    Vector3 OldPos;
    private Vector2 EndMovePos;                                              //終点
    private Vector2 StartMovePos;                                            //始点
    public float GoalMovetime = 0.05f;                                       //目的地へ到達するまでの時間（秒）
    float Nowtime = 0;                                                       //移動時間（秒）

    public float MoveDistance;                                               //移動距離
    public MoveVector MoveVec;                                               //移動方向
    bool MoveFlag = false;                                                   //移動させるかどうか
    bool OuhukuFlag = false;                                                  //true:行き　false:帰り

    private bool BeatFlag;                                                   //ビートが指定した回数になったかどうか
    public int MaxBeatNum = 5;                                               //ビート最大数指定
    float BeatCount = 0;

    /* [HideInInspector] */
    public List<GameObject> PutOnMobius = new List<GameObject>();            //線上に乗っているメビウスオブジェクト
    [HideInInspector] public List<MoveMobius> Mm = new List<MoveMobius>();
    [HideInInspector] public List<LinePutMobius> Lpm = new List<LinePutMobius>();

    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    CrossLine Cl;                                                           //CrossLineスクリプト格納用

    static bool StopFlag = false;//true:止める　false:動く

    // Start is called before the first frame update
    void Start()
    {
        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
        Cl = this.GetComponent<CrossLine>();

        //Cl.LineMovingFlag = true;

        StartMovePos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StopFlag)
        {
            MoveLineUpdate();
        }
    }

    //EnemyMobiusの更新
    private void MoveLineUpdate()
    {
        Cl.MoveLineFlag = true;
        Cl.MoveFlag = MoveFlag;

        //if (Time.timeScale != 0)////時間が止まっていなければ
        //{
            PutOnMobiusSetting();
            MovePosSet();
            BeatCounter();
            OuhukuMove();
        //}

        OldPos = this.transform.position;
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

    //終点となる座標をセットする
    private void MovePosSet()
    {
        switch (MoveVec)
        {
            case MoveVector.Up:
                EndMovePos = new Vector2(StartMovePos.x, StartMovePos.y + MoveDistance);
                break;
            case MoveVector.Down:
                EndMovePos = new Vector2(StartMovePos.x, StartMovePos.y - MoveDistance);
                break;
            case MoveVector.Left:
                EndMovePos = new Vector2(StartMovePos.x - MoveDistance, StartMovePos.y);
                break;
            case MoveVector.Right:
                EndMovePos = new Vector2(StartMovePos.x + MoveDistance, StartMovePos.y);
                break;

        }
    }

    //指定したビート数に達したら移動（往復）
    private void OuhukuMove()
    {
        if (BeatFlag)//指定したビート数に達したら
        {
            //移動開始
            MoveFlag = true;
            Nowtime = 0;
            OuhukuFlag = !OuhukuFlag;

        }

        if (MoveFlag)//移動させる
        {
            bool EndFlag = false;//移動が終わったかどうか

            if (Nowtime >= GoalMovetime)//時間になったら
            {
                Nowtime = GoalMovetime;
                EndFlag = true;
            }

            if (OuhukuFlag)//行き
            {
                this.transform.position = SenkeiHokan(StartMovePos, EndMovePos, Nowtime, 0, GoalMovetime);
            }
            else//帰り
            {
                this.transform.position = SenkeiHokan(EndMovePos, StartMovePos, Nowtime, 0, GoalMovetime);
            }

            if (PutOnMobius.Count != 0)
            {
                Vector3 pos = this.transform.position;
                Vector3 AddPos = pos - OldPos;//線が移動したときの変化量を取得

                for (int i = 0; i < PutOnMobius.Count; i++)
                {
                    //メビウスに移動した変化量を加える
                    PutOnMobius[i].transform.position += AddPos;
                    Mm[i].MovePos += AddPos;
                    Mm[i].StartMovePos += AddPos;
                    Mm[i].OldPos += AddPos;
                }
            }

            Nowtime += Time.deltaTime;

            if (EndFlag)
            {
                //移動終了
                MoveFlag = false;

                //if (PutOnMobius.Count != 0)
                //{
                //    for (int i = 0; i < PutOnMobius.Count; i++)
                //    {
                //        PutOnMobius[i].GetComponent<MoveMobius>().MoveLineObj = null;
                //    }
                //    PutOnMobius.Clear();
                //    Mm.Clear();
                //    Lpm.Clear();
                //}

            }

        }
        else
        {
            //if (PutOnMobius.Count != 0)
            //{
            //    for (int i = 0; i < PutOnMobius.Count; i++)
            //    {
            //        PutOnMobius[i].GetComponent<MoveMobius>().MoveLineObj = null;
            //    }
            //    PutOnMobius.Clear();
            //    Mm.Clear();
            //    Lpm.Clear();
            //}
        }
    }

    public void PutMobiusOnOff(bool flag,GameObject _obj)
    {
        MoveMobius otherMm = _obj.GetComponent<MoveMobius>();

        if (flag)
        {
            otherMm.MoveLineObj = this.gameObject;
            PutOnMobius.Add(_obj);
            Mm.Add(otherMm);
            Lpm.Add(_obj.GetComponent<LinePutMobius>());
        }
        else
        {
            otherMm.MoveLineObj = null;
            PutOnMobius.Remove(_obj);
            Mm.Remove(otherMm);
            Lpm.Remove(_obj.GetComponent<LinePutMobius>());
        }
    }

    //線に乗っているメビウスにいろいろ情報をセット
    private void PutOnMobiusSetting()
    {
        Vector2 vec = Vector2.zero;
        switch (MoveVec)
        {
            case MoveVector.Up:
                vec = Vector2.up;
                break;
            case MoveVector.Down:
                vec = Vector2.down;
                break;
            case MoveVector.Left:
                vec = Vector2.left;
                break;
            case MoveVector.Right:
                vec = Vector2.right;
                break;
        }

        if (!OuhukuFlag)
        {
            vec = -vec;
        }

        if (Lpm.Count != 0)
        {
            for (int i = 0; i < Lpm.Count; i++)
            {
                Lpm[i].SetMoveLineFlag(MoveFlag);
                Lpm[i].SetMoveLineVec(vec);
            }
        }
    }

    //	P0：始点　,P1：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private Vector2 SenkeiHokan(Vector2 P0, Vector2 P1, float t, float t0, float t1)
    {
        Vector2 pos = Vector2.zero;
        pos.x = P0.x + (P1.x - P0.x) * (t - t0) / (t1 - t0);
        pos.y = P0.y + (P1.y - P0.y) * (t - t0) / (t1 - t0);

        return pos;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Mobius"))
    //    {
    //        MoveMobius otherMm = other.gameObject.GetComponent<MoveMobius>();

    //        if (!MoveFlag)//線が動いていないとき
    //        {
    //            if (otherMm.MoveLineObj == null)//当たったメビウスがまだどの動く線にくっつくか決めてなければ
    //            {
    //                otherMm.MoveLineObj = this.gameObject;
    //                PutOnMobius.Add(other.gameObject);
    //                Mm.Add(otherMm);
    //                Lpm.Add(other.GetComponent<LinePutMobius>());
    //            }

    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Mobius"))
    //    {
    //        MoveMobius otherMm = other.gameObject.GetComponent<MoveMobius>();

    //        if (!MoveFlag)//線が動いていないとき
    //        {
    //            //if (otherMm.MoveLineObj != null)//何かしらくっついている場合
    //            //{
    //                if (otherMm.MoveLineObj == this.gameObject)//メビウスがくっついている線が自身であれば
    //                {
    //                    //Debug.Log(this.name + "から降りた");
    //                    otherMm.MoveLineObj = null;
    //                    PutOnMobius.Remove(other.gameObject);
    //                    Mm.Remove(otherMm);
    //                    Lpm.Remove(other.GetComponent<LinePutMobius>());
    //                }
    //            //}
    //        }
    //    }
    //}

    public bool GetMoveFlag()
    {
        return MoveFlag;
    }

    static public void StopFlagSet(bool flag)
    {
        StopFlag = flag;
    }

}
