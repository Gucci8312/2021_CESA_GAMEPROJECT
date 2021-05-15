// @file   TutorialManager.cs
// @brief  
// @author T,Cho
// @date   2021/05/04 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject videoPanel1;
    [SerializeField] GameObject videoPanel2;

    [SerializeField] GameObject checkPoint1;
    [SerializeField] GameObject checkPoint2;

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
        if (checkPoint1.GetComponent<CheckVideoEvent>().checkCollider)
        {
            videoPanel1.SetActive(true);
            ScriptsOff();
            checkPoint1.GetComponent<CheckVideoEvent>().checkCollider = false;
        }
        else if (checkPoint2.GetComponent<CheckVideoEvent>().checkCollider)
        {
            videoPanel2.SetActive(true);
            ScriptsOff();
            checkPoint2.GetComponent<CheckVideoEvent>().checkCollider = false;
        }
        if (videoPanel1.GetComponentInChildren<VideoPlay>().endVideo)
        {
            videoPanel1.SetActive(false);
            ScriptsOn();
            videoPanel1.GetComponentInChildren<VideoPlay>().endVideo = false;
        }
        else if (videoPanel2.GetComponentInChildren<VideoPlay>().endVideo)
        {
            videoPanel2.SetActive(false);
            ScriptsOn();
            videoPanel2.GetComponentInChildren<VideoPlay>().endVideo = false;
            this.gameObject.SetActive(false);
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
