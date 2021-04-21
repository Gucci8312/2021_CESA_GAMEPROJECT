// @file   TtilePLight.cs
// @brief  Titleのライトを管理するクラス定義
// @author T,Cho
// @date   2021/04/21 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// @name   TtilePLight
// @brief  Titleのライトを管理するクラス
public class TtilePLight : MonoBehaviour
{
    [SerializeField]
    GameObject m_beatText;      //Beatテキストオブジェクト
    [SerializeField]
    GameObject m_runText;        //Runテキストオブジェクト

    [SerializeField]
    GameObject m_areaSelectButtonObj;

    [SerializeField]
    GameObject m_endButtonObj;

    [SerializeField]
    GameObject m_pinkPointLight;    //ピンクのライトオブジェクト

    Light m_light;                  //白色のライト（このオブジェクトのライト）
    Light m_pinkLight;              //ピンクのライト（別のオブジェクトのライト）
    bool m_up;                      //ピンクのライトのIntensityが上がるかどうかの変数
    bool m_down;                    //ピンクのライトのIntensityが下がるかどうかの変数

    float batibati_count;           //バチバチする回数カウント
    Color m_textColor;              

    float whiteIntensity;          
    float pinkIntensity;

    // @name   OnEnable
    // @brief  インスペクタービュー参照できるクラス
    private void OnEnable()
    {
        m_light = GetComponent<Light>();
        whiteIntensity = m_light.intensity;
        m_pinkLight = m_pinkPointLight.GetComponent<Light>();
        pinkIntensity = m_pinkLight.intensity;
        m_pinkLight.intensity = 0f;
        m_textColor = Color.clear;
        ChangeTextColorClear(m_beatText);
        ChangeTextColorClear(m_runText);

        m_areaSelectButtonObj.SetActive(false);
        m_endButtonObj.SetActive(false);
        StartCoroutine("MomentChangeSpotLight");

    }

    // @name   ChangeTextColorClear
    // @brief  文字がに透明になる
    void ChangeTextColorClear(GameObject _textObj)
    {
        Color clear = Color.clear;
        _textObj.GetComponent<SpriteRenderer>().color = clear;
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
        if (m_textColor.r >= 1.0f) {
            m_areaSelectButtonObj.SetActive(true);
            m_endButtonObj.SetActive(true);
            return; 
        }
        m_textColor.r += 0.1f * (1f/50f);
        m_textColor.g += 0.1f * (1f/50f);
        m_textColor.b += 0.1f * (1f/50f);
        m_textColor.a += 0.1f * (1f / 10f);
        _textObj.GetComponent<SpriteRenderer>().color = m_textColor;
            
    }

    // @name   MomentChangeSpotLight
    // @brief  白色のライトが瞬間で点滅する
    IEnumerator MomentChangeSpotLight()
    {
        while (batibati_count < 6.0f)
        {

            if (m_light.intensity <= 0f)
            {
                m_light.intensity = whiteIntensity;
                batibati_count += 1f;
                ChangeTextColorBlack(m_beatText);
                ChangeTextColorBlack(m_runText);
                yield return new WaitForSeconds(0.1f);
            }

            if (m_light.intensity >= whiteIntensity)
            {
                m_light.intensity = 0f;
                batibati_count += 1f;
                ChangeTextColorClear(m_beatText);
                ChangeTextColorClear(m_runText);
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log(batibati_count);
            if (batibati_count == 3f)
            {
                yield return new WaitForSeconds(0.4f);
            }
            if (batibati_count == 7f)
            {
                yield return new WaitForSeconds(1.0f);
                StartCoroutine("SlowChangeSpotLight");
                yield return null;
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

            if (m_down && !m_up)
            {
                m_pinkLight.intensity--;
            }
            else if (!m_down && m_up)
            {
                m_pinkLight.intensity++;
            }
            ChangeTextColorWhite(m_beatText);
            ChangeTextColorWhite(m_runText);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
