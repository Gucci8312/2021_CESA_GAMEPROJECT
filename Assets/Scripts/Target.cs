using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 衝突時
    private void OnCollisionEnter(Collision other)
    {
        // プレイヤーに当たった時
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("チェックポイント通過");
            Destroy(this.gameObject);
        }
    }


}
