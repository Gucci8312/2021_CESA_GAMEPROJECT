using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLight : MonoBehaviour
{
    GameObject UI;
    public GameObject Light;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.GetComponent<UIManeger>().GameClearFlg)
        {
            Light.SetActive(false);
        }
    }
}
