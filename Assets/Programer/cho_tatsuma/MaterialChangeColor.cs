using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Init()
    {
        m_colorNum = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_colorNum <= 0.0f)
        {
            mat.SetColor("_EmissionColor", new Color(0.0f + m_colorNum, 0.0f + m_colorNum, 0.0f + m_colorNum));
            m_colorNum += 0.001f;
        }

    }
}
