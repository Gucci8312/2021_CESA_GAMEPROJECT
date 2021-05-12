﻿// @file   VolumeSlider.cs
// @brief  設定画面によるボリュームスライダー用クラス定義
// @author T,Cho
// @date   2021/04/19 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// @name   VolumeSlider
// @brief  設定画面によるボリュームスライダー用クラス
public class VolumeSlider : MonoBehaviour
{
    public enum Type
    {
        MASTER = 1,
        BGM,
        SE,
        NONE
    }

    [SerializeField]
    Type type = Type.MASTER;            //ボリューム調整タイプ変数

    public Slider slider;                    //スライダーバーの変数
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {

    }

    // @name   VolumeChange
    // @brief  音量を変える
    public void VolumeChange()
    {
        switch (type)
        {
            //マスター音量
            case Type.MASTER:
                SoundManager.MasterVolume = slider.value;
                break;
            //BGM音量
            case Type.BGM:
                SoundManager.BgmVolume = slider.value;
                break;
            //SE音量
            case Type.SE:
                SoundManager.SeVolume = slider.value;
                break;
        }
    }
}
