using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UIを右からスライドさせるスクリプト
public class ActiveUI : MonoBehaviour
{
    public enum UIColors
    {
        NONE=0,
        SPRITERENDER,//SpriteRendererコンポーネント
        IMAGE,//Imageコンポーネント
        TEXT,//Textコンポーネント
    }
    /*public*/ UIColors UiColors;

    /*public*/ Vector3 SlidePos;//スライドさせる座標（初期座標）
    /*public*/ Vector3 OldSlidePos;//スライドさせる前の座標

    Transform ThisTransform;//メニューのTransform

    SpriteRenderer SpriteColor;//SpriteRendererコンポーネント
    Image ImageColor;//Imageコンポーネント
    Text TextColor;//Textコンポーネント

    static public float SlideTime = 1.0f;//スライドさせたい時間（秒）
    static public float NowSlideTime = 0;//現在スライドしている時間（秒）

    bool Onceflag=true;//スライドしてないとき一回だけ実行させる用

    // Start is called before the first frame update
    void Awake()
    {
        ThisTransform = this.GetComponent<Transform>();

        SlidePos = ThisTransform.localPosition;
        OldSlidePos = ThisTransform.localPosition;
        OldSlidePos.x += 400;//画面外に離れさせる
        ThisTransform.localPosition = OldSlidePos;

        if (this.GetComponent<SpriteRenderer>() != null)
        {
            SpriteColor = this.GetComponent<SpriteRenderer>();
            UiColors = UIColors.SPRITERENDER;
        }
        else if (this.GetComponent<Image>() != null)
        {
            ImageColor = this.GetComponent<Image>();
            UiColors = UIColors.IMAGE;
        }
        else if (this.GetComponent<Text>() != null)
        {
            TextColor = this.GetComponent<Text>();
            UiColors = UIColors.TEXT;
        }


        if (!ActiveUIManager.MenuInOutFlag)//メニューが閉じられるとき
        {
            ThisTransform.localPosition = OldSlidePos;
            AlphaColorChange(0);
        }
        if (ActiveUIManager.MenuInOutFlag)//メニューが開かれるとき
        {
            ThisTransform.localPosition = SlidePos;
            AlphaColorChange(1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ActiveUIManager.SlideFlag)//スライドしてるとき
        {
            Onceflag = true;

            Vector3 StartPos, GoalPos;//始点、終点座標
            StartPos = GoalPos = Vector3.zero;

            float StartA, GoalA;//始点、終点アルファ
            StartA = GoalA = 0;

            if (!ActiveUIManager.MenuInOutFlag)//メニューが閉じられるとき
            {
                StartPos = SlidePos;
                GoalPos = OldSlidePos;
                StartA = 1;
                GoalA = 0;
            }
            if (ActiveUIManager.MenuInOutFlag)//メニューが開かれるとき
            {
                GoalPos = SlidePos;
                StartPos = OldSlidePos;
                StartA = 0;
                GoalA = 1;
            }

            //線形補間でスライド
            ThisTransform.localPosition = SenkeiHokan(StartPos, GoalPos, ActiveUIManager.NowSlideTime, 0, ActiveUIManager.SlideTime);
            float a= SenkeiHokan(StartA, GoalA, ActiveUIManager.NowSlideTime, 0, ActiveUIManager.SlideTime);
            AlphaColorChange(a);
        }
        else//スライドしてないとき
        {
            if (Onceflag)
            {
                if (!ActiveUIManager.MenuInOutFlag)//メニューが閉じられるとき
                {
                    ThisTransform.localPosition = OldSlidePos;
                    AlphaColorChange(0);
                }
                if (ActiveUIManager.MenuInOutFlag)//メニューが開かれるとき
                {
                    ThisTransform.localPosition = SlidePos;
                    AlphaColorChange(1);
                }
                Onceflag = false;
            }
        }
    }

    //アルファ値を変更する関数（引数0～1）
    private void AlphaColorChange(float _a)
    {
        Color color;

        switch (UiColors)
        {
             
            case UIColors.NONE:
                break;

            case UIColors.SPRITERENDER:
                color = SpriteColor.color;
                color.a = _a;
                SpriteColor.color = color;
                break;

            case UIColors.IMAGE:
                color = ImageColor.color;
                color.a = _a;
                ImageColor.color = color;
                break;

            case UIColors.TEXT:
                color = TextColor.color;
                color.a = _a;
                TextColor.color = color;
                break;
        }
    }

    //	P0：始点　,P1：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private Vector3 SenkeiHokan(Vector3 P0, Vector3 P1, float t, float t0, float t1)
    {
        Vector3 pos = P0;
        pos.x = P0.x + (P1.x - P0.x) * (t - t0) / (t1 - t0);
        pos.y = P0.y + (P1.y - P0.y) * (t - t0) / (t1 - t0);

        return pos;
    }

    //	S：始点　,G：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private float SenkeiHokan(float S, float G, float t, float t0, float t1)
    {
        float n;
        n = S + (G - S) * (t - t0) / (t1 - t0);
        return n;
    }
}
