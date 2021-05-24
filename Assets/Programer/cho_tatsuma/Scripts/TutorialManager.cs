// @file   TutorialManager.cs
// @brief  チュートリアル時のビデオパネルの操作定義クラス
// @author T,Cho
// @date   2021/05/04 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

// @name   TutorialManager
// @brief  チュートリアル時のビデオパネルの操作定義
public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] videoPanel = default;

    [SerializeField] GameObject[] checkPoint = default;
    [SerializeField] GameObject APButton = default;
    [SerializeField] GameObject AButton2 = default;

    [SerializeField] GameObject BPButton = default;
    [SerializeField] GameObject BButton2 = default;

    [SerializeField] Text orText = default;

    Rythm rythm;
    GameObject player;
    GameObject enemy;
    GameObject[] mobius;

    bool endTutorial = false;

    [SerializeField] Volume bloom = default;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        mobius = GameObject.FindGameObjectsWithTag("Mobius");
        rythm = GameObject.Find("rythm_circle").GetComponent<Rythm>();
        AButton2.SetActive(false);
        APButton.SetActive(false);
        BButton2.SetActive(false);
        BPButton.SetActive(false);
        orText.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (endTutorial)
        {
            if (rythm.rythmSendCheckFlag)
            {
                APButton.SetActive(false);
                BPButton.SetActive(false);
                AButton2.SetActive(true);
                BButton2.SetActive(true);
            }
            else
            {
                APButton.SetActive(true);
                BPButton.SetActive(true);
                AButton2.SetActive(false);
                BButton2.SetActive(false);
            }
        }
        //すべてのチェックポイントにキャラクターが触れているかチェック
        for (int idx = 0; idx < checkPoint.Length; idx++)
        {
            CheckPointColliderCheck(idx);
        }

        //ビデオが終了したかどうかをチェック
        for (int idx = 0; idx < checkPoint.Length; idx++)
        {
            CheckVideoEnd(idx);
        }
    }


    // @name   ActiveUpPanel
    // @brief  ビデオ終了時パネルが上に上がるようにする
    void ActiveUpPanel(int _idx)
    {
        videoPanel[_idx].GetComponent<UpPanel>().enabled = true;
        videoPanel[_idx].GetComponent<DownPanel>().enabled = false; //同時にダウンパネルのアクティブを消す
    }

    // @name   CheckPointColliderCheck
    // @brief  キャラクターがチェックポイントに到達したかどうかを随時調べる（増えれば増えるほど重たくなります）
    void CheckPointColliderCheck(int _idx)
    {
        if (checkPoint[_idx].GetComponent<CheckVideoEvent>().checkCollider)
        {
            videoPanel[_idx].SetActive(true);
            ScriptsOff();
            checkPoint[_idx].GetComponent<CheckVideoEvent>().checkCollider = false;
        }

    }

    // @name   CheckVideoEnd
    // @brief  ビデオが終了したかどうか随時チェックします（増えれば増えるほど重たくなります。）
    void CheckVideoEnd(int _idx)
    {
        if (videoPanel[_idx].GetComponentInChildren<VideoPlay>().endVideo)
        {
            ActiveUpPanel(_idx);
            Invoke("ScriptsOn", 0.8f);
            videoPanel[_idx].GetComponentInChildren<VideoPlay>().endVideo = false;
            endTutorial = true;
            orText.enabled = true;
        }

    }
    // @name   ScriptsOff
    // @brief  特定のスクリプトを切りたい用
    void ScriptsOff()
    {
        player.GetComponent<PlayerMove>().enabled = false;
        enemy.GetComponent<EnemyMove>().enabled = false;
        for (int i = 0; i < mobius.Length; i++)
            mobius[i].GetComponent<EnemyMobius>().enabled = false;

        Bloom bloom_propaty;
        bloom.profile.TryGet(out bloom_propaty);
        bloom_propaty.intensity.value = 0f;
    }

    // @name   ScriptsOn
    // @brief  切ったスクリプトをONにする。
    void ScriptsOn()
    {
        player.GetComponent<PlayerMove>().enabled = true;
        enemy.GetComponent<EnemyMove>().enabled = true;
        for (int i = 0; i < mobius.Length; i++)
            mobius[i].GetComponent<EnemyMobius>().enabled = true;
        Bloom bloom_propaty;
        bloom.profile.TryGet(out bloom_propaty);
        bloom_propaty.intensity.value = 2f;
    }
}
