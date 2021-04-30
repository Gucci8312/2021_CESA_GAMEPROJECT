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

    public bool gameStartFlg = false;
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

        gameStartFlg = true;
        StartCoroutine(FadeCroutine());
    }

    // Update is called once per frame
    void Update()
    {

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
}
