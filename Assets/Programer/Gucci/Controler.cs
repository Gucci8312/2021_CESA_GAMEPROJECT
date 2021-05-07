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
        bool Response = false;
        if (Input.GetKeyDown("joystick button 0"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Response = true;
        }
        return Response;
    }

    public static bool GetMenuButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 7"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Response = true;
        }
        return Response;
    }

    public static bool SubMitButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 0"))
        {
           // SoundManager.PlaySeName("決定音");
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            //SoundManager.PlaySeName("決定音");
            Response = true;
        }
        return Response;
    }

    public static bool GetRythmButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 1"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Response = true;
        }
        return Response;
    }

    public static bool Up()
    {
        bool Response = false;
        if (Input.GetAxis("Vertical")<0.0f)
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Response = true;
        }
        return Response;
    }
    public static bool Down()
    {
        bool Response = false;
        if (Input.GetAxis("Vertical") > 0.0f)
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Response = true;
        }
        return Response;
    }
}

// A Button                 0
// B Button                 1
// X Button                 2
// Y Button                 3
// LeftBumper Button        4
// RightBumper Button       5
// Buck Button              6
// Start Button             7
// LeftStickClik            8
// RightStickClick          9