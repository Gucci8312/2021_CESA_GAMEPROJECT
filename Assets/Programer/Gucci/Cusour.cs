using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cusour : MonoBehaviour
{
    public GameObject[] WindowButton;
    public GameObject Window;
    int Idx = 0;
    public int NextStageNum;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, WindowButton[0].transform.position.y, Pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = this.gameObject.transform.position;

        if (Controler.UpButtonFlg())
        {
            Idx--;
            if (Idx < 0)
            {
                Idx++;
            }
            transform.position = new Vector3(Pos.x, WindowButton[Idx].transform.position.y, Pos.z);
        }
        else if (Controler.DownButtonFlg())
        {
            Idx++;
            if (Idx > WindowButton.Length - 1)
            {
                Idx--;
            }
            transform.position = new Vector3(Pos.x, WindowButton[Idx].transform.position.y, Pos.z);
        }


        if (Controler.SubMitButtonFlg())
        {
            if (WindowButton[Idx].name == "PLAY")
            {
                Window.SetActive(!Window.activeSelf);
                Time.timeScale = 1.0f;
            }
            else if (WindowButton[Idx].name == "RETRY")
            {
                RestartBotton();
                Time.timeScale = 1.0f;
            }
            else if (WindowButton[Idx].name == "STAGESELECT")
            {
                StageSelectBotton();
                Time.timeScale = 1.0f;
            }
            else if (WindowButton[Idx].name == "END")
            {
                GameEnd();
            }
            else if (WindowButton[Idx].name == "NEXTSTAGE")
            {
                Time.timeScale = 1.0f;
                StageSelect.LoadStage(NextStageNum, this);
            }
        }
    }

    public void CursorPosSet(int _Idx)
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, WindowButton[_Idx].transform.position.y, Pos.z);
    }

    // エスケープボタンが押されたとき
    public void EscapeBotton()
    {
        // Debug.Log("エスケープボタンが押された");
        Window.active = false;
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
