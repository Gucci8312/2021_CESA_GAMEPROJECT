// @file   TutorialManager.cs
// @brief  チュートリアル時のビデオパネルの操作定義クラス
// @author T,Cho
// @date   2021/05/04 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// @name   TutorialManager
// @brief  チュートリアル時のビデオパネルの操作定義
public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] videoPanel;

    [SerializeField] GameObject[] checkPoint;

    GameObject player;
    GameObject enemy;
    GameObject[] mobius;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        mobius = GameObject.FindGameObjectsWithTag("Mobius");
    }

    // Update is called once per frame
    void Update()
    {
        //すべてのチェックポイントにキャラクターが触れているかチェック
        for(int idx  = 0; idx < checkPoint.Length; idx++)
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

    }

    // @name   ScriptsOn
    // @brief  切ったスクリプトをONにする。
    void ScriptsOn()
    {
        player.GetComponent<PlayerMove>().enabled = true;
        enemy.GetComponent<EnemyMove>().enabled = true;
        for (int i = 0; i < mobius.Length; i++)
            mobius[i].GetComponent<EnemyMobius>().enabled = true;
    }
}
