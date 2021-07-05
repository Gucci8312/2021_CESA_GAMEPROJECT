using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    static bool[] OpenStageFlg = new bool[26];
    static bool[] ClearStageFlg = new bool[26];
    static int[] StageParsent = new int[26];
    static bool[] TimeAttackClearFlg = new bool[26];
    static int NowStage = 1;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void SetNowStage(int _NowStage)
    {
        NowStage = _NowStage;
    }

    static public int GetNowStage()
    {
        return NowStage;
    }
    static public void SetOpenFlg(int _Idx)
    {
        OpenStageFlg[_Idx] = true;
    }
    static public void SetParcent(int _Idx, int _Parcent)
    {
        if (StageParsent[_Idx] <= _Parcent)
        {
            StageParsent[_Idx] = _Parcent;
        }
        if (StageParsent[_Idx] == 100)
        {
            TimeAttackClearFlg[_Idx] = true;
        }
    }
    static public void SetClearFlg(int _Idx)
    {
        ClearStageFlg[_Idx] = true;
        StageControl.SetOpenFlg(_Idx + 1);
    }

    static public bool GetOpenFlg(int _Idx)
    {
        if (_Idx >= 24)
        {
            _Idx = 24;
        }
        return OpenStageFlg[_Idx];
    }
    static public bool GetTimeAttackClearFlg(int _Idx)
    {
        return TimeAttackClearFlg[_Idx];
    }
    static public bool GetClearFlg(int _Idx)
    {
        return ClearStageFlg[_Idx];
    }
    static public int GetStageParsent(int _Idx)
    {
        if (_Idx >= 24)
        {
            _Idx = 24;
        }
        return StageParsent[_Idx];
    }

    static public void AllStageOpen()
    {
        for (int Idx = 0; Idx < 25; Idx++)
        {
            OpenStageFlg[Idx] = true;
            TimeAttackClearFlg[Idx] = true;
        }
    }

    static public int OpenStageNum()
    {
        int Idx = 0;
        while (OpenStageFlg[Idx])
        {
            Idx++;
        }
        return Idx;
    }

    static public void ReleaseStage(int _StageNum, bool _ClearFlg, int _Parsent, bool _TimeAttackFlg)
    {
        //for (int Idx = 0; Idx < _StageNum; Idx++)
        //{
        //    OpenStageFlg[Idx] = true;
        //}
        OpenStageFlg[_StageNum] = _ClearFlg;
        StageParsent[_StageNum] = _Parsent;
        TimeAttackClearFlg[_StageNum] = _TimeAttackFlg;
    }
}
