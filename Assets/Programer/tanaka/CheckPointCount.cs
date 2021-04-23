using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointCount : MonoBehaviour
{
    public int CheckPointNum;//通過したチェックポイント数

    // Start is called before the first frame update
    void Start()
    {
        CheckPointNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Textコンポーネント取得
      //  Text checkpoint_text = this.GetComponent<Text>();

        //Text表示
       // checkpoint_text.text = "チェックポイント　" + CheckPointNum + "/3";
    }
}
