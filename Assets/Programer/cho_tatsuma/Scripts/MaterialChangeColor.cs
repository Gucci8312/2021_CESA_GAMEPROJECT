// @file   MaterialChangeColor
// @brief  マテリアルの色を変えるクラス定義
// @author T,Cho
// @date   2021/04/20 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   MaterialChangeColor
// @brief  マテリアルの色を変えるクラス定義
public class MaterialChangeColor : MonoBehaviour
{
    static float m_colorNum;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.EnableKeyword("_EMISSION");
        m_colorNum = 0.0f;
        mat.SetColor("_EmissionColor", new Color(0.0f , 0.0f , 0.0f));
    }

    // @name   Init
    // @brief  初期化するための関数
    public void Init()
    {
        m_colorNum = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_colorNum <= 1.0f)
        {
            mat.SetColor("_EmissionColor", new Color(0.0f + m_colorNum, 0.0f + m_colorNum, 0.0f + m_colorNum));
            m_colorNum += 0.0001f;
        }

    }
}
