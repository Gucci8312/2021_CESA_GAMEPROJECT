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

    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkPoint1.GetComponent<CheckVideoEvent>().checkCollider)
        {
            videoPanel1.SetActive(true);
            player.GetComponent<PlayerMove>().enabled = false;
            enemy.GetComponent<EnemyMove>().enabled = false;
            checkPoint1.GetComponent<CheckVideoEvent>().checkCollider = false;
        }
        else if (checkPoint2.GetComponent<CheckVideoEvent>().checkCollider)
        {
            videoPanel2.SetActive(true);
            player.GetComponent<PlayerMove>().enabled = false;
            enemy.GetComponent<EnemyMove>().enabled = false;
            checkPoint2.GetComponent<CheckVideoEvent>().checkCollider = false;
        }
        if (videoPanel1.GetComponent<VideoPlay>().endVideo)
        {
            videoPanel1.SetActive(false);
            player.GetComponent<PlayerMove>().enabled = true;
            enemy.GetComponent<EnemyMove>().enabled = true;
        }
        else if (videoPanel2.GetComponent<VideoPlay>().endVideo)
        {
            videoPanel2.SetActive(false);
            player.GetComponent<PlayerMove>().enabled = true;
            enemy.GetComponent<EnemyMove>().enabled = true;
            this.gameObject.SetActive(false);
        }
    }
}
