using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobiusColor : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] Color = new Material[4];//色のマテリアル（全てのメビウスに色を共通させる）
    private int NowColorNum;//現在のメビウスの色の番号

    private GameObject[] MobiusObjs = new GameObject[2];//色を変えたいメビウスオブジェクト
    PlayerMove player;

    private bool ColorSameFlag=false;//プレイヤーが触れているメビウスの色と同じかどうか

    public bool ColorChangeFlag = false;//色変えるかどうか

    void Start()
    {
        MobiusObjs[0] = this.transform.Find("メビウス").gameObject;
        MobiusObjs[1] = this.transform.Find("メビウス.001").gameObject;

        Material MobiusMate = MobiusObjs[0].GetComponent<Renderer>().material;//メビウスにセットされているマテリアルを取得

        for(int i = 0; i < Color.Length; i++)
        {
            if (Color[i].color==MobiusMate.color)
            {
                NowColorNum = i;//色の番号を取得
                break;
            }
        }

        player = GameObject.Find("Player").GetComponent<PlayerMove>();

    }

    // Update is called once per frame
    void Update()
    {
        if (ColorChangeFlag)
        {
            ColorChange();
        }
        ColorCheck();
    }

    private void ColorChange()//色を変える
    {
        if (Input.GetKeyDown(KeyCode.Space))//2ビートごとに（今はスペースキーで変えるのみ）
        {
            NowColorNum++;
            if (NowColorNum > Color.Length - 1)
            {
                NowColorNum = 0;
            }
            for (int i = 0; i < MobiusObjs.Length; i++) {
                MobiusObjs[i].GetComponent<Renderer>().material = Color[NowColorNum];
            }
        }
    }

    private void ColorCheck()//色が同じかどうか調べる
    {
        if (NowColorNum == player.GetNowMobiusColor())//自分の色番号とプレイヤーが触れているメビウスの色番号が同じ場合
        {
            ColorSameFlag = true;
        }
        else
        {
            ColorSameFlag = false;
        }
    }

    public int GetNowColorNum()
    {
        return NowColorNum;
    }

    public bool GetColorSameFlag()
    {
        return ColorSameFlag;
    }
}
