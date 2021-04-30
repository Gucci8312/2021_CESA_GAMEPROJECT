using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLine : MonoBehaviour
{
    public enum Vector
    {
        Up = 0,
        Down,
        Left,
        Right,
    }
    Vector2 OldPos;
    private Vector2 EndMovePos;                                              //終点
    private Vector2 StartMovePos;                                            //始点
    public float GoalMovetime = 0.05f;                                       //目的地へ到達するまでの時間（秒）
    float Nowtime = 0;                                                       //移動時間（秒）

    public float MoveDistance;                                               //移動距離
    public Vector LineMoveVec;                                               //移動方向
    bool MoveFlag;                                                           //移動させるかどうか
    bool OuhukuFlag = true;                                                 //true:行き　false:帰り

    private bool BeatFlag;                                                   //ビートが指定した回数になったかどうか
    public int MaxBeatNum = 5;                                               //ビート最大数指定
    public float BeatCount = 0;

    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    CrossLine Cl;

    // Start is called before the first frame update
    void Start()
    {
        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
        Cl = this.GetComponent<CrossLine>();

        Cl.LineMovingFlag = true;

        StartMovePos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePosSet();
        BeatCounter();
        MoveVacation();

        OldPos = this.transform.position;
    }

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

    private void MovePosSet()
    {
        switch (LineMoveVec)
        {
            case Vector.Up:
                EndMovePos = new Vector2(StartMovePos.x, StartMovePos.y + MoveDistance);
                break;
            case Vector.Down:
                EndMovePos = new Vector2(StartMovePos.x, StartMovePos.y - MoveDistance);
                break;
            case Vector.Left:
                EndMovePos = new Vector2(StartMovePos.x - MoveDistance, StartMovePos.y);
                break;
            case Vector.Right:
                EndMovePos = new Vector2(StartMovePos.x + MoveDistance, StartMovePos.y);
                break;

        }
    }

    private void MoveVacation()
    {
        if (BeatFlag)
        {
            MoveFlag = true;
        }

        if (MoveFlag)
        {
            
            if (OuhukuFlag)
            {
                this.transform.position = SenkeiHokan(StartMovePos, EndMovePos, Nowtime, 0, GoalMovetime);
            }
            else
            {
                this.transform.position = SenkeiHokan(EndMovePos, StartMovePos, Nowtime, 0, GoalMovetime);
            }

            //Vector2 pos = this.transform.position;
            //Vector2 AddPos = pos- OldPos;

            //for(int i = 0; i < Cl.CrossPos.Count; i++)
            //{
            //    Cl.CrossPos[i] += AddPos;
            //}

            Nowtime += Time.deltaTime;

            if (Nowtime >= GoalMovetime)
            {
                //if (OuhukuFlag)
                //{
                //    this.transform.position = EndMovePos;
                //}
                //else
                //{
                //    this.transform.position = StartMovePos;
                //}
                Nowtime = 0;
                OuhukuFlag = !OuhukuFlag;
                MoveFlag = false;
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

    private void OnTriggerStay(Collider other)
    {
       
    }
}
