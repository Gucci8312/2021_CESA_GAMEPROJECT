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
    bool SoundFlg;
    public GameObject SoundObj;
    // GameObject SoundRes;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, WindowButton[0].transform.position.y, Pos.z);
        // SoundRes = (GameObject)Resources.Load("VolumeSettings");
        PauseManager.GameObjectFindInit();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, WindowButton[Idx].transform.position.y, Pos.z);

        if (!SoundFlg)
        {
            if (Controler.GetUpButtonFlg())
            {
                Idx--;
                if (Idx < 0)
                {
                    Idx++;
                }
            }
            else if (Controler.GetDownButtonFlg())
            {
                Idx++;
                if (Idx > WindowButton.Length - 1)
                {
                    Idx--;
                }
            }
            if (Controler.GetCanselButtonFlg())
            {
                Window.SetActive(!Window.activeSelf);
                //Time.timeScale = 1.0f;
                PauseManager.OffPause();
                //if(SoundFlg)
                //{
                //    Destroy(SoundObj);
                //}
            }
        }
        else
        {
            if (Controler.GetCanselButtonFlg())
            {
                SoundFlg = false;
                // Destroy(SoundObj);
                SoundObj.SetActive(false);
            }
        }


        if (Controler.SubMitButtonFlg())
        {
            if (WindowButton[Idx].name == "PLAY")
            {
                Window.SetActive(!Window.activeSelf);
                //Time.timeScale = 1.0f;
                PauseManager.OffPause();
            }
            else if (WindowButton[Idx].name == "RETRY")
            {
                RestartBotton();
               // Time.timeScale = 1.0f;
                PauseManager.OffPause();
            }
            else if (WindowButton[Idx].name == "STAGESELECT")
            {
                StageSelectBotton();
                // Time.timeScale = 1.0f;
                PauseManager.OffPause();
            }
            else if (WindowButton[Idx].name == "END")
            {
                GameEnd();
            }
            else if (WindowButton[Idx].name == "NEXTSTAGE")
            {
                //Time.timeScale = 1.0f;
                PauseManager.OffPause();
                StageSelect.LoadStage(NextStageNum, this);
            }
            else if (WindowButton[Idx].name == "SOUND")
            {
                SoundFlg = true;
                SoundObj.SetActive(true);
                // SoundObj = (GameObject)Instantiate(SoundRes, new Vector3(-80.0f, 25.0f, -290.0f), Quaternion.identity);
                // SoundObj.transform.parent=this.gameObject.transform;
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
        PauseManager.OffPause();
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
