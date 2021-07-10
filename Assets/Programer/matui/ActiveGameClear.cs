using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームクリア時の演出スクリプト
//GameClearオブジェクトにつける
//（子にGAMECLEAR,CLEARYAZIRUSI,NEXTSTAGE,StAGESELECTのUIが必要）
public class ActiveGameClear : MonoBehaviour
{
    public enum ClearPhase//クリア時の演出順
    {
        PHASE_1 = 0,
        PHASE_2,
    }
    public ClearPhase NowPhase;//現在のクリア演出


    public enum Process//処理
    {
        INIT = 0,//初期化処理
        UPDATE,//更新処理
        UNINIT,//終了処理
    }
    public Process NowProcess;//現在の処理


    public struct ObjParameter//演出に必要な変数を集めた構造体
    {
        public GameObject Obj;
        public Transform ThisTransform;
        public SpriteRenderer SpriteColor;
        public Vector3 InitPos;
        public Vector3 InitScale;
    }

    public ObjParameter ScoreObj;//スコアオブジェクト
    public ObjParameter GameClearObj;//ゲームクリアUIオブジェクト
    public ObjParameter[] StageSelectObj=new ObjParameter[3];//0:STAGESELECT,1:CLEARYASZIRUSI ,2:NEXTSTAGE（最終ステージの時は存在しない）

    int SelectObjDownNum = 0;

    public bool ClearFlag = false;

    //Transform ThisTransform;

    //Vector3 InitPos;
    //Vector3 InitScale;

    //各フェーズで使う時間
    float NowTime_1=0;
    float NowTime_2=0;

    float NextTime;//次のフェーズへ移行するための時間

    // Start is called before the first frame update
    void Start()
    {
        //ThisTransform = this.GetComponent<Transform>();

        ////各初期値を取得
        //InitPos = ThisTransform.position;
        //InitScale = ThisTransform.localScale;
        this.transform.localScale = new Vector3(1, 1, 1);
        Init();

        //ScoreObj.Obj.SetActive(false);
        //GameClearObj.Obj.SetActive(false);
        //StageSelectObj.Obj.SetActive(false);

    }
    //void Awake()
    //{
    //    if (!this.gameObject.activeSelf)
    //    {
    //        this.gameObject.SetActive(true);
    //    }
    //    this.gameObject.SetActive(true);

