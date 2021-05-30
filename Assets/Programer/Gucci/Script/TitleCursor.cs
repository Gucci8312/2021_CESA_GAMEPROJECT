using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCursor : MonoBehaviour
{
    public GameObject[] WindowButton;
    public GameObject Window;
    int Idx = 0;

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

        if (Controler.GetUpButtonFlg())
        {
            Idx--;
            if (Idx < 0)
            {
                Idx++;
            }
            transform.position = new Vector3(Pos.x, WindowButton[Idx].transform.position.y, Pos.z);
        }
        else if (Controler.GetDownButtonFlg())
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
            if (WindowButton[Idx].name == "Yes")
            {
                GameEnd();
                Time.timeScale = 1.0f;
            }
            else if (WindowButton[Idx].name == "No")
            {
                Window.SetActive(!Window.activeSelf);
                Time.timeScale = 1.0f;
            }   
        }
        if(Controler.GetCanselButtonFlg())
        {
            Window.SetActive(false);
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

    public void GameEnd()
    {
        UnityEngine.Application.Quit();
    }
}
