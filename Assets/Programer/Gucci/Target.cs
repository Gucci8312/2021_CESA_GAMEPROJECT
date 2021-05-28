using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Vector3 LocalPos;//親オブジェクトからの相対的な場所
    CheckPointCount CheckPointUi;
    PlayerMove player;
    public bool ColFlg;
    Vector3 ColPos;
    GameObject GetChackPointEffect;
    float kaitenn;

    // Start is called before the first frame update
    void Start()
    {
        LocalPos = this.transform.localPosition;
        CheckPointUi = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        GetChackPointEffect = GameObject.Find("GetCheckPointEffect");
    }

    // Update is called once per frame
    void Update()
    {
        //何故か親オブジェクトについていかないので、代入してあげて追従させている
        this.transform.localPosition = LocalPos;

        // 回転で使うかも  
        //kaitenn = 3.0f;
        //transform.Rotate(0.0f, kaitenn, 0.0f);
        //transform.Rotate(0.0f, transform.localRotation.y * kaitenn, 0.0f);
        if (ColFlg)
        {
            kaitenn-=1.0f;
            //gameObject.transform.Rotate(,,);
            Vector3 WorldAngle = transform.localEulerAngles;
            WorldAngle.x = 0.0f;
            WorldAngle.y = 0.0f;
            WorldAngle.z = kaitenn;
            //transform.eulerAngles = WorldAngle;
            transform.Rotate(WorldAngle);
        }
    }

    // 衝突時
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに当たった時
        if (other.gameObject.tag == "Player")
        {
            // if (!player.GetStartFlg())
            {
                if (!other.gameObject.GetComponent<PlayerMove>().GetJumpNow())//プレイヤーがジャンプして取れちゃうバグを制限
                {
                    Debug.Log("チェックポイント通過");
                    CheckPointUi.CheckPointNum++;
                    //Destroy(this.gameObject);
                    ColFlg = true;
                    ColPos = other.ClosestPointOnBounds(this.transform.position);
                    SoundManager.PlaySeName("checkpoint_sin");
                   Destroy(GetComponent<CapsuleCollider>());
                    Invoke("Delete", 1.0f);
                }
            }
        }
    }

    void Delete()
    {
        Destroy(this.gameObject);
    }

    public bool GetColFlg()
    {
        return ColFlg;
    }

    public Vector3 GetColPos()
    {
        return ColPos;
    }
}