    //    this.transform.localScale = new Vector3(1, 1, 1);
    //}


    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            ClearFlag = true;
        }

        if (ClearFlag)
        {
            switch (NowPhase)
            {
                case ClearPhase.PHASE_1:
                    ClearPhase1_Update();
                    break;

                case ClearPhase.PHASE_2:
                    ClearPhase2_Update();
                    break;
            }
        }
    }


    private void ClearPhase1_Update()
    {
        float ScaleTime = 0.5f;//拡大、縮小させるまでの時間を設定

        //拡大、縮小時の大きさを設定
        Vector3 MaxScale = GameClearObj.InitScale;
        MaxScale.x += MaxScale.x * 0.5f;
        MaxScale.y += MaxScale.y * 0.5f;

        Vector3 MinScale = GameClearObj.InitScale;
        MinScale.x -= MinScale.x * 0.2f;
        MinScale.y -= MinScale.y * 0.2f;

        Color color;

        switch (NowProcess)
        {
            case Process.INIT:
                if (NowTime_1 > 0.25f)//0.25はプレイヤーが画面真ん中に出るまでの時間
                {
                    ActiveOnOff(GameClearObj.Obj, true);

                    //大きい状態からスタートさせる
                    GameClearObj.ThisTransform.localScale = MaxScale;

                    //透明からスタートさせる
                    color = GameClearObj.SpriteColor.color;
                    color.a = 0;
                    GameClearObj.SpriteColor.color = color;

                    //時間設定
                    NextTime = 3;
                    NowTime_1 = 0;

                    NowProcess = Process.UPDATE;//UPDATEへ移行
                }
                else
                {
                    NowTime_1 += Time.deltaTime;
                }
                break;

            case Process.UPDATE:
                if (NowTime_1 < ScaleTime)
                {
                    if (NowTime_1 < ScaleTime / 3)
                    {
                        //小さくさせる
                        GameClearObj.ThisTransform.localScale =
                            SenkeiHokan(MaxScale, MinScale, NowTime_1, 0, ScaleTime / 2);
                    }
                    else
                    {
                        //大きくさせる
                        GameClearObj.ThisTransform.localScale =
                            SenkeiHokan(MinScale, GameClearObj.InitScale, NowTime_1, 0, ScaleTime);
                    }

                    //フェードインする
                    color = GameClearObj.SpriteColor.color;
                    color.a = SenkeiHokan(0,1,NowTime_1,0,ScaleTime);
                    GameClearObj.SpriteColor.color = color;
                   
                }

                if (NowTime_1 > NextTime - 0.5)
                {
                    //フェードアウトさせていく
                    color = GameClearObj.SpriteColor.color;
                    color.a = SenkeiHokan(1, 0, NowTime_1, NextTime - 0.5f, NextTime);
                    GameClearObj.SpriteColor.color = color;

                }


                if (NowTime_1 > NextTime)
                {
                    NowProcess = Process.UNINIT;//UNINITへ移行
                }

                NowTime_1 += Time.deltaTime;

                break;

            case Process.UNINIT:
                GameClearObj.Obj.SetActive(false);

                NowPhase = ClearPhase.PHASE_2;
                NowProcess = Process.INIT;//INITへ移行

                break;
        }
    }

    private void ClearPhase2_Update()
    {
        Vector3 AddPos=Vector3.zero;//下へ移動させる用の変数
        AddPos.y -= 1;

        switch (NowProcess)
        {
            case Process.INIT:
                //必要なオブジェクトを表示
                //ActiveOnOff(ScoreObj.Obj, true);
                for (int i = 0; i < StageSelectObj.Length - SelectObjDownNum; i++)
                {
                    ActiveOnOff(StageSelectObj[i].Obj, true);

                    //透明からスタートさせる
                    Color color = StageSelectObj[i].SpriteColor.color;
                    color.a = 0;
                    StageSelectObj[i].SpriteColor.color = color;
                }
                ScoreObj.ThisTransform.position = new Vector3(175, 100, -150);
                ScoreObj.ThisTransform.localScale = new Vector3(3, 3, 1);

                //時間設定
                NextTime = 0.5f;
                NowTime_2 = 0;

                NowProcess = Process.UPDATE;//UPDATEへ移行
                break;

            case Process.UPDATE:

                if (NowTime_2 > NextTime)
                {
                    //NowProcess = Process.UNINIT;//UNINITへ移行
                }
                else
                {
                    for (int i = 0; i < StageSelectObj.Length - SelectObjDownNum; i++)
                    {
                        //下へ移動させる
                        StageSelectObj[i].ThisTransform.localPosition =
                                SenkeiHokan(StageSelectObj[i].InitPos, StageSelectObj[i].InitPos + AddPos, NowTime_2, 0, NextTime);

                        //フェードインする
                        Color color = StageSelectObj[i].SpriteColor.color;
                        color.a = SenkeiHokan(0, 1, NowTime_2, 0, NextTime);
                        StageSelectObj[i].SpriteColor.color = color;
                    }


                    NowTime_2 += Time.deltaTime;
                }
                break;

            case Process.UNINIT:
               

                break;
        }
    }


    //	S：始点　,G：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private Vector3 SenkeiHokan(Vector3 S, Vector3 G, float t, float t0, float t1)
    {
        Vector3 pos = S;
        pos.x = S.x + (G.x - S.x) * (t - t0) / (t1 - t0);
        pos.y = S.y + (G.y - S.y) * (t - t0) / (t1 - t0);

        return pos;
    }

    //	S：始点　,G：終点　,t：時間　,t0：始点位置での時間　,t1：終点位置での時間
    private float SenkeiHokan(float S, float G, float t, float t0, float t1)
    {
        float n;
        n = S + (G - S) * (t - t0) / (t1 - t0);
        return n;
    }

    private void ActiveOnOff(GameObject _obj,bool _flag)
    {
        if (_obj.activeSelf != _flag)
        {
            _obj.SetActive(_flag);
        }
    }

    //初期化（初回のみ）
    private void Init()
    {
        GameClearObj.Obj = this.transform.Find("GAMECLEAR").gameObject;
        GameClearObj.ThisTransform = GameClearObj.Obj.GetComponent<Transform>();
        GameClearObj.SpriteColor = GameClearObj.Obj.GetComponent<SpriteRenderer>();
        GameClearObj.InitPos = GameClearObj.ThisTransform.localPosition;
        GameClearObj.InitScale = GameClearObj.ThisTransform.localScale;
        ActiveOnOff(GameClearObj.Obj,false);

        ScoreObj.Obj = GameObject.Find("Score");
        ScoreObj.ThisTransform = ScoreObj.Obj.GetComponent<Transform>();
        ScoreObj.SpriteColor = ScoreObj.Obj.GetComponent<SpriteRenderer>();
        ScoreObj.InitPos = ScoreObj.ThisTransform.localPosition;
        ScoreObj.InitScale = ScoreObj.ThisTransform.localScale;
        //ActiveOnOff(ScoreObj.Obj, false);


        StageSelectObj[0].Obj = this.transform.Find("CLEARYASZIRUSI").gameObject;
        StageSelectObj[1].Obj = this.transform.Find("STAGESELECT").gameObject;
        StageSelectObj[2].Obj = this.transform.Find("NEXTSTAGE").gameObject;

        if (!StageSelectObj[2].Obj.activeSelf)
        {
            SelectObjDownNum += 1;
        }

        for (int i = 0; i < StageSelectObj.Length - SelectObjDownNum; i++)
        {
            StageSelectObj[i].ThisTransform = StageSelectObj[i].Obj.GetComponent<Transform>();
            StageSelectObj[i].SpriteColor = StageSelectObj[i].Obj.GetComponent<SpriteRenderer>();
            StageSelectObj[i].InitPos = StageSelectObj[i].ThisTransform.localPosition;
            StageSelectObj[i].InitScale = StageSelectObj[i].ThisTransform.localScale;
            ActiveOnOff(StageSelectObj[i].Obj, false);

        }

    }
}