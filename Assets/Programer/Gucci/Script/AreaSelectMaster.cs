using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSelectMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ステージ1ボタン
    public void ClickStage1Botton()
    {
        SceneManager.LoadScene("Stage1");
    }
}
