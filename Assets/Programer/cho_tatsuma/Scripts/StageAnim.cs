// @file   StageAnim.cs
// @brief  ステージセレクト時のステージ表示アニメーション
// @author T,Cho
// @date   2021/05/24 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnim : MonoBehaviour
{
    public enum MOVE_TYPE {
        NONE,
        LEFT,
        RIGHT,
    }

    public enum ALPHA_TYPE
    {
        NONE,
        CLEAR,
        BLACK,
    }

    public MOVE_TYPE moveType;
    public ALPHA_TYPE alphaType;
    public float speed;
    private float m_framCount;
    private Color m_color;

    public float targetX;               //どこまで進むか
    public float max_alpha;                 //最大アルファ値
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

    public void Start()
    {
        switch (alphaType)
        {
            case ALPHA_TYPE.NONE:
                m_color.r = this.gameObject.GetComponent<SpriteRenderer>().color.r;
                m_color.g = this.gameObject.GetComponent<SpriteRenderer>().color.g;
                m_color.b = this.gameObject.GetComponent<SpriteRenderer>().color.b;
                m_color.a = 1.0f;
                this.gameObject.GetComponent<SpriteRenderer>().color = m_color;
                break;
            case ALPHA_TYPE.CLEAR:
                m_color.r = this.gameObject.GetComponent<SpriteRenderer>().color.r;
                m_color.g = this.gameObject.GetComponent<SpriteRenderer>().color.g;
                m_color.b = this.gameObject.GetComponent<SpriteRenderer>().color.b;
                m_color.a = 0.0f;
                this.gameObject.GetComponent<SpriteRenderer>().color = m_color;
                break;
        }
     //   this.gameObject.transform.position = new Vector3(11.0f, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case MOVE_TYPE.LEFT:
                if (this.gameObject.transform.position.x > targetX)
                {
                    this.gameObject.transform.position = new Vector3(gameObject.transform.position.x - Time.deltaTime * speed, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else
                {
                    this.gameObject.transform.position = new Vector3(targetX, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                break;
            case MOVE_TYPE.RIGHT:
                if (this.gameObject.transform.position.x < targetX)
                {
                    this.gameObject.transform.position = new Vector3(gameObject.transform.position.x + Time.deltaTime * speed, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else
                {
                    this.gameObject.transform.position = new Vector3(targetX, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                break;
        }

        if (m_framCount % 5 == 0 && m_color.a <= max_alpha)
        {
            m_color.a += 0.02f;
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = m_color;

        m_framCount++;

    }
}
