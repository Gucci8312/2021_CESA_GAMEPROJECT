using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    static bool UpStickFlg;
    static bool DownStickFlg;
    static bool RightStickFlg;
    static bool LeftStickFlg;
    static bool InputFlg = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void TrueInputFlg()
    {
        InputFlg = true;
    }

    static public void FalseInputFlg()
    {
        InputFlg = false;
    }

    public void WaitInput(float _Time)
    {
        InputFlg = false;
        Invoke("InputFlg", _Time);
    }

    public static bool GetJumpButtonFlg()
    {
        bool Response = false;

       // if (InputFlg)
       // {
            if (Input.GetKeyDown("joystick button 0"))
            {
                Response = true;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                Response = true;
            }
            //return Response;
            //bool Response = false;
            else if (Input.GetAxis("LTrigger") != 0.0f && LeftStickFlg == false)
            {
                Response = true;
                LeftStickFlg = true;
            }
            else if (Input.GetAxis("LTrigger") == 0.0f)
            {
                LeftStickFlg = false;
            }
      //  }

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
            SoundManager.PlaySeName("決定音");
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.PlaySeName("決定音");
            Response = true;
        }
        return Response;
    }

    public static bool GetCanselButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 1"))
        {
            // SoundManager.PlaySeName("決定音");
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
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
        else if (Input.GetAxis("RTrigger") != 0.0f && RightStickFlg == false)
        {
            Response = true;
            RightStickFlg = true;
        }
        else if (Input.GetAxis("RTrigger") == 0.0f)
        {
            RightStickFlg = false;
        }
        return Response;
    }

    public static bool GetUpButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Vertical") > 0.0f && UpStickFlg == false)
        {
            Response = true;
            UpStickFlg = true;
            return Response;
        }
        else if (Input.GetAxisRaw("Vertical") == 0.0f)
        {
            UpStickFlg = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Response = true;
        }

        return Response;
    }
    public static bool GetDownButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Vertical") < 0.0f && DownStickFlg == false)
        {
            Response = true;
            DownStickFlg = true;
            return Response;
        }
        else if (Input.GetAxisRaw("Vertical") == 0.0f)
        {
            DownStickFlg = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Response = true;
        }
        return Response;
    }

    public static bool GetRightButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Horizontal") > 0.0f && RightStickFlg == false)
        {
            Response = true;
            RightStickFlg = true;
            return Response;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0.0f)
        {
            RightStickFlg = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Response = true;
        }
        return Response;
    }
    public static bool GetLeftButtonFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("Horizontal") < 0.0f && LeftStickFlg == false)
        {
            Response = true;
            LeftStickFlg = true;
            return Response;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0.0f)
        {
            LeftStickFlg = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Response = true;
        }
        return Response;
    }

    public static bool GetLeftTriggerFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("LTrigger") < 0.0f /*&& LeftStickFlg == false*/)
        {
            Response = true;
            // LeftStickFlg = true;
        }
        else if (Input.GetAxisRaw("LTrigger") == 0.0f)
        {
            // LeftStickFlg = false;
        }
        return Response;
    }
    public static bool GetRightTriggerFlg()
    {
        bool Response = false;
        if (Input.GetAxisRaw("RTrigger") < 0.0f /*&& LeftStickFlg == false*/)
        {
            Response = true;
            // LeftStickFlg = true;
        }
        else if (Input.GetAxisRaw("RTrigger") == 0.0f)
        {
            //LeftStickFlg = false;
        }
        return Response;
    }

    public static bool GetXButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 2"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Response = true;
        }
        return Response;
    }

    public static bool GetYButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 3"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Response = true;
        }
        return Response;
    }

    public static bool GetLBButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 4"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Response = true;
        }
        return Response;
    }

    public static bool GetRBButtonFlg()
    {
        bool Response = false;
        if (Input.GetKeyDown("joystick button 5"))
        {
            Response = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
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