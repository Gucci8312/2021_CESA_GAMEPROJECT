using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameMaster : MonoBehaviour
{
    public GameObject Menu;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        //   Menu= GameObject.Find("Menu");
        Player = GameObject.Find("Player");                                        //全てのメビウス取得

        Application.targetFrameRate = 60;
        Player = GameObject.Find("Player");
       // SoundManager.Instance.PlayBgmName("Alley");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)| Controler.GetMenuButtonFlg())
        {
            Debug.Log("メニューボタン押された");
            if(Menu.active == true)
            {  
                Menu.active = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                Menu.active = true;
                Time.timeScale = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if(Player.GetComponent<PlayerMove>().GetCollisionState())
        {
            SceneManager.LoadScene("TittleScene");
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
