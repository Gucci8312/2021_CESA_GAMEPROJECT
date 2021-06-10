using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//スコア表示スクリプト（テキストオブジェクトに割り当てる）
public class ScoreTime : MonoBehaviour
{
    public float T_Seconds;// 分
    public float T_Minute;// 秒

    //ランク　3桁目以降が分　2桁、1桁が秒
    public int S_Time = 010;
    public int A_Time = 020;
    public int B_Time = 030;

    //private GameObject Gamemaneger;
    //private ButtonCall ButtonC;

    public GameObject rank;                         //ランクのオブジェクト（子にS、A、B、Cの順で入れる）

    //private GameObject player;

    static public bool StopFlag = false;            //一時停止させるかどうか
    static public bool ShowFlag = false;            //スコアを表示させるかどうか（trueで表示、その後falseになる）


    // Start is called before the first frame update
    void Start()
    {
        rank.SetActive(false);//非表示
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ScoreTime.ShowFlag = true;

        }

        if (!StopFlag) 
        {
            TimeCounter();
        }

        if (ShowFlag)
        {
            ShowTimeRank();
            ScoreTime.ShowFlag = false;
        }
    }

    //時間計測
    private void TimeCounter()
    {
        T_Seconds += Time.deltaTime;
        if (T_Seconds >= 60)
        {
            T_Minute += 1;
            T_Seconds = 0;
        }
    }

    //スコア表示
    private void ShowTimeRank()
    {
        float Time = (T_Minute * 100) + T_Seconds;//分数と秒数

        if (Time_Rank(Time) == 0)//S評価
        {
            this.GetComponent<Text>().text = T_Minute + "分" + ((int)T_Seconds) + "秒 "; //得点を表示する
            rank.SetActive(true);//表示
            rank.transform.GetChild(0).gameObject.SetActive(true);//表示
        }
        else if (Time_Rank(Time) == 1)//A評価
        {
            this.GetComponent<Text>().text = T_Minute + "分" + ((int)T_Seconds) + "秒 "; //得点を表示する
            rank.SetActive(true);//表示
            rank.transform.GetChild(1).gameObject.SetActive(true);//表示

        }
        else if (Time_Rank(Time) == 2)//B評価
        {
            this.GetComponent<Text>().text = T_Minute + "分" + ((int)T_Seconds) + "秒 "; //得点を表示する
            rank.SetActive(true);//表示
            rank.transform.GetChild(2).gameObject.SetActive(true);//表示

        }
        else//D評価
        {
            this.GetComponent<Text>().text = T_Minute + "分" + ((int)T_Seconds) + "秒 "; //得点を表示する
            rank.SetActive(true);//表示
            rank.transform.GetChild(3).gameObject.SetActive(true);//表示
        }
    }

    //時間によってスコアを判断する関数
    private float Time_Rank(float _time)
    {
        if (_time < S_Time)
        {
            return 0;
        }
        else if (_time >= S_Time && _time < A_Time)
        {
            return 1;
        }
        else if (_time >= A_Time && _time < B_Time)
        {
            return 2;
        }

        return 3;
    }
}
