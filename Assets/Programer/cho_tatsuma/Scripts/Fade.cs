// @file   Fade
// @brief  フェードイン・フェードアウト定義
// @author T,Cho
// @date   2021/04/13 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// @name   Fade
// @brief  フェードイン・フェードアウト
public class Fade : MonoBehaviour
{
    [SerializeField] Image m_image = default;                 //フェードイン・アウトに使うためのイメージボード
    bool fadeFlg = false;                            //フェードするかしないかを判定するためのフラグ
    public bool fadeFinished;                       //フェードが完了したかどうかを返す
    // Start is called before the first frame update
    void Start()
    {
        fadeFlg = false;
        fadeFinished = false;
        m_image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        m_image.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // @name   StartFadeIn
    // @brief  フェードイン呼び出し
    public void StartFadeIn()
    {
        if (fadeFlg) return;
        m_image.color = new Color(0.0f, 0.0f, 0.0f, 1.0f) ;
        m_image.gameObject.SetActive(true);
        fadeFlg = true;
        fadeFinished = false;

        StartCoroutine(FadeIn());
    }

    // @name   StartFadeOut
    // @brief  フェードアウト呼び出し
    public void StartFadeOut()
    {
        if (fadeFlg) return;
        m_image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        m_image.gameObject.SetActive(true);
        fadeFlg = true;
        fadeFinished = false;
        Debug.Log("StartFadeOutCoroutine");
        StartCoroutine(FadeOut());
    }

    // @name   FadeIn
    // @brief  フェードイン実行処理
    IEnumerator FadeIn()
    {
        for (float i = 1.0f; i >= 0.0f; i -= 0.1f){
            m_image.color = new Color(0.0f, 0.0f, 0.0f, i);
            if(m_image.color.a <= 0.0f)
            {
                //終了処理
                fadeFinished = true;
            }
           yield return new WaitForSeconds(0.1f);
        }
        m_image.gameObject.SetActive(false);
       yield return null;
    }

    // @name   FadeOut
    // @brief  フェードアウト実行処理
    IEnumerator FadeOut()
    {
        for (float i = 0.0f; m_image.color.a <= 1.0f; i += 0.1f)
        {
            m_image.color = new Color(0.0f, 0.0f, 0.0f, i);
            Debug.Log(m_image.color.a);
            if(m_image.color.a > 1.0f)
            {
                //終了処理
                Debug.Log("finish");
                fadeFinished = true;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
