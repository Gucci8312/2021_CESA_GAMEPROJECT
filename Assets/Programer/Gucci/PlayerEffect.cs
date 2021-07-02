using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEffect : MonoBehaviour
{
    GameObject DushEffect;
    GameObject SmokeEffect;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player"); ;
        DushEffect = GameObject.Find("DushEffect"); 
        SmokeEffect = GameObject.Find("SmokeEffect");
        //Instantiate(DushEffect,Player.transform.position,Quaternion.identity);
        // vf = this.gameObject.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
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
    }
}
