using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameMaster : MonoBehaviour
{
    [SerializeField] GameObject m_soundManagerPrefab;          //生成用プレハブ

    public GameObject Menu;
    public string BgmName;
    GameObject Player;
    GameObject UI;
    public int ScoreNum = 100;

    private float frame_count = 0;
    ObjectDraw objDraw;
    GameObject ScoreObj;
    CheckPointCount CheckPoint;
    /* static public */
    bool MenuFlag = false;                        //true:メニューが開いてる false:閉じてる
    public float SlideTime = 0.25f;//スライドさせたい時間（秒
    public float light;
    // public int DrowScore;
    private void Awake()
    {
        GameObject m_soundManager = GameObject.Find("SoundManager(Clone)");     //サウンドマネージャー検索
        //サウンドマネージャーがなければ生成
        if (m_soundManager == null)
        {
            Instantiate(m_soundManagerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //   Menu= GameObject.Find("Menu");
        CheckPoint = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        CheckPoint.inci(light);
        Player = GameObject.Find("Player");                                        //全てのメビウス取得
                                                                                   // Menu = GameObject.Find("Menu");                                        //全てのメビウス取得
        PauseManager.GameObjectFindInit();
        Application.targetFrameRate = 60;
        StartCoroutine("CheckLoop");
        Invoke("OnStartBGM", 0.1f);
        UI = GameObject.Find("UI");
        NumControl.InitNum();
        SupureManager.ResetScore();
        objDraw = GetComponent<ObjectDraw>();

        if (this.GetComponent<ActiveUIManager>() == null)
        {
            this.gameObject.AddComponent<ActiveUIManager>();
        }
        this.GetComponent<ActiveUIManager>().Menu = Menu;
    }
    private void OnEnable()
    {
        ScoreObj = GameObject.Find("Score");
        if(ScoreObj!=null)
        {
            ScoreObj.GetComponent<ExpantionShrink>().musicOn = false;
        }
    }
    void OnStartBGM()
    {
        SoundManager.PlayBgmName(BgmName);
    }
    // Update is called once per frame
    void Update()
    {
        //if (frame_count % 3 == 0)
        if (objDraw != null)
        {
            objDraw.Object_Draw_Update(SupureManager.GetScore());
        }
        if (!UI.GetComponent<UIManeger>().GameClearFlg && !UI.GetComponent<UIManeger>().GameOverFlg && GameStart.Blinking_Flag)
        {
            if (!ActiveUIManager.SlideFlag)//メニューがスライドしてないとき
            {
                if (!ActiveUIManager.MenuInOutFlag)//メニューが透明になったら
                {
                    Menu.SetActive(false);//メニューを消す
                }


                if (Input.GetKeyDown(KeyCode.Escape) || Controler.GetMenuButtonFlg())
                {
                    Debug.Log("メニューボタン押された");
                    ActiveUIManager.SlideFlag = true;

                    if (ActiveUIManager.MenuInOutFlag)//メニューが開かれているとき
                    {
                        PauseManager.OffPause();
                    }
                    else//メニューが閉じられているとき
                    {
                        Menu.SetActive(true);
                        SoundManager.PlaySeName("メニュー開く");
                        PauseManager.OnPause();

                    }
                    ActiveUIManager.MenuInOutFlag = !ActiveUIManager.MenuInOutFlag;


                    //if (Menu.active == true)
                    //{
                    //    Menu.active = false;
                    //    // Time.timeScale = 1.0f;
                    //    PauseManager.OffPause();
                    //}
                    //else
                    //{
                    //    Menu.active = true;
                    //    // Time.timeScale = 0.0f;
                    //    SoundManager.PlaySeName("メニュー開く");
                    //    PauseManager.OnPause();
                    //}
                }

            }
        }

        frame_count++;


        if (Menu.activeSelf)
        {
            MenuFlag = true;
        }
        else
        {
            MenuFlag = false;
        }

        ActiveUIManager.SlideTime = SlideTime;
    }

    private void FixedUpdate()
    {
        //if (Player.GetComponent<PlayerMove>().GetCollisionState())
        //{
        //    //SceneManager.LoadScene("TitleScene");
        //}
    }

    IEnumerator CheckLoop()
    {
        while (true)
        {
            SoundManager.CheckLoop();
            yield return new WaitForSeconds(0.01f);
        }
    }
    // エスケープボタンが押されたとき
    public void ClickEscapeBotton()
    {
        // Debug.Log("エスケープボタンが押された");
        Menu.active = false;
        PauseManager.OffPause();
    }

    // リスタートボタンが押されたとき
    public void ClickRestartBotton()
    {
        //Debug.Log("リスタートボタンが押された");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // タイトルボタンが押されたとき
    public void ClickTittleBotton()
    {
        //Debug.Log("タイトルボタンが押された");
        SceneManager.LoadScene("TittleScene");
    }

    public bool GetMenuFlag()
    {
        return MenuFlag;
    }

    

}
