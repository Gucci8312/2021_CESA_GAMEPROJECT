using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointCount : MonoBehaviour
{
    public int CheckPointNum;//通過したチェックポイント数
    float Inci;
    public float inci_s;
    // Start is called before the first frame update
    void Start()
    {
        CheckPointNum = 0;
    }

    public void inci(float a)
	{
        inci_s = a;

    }
    public void Incity()
	{

        Inci += inci_s;

    }

    public float GetIncity()
	{
        return Inci;
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
