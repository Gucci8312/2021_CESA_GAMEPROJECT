using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    PlayerMove PlayerScript;
    float AngleY;
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = GameObject.Find("Player").GetComponent<PlayerMove>();
        AngleY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerScript.GetPause())
        {
            
        }
        else
        {
           
        }
    }

    public void NormalModel()
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

                if (AngleY < InsideAngleSum)
                {
                    AngleY += 10;
                }
                else if (AngleY > InsideAngleSum)
                {
                    AngleY -= 10;
                }
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, PlayerScript.GetModelAngle() + InsideAngleSum);
                if (AngleY < InsideAngleSum)
                {
                    AngleY += 10;
                }
                else if (AngleY > InsideAngleSum)
                {
                    AngleY -= 10;
                }
            }

            this.transform.Rotate(0, InsideAngleSum, 0);
    }

}

