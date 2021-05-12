// @file   SelectSoundOption.cs
// @brief  
// @author T,Cho
// @date   2021/05/10 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectSoundOption : MonoBehaviour
{
    const float MAX_ROTATE_ANGLE = 0.3f;
    const float MAX_ROTATE_ANGLE2 = -(MAX_ROTATE_ANGLE - 0.1f);
    private GameObject[] m_sliderHandle;    //スライダーハンドル
    private int m_nowId;
    float rotateAngle;
    // Start is called before the first frame update
    void Start()
    {
        m_sliderHandle = GameObject.FindGameObjectsWithTag("Handle");
        m_nowId = 0;
        m_sliderHandle[m_nowId].GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        rotateAngle = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

        //現在選択されているオブジェクトの情報保存
       RectTransform nowObj =  m_sliderHandle[m_nowId].GetComponent<RectTransform>();
       
        nowObj.Rotate(new Vector3(0.0f,0.0f,rotateAngle));


        if (nowObj.localRotation.z >= MAX_ROTATE_ANGLE)
        {
            rotateAngle = -1.0f;
        }
        else if (nowObj.localRotation.z <= MAX_ROTATE_ANGLE2)
        {
            rotateAngle = 1.0f;
        }

        if (Controler.GetDownButtonFlg())
        {
            //選択オブジェクトのサイズを戻す
            m_sliderHandle[m_nowId].GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
            m_sliderHandle[m_nowId].GetComponent<RectTransform>().rotation = Quaternion.identity;
            //選択オブジェクトの変更
            m_nowId++;
            if(m_nowId >= m_sliderHandle.Length)
            {
                m_nowId = m_sliderHandle.Length - 1;
            }
            m_sliderHandle[m_nowId].GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        }
        else if (Controler.GetUpButtonFlg())
        {
            //選択オブジェクトのサイズを戻す
            m_sliderHandle[m_nowId].GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
            m_sliderHandle[m_nowId].GetComponent<RectTransform>().rotation = Quaternion.identity;
            //選択オブジェクトの変更
            m_nowId--;
            if (m_nowId <= 0)
            {
                m_nowId = 0;
            }
            m_sliderHandle[m_nowId].GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        }
        else if (Controler.GetLeftButtonFlg())
        {
            m_sliderHandle[m_nowId].GetComponentInParent<VolumeSlider>().slider.value -= 0.25f;
            if (m_sliderHandle[m_nowId].GetComponentInParent<VolumeSlider>().slider.value <= 0.0f)
            {
                m_sliderHandle[m_nowId].GetComponentInParent<VolumeSlider>().slider.value = 0.0f;
            }
        }
        else if (Controler.GetRightButtonFlg())
        {
            m_sliderHandle[m_nowId].GetComponentInParent<VolumeSlider>().slider.value += 0.25f;
            if (m_sliderHandle[m_nowId].GetComponentInParent<VolumeSlider>().slider.value >= 1.0f)
            {
                m_sliderHandle[m_nowId].GetComponentInParent<VolumeSlider>().slider.value = 1.0f;
            }
        }
    }
}
