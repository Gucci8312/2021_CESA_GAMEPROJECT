using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSelectManeger : MonoBehaviour
{
    [SerializeField] GameObject m_soundManagerPrefab;          //生成用プレハブ


    bool MenuFlg;
    public GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.StopBGM();
        SoundManager.PlayBgmName("stageselect");
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
        if(Controler.GetMenuButtonFlg())
        {
            MenuFlg = !MenuFlg;
            Menu.SetActive(MenuFlg);
        }
    }

    public bool GetMenuFlg()
    {
        if(Menu.activeSelf== false)
        {
            MenuFlg = false;
        }
        return MenuFlg;
    }
}
