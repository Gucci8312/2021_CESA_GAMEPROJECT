﻿// @file   SoundManager.cs
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
    [SerializeField, Range(0, 1), Tooltip("マスタ音量")]             //マスターボリューム用変数（Serializeでprivate化、Rangeで0 ~ 1 に変更, Tooltipでインスペクタービュー上でわかりやすく）
    float masterVolume = 1.0f;
    [SerializeField, Range(0, 1), Tooltip("BGM音量")]                 //BGMボリューム用変数（Serializeでprivate化、Rangeで0 ~ 1 に変更, Tooltipでインスペクタービュー上でわかりやすく
    float bgmVolume = 1.0f;
    [SerializeField, Range(0, 1), Tooltip("SE音量")]                //SEボリューム用変数（Serializeでprivate化、Rangeで0 ~ 1 に変更, Tooltipでインスペクタービュー上でわかりやすく
    float seVolume = 1.0f;

    AudioClip[] m_bgm;                                              //BGMを格納する配列
    AudioClip[] m_se;                                               //SEを格納する配列

    Dictionary<string, int> m_bgmIndex = new Dictionary<string, int>();   //C++でいうMap
    Dictionary<string, int> m_seIndex = new Dictionary<string, int>();    //C++でいうMap　この二つは簡単にアクセスする用

   [SerializeField] AudioSource m_bgmAudioSource;                                       //BGMを鳴らすための変数
   [SerializeField] AudioSource m_seAudioSource;                                        //SEを鳴らすための変数

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
    new private void Awake()
    {
        if(this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        //シーンをまたいでも破壊しない変数に指定。
        DontDestroyOnLoad(this.gameObject);


        //リソースフォルダからBGMフォルダ/SEフォルダにアクセスして、それぞれすべてを読み込んでいく（配列要素０から自動的に読み込まれていく）
        m_bgm = Resources.LoadAll<AudioClip>("BGM");
        m_se = Resources.LoadAll<AudioClip>("SE");

        //bgmIndex連想配列（簡易アクセス用BGM配列）に情報を格納していく
        for (int i = 0; i < m_bgm.Length; i++)
        {
            m_bgmIndex.Add(m_bgm[i].name,i);
        }

        //seIndex連想配列（簡易アクセス用SE配列）に情報を格納していく
        for (int i = 0; i < m_se.Length; i++)
        {
            m_seIndex.Add(m_se[i].name, i);
        }
    }

    // @name   GetBgmIndex
    // @brief  名前からBGM配列の要素を返す
    // @param  BGＭの名前
    private int GetBgmIndex(string name)
    {
        //配列の中にその名前のＢＧＭがあるのかどうかを返す
        if (m_bgmIndex.ContainsKey(name))
        {
            return m_bgmIndex[name];
        }

        Debug.Log("その名前のBGMはありません");
        return 0;
    }

    // @name   GetSeIndex
    // @brief  名前からSE配列の要素を返す
    // @param  SEの名前
    private int GetSeIndex(string name)
    {
        //配列の中にその名前のＳＥがあるのかどうかを返す
        if (m_seIndex.ContainsKey(name))
        {
            return m_seIndex[name];
        }

        Debug.Log("その名前のBGMはありません");
        return 0;
    }


    // @name   PlayBGM
    // @brief  BGMの再生(GetBgmIndexから要素を拾ってきて鳴らす)
    private void PlayBGM(int num)
    {
        num = Mathf.Clamp(num, 0, m_bgm.Length);

        m_bgmAudioSource.clip = m_bgm[num];
        m_bgmAudioSource.loop = true;
        m_bgmAudioSource.volume = BgmVolume * MasterVolume;
        m_bgmAudioSource.Play();
    }

    // @name   PlayBgmName
    // @brief  BGMの再生(名前から音を鳴らす。基本これを呼んで使う)
    public void PlayBgmName(string name)
    {
        PlayBGM(GetBgmIndex(name));
    }

    // @name   StopBGM
    // @brief  BGMの停止(名前から音を停止。基本これを呼んで使う)
    public void StopBGM()
    {
        m_bgmAudioSource.Stop();
        m_bgmAudioSource.clip = null;   //音を止めたのでClipをNullに
    }

    // @name   PlaySE
    // @brief  GetSeIndexから要素を拾ってきて鳴らす
    private void PlaySE(int num)
    {
        num = Mathf.Clamp(num, 0, m_se.Length);
        m_seAudioSource.PlayOneShot(m_se[num], SeVolume * MasterVolume);
    }
    // @name   PlaySeName
    // @brief  BGMの再生(名前から音を鳴らす。基本これを呼んで使う)
    public void PlaySeName(string name)
    {
        PlaySE(GetSeIndex(name));
    }

    // @name   StopSE
    // @brief  SEの停止(名前から音を停止。基本これを呼んで使う)
    public void StopSE()
    {
        m_seAudioSource.Stop();
        m_seAudioSource.clip = null;    //音を止めたのでClipをNullに
    }
}
