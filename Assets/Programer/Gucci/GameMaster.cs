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

<<<<<<< HEAD:Assets/Programer/Gucci/Script/GameMaster.cs
=======
<<<<<<< HEAD:Assets/Programer/Gucci/GameMaster.cs
=======
>>>>>>> f5ab7ec7c429af89752733e72fb77e5076247480:Assets/Programer/Gucci/GameMaster.cs
    private float frame_count = 0;
    private int scoreUp = 0;
    GameObject ScoreObj;
    ObjectDraw objDraw;

    // public int DrowScore;
>>>>>>> ac80fc59d50ecb856359ffdfa34f34eba4e94ef9:Assets/Programer/Gucci/Script/GameMaster.cs
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
        Player = GameObject.Find("Player");                                        //全てのメビウス取得
       // Menu = GameObject.Find("Menu");                                        //全てのメビウス取得

        Application.targetFrameRate = 60;
        StartCoroutine("CheckLoop");
        Invoke("OnStartBGM", 0.1f);
<<<<<<< HEAD:Assets/Programer/Gucci/GameMaster.cs
=======
        UI = GameObject.Find("UI");
        NumControl.InitNum();
        SupureManager.ResetScore();
        ScoreObj = GameObject.Find("Score");
        ScoreObj.GetComponent<ExpantionShrink>().musicOn = false;
        objDraw = GetComponent<ObjectDraw>();
<<<<<<< HEAD:Assets/Programer/Gucci/Script/GameMaster.cs
=======
>>>>>>> ac80fc59d50ecb856359ffdfa34f34eba4e94ef9:Assets/Programer/Gucci/Script/GameMaster.cs
>>>>>>> f5ab7ec7c429af89752733e72fb77e5076247480:Assets/Programer/Gucci/GameMaster.cs
    }

    void OnStartBGM()
    {
        SoundManager.PlayBgmName(BgmName);
    }
    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD:Assets/Programer/Gucci/Script/GameMaster.cs
        //if (frame_count % 3 == 0)
        if (objDraw != null)
        {
=======
<<<<<<< HEAD:Assets/Programer/Gucci/GameMaster.cs
        if(Input.GetKeyDown(KeyCode.Escape)| Controler.GetMenuButtonFlg())
        {
            Debug.Log("メニューボタン押された");
            if(Menu.active == true)
            {  
                Menu.active = false;
                Time.timeScale = 1.0f;
            }
            else
=======
        //if (frame_count % 3 == 0)
        if (objDraw != null)
        {
>>>>>>> f5ab7ec7c429af89752733e72fb77e5076247480:Assets/Programer/Gucci/GameMaster.cs
            objDraw.Object_Draw_Update(SupureManager.GetScore());
        }
        if (scoreUp < (int)SupureManager.GetScore())
        {
            scoreUp++;
            ScoreObj.GetComponent<ExpantionShrink>().isExpantion = true;
            NumControl.DrawScore(scoreUp);
        }
        if (!UI.GetComponent<UIManeger>().GameClearFlg && !UI.GetComponent<UIManeger>().GameOverFlg && GameStart.Blinking_Flag)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Controler.GetMenuButtonFlg())
>>>>>>> ac80fc59d50ecb856359ffdfa34f34eba4e94ef9:Assets/Programer/Gucci/Script/GameMaster.cs
            {
                Menu.active = true;
                Time.timeScale = 0.0f;
            }
        }
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
}
