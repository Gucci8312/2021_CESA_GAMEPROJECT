using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSelectManeger : MonoBehaviour
{
    [SerializeField] GameObject m_soundManagerPrefab;          //生成用プレハブ


    bool MenuFlg;
    public GameObject Menu;

   ///* static public */bool MenuFlag = false;                        //true:メニューが開いてる false:閉じてる
    public float SlideTime = 0.25f;//スライドさせたい時間（秒
    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.StopBGM();
        SoundManager.PlayBgmName("stageselect");

        if (this.GetComponent<ActiveUIManager>() == null)//ActiveUIManagerスクリプトがない場合追加
        {
            this.gameObject.AddComponent<ActiveUIManager>();
        }
        this.GetComponent<ActiveUIManager>().Menu = Menu;

    }


    private void Awake()
    {
        GameObject m_soundManager = GameObject.Find("SoundManager(Clone)");     //サウンドマネージャー検索
        //サウンドマネージャーがなければ生成
        if (m_soundManager == null)
        {
            Instantiate(m_soundManagerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ActiveUIManager.SlideFlag)//メニューがスライドしてないとき
        {
            if (!ActiveUIManager.MenuInOutFlag)//メニューが透明になったら
            {
                Menu.SetActive(false);//メニューを消す
            }

            if (Controler.GetMenuButtonFlg())
            {
                //MenuFlg = !MenuFlg;
                //Menu.SetActive(MenuFlg);
                //if (MenuFlg)
                //{
                //    SoundManager.PlaySeName("メニュー開く");
                //}

                ActiveUIManager.SlideFlag = true;
                if (ActiveUIManager.MenuInOutFlag)//メニューが開かれているとき
                {
                    PauseManager.OffPause();
                }
                else//メニューが閉じられているとき
                {
                    Menu.SetActive(true);
                    SoundManager.PlaySeName("メニュー開く");
                    PauseManager.OnPause();

                }
                ActiveUIManager.MenuInOutFlag = !ActiveUIManager.MenuInOutFlag;

            }

        }


        if (Menu.activeSelf)
        {
            MenuFlg = true;
        }
        else
        {
            MenuFlg = false;
        }

        ActiveUIManager.SlideTime = SlideTime; 
    }

    public bool GetMenuFlg()
    {
        //if (Menu.activeSelf == false)
        //{
        //    MenuFlg = false;
        //}
        return MenuFlg;
    }
}
