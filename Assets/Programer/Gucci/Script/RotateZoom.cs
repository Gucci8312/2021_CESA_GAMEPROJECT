using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZoom : MonoBehaviour
{
    int Power = 10;
    float CountDown=1;
    UnityEngine.Quaternion Quation;
    // Start is called before the first frame update
    void Start()
    {
           //Quation=Transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        CountDown-=Time.deltaTime;
        if(CountDown>0)
        {
            //Angle++;
            Vector3 WorldAngle = transform.localEulerAngles;
            WorldAngle.x = 0.0f;
            WorldAngle.y = 0.0f;
            WorldAngle.z = Power;
            transform.Rotate(WorldAngle);
        }
        else
        {
            //transform.rotation=;
        }
    }
}
