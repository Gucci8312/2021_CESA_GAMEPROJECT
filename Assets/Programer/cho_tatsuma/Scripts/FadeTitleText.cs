// @file   FadeTitleText.cs
// @brief  PressAButtonテキストをフェードさせるクラス定義
// @author T,Cho
// @date   2021/04/26 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   FadeTitleText
// @brief  PressAButtonテキストをフェードさせるクラス
public class FadeTitleText : MonoBehaviour
{
    Color m_textColor;
    bool m_up;
    bool m_down;

    public bool gameStartFlg;
    public bool saveLoadFlg = false;
    bool once;
    // Start is called before the first frame update
    void Start()
    {
        m_textColor.r = 1.0f;
        m_textColor.g = 1.0f;
        m_textColor.b = 1.0f;
        m_textColor.a = 0.2f;
        this.gameObject.GetComponent<SpriteRenderer>().color = m_textColor;
        m_up = false;
        m_down = false;

        gameStartFlg = false;
        once = false;
        StartCoroutine(FadeCroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStartFlg && !once)
        {
            StopCoroutine(FadeCroutine());
            m_textColor.a = 1.0f;
            this.gameObject.GetComponent<SpriteRenderer>().color = m_textColor;
            StartCoroutine(Flash());
            once = true;
        }
    }

    IEnumerator FadeCroutine()
    {
        while (true)
        {
            if (m_textColor.a <= 0.2f)
            {
                m_up = true;
                m_down = false;
            }
            else if (m_textColor.a >= 0.8f)
            {
                m_up = false;
                m_down = true;
            }

            if (m_up)
            {
                m_textColor.a += 0.1f;
            }else if (m_down)
            {
                m_textColor.a -= 0.1f;
            }
            this.gameObject.GetComponent<SpriteRenderer>().color = m_textColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Flash()
    {
        while (true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            //StageSelect.GoStageSelect(this);
            gameStartFlg = false;
            saveLoadFlg = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield break;
        }
    }
}
