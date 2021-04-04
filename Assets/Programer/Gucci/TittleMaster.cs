using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // エリアセレクトボタン
    public void ClickAreaSelectBotton()
    {
        //Debug.Log("エリアセレクトボタンが押された");
        SceneManager.LoadScene("AreaSelectScene");
    }

    // ゲーム終了ボタン
    public void ClickGameEndBotton()
    {
        //Debug.Log("ゲーム終了ボタンが押された");
        Application.Quit();
    }
}
