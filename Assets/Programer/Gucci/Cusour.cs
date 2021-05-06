using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cusour : MonoBehaviour
{
    public GameObject[] MenuButton;
    public GameObject Menu;
    int Idx = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, MenuButton[0].transform.position.y, Pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = this.gameObject.transform.position;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Idx--;
            if(Idx<0)
            {
                Idx++;
            }
            transform.position = new Vector3(Pos.x, MenuButton[Idx].transform.position.y, Pos.z);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Idx++;
           if(Idx>MenuButton.Length-1)
            {
                Idx--;
            }
            transform.position = new Vector3(Pos.x, MenuButton[Idx].transform.position.y, Pos.z);
        }


        if (Controler.SubMitButtonFlg())
        {
            if (Pos.y == MenuButton[0].transform.position.y)
            {
                Menu.SetActive(!Menu.activeSelf);
                Time.timeScale = 1.0f;
            }
            else if (Pos.y == MenuButton[1].transform.position.y)
            {
                RestartBotton();
                Time.timeScale = 1.0f;
            }
            else if (Pos.y == MenuButton[2].transform.position.y)
            {
                StageSelectBotton();
                Time.timeScale = 1.0f;
            }
            else if (Pos.y == MenuButton[3].transform.position.y)
            {
                // Time.timeScale = 1.0f;
                GameEnd();
            }
        }
    }

    public void CursorPosSet(int _Idx)
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, MenuButton[_Idx].transform.position.y, Pos.z);
    }

    // エスケープボタンが押されたとき
    public void EscapeBotton()
    {
        // Debug.Log("エスケープボタンが押された");
        // Menu.active = false;
    }

    // リスタートボタンが押されたとき
    public void RestartBotton()
    {
        //Debug.Log("リスタートボタンが押された");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // タイトルボタンが押されたとき
    public void StageSelectBotton()
    {
        //Debug.Log("タイトルボタンが押された");
        SceneManager.LoadScene("StageSelectScene");
    }

    public void GameEnd()
    {
        UnityEngine.Application.Quit();
    }
}
