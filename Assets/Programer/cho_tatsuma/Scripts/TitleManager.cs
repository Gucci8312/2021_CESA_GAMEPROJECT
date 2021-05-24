// @file   TitleManager.cs
// @brief  タイトルを管理するクラス定義
// @author T,Cho
// @date   2021/04/23 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
// @name   TitleManager
// @brief  タイトルを管理するクラス
public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject m_soundManagerPrefab;          //生成用プレハブ
    public string titleBgm;                                     //タイトルBGM

    public GameObject[] mobiusArray;                            //メビウスの輪格納配列
    public GameObject mobius;                                  //合体メビウスの輪格納配列
    bool startMobiusAnimation;
    static GameObject m_titlePointLight;              //ポイントライトオブジェクト
    [SerializeField]
    GameObject m_pressTextObj;
    FadeTitleText m_fadeTextScript;
    public GameObject Window;

    [SerializeField] Volume bloom;

    private void Awake()
    {
        GameObject m_soundManager = GameObject.Find("SoundManager(Clone)");     //サウンドマネージャー検索
        //サウンドマネージャーがなければ生成
        if (m_soundManager == null)
        {
            Instantiate(m_soundManagerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }

        for (int i = 0; i < mobiusArray.Length; i++)
        {
            mobiusArray[i].GetComponent<TitleMoveMobius>().enabled = false;
        }
    }

    private void OnEnable()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1.0f;
        m_titlePointLight = GameObject.Find("YellowLight");
    }

    // Start is called before the first frame update
    void Start()
    {
        startMobiusAnimation = false;
        m_fadeTextScript = m_pressTextObj.gameObject.GetComponent<FadeTitleText>();
    }

    private void Update()
    {
        SoundManager.CheckLoop();
        if (Window.activeSelf == false)
        {
            if (Controler.SubMitButtonFlg())
            {
                m_fadeTextScript.gameStartFlg = true;
            }
            else if (Controler.GetCanselButtonFlg())
            {
                UnityEngine.Application.Quit();
            }
        }

        if (Controler.GetMenuButtonFlg())
        {
            Bloom bloom_propaty;
            bloom.profile.TryGet(out bloom_propaty);
            if (bloom_propaty.intensity.value == 5f)
                bloom_propaty.intensity.value = 0f;
            else if (bloom_propaty.intensity.value == 0f)
                bloom_propaty.intensity.value = 5f;
        }

        if (Controler.GetMenuButtonFlg())
        {
            Window.SetActive(!Window.activeSelf);
        }
    }
}
