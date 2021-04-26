using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrearOverCursor : MonoBehaviour
{
    public GameObject[] Button;
    public int Idx = 0;
    bool InputFlg;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, Button[0].transform.position.y, Pos.z);
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
            transform.position = new Vector3(Pos.x, Button[Idx].transform.position.y, Pos.z);
            InputFlg = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) | 0 > Input.GetAxis("Vertical") & InputFlg == false)
        {
            Idx++;
            if (Idx > Button.Length - 1)
            {
                Idx--;
            }
            transform.position = new Vector3(Pos.x, Button[Idx].transform.position.y, Pos.z);
            InputFlg = true;
        }
        else if (0.0f == Input.GetAxis("Vertical"))
        // else if(-0.1 > Input.GetAxis("Vertical")| 0.1 < Input.GetAxis("Vertical"))
        {
            InputFlg = false;
        }

        if (Controler.SubMitButtonFlg())
        {
            if (Pos.y == Button[0].transform.position.y)
            {        
                RestartBotton();

            }
            else if (Pos.y == Button[1].transform.position.y)
            {
                TitleButton();
            }
        }
    }

    public void CursorPosSet(int _Idx)
    {
        Vector3 Pos = this.gameObject.transform.position;
        transform.position = new Vector3(Pos.x, Button[_Idx].transform.position.y, Pos.z);
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
