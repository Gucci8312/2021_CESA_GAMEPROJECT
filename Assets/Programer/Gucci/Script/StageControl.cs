using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    static bool[] OpenStageFlg = new bool[26];
    static int NowStage=1;

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
        OpenStageFlg[_Idx ] = true;
    }

    static public bool GetOpenFlg(int _Idx)
    {
        if(_Idx>=24)
        {
            _Idx = 24;
        }
        return OpenStageFlg[_Idx];
    }

    static public void AllStageOpen()
    {
        for (int Idx = 0; Idx < 25; Idx++)
        {
            OpenStageFlg[Idx] = true;
        }
    }

    static public int OpenStageNum()
    {
        int Idx=0;
        while(OpenStageFlg[Idx])
        {
            Idx++;
        }
        return Idx;
    }

    static public void ReleaseStage(int _StageNum)
    {
        for (int Idx = 0; Idx < _StageNum; Idx++)
        {
            OpenStageFlg[Idx] = true;
        }
    }
}
