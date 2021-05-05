using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectControl : MonoBehaviour
{
    GameObject DushEffect;
    GameObject SmokeEffect;
    GameObject Player;
    int OnpuNum;
    GameObject obj;
    Vector3 Pos;
    static bool OnpuDownFlg;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("PLAYER"); ;
        DushEffect = GameObject.Find("DushEffect");
        SmokeEffect = GameObject.Find("SmokeEffect");
        obj = (GameObject)Resources.Load("音符");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (Controler.GetJumpButtonFlg())
        if (Player.GetComponent<PlayerMove>().GetHipDropNow())
        {
            Debug.Log("ヒップドロップ");
            // SmokeEffect.SetActive(true);
            SmokeEffect.GetComponent<Effect>().EffectPlay();
        }
        // if (DushEffect.GetComponent<Effect>().GetFlg())
        if (DushEffect.activeSelf)
        {
            float Angle = Player.GetComponent<PlayerMove>().GetAngle();
            float RadAngle = Mathf.Deg2Rad * Player.GetComponent<PlayerMove>().GetAngle();

            DushEffect.GetComponent<VisualEffect>().SetFloat("Angle", Angle);

            float x = Player.transform.position.x + Mathf.Cos(RadAngle);
            float y = Player.transform.position.y + Mathf.Sin(RadAngle);

            DushEffect.transform.position = new Vector3(x, y, 0);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CreateOnpu();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnpuDownFlg = true;
        }
    }

    void CreateOnpu()
    {
        // GameObject TempObj = Instantiate(obj);
        Pos = this.gameObject.transform.position;
         GameObject TempObj = (GameObject)Instantiate(obj,Pos,Quaternion.identity);
        TempObj.transform.parent = this.gameObject.transform;
    }
    static public bool GetOnpuDownFlg()
    {
        return OnpuDownFlg;
    }
}
