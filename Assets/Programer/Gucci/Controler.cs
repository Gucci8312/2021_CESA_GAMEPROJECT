using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool GetJumpButtonFlg()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            return true;
        }
        return false;
    }

    public static bool GetHipDropButtonFlg()
    {
        if (Input.GetKeyDown("joystick button 2"))
        {
            return true;
        }
        return false;
    }

    public static bool GetMenuButtonFlg()
    {
        if (Input.GetKeyDown("joystick button 7"))
        {
            return true;
        }
        return false;
    }
}
