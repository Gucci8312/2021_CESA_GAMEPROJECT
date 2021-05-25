using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    PlayerMove PlayerScript;
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = GameObject.Find("Player").GetComponent<PlayerMove>();

    }

    // Update is called once per frame
    void Update()
    {
        //int Point = PlayerScript.GetStartPoint();
        float InsideAngleSum = 0f;
        if (PlayerScript.GetInsideFlg())
        {
            InsideAngleSum = 180f;
        }
        else
        {
            InsideAngleSum = 0f;
        }


        if (PlayerScript.GetRotateLeftFlg())
        {
            this.transform.eulerAngles = new Vector3(0, 180, 360f - PlayerScript.GetModelAngle() + InsideAngleSum);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, PlayerScript.GetModelAngle() + InsideAngleSum);
        }

        this.transform.Rotate(0, InsideAngleSum, 0);

    }
}

