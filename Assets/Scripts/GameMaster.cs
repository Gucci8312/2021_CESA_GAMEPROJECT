using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Menu.active == true)
            {  
                Menu.active = false;
            }
            else
            {
                Menu.active = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(Player.GetComponent<PlayerMove>().GetCollisionState())
        {
          //  SceneManager.LoadScene("TittleScene");
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
