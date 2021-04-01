using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Vector3 LocalPos;//親オブジェクトからの相対的な場所
    CheckPointCount CheckPointUi;
    PlayerMove player;
    // Start is called before the first frame update
    void Start()
    {
        LocalPos = this.transform.localPosition;
        CheckPointUi = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //何故か親オブジェクトについていかないので、代入してあげて追従させている
        this.transform.localPosition = LocalPos;
    }

    // 衝突時
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに当たった時
        if (other.gameObject.tag == "Player")
        {            
            if(this.transform.parent.name=="Mobius (" + player.GetNowMobiusNum() + ")")//同じメビウス状にいるかどうか
            {
                Debug.Log("チェックポイント通過");
                CheckPointUi.CheckPointNum++;
                Destroy(this.gameObject);
            }
            
        }
    }


}
