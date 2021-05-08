// @file   StageSelectInit.cs
// @brief  StageSelectクラスをスタティッククラスにしたことによる追加クラス
// @author T,Cho
// @date   2021/04/27 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// @name   StageSelectInit
// @brief  StageSelectクラスを初期化するためだけのクラス
public class StageSelectInit : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        //フェードオブジェクトからフェードクラスコンポーネントを取得
        image = GameObject.Find("FadeImage").GetComponent<Image>();
        image.enabled = true;
        StageSelect.m_fadeImage = image;
        StageSelect.m_fade = StageSelect.m_fadeImage.gameObject.GetComponent<Fade>();
    }
}
