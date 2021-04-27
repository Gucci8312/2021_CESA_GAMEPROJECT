using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameClear : MonoBehaviour
{
  
    private GameObject[] checkpointobjects;     //配列(チェックポイントのデータを収納)

    public GameObject UI;

    void Start()
    {

    }

    void Update()
    {
        checkpointobjects = GameObject.FindGameObjectsWithTag("CheckPoint");    //CheckPointのタグを鉾に入れる
        print(checkpointobjects.Length);

        if (checkpointobjects.Length == 0)  //チェックポイント0になったら
        {
            Debug.Log(" ゲームクリア");
            UI.active = true;
            Time.timeScale = 0.0f;
        }


        if (UI.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TittleScene");
            }
        }

    }

}

