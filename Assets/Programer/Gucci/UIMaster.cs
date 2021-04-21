using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    public GameObject[] JudgeUI;
    CheckPointCount CountScript;
    // Start is called before the first frame update
    void Start()
    {
        CountScript = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
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
