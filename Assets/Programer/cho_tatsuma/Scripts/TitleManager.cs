// @file   TitleManager.cs
// @brief  タイトルを管理するクラス定義
// @author T,Cho
// @date   2021/04/23 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// @name   TitleManager
// @brief  タイトルを管理するクラス
public class TitleManager : MonoBehaviour
{   
    [SerializeField]  GameObject m_soundManagerPrefab;          //生成用プレハブ
    public string titleBgm;                                     //タイトルBGM

    static GameObject m_titlePointLight;              //ポイントライトオブジェクト
    [SerializeField]
    GameObject m_pressTextObj;
    FadeTitleText m_fadeTextScript;
    // Start is called before the first frame update
    void Start()
    {
        m_fadeTextScript = m_pressTextObj.gameObject.GetComponent<FadeTitleText>();
    }
    private void OnEnable()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1.0f;
        m_titlePointLight = GameObject.Find("YellowLight");
        m_titlePointLight.GetComponent<TtilePLight>().OnInit();
    }
    private void Update()
    {
        if (m_fadeTextScript.gameStartFlg)
        {
            if (Controler.SubMitButtonFlg())
            {
                StageSelect.GoStageSelect(this);
            }
        }
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
}
