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

    public GameObject[] mobiusArray;                            //メビウスの輪格納配列
    public GameObject mobius;                                  //合体メビウスの輪格納配列
    bool startMobiusAnimation;
    static GameObject m_titlePointLight;              //ポイントライトオブジェクト
    [SerializeField]
    GameObject m_pressTextObj;
    FadeTitleText m_fadeTextScript;
    public GameObject Window;

  
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
        m_titlePointLight.GetComponent<TtilePLight>().OnInit();
    }

    // Start is called before the first frame update
    void Start()
    {
        startMobiusAnimation = false;
        m_fadeTextScript = m_pressTextObj.gameObject.GetComponent<FadeTitleText>();
    }

    private void Update()
    {
        if (m_fadeTextScript.gameStartFlg)
        {
            if (Controler.SubMitButtonFlg())
            {
                StageSelect.GoStageSelect(this);
            }
            else if(Controler.GetCanselButtonFlg())
            {
                UnityEngine.Application.Quit();
            }
        }

        if (!startMobiusAnimation && m_titlePointLight.GetComponent<TtilePLight>().titleAnimationFinished)
        {
            startMobiusAnimation = true;
            for (int i = 0; i < mobiusArray.Length - 2; i++)
            {
                mobiusArray[i].GetComponent<TitleMoveMobius>().enabled = true;
            }
        }

        if (mobiusArray[0].GetComponent<TitleMoveMobius>().move_flg_back && mobiusArray[0].GetComponent<TitleMoveMobius>().enabled)
        {
            mobiusArray[0].GetComponent<TitleMoveMobius>().enabled = false;
            mobiusArray[1].GetComponent<TitleMoveMobius>().enabled = false;
            mobius.GetComponent<MobiusAttachPos>().m_nowMobiusNo = 2;
            mobiusArray[2].GetComponent<TitleMoveMobius>().Start();
            mobiusArray[3].GetComponent<TitleMoveMobius>().Start();
            mobiusArray[2].GetComponent<TitleMoveMobius>().enabled = true;
            mobiusArray[3].GetComponent<TitleMoveMobius>().enabled = true;
        }

        if (mobiusArray[2].GetComponent<TitleMoveMobius>().move_flg_back && mobiusArray[2].GetComponent<TitleMoveMobius>().enabled)
        {
            mobiusArray[0].GetComponent<TitleMoveMobius>().enabled = true;
            mobiusArray[1].GetComponent<TitleMoveMobius>().enabled = true;
            mobiusArray[0].GetComponent<TitleMoveMobius>().Start();
            mobiusArray[1].GetComponent<TitleMoveMobius>().Start();
            mobius.GetComponent<MobiusAttachPos>().m_nowMobiusNo = 0;
            mobiusArray[2].GetComponent<TitleMoveMobius>().enabled = false;
            mobiusArray[3].GetComponent<TitleMoveMobius>().enabled = false;
        }

        if(Controler.GetCanselButtonFlg())
        {
            Window.SetActive(true);
        }
    }
}
