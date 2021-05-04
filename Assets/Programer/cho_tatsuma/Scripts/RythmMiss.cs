// @file   RythmMiss.cs
// @brief  
// @author T,Cho
// @date   2021/05/01 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmMiss : MonoBehaviour
{
    public float speed;
    private float m_framCount;
    private Color m_color;
    // Start is called before the first frame update
    void Start()
    {
        m_color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + Time.deltaTime * speed, gameObject.transform.position.z);
        if(m_framCount % 2 == 0)
        {
            m_color.a -= 0.1f;
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = m_color;

        if(this.gameObject.GetComponent<SpriteRenderer>().color.a <= 0.0f)
        {
            Destroy(this.gameObject);
        }
        m_framCount++;
    }
}
