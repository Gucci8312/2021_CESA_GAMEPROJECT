﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cusour : MonoBehaviour
{
    public GameObject[] MenuButton;
    public GameObject Menu;
    // GameObject SoundObj;
    public int Idx = 0;
    bool InputFlg;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, MenuButton[0].transform.position.y, Pos.z);
        Idx = 0;
        InputFlg = false;
        // SoundObj = GameObject.Find("VolumeSettings");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = this.gameObject.transform.position;

        if (Input.GetKeyDown(KeyCode.UpArrow) | 0 < Input.GetAxis("Vertical") & InputFlg == false)
        // if (Controler.Up())
        {
            Idx--;
            if (Idx < 0)
            {
                Idx++;
            }
            transform.position = new Vector3(Pos.x, MenuButton[Idx].transform.position.y, Pos.z);
            InputFlg = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) | 0 > Input.GetAxis("Vertical") & InputFlg == false)
        {
            Idx++;
            if (Idx > MenuButton.Length - 1)
            {
                Idx--;
            }
            transform.position = new Vector3(Pos.x, MenuButton[Idx].transform.position.y, Pos.z);
            InputFlg = true;
        }
        else if (0.0f == Input.GetAxis("Vertical"))
        // else if(-0.1 > Input.GetAxis("Vertical")| 0.1 < Input.GetAxis("Vertical"))
        {
            InputFlg = false;
        }

        if (Controler.SubMitButtonFlg())
        {
            if (Pos.y == MenuButton[0].transform.position.y)
            {
                Time.timeScale = 1.0f;
                Menu.SetActive(!Menu.activeSelf);
            }
            else if (Pos.y == MenuButton[1].transform.position.y)
            {
                RestartBotton();
            }
            else if (Pos.y == MenuButton[2].transform.position.y)
            {
                TitleButton();
            }
            else if (Pos.y == MenuButton[3].transform.position.y)
            {
                // SoundObj.SetActive(true);
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
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // タイトルボタンが押されたとき
    public void TitleButton()
    {
        //Debug.Log("タイトルボタンが押された");
        SceneManager.LoadScene("TitleScene");
    }
}
