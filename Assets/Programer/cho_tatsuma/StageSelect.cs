// @file   StageSelect.cs
// @brief  ステージセレクト定義クラス
// @author T,Cho
// @date   2021/04/14 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// @name   StageSelect
// @brief  ステージセレクト定義
public class StageSelect : MonoBehaviour
{
    
    [SerializeField] private Image m_fadeImage = default;           //フェードゲームオブジェクトを選択（UIImage）
    private Fade m_fade;                                            //フェードクラスを取得

    [SerializeField] private Slider m_gauge = default;              //ローディング画面のスライダー
    private AsyncOperation m_async;                                 //同期処理

    // Start is called before the first frame update
    void Start()
    {
        //フェードオブジェクトからフェードクラスコンポーネントを取得
        m_fade = m_fadeImage.gameObject.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // @name   GoStageSelect
    // @brief  ステージセレクト画面への遷移
    public void GoStageSelect()
    {
        m_fade.StartFadeOut();
        Debug.Log("StartFadeOut");
        StartCoroutine(Loading("StageSelectScene"));
    }

    public void LoadStage(int _num)
    {
        m_fade.gameObject.SetActive(true);
        m_fade.StartFadeOut();
        StartCoroutine(Loading("Stage" + _num));
    }

    // @name   Loading
    // @brief  NowLodingに対応させるための関数
    IEnumerator Loading(string _stageName)
    {
        while (true)
        {
            //フェードが終わったこと知らせた後
            if (m_fade.fadeFinished)
            {
                //ローディング情報を返す
                m_async = SceneManager.LoadSceneAsync(_stageName);

                //ロード中なら入る文
                while (!m_async.isDone)
                {
                    //ローディング時のゲージのスライダーを進める
                    //var progressVal = Mathf.Clamp01(m_async.progress / 0.9f);
                    //m_gauge.value = progressVal;
                    yield return null;
                }
                //終了処理
                m_fadeImage.gameObject.SetActive(false);
                m_fade.fadeFinished = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // @name   ClickGameEndBotton
    // @brief  ゲーム終了ボタン
    public void ClickGameEndBotton()
    {
        //Debug.Log("ゲーム終了ボタンが押された");
        Application.Quit();
    }
}
