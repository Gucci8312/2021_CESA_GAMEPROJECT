// @file   StageSelect.cs
// @brief  ステージセレクト定義クラス
// @author T,Cho
// @date   2021/04/14 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//すべての引数はthisでお願いします。

// @name   StageSelect
// @brief  ステージセレクト定義
public static class StageSelect
{
    static public Image m_fadeImage;           //フェードゲームオブジェクトを選択（UIImage）
    static public Fade m_fade;                                            //フェードクラスを取得

    [SerializeField] static private Slider m_gauge = default;              //ローディング画面のスライダー
    static private AsyncOperation m_async;                                 //同期処理

    // @name   GoStageSelect
    // @brief  ステージセレクト画面への遷移
    static public void GoStageSelect(MonoBehaviour monoBehaviour)
    {
        m_fade.StartFadeOut();
        monoBehaviour.StartCoroutine(Loading("StageSelectScene"));
    }

    // @name   GoStageSelect
    // @brief  ステージセレクト画面への遷移
    static public void GoTitleScene(MonoBehaviour monoBehaviour)
    {
        if (Time.timeScale == 0f) Time.timeScale = 1.0f;
        m_fade.StartFadeOut();
        monoBehaviour.StartCoroutine(Loading("TitleScene"));
    }

   static public void LoadStage(int _num, MonoBehaviour monoBehaviour)
    {
      m_fade.gameObject.SetActive(true);
      m_fade.StartFadeOut();
       monoBehaviour.StartCoroutine(Loading("Stage " + _num));
    }

    // @name   Loading
    // @brief  NowLodingに対応させるための関数
   public static IEnumerator Loading(string _stageName)
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
               // m_fadeImage.gameObject.SetActive(false);
                m_fade.fadeFinished = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // @name   ClickGameEndBotton
    // @brief  ゲーム終了ボタン
   static public void ClickGameEndBotton()
    {
        //Debug.Log("ゲーム終了ボタンが押された");
        Application.Quit();
    }
}
