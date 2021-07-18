using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeAttack : MonoBehaviour
{
    static private int MaxTime;
    static private float m_time;                    //ゲーム時間
    [Header("時間切れかどうか")]
    static public bool timeUp;                      //時間切れかどうか
    [Header("時間制限")]
    public int timeLimit;                           //時間制限

    // Start is called before the first frame update
    void Start()
    {
        m_time = timeLimit;
        MaxTime = timeLimit;
        timeUp = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Pause状態じゃなければタイムを減算
        if (!PauseManager.pause_value)
        {
            MinusTime();
        }

        //時間切れになったら
        if(m_time <= 0f)
        {
            //ゲームオーバー処理
            timeUp = true;
        }

        NumControl.DrawTime(GetTime());
        Debug.Log(GetTime());
    }

    //@name     MinusTime
    //@brief    時間減算
    private void MinusTime()
    {
        m_time -= Time.deltaTime;
    }

    //@name     GetTime
    //@brief    ゲーム内の時間を取る     
    static public int GetTime()
    {
        return (int)m_time;
    }

    //@name     GetMaxTime
    //@brief    もともとのタイムリミット自体を返す     
    static public int GetMaxTime()
    {
        return MaxTime;
    }
    static public void SetTimer(int _Time)
    {
        m_time = _Time;
        MaxTime = _Time;
        timeUp = false;
    }
}

