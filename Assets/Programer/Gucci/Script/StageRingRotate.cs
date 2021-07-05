using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRingRotate : MonoBehaviour
{
    bool RotateFlg;
    public int Angle;
    Vector3 InitPos;
    bool AngleFlg;
    int RotateSpeed = 20;
    float OffSet = 0.65f;


    // Start is called before the first frame update
    void Start()
    {
        InitPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (RotateFlg)
        {
            if (AngleFlg)
            {
                Angle = 0;
                AngleFlg = false;
            }
            if (Angle < 260)
            {
                Angle += RotateSpeed;
            }

            Vector3 Pos = this.transform.position;
            //int Angle = 180;
            //Pos.x = Mathf.Cos(Angle) * Index;
            Pos.y = InitPos.y + Mathf.Sin(Mathf.Deg2Rad * Angle) * OffSet;
            Pos.z = InitPos.z + Mathf.Cos(Mathf.Deg2Rad * Angle) * OffSet;

            //this.transform.Translate(Pos.x, Pos.y, 10);
            this.transform.position = Pos;
        }
        else
        {
            if (Angle >= 110)
            {
                Angle -= RotateSpeed;
            }

            Vector3 Pos = this.transform.position;
            //int Angle = 0;

            //Pos.x = Mathf.Cos(Angle) * Index;
            Pos.y = InitPos.y + Mathf.Sin(Mathf.Deg2Rad * Angle) * OffSet;
            Pos.z = InitPos.z + Mathf.Cos(Mathf.Deg2Rad * Angle) * OffSet-1;

            //this.transform.Translate(Pos.x, Pos.y, 10);
            this.transform.position = Pos;
            //this.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
            AngleFlg = true;
        }
    }
    public void SetRotateFlg(bool _Flg)
    {
        RotateFlg = _Flg;
    }
    public bool GetRotateFlg()
    {
        return RotateFlg;
    }
}
