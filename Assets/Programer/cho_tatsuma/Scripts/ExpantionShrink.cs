// @file   ExpantionShrink.cs
// @brief  オブジェクト拡縮クラス定義
// @author T,Cho
// @date   2021/05/14 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   ExpantionShrink
// @brief  オブジェクト拡縮クラス
public class ExpantionShrink : MonoBehaviour
{
    const float MIN_SHRINK = 0.7f;      //最大縮小サイズ
    Rythm rythm;
    bool isExpantion;
    bool isShrink;
    bool m_gameStart;
    float i = 1f;
    float j = 1f;
    Transform myTransform;
    Vector3 initLocalScale;
      
    // Start is called before the first frame update
    void Start()
    {
        rythm = GameObject.Find("rythm_circle").GetComponent<Rythm>();
        isExpantion = false;
        isShrink = false;
        m_gameStart = false;
        Invoke("GameStart", 2.0f);
    }

    private void OnEnable()
    {
        initLocalScale = gameObject.transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_gameStart) return;
        myTransform = gameObject.transform;
        if (rythm.rythmSendCheckFlag && !isExpantion && !isShrink)
        {
            isExpantion = true;
        }
        Shrink();

        Expantion();
    }

    void GameStart()
    {
        m_gameStart = true;
    }

    void Expantion()
    {
        if (isExpantion)
        {
            myTransform.localScale = new Vector3(myTransform.localScale.x * i, myTransform.localScale.y * i, myTransform.localScale.z);
            i -= 0.01f;
            if (myTransform.localScale.x <= initLocalScale.x * MIN_SHRINK)
            {
                myTransform.localScale = new Vector3(initLocalScale.x * MIN_SHRINK, initLocalScale.y * MIN_SHRINK, myTransform.localScale.z);
                isShrink = true;
                isExpantion = false;
                i = 1.0f;
            }
        }
    }

    void Shrink()
    {
        if (isShrink)
        {
            myTransform.localScale = new Vector3(myTransform.localScale.x * j, myTransform.localScale.y * j, myTransform.localScale.z);
            j += 0.2f;
            if (myTransform.localScale.x >= initLocalScale.x)
            {
                myTransform.localScale = new Vector3(initLocalScale.x, initLocalScale.y, myTransform.localScale.z);
                isShrink = false;
                j = 1.0f;
            }
        }
    }
}
