using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    public GameObject[] JudgeUI;
    CheckPointCount CountScript;
    GameObject[] CheackPointObj;
    // Start is called before the first frame update
    void Start()
    {
        CountScript = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        CheackPointObj = GameObject.FindGameObjectsWithTag("CheackPointJudge");

        for (int i = 0; i < CheackPointObj.Length; i++)
        {
            CheackPointObj[i] = GameObject.Find("CheckpointJudge (" + i + ")");                                        //全てのメビウス取得
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int idx=0;idx< CountScript.CheckPointNum;idx++)
        {
            JudgeUI[idx].SetActive(true);
        }
    }
}
