using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メニューをスライドさせるスクリプト
//GameManagerに割り当てる
public class ActiveMenu : MonoBehaviour
{
    /*public*/ GameObject Menu;

    static public bool SlideFlag = false;//メニューをスライドさせるかどうか(メニューボタン押したときにtrueにすると動く)

    Vector3 SlidePos;//スライドさせる座標（初期座標）
    Vector3 OldSlidePos;//スライドさせる前の座標

    Transform MenuTransform;//メニューのTransform

    public float SlideTime = 1.0f;//スライドさせたい時間（秒）
    private float NowSlideTime=0;//スライドしている時間（秒）

    List<SpriteRenderer> UiColor=new List<SpriteRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        Menu = this.GetComponent<GameMaster>().Menu;
        //Menu.SetActive(true);
        MenuTransform = Menu.GetComponent<Transform>();
        SlidePos = MenuTransform.position;

        //Vector3 addpos;
        OldSlidePos = MenuTransform.position;
        OldSlidePos.x += 400;//画面外に離れさせる
        MenuTransform.position = OldSlidePos;

        for (int i = 0; i < MenuTransform.childCount - 1; i++)
        {
            UiColor.Add(MenuTransform.GetChild(i).gameObject.GetComponent<SpriteRenderer>());
        }

        //Menu.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Controler.GetMenuButtonFlg())
        {
            SlideFlag = true;
        }


        if (SlideFlag)
        {
            NowSlideTime += Time.deltaTime;
            if (NowSlideTime >= SlideTime)
            {
                NowSlideTime = SlideTime;
                SlideFlag = false;
            }

            Vector3 StartPos, GoalPos;
            StartPos = GoalPos = Vector3.zero;

            float StartA, GoalA;
            StartA = GoalA = 0;

            if (Menu.activeSelf == false)
            {
                StartPos = SlidePos;
                GoalPos = OldSlidePos;
                StartA = 1;
                GoalA = 0;
            }
            if (Menu.activeSelf == true)
            {
                GoalPos = SlidePos;
                StartPos = OldSlidePos;
                StartA = 0;
                GoalA = 1;
            }

            MenuTransform.position = SenkeiHokan(StartPos, GoalPos, NowSlideTime, 0, SlideTime);
    
            for (int i = 0; i < UiColor.Count; i++)
            {
                Color color = UiColor[i].color;
                color.a = SenkeiHokan(StartA, GoalA, NowSlideTime, 0, SlideTime);
                UiColor[i].color = color;
            }

        }
        else
        {
            NowSlideTime = 0;
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
