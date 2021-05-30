using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effect : MonoBehaviour
{
    bool Flg = true;
    VisualEffect vf;

    // Start is called before the first frame update
    void Start()
    {
        vf = this.gameObject.GetComponent<VisualEffect>();
        // vf.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Flg)
        //{
        //    vf.Play();
        //}
        //else
        //{
        //    vf.Stop();
        //}
    }

    public void SetFlg(bool _Flg)
    {
        Flg = _Flg;
    }
    public bool GetFlg()
    {
        return Flg;
    }

    //VisualEffect vf;
    //GameObject Player;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    Player = GameObject.Find("Player"); ;

    //    vf = this.gameObject.GetComponent<VisualEffect>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //if (Flg)
    //    //{
    //    //    vf.Play();
    //    //}
    //    //else
    //    //{
    //    //    vf.Stop();
    //    //}
    //    float Angle = Player.GetComponent<PlayerMove>().GetAngle();
    //    float RadAngle = Mathf.Deg2Rad * Player.GetComponent<PlayerMove>().GetAngle();

    //    vf.SetFloat("Angle", Angle);

    //    float x = Player.transform.position.x + Mathf.Cos(RadAngle);
    //    float y = Player.transform.position.y + Mathf.Sin(RadAngle);

    //    this.gameObject.transform.position = new Vector3(x, y, 0);

    //}

    public void EffectPlay()
    {
        vf.Play();
    }
}
