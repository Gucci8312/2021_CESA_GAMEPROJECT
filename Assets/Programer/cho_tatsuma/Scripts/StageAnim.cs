// @file   StageAnim.cs
// @brief  ステージセレクト時のステージ表示アニメーション
// @author T,Cho
// @date   2021/05/24 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnim : MonoBehaviour
{
    public float speed;
    private float m_framCount;
    private Color m_color;

    // Start is called before the first frame update
    //void Start()
    //{
    //    m_color.r = this.gameObject.GetComponent<SpriteRenderer>().color.r;
    //    m_color.g = this.gameObject.GetComponent<SpriteRenderer>().color.g;
    //    m_color.b = this.gameObject.GetComponent<SpriteRenderer>().color.b;
    //    m_color.a = 0.0f;
    //    this.gameObject.transform.position = new Vector3(11.0f, gameObject.transform.position.y, gameObject.transform.position.z);
    //    this.gameObject.GetComponent<SpriteRenderer>().color = m_color;
    //}

    public void Init()
    {
        m_color.r = this.gameObject.GetComponent<SpriteRenderer>().color.r;
        m_color.g = this.gameObject.GetComponent<SpriteRenderer>().color.g;
        m_color.b = this.gameObject.GetComponent<SpriteRenderer>().color.b;
        m_color.a = 0.0f;
        this.gameObject.transform.position = new Vector3(11.0f, gameObject.transform.position.y, gameObject.transform.position.z);
        this.gameObject.GetComponent<SpriteRenderer>().color = m_color;
    }
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x > 7.5f)
        {
            this.gameObject.transform.position = new Vector3(gameObject.transform.position.x - Time.deltaTime * speed, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else
        {
            this.gameObject.transform.position = new Vector3(7.5f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (m_framCount % 5 == 0 && m_color.a <= 0.3f)
        {
            m_color.a += 0.02f;
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = m_color;

        m_framCount++;

    }
}
