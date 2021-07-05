// @file   ExpantionShrink.cs
// @brief  オブジェクト拡縮クラス定義
// @author T,Cho
// @date   2021/05/14 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   ExpantionShrink
// @brief  オブジェクト拡縮クラス
//拡大縮小の英語文字が逆になっていてわかりにくいかもごめん。
public class ExpantionShrink : MonoBehaviour
{
    public float MIN_SHRINK = 0.7f;      //最大縮小サイズ　ここを変えるとリズムのタイミングでの縮小サイズが変わる
    Rythm rythm;
    [Header("音に合わせるか")]
    public bool musicOn;
    public bool isExpantion;
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
        musicOn = true;
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
        if (rythm.rythmSendCheckFlag && !isExpantion && !isShrink && musicOn)
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

    //縮小
    void Expantion()
    {
        if (isExpantion)
        {
            //徐々に縮小
            myTransform.localScale = new Vector3(myTransform.localScale.x * i, myTransform.localScale.y * i, myTransform.localScale.z);
            i -= 0.01f;
            //最大縮小サイズより小さくなったら　最大縮小サイズに強制固定
            if (myTransform.localScale.x <= initLocalScale.x * MIN_SHRINK)
            {
                myTransform.localScale = new Vector3(initLocalScale.x * MIN_SHRINK, initLocalScale.y * MIN_SHRINK, myTransform.localScale.z);
                isShrink = true;
                isExpantion = false;
                i = 1.0f;
            }
        }
    }

    //拡大
    void Shrink()
    {
        if (isShrink)
        {
            //徐々に拡大
            myTransform.localScale = new Vector3(myTransform.localScale.x * j, myTransform.localScale.y * j, myTransform.localScale.z);
            j += 0.2f;
            //元のサイズより越えてしまったら　元のサイズに強制変更
            if (myTransform.localScale.x >= initLocalScale.x)
            {
                myTransform.localScale = new Vector3(initLocalScale.x, initLocalScale.y, myTransform.localScale.z);
                isShrink = false;
                j = 1.0f;
            }
        }
    }
}
