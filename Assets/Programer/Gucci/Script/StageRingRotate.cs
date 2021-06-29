﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRingRotate : MonoBehaviour
{
    bool RotateFlg;
    public int Angle;
    Vector3 InitPos;
    bool AngleFlg;

    // Start is called before the first frame update
    void Start()
    {
        InitPos = this.transform.position;
    }
    //BufOut[index].x = cos(ToRad((float)index)) * BufInMoveData[0].Speed.x;
    //BufOut[index].y = sin(ToRad((float)index)) * BufInMoveData[0].Speed.y;
    //BufOut[index].z = cos(ToRad(BufInMoveData[0].Angle.z)) * BufInMoveData[0].Speed.z;

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
                Angle += 5;
            }

            Vector3 Pos = this.transform.position;
            //int Angle = 180;
            float Index = 0.5f;
            //Pos.x = Mathf.Cos(Angle) * Index;
            Pos.y = InitPos.y + Mathf.Sin(Mathf.Deg2Rad * Angle) * Index;
            Pos.z = InitPos.z + Mathf.Cos(Mathf.Deg2Rad * Angle) * Index;

            //this.transform.Translate(Pos.x, Pos.y, 10);
            this.transform.position = Pos;
        }
        else
        {
            if (Angle >= 90)
            {
                Angle -= 5;
            }

            Vector3 Pos = this.transform.position;
            //int Angle = 0;
            float Index = 0.5f;
            //Pos.x = Mathf.Cos(Angle) * Index;
            Pos.y = InitPos.y + Mathf.Sin(Mathf.Deg2Rad * Angle) * Index;
            Pos.z = InitPos.z + Mathf.Cos(Mathf.Deg2Rad * Angle) * Index;

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
}