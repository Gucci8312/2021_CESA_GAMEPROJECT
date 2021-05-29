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
        PauseManager.GameObjectFindInit();
        Application.targetFrameRate = 60;
        StartCoroutine("CheckLoop");
        Invoke("OnStartBGM", 0.1f);
        UI = GameObject.Find("UI");
    }

    void OnStartBGM()
    {
        SoundManager.PlayBgmName(BgmName);
    }
    // Update is called once per frame
    void Update()
    {
        if (!UI.GetComponent<UIManeger>().GameClearFlg && !UI.GetComponent<UIManeger>().GameOverFlg)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Controler.GetMenuButtonFlg())
            {
                Debug.Log("メニューボタン押された");
                if (Menu.active == true)
                {
                    Menu.active = false;
                    // Time.timeScale = 1.0f;
                    PauseManager.OffPause();
                }
                else
                {
                    Menu.active = true;
                    // Time.timeScale = 0.0f;
                    SoundManager.PlaySeName("メニュー開く");
                    PauseManager.OnPause();
                }
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
}
