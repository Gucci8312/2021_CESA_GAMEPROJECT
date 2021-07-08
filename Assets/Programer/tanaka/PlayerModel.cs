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
<<<<<<< HEAD
        //int Point = PlayerScript.GetStartPoint();
        float InsideAngleSum = 0f;
        if (PlayerScript.InsideFlg)
=======
        if (!PlayerScript.GetPause())
>>>>>>> ac80fc59d50ecb856359ffdfa34f34eba4e94ef9
        {
            
        }
        else
        {
           
        }
    }

<<<<<<< HEAD

        if (PlayerScript.RotateLeftFlg)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 360f - PlayerScript.angle + InsideAngleSum);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, PlayerScript.angle + InsideAngleSum);
        }

        this.transform.Rotate(0, InsideAngleSum, 0);
=======
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
>>>>>>> ac80fc59d50ecb856359ffdfa34f34eba4e94ef9

            this.transform.Rotate(0, InsideAngleSum, 0);
    }

}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerModel : MonoBehaviour
//{
//    PlayerMove PlayerScript;
//    bool InsideflgCopy;
//    // Start is called before the first frame update
//    void Start()
//    {
//        PlayerScript = GameObject.Find("Player").GetComponent<PlayerMove>();
//        this.transform.Rotate(90, 0, 0);
//        InsideflgCopy = PlayerScript.InsideFlg;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //int Point = PlayerScript.GetStartPoint();
//        float InsideAngleSum = 0;
//        if (PlayerScript.InsideFlg != InsideflgCopy)
//        {
//            InsideAngleSum = 180;
//        }
//        else
//        {
//            InsideAngleSum = 0;
//        }
//        InsideflgCopy = PlayerScript.InsideFlg;

//        if (PlayerScript.RotateLeftFlg)
//        {
//            //this.transform.eulerAngles = new Vector3(0, 180, InsideAngleSum);
//        }
//        else
//        {
//            //this.transform.eulerAngles = new Vector3(0, 0, InsideAngleSum);
//        }

//        this.transform.Rotate(0, 0, InsideAngleSum);

//    }
//}
