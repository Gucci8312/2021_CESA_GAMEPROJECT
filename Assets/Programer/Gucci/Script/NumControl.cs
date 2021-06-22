using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumControl : MonoBehaviour
{
    static GameObject[] OneObj = new GameObject[10];
    static GameObject[] TenObj = new GameObject[10];
    static GameObject[] HundredObj = new GameObject[10];
    static int TempOne = 0, TempTen = 0, TempHundred = 1;
    static Material NumColor;
    static int NowScore;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    static void ChangeColor(float R, float G, float B, float A)
    {
        Color color = new Vector4(R, G, B, A);
        OneObj[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        for (int Idx = 0; Idx < 10; Idx++)
        {
            OneObj[Idx].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        }
        for (int Idx = 0; Idx < 10; Idx++)
        {
            TenObj[Idx].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        }
        for (int Idx = 1; Idx < 10; Idx++)
        {
            HundredObj[Idx].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        }
    }

    static public void InitNum()
    {
        for (int Idx = 0; Idx < 10; Idx++)
        {
            OneObj[Idx] = GameObject.Find(Idx.ToString());
            OneObj[Idx].SetActive(false);
            //if (OneObj[Idx] == null)
            //{
            //    Debug.Log("numobjがない");
            //}
        }
        TenObj[0] = GameObject.Find("00");
        TenObj[0].SetActive(false);

        int Cnt = 10;
        for (int Idx = 1; Idx < 10; Idx++)
        {
            TenObj[Idx] = GameObject.Find(Cnt.ToString());
            TenObj[Idx].SetActive(false);
            Cnt += 10;
        }
        Cnt = 100;
        // HundredObj[0] = GameObject.Find("100");
        //HundredObj[0].SetActive(false);
        for (int Idx = 1; Idx < 10; Idx++)
        {
            HundredObj[Idx] = GameObject.Find(Cnt.ToString());
            HundredObj[Idx].SetActive(false);
            Cnt += 100;
        }
        DrawScore(0);
    }

    // 
    static public void DrawScore(int _Num)
    {
        int One, Ten, Hundred;
        NowScore = _Num;
        One = _Num % 10;        // 一の位
        Ten = (_Num % 100) / 10;        // 十の位
        Hundred = _Num / 100;   // 百の位
        if (TempTen > 9)
        {
            TempTen = 0;
        }
        if (_Num < 30)
        {
            ChangeColor(1.0f, 0.6f, 0.0f, 0.0f);
        }
        else if (_Num < 50)
        {
            ChangeColor(1.0f, 1.0f, 0.0f, 0.0f);
        }
        else if (_Num < 70)
        {
            ChangeColor(0.0f, 1.0f, 0.0f, 0.0f);
        }
        else if (_Num == 100)
        {
            ChangeColor(0.0f, 1.0f, 1.0f, 0.0f);
        }
        //Debug.Log(One);
        //Debug.Log(Ten);

        //if (_Num == 100)
        //{
        //    HundredObj[0].SetActive(true);

        //    OneObj[0].SetActive(true);
        //    TenObj[0].SetActive(true);

        //    //OneObj[TempOne].SetActive(false);
        //    // TenObj[TempTen].SetActive(false);
        //}
        //else
        //{
        // 一の位
        switch (One)
        {
            case 0:
                OneObj[TempOne].SetActive(false);
                OneObj[0].SetActive(true);
                TempOne = 0;
                break;

            case 1:
                OneObj[TempOne].SetActive(false);
                OneObj[1].SetActive(true);
                TempOne = 1;
                break;

            case 2:
                OneObj[TempOne].SetActive(false);
                OneObj[2].SetActive(true);
                TempOne = 2;
                break;

            case 3:
                OneObj[TempOne].SetActive(false);
                OneObj[3].SetActive(true);
                TempOne = 3;
                break;

            case 4:
                OneObj[TempOne].SetActive(false);
                OneObj[4].SetActive(true);
                TempOne = 4;
                break;

            case 5:
                OneObj[TempOne].SetActive(false);
                OneObj[5].SetActive(true);
                TempOne = 5;
                break;

            case 6:
                OneObj[TempOne].SetActive(false);
                OneObj[6].SetActive(true);
                TempOne = 6;
                break;

            case 7:
                OneObj[TempOne].SetActive(false);
                OneObj[7].SetActive(true);
                TempOne = 7;
                break;

            case 8:
                OneObj[TempOne].SetActive(false);
                OneObj[8].SetActive(true);
                TempOne = 8;
                break;

            case 9:
                OneObj[TempOne].SetActive(false);
                OneObj[9].SetActive(true);
                TempOne = 9;
                break;
        }

        // 十の位
        switch (Ten)
        {
            case 0:
                TenObj[TempTen].SetActive(false);
                TenObj[0].SetActive(true);
                TempTen = 0;
                break;
            case 1:
                TenObj[TempTen].SetActive(false);
                TenObj[1].SetActive(true);
                TempTen = 1;
                break;

            case 2:
                TenObj[TempTen].SetActive(false);
                TenObj[2].SetActive(true);
                TempTen = 2;
                break;

            case 3:
                TenObj[TempTen].SetActive(false);
                TenObj[3].SetActive(true);
                TempTen = 3;
                break;

            case 4:
                TenObj[TempTen].SetActive(false);
                TenObj[4].SetActive(true);
                TempTen = 4;
                break;

            case 5:
                TenObj[TempTen].SetActive(false);
                TenObj[5].SetActive(true);
                TempTen = 5;
                break;

            case 6:
                TenObj[TempTen].SetActive(false);
                TenObj[6].SetActive(true);
                TempTen = 6;
                break;

            case 7:
                TenObj[TempTen].SetActive(false);
                TenObj[7].SetActive(true);
                TempTen = 7;
                break;

            case 8:
                TenObj[TempTen].SetActive(false);
                TenObj[8].SetActive(true);
                TempTen = 8;
                break;

            case 9:
                TenObj[TempTen].SetActive(false);
                TenObj[9].SetActive(true);
                TempTen = 9;
                break;
        }
        switch (Hundred)
        {
            case 0:
                HundredObj[TempHundred].SetActive(false);
                //HundredObj[0].SetActive(true);
                //TempHundred = 0;
                break;
            case 1:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[1].SetActive(true);
                TempHundred = 1;
                break;

            case 2:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[2].SetActive(true);
                TempHundred = 2;
                break;

            case 3:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[3].SetActive(true);
                TempHundred = 3;
                break;

            case 4:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[4].SetActive(true);
                TempHundred = 4;
                break;

            case 5:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[5].SetActive(true);
                TempHundred = 5;
                break;

            case 6:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[6].SetActive(true);
                TempHundred = 6;
                break;

            case 7:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[7].SetActive(true);
                TempHundred = 7;
                break;

            case 8:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[8].SetActive(true);
                TempHundred = 8;
                break;

            case 9:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[9].SetActive(true);
                TempHundred = 9;
                break;
        }
        // }

    }

    static public void DrawTime(int _Num)
    {
        int One, Ten, Hundred;

        One = _Num % 10;        // 一の位
        Ten = (_Num % 100) / 10;        // 十の位
        Hundred = _Num / 100;   // 百の位
        if (TempTen > 9)
        {
            TempTen = 0;
        }
        //Debug.Log(One);
        //Debug.Log(Ten);

        //if (_Num == 100)
        //{
        //    HundredObj[0].SetActive(true);

        //    OneObj[0].SetActive(true);
        //    TenObj[0].SetActive(true);

        //    //OneObj[TempOne].SetActive(false);
        //    // TenObj[TempTen].SetActive(false);
        //}
        //else
        //{
        // 一の位
        switch (One)
        {
            case 0:
                OneObj[TempOne].SetActive(false);
                OneObj[0].SetActive(true);
                TempOne = 0;
                break;

            case 1:
                OneObj[TempOne].SetActive(false);
                OneObj[1].SetActive(true);
                TempOne = 1;
                break;

            case 2:
                OneObj[TempOne].SetActive(false);
                OneObj[2].SetActive(true);
                TempOne = 2;
                break;

            case 3:
                OneObj[TempOne].SetActive(false);
                OneObj[3].SetActive(true);
                TempOne = 3;
                break;

            case 4:
                OneObj[TempOne].SetActive(false);
                OneObj[4].SetActive(true);
                TempOne = 4;
                break;

            case 5:
                OneObj[TempOne].SetActive(false);
                OneObj[5].SetActive(true);
                TempOne = 5;
                break;

            case 6:
                OneObj[TempOne].SetActive(false);
                OneObj[6].SetActive(true);
                TempOne = 6;
                break;

            case 7:
                OneObj[TempOne].SetActive(false);
                OneObj[7].SetActive(true);
                TempOne = 7;
                break;

            case 8:
                OneObj[TempOne].SetActive(false);
                OneObj[8].SetActive(true);
                TempOne = 8;
                break;

            case 9:
                OneObj[TempOne].SetActive(false);
                OneObj[9].SetActive(true);
                TempOne = 9;
                break;
        }

        // 十の位
        switch (Ten)
        {
            case 0:
                TenObj[TempTen].SetActive(false);
                TenObj[0].SetActive(true);
                TempTen = 0;
                break;
            case 1:
                TenObj[TempTen].SetActive(false);
                TenObj[1].SetActive(true);
                TempTen = 1;
                break;

            case 2:
                TenObj[TempTen].SetActive(false);
                TenObj[2].SetActive(true);
                TempTen = 2;
                break;

            case 3:
                TenObj[TempTen].SetActive(false);
                TenObj[3].SetActive(true);
                TempTen = 3;
                break;

            case 4:
                TenObj[TempTen].SetActive(false);
                TenObj[4].SetActive(true);
                TempTen = 4;
                break;

            case 5:
                TenObj[TempTen].SetActive(false);
                TenObj[5].SetActive(true);
                TempTen = 5;
                break;

            case 6:
                TenObj[TempTen].SetActive(false);
                TenObj[6].SetActive(true);
                TempTen = 6;
                break;

            case 7:
                TenObj[TempTen].SetActive(false);
                TenObj[7].SetActive(true);
                TempTen = 7;
                break;

            case 8:
                TenObj[TempTen].SetActive(false);
                TenObj[8].SetActive(true);
                TempTen = 8;
                break;

            case 9:
                TenObj[TempTen].SetActive(false);
                TenObj[9].SetActive(true);
                TempTen = 9;
                break;
        }
        switch (Hundred)
        {
            case 0:
                HundredObj[TempHundred].SetActive(false);
                //HundredObj[0].SetActive(true);
                //TempHundred = 0;
                break;
            case 1:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[1].SetActive(true);
                TempHundred = 1;
                break;

            case 2:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[2].SetActive(true);
                TempHundred = 2;
                break;

            case 3:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[3].SetActive(true);
                TempHundred = 3;
                break;

            case 4:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[4].SetActive(true);
                TempHundred = 4;
                break;

            case 5:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[5].SetActive(true);
                TempHundred = 5;
                break;

            case 6:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[6].SetActive(true);
                TempHundred = 6;
                break;

            case 7:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[7].SetActive(true);
                TempHundred = 7;
                break;

            case 8:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[8].SetActive(true);
                TempHundred = 8;
                break;

            case 9:
                HundredObj[TempHundred].SetActive(false);
                HundredObj[9].SetActive(true);
                TempHundred = 9;
                break;
        }
        // }

    }

    static public int GetScore()
    {
        return NowScore;
    }
}
