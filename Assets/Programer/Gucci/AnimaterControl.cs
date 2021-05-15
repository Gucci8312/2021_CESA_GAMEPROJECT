using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaterControl : MonoBehaviour
{
    private CharacterController CharaContorler;
    private Animator Anim;
    string NowState;
    string OldState;

    // Start is called before the first frame update
    void Start()
    {
        CharaContorler = GetComponent<CharacterController>();
        Anim = GetComponent<Animator>();
        NowState = "Walk";
        Anim.SetBool("WalkFlg", true);

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    HipDrop();
        //    OldState = NowState;
        //    NowState = "HipDrop";
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    Run();
        //    OldState = NowState;
        //    NowState = "Run";
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    Walk();
        //    OldState = NowState;
        //    NowState = "Walk";
        //}
    }
    public void HipDrop()
    {
      //  OldState = NowState;
       // NowState = "HipDrop";
        Anim.SetBool("HipDropFlg", true);
    }

    public void Walk()
    {
        Anim.SetBool("WalkFlg", true);
        Anim.SetBool("RunFlg", false);
       // OldState = NowState;
       // NowState = "Walk";
    }

    public void Run()
    {
        Anim.SetBool("RunFlg", true);
        Anim.SetBool("WalkFlg", false);
        //OldState = NowState;
       // NowState = "Run";
    }

    void StopHipDrop()
    {
        Debug.Log("ヒップドロップ終わった");
        Anim.SetBool("HipDropFlg", false);
        if (OldState == "Walk")
        {
            Walk();
        }
        else if (OldState == "Run")
        {
            Run();
        }
    }

    public void GameClearRightVer()
    {
        Anim.SetBool("GameClearRFlg", true);
    }   
    public void GameClearLightVer()
    {
        Anim.SetBool("GameClearLFlg", true);
    }
}
