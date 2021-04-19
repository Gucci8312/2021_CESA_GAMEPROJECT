// @file   SoundManager.cs
// @brief  音を管理するクラス定義
// @author T,Cho
// @date   2021/04/19 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   SoundManager
// @brief  音を管理するクラス
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField, Range(0, 1), Tooltip("マスタ音量")]
    float masterVolume = 1.0f;
    [SerializeField, Range(0, 1), Tooltip("BGM音量")]
    float bgmVolume = 1.0f;
    [SerializeField, Range(0, 1), Tooltip("SE音量")]
    float seVolume = 1.0f;

    AudioClip[] m_bgm;
    AudioClip[] m_se;

    Dictionary<string, int> bgmIndex = new Dictionary<string, int>();
    Dictionary<string, int> seIndex = new Dictionary<string, int>();

    AudioSource m_bgmAudioSource;
    AudioSource m_seAudioSource;

    //プロパティ変数(マスターボリューム)
    public float MasterVolume
    {
        set
        {
            //全体の重みを計算
            masterVolume = Mathf.Clamp01(value);
            //音量変更（全体）
            m_bgmAudioSource.volume = bgmVolume * masterVolume;
            m_seAudioSource.volume = seVolume * masterVolume;
        }
        get
        {
            //マスター音量を返す
            return masterVolume;
        }
    }

    //プロパティ変数(BGMボリューム)
    public float BgmVolume
    {
        set
        {
            //全体の重みを計算
            bgmVolume = Mathf.Clamp01(value);
            //音量変更（全体）
            m_bgmAudioSource.volume = bgmVolume * masterVolume;

        }
        get
        {
            return bgmVolume;
        }
    }

    //プロパティ変数(SEボリューム)
    public float SeVolume
    {
        set
        {
            //全体の重みを計算
            seVolume = Mathf.Clamp01(value);
            //音量変更（全体）
            m_seAudioSource.volume = seVolume * masterVolume;

        }
        get
        {
            return seVolume;
        }
    }

    //始まった瞬間Start関数よりも早い関数
    private void Awake()
    {
        if(this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
