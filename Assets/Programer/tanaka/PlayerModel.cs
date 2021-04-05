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
        int Point = PlayerScript.GetStartPoint();
        float InsideAngleSum = 0;
        if (PlayerScript.InsideFlg)
        {
            InsideAngleSum = 180;
        }
        else
        {
            InsideAngleSum = 0;
        }


        if (PlayerScript.RotateLeftFlg)
        {
            this.transform.eulerAngles = new Vector3(0, 180, (45 * Point) + InsideAngleSum);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, 360 - (45 * Point) + InsideAngleSum);
        }

        this.transform.Rotate(0, InsideAngleSum, 0);

    }
}
