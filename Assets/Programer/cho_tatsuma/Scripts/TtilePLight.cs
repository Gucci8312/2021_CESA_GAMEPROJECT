// @file   TtilePLight.cs
// @brief  Titleのライトを管理するクラス定義
// @author T,Cho
// @date   2021/04/21 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// @name   TtilePLight
// @brief  Titleのライトを管理するクラス
public class TtilePLight : MonoBehaviour
{
    [SerializeField]
    GameObject m_beatText;      //Beatテキストオブジェクト

    [SerializeField]
    GameObject[] m_titleBeatObj;

    [SerializeField]
    GameObject m_pressAButtonText;  //PressAButtonテキストオブジェクト

    [SerializeField]
    GameObject m_pinkPointLight;    //ピンクのライトオブジェクト

    Light m_light;                  //白色のライト（このオブジェクトのライト）
    Light m_pinkLight;              //ピンクのライト（別のオブジェクトのライト）
    bool m_up;                      //ピンクのライトのIntensityが上がるかどうかの変数
    bool m_down;                    //ピンクのライトのIntensityが下がるかどうかの変数

    float batibati_count;           //バチバチする回数カウント
    Color m_textColor;
    Color clearAlpha;

    float whiteIntensity;
    float pinkIntensity;

    public bool titleAnimationFinished;
    // @name   OnInit
    // @brief  初期化関数
    public void Start()
    {
        m_light = GetComponent<Light>();
        whiteIntensity = m_light.intensity;
        m_light.intensity = 0f;
        m_pinkLight = m_pinkPointLight.GetComponent<Light>();
        pinkIntensity = m_pinkLight.intensity;
        m_pinkLight.intensity = 0f;
        m_textColor = Color.clear;
        titleAnimationFinished = false;
        ChangeTextColorClear(m_beatText);
        for(int idx = 0; idx < m_titleBeatObj.Length; idx++)
        {
            ChangeTextColorClearAlphaOnly(m_titleBeatObj[idx]);
        }
        SoundManager.PlaySeName("batibati");
        m_pressAButtonText.SetActive(false);
        StartCoroutine(MomentChangeSpotLight());
    }

    private void OnDisable()
    {
        StopCoroutine(MomentChangeSpotLight());
        StopCoroutine(SlowChangeSpotLight());
    }
    // @name   ChangeTextColorClear
    // @brief  文字がに透明になる
    void ChangeTextColorClear(GameObject _textObj)
    {
        Color clear = Color.clear;
        _textObj.GetComponent<SpriteRenderer>().color = clear;
    }

    // @name   ChangeTextColorClearAlphaOnly
    // @brief  文字がに透明になる(Alphaのみ変更)
    void ChangeTextColorClearAlphaOnly(GameObject _obj)
    {
        clearAlpha.a = 0.0f;
        clearAlpha.r = _obj.GetComponent<SpriteRenderer>().color.r;
        clearAlpha.g = _obj.GetComponent<SpriteRenderer>().color.g;
        clearAlpha.b = _obj.GetComponent<SpriteRenderer>().color.b;
        _obj.GetComponent<SpriteRenderer>().color = clearAlpha;

    }

    // @name   ChangeTextColorWhiteAlphaOnly
    // @brief  文字が徐々に色付きになる(Alphaのみ変更)
    void ChangeTextColorWhiteAlphaOnly(GameObject _obj)
    {
        if (clearAlpha.a >= 1.0f)
        {
            return;
        }
        clearAlpha.a = _obj.GetComponent<SpriteRenderer>().color.a;
        clearAlpha.r = _obj.GetComponent<SpriteRenderer>().color.r;
        clearAlpha.g = _obj.GetComponent<SpriteRenderer>().color.g;
        clearAlpha.b = _obj.GetComponent<SpriteRenderer>().color.b;
        clearAlpha.a += m_textColor.a * 0.01f;
        _obj.GetComponent<SpriteRenderer>().color = clearAlpha;

    }

    // @name   ChangeTextColorBlack
    // @brief  文字がに真っ黒になる
    void ChangeTextColorBlack(GameObject _textObj)
    {
        Color color = Color.black;
        _textObj.GetComponent<SpriteRenderer>().color = color;
    }

    // @name   ChangeTextColorWhite
    // @brief  文字が徐々に色付きになる
    void ChangeTextColorWhite(GameObject _textObj)
    {
        if (m_textColor.r >= 1.0f && !titleAnimationFinished)
        {
            m_pressAButtonText.SetActive(true);
            titleAnimationFinished = true;
            return;
        }
        m_textColor.r += 0.1f * (1f / 10f);
        m_textColor.g += 0.1f * (1f / 10f);
        m_textColor.b += 0.1f * (1f / 10f);
        m_textColor.a += 0.1f * (1f / 10f);
        _textObj.GetComponent<SpriteRenderer>().color = m_textColor;

    }

    // @name   MomentChangeSpotLight
    // @brief  白色のライトが瞬間で点滅する
    IEnumerator MomentChangeSpotLight()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_light.intensity <= 0f)
            {
                m_light.intensity = 50;
                ChangeTextColorBlack(m_beatText);
                yield return new WaitForSeconds(0.1f);
            }

            if (m_light.intensity >= 50)
            {
                m_light.intensity = 0f;
                ChangeTextColorClear(m_beatText);
                yield return new WaitForSeconds(0.1f);
            }
            if (i == 1)
            {
                yield return new WaitForSeconds(0.4f);
            }
            if (i == 3)
            {
                SoundManager.StopSE();
                yield return new WaitForSeconds(1.0f);
                SoundManager.PlayBgmName("title");
                StartCoroutine("SlowChangeSpotLight");
            }
            yield return null;
        }
    }

    // @name   SlowChangeSpotLight
    // @brief  Pinkのライトがスローに点滅する
    IEnumerator SlowChangeSpotLight()
    {
        while (true)
        {

            if (m_pinkLight.intensity <= 0)
            {
                m_down = false;
                m_up = true;
            }

            if (m_pinkLight.intensity >= pinkIntensity)
            {
                m_down = true;
                m_up = false;
            }

            if (m_down)
            {
             //   m_pinkLight.intensity--;
            }
            else if (m_up)
            {
                m_pinkLight.intensity++;
            }
            for (int idx = 0; idx < m_titleBeatObj.Length; idx++)
            {
                ChangeTextColorWhiteAlphaOnly(m_titleBeatObj[idx]);
            }
            ChangeTextColorWhite(m_beatText);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
