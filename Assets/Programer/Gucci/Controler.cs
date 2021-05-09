using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    static bool UpStickFlg;
    static bool DownStickFlg;
    static bool RightStickFlg;
    static bool LeftStickFlg;

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
    
    public static bool CanselButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 0"))
        {
            // SoundManager.PlaySeName("決定音");
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

    public static bool UpButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Vertical") > 0.0f && UpStickFlg == false)
        {
            Response = true;
            UpStickFlg = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 0.0f)
        {
            UpStickFlg = false;
        }
        return Response;
    }
    public static bool DownButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Vertical") < 0.0f && DownStickFlg == false)
        {
            Response = true;
            DownStickFlg = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 0.0f)
        {
            DownStickFlg = false;
        }
        return Response;
    }

    public static bool RightButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Horizontal") > 0.0f && RightStickFlg == false)
        {
            Response = true;
            RightStickFlg = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0.0f)
        {
            RightStickFlg = false;
        }
        return Response;
    }
    public static bool LeftButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Horizontal") < 0.0f && LeftStickFlg == false)
        {
            Response = true;
            LeftStickFlg = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0.0f)
        {
            LeftStickFlg = false;
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