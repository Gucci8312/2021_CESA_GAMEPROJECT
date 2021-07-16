using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Target : MonoBehaviour
{
    Vector3 LocalPos;//親オブジェクトからの相対的な場所
    CheckPointCount CheckPointUi;
    PlayerMove player;
    public bool ColFlg;
    Vector3 ColPos;
    GameObject GetChackPointEffect;
    float kaitenn;
    public VisualEffect vfx;
    GameObject RythmObj;                                    //リズムオブジェクト
    Rythm rythm;                                            //リズムスクリプト取得用

   GameObject DirectionLight;
   float incity;

    // Start is called before the first frame update
    void Start()
    {
        LocalPos = this.transform.localPosition;
        CheckPointUi = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        GetChackPointEffect = GameObject.Find("GetCheckPointEffect");
        //vfx = RythmObj.GetComponent<VisualEffect>();
        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
        DirectionLight= GameObject.Find("DL");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.rythm.rythmSendCheckFlag)
        {
            vfx.SendEvent("PlayDenki");
        }
        //何故か親オブジェクトについていかないので、代入してあげて追従させている
        this.transform.localPosition = LocalPos;

        // 回転で使うかも  
        //kaitenn = 3.0f;
        //transform.Rotate(0.0f, kaitenn, 0.0f);
        //transform.Rotate(0.0f, transform.localRotation.y * kaitenn, 0.0f);
        if (ColFlg)
        {
            kaitenn -= 1.0f;
            Vector3 WorldAngle = transform.localEulerAngles;
            WorldAngle.x = 0.0f;
            WorldAngle.y = 0.0f;
            WorldAngle.z = kaitenn;
            transform.Rotate(WorldAngle);
            // gameObject.GetComponent<Rigidbody>().AddForce(0.0f, 10000.0f, 0.0f);
            //Vector3 Pos = gameObject.transform.position;
            // gameObject.transform.Translate(Pos.x, Pos.y+kaitenn, Pos.z);

        }
        else
        {
            kaitenn = 2.0f;
            Vector3 WorldAngle = transform.localEulerAngles;
            WorldAngle.x = 0.0f;
            WorldAngle.y = 0.0f;
            WorldAngle.z = kaitenn;
            //transform.Rotate(WorldAngle);
        }
        //kaitenn = 0.1f;
        //if (ColFlg)
        //{
        //    kaitenn -= 0.5f;
        //    gameObject.GetComponent<Rigidbody>().AddForce(0.0f, 100.0f, 0.0f);
        //    //gameObject.transform.Rotate(,,);
        //}
        //Vector3 WorldAngle = transform.localEulerAngles;
        //WorldAngle.x = 0.0f;
        //WorldAngle.y = 0.0f;
        //WorldAngle.z = kaitenn;
        ////transform.eulerAngles = WorldAngle;
        //transform.Rotate(WorldAngle);
        
    }

    // 衝突時
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに当たった時
        if (other.gameObject.tag == "Player")
        {
            
            if (!other.gameObject.GetComponent<PlayerMove>().GetJumpNow())//プレイヤーがジャンプして取れちゃうバグを制限
            {
                CheckPointUi.Incity();
            
                Debug.Log(CheckPointUi.GetIncity());
                DirectionLight.GetComponent<Light>().intensity = CheckPointUi.GetIncity();
                Debug.Log("チェックポイント通過");
                CheckPointUi.CheckPointNum++;
                //Destroy(this.gameObject);
                ColFlg = true;
                ColPos = other.ClosestPointOnBounds(this.transform.position);
                SoundManager.PlaySeName("checkpoint_sin");
                Destroy(GetComponent<CapsuleCollider>());
                Invoke("Delete", 1.0f);
               

            }
            //LightNum[(Select_Scene - 1)].GetComponent<Light>().intensity = LIGHT_ON;
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
