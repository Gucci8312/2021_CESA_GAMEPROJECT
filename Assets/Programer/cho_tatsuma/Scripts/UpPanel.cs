// @file   UpPanel.cs
// @brief  適当に上に飛ばすだけのスクリプトです
// @author T,Cho
// @date   2021/05/17 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//動けば正義スクリプトで組んでます



public class UpPanel : MonoBehaviour
{
    public float speed;
    bool down;
    int cnt;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cnt <= 2)
        {
            //ちょっと下に弾ませたい。
            this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - Time.deltaTime * speed, this.transform.position.z);
        }
        else if(cnt >= 25)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Time.deltaTime * speed * 2.0f, this.transform.position.z);
        }
        if (this.gameObject.transform.position.y >= 100)
        {
            Invoke("SetActiveFlase", 0.5f);
        }
        cnt++;
    }
    void SetActiveFlase()
    {
        this.gameObject.SetActive(false);
    }
}
