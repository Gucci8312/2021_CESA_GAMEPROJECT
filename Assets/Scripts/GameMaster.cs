using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
     //   Menu= GameObject.Find("Menu");
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
        //Scene NowScene = SceneManager.GetActiveScene();   // エラーなくなればこっちつかう
       SceneManager.LoadScene("Stage1");
    }

    // タイトルボタンが押されたとき
    public void ClickTittleBotton()
    {
        //Debug.Log("タイトルボタンが押された");
        SceneManager.LoadScene("TittleScene");
    }
}
