using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaterControl : MonoBehaviour
{
    private CharacterController CharaContorler;
    private Animator Anim;
    string NowStateFlg;
    string OldState;

    // Start is called before the first frame update
    void Start()
    {
        CharaContorler = GetComponent<CharacterController>();
        Anim = GetComponent<Animator>();
        NowStateFlg = "WaitFlg";
        Anim.SetBool("WaitFlg", true);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Wait()
    {
        //  OldState = NowState;
        // NowState = "HipDrop";
        if (NowStateFlg != "WaitFlg")
        {
            Anim.SetBool(NowStateFlg, false);
        }
        Anim.SetBool("WaitFlg", true);
        NowStateFlg = "WaitFlg";
    }
    public void HipDrop()
    {
        //  OldState = NowState;
        // NowState = "HipDrop";
        if (NowStateFlg != "HipDropFlg")
        {
            Anim.SetBool(NowStateFlg, false);
        }
        Anim.SetBool("HipDropFlg", true);
        NowStateFlg = "HipDropFlg";
        //Anim.SetBool(NowStateFlg, false);
    }

    public void Walk()
    {
        if (NowStateFlg != "WalkFlg")
        {
            Anim.SetBool(NowStateFlg, false);
        }
        Anim.SetBool("WalkFlg", true);
        NowStateFlg = "WalkFlg";
        // NowState = "Walk";
    }

    public void Run()
    {
        if (NowStateFlg != "RunFlg")
        {
            Anim.SetBool(NowStateFlg, false);
        }
        Anim.SetBool("RunFlg", true);
        NowStateFlg = "RunFlg";
        //Anim.SetBool("WalkFlg", false);
        OldState = "Run";
        // NowState = "Run";
    }

    void StopHipDrop()
    {
        //Debug.Log("ヒップドロップ終わった");
        //if (NowStateFlg != "HipDropFlg")
        //{
        //    Anim.SetBool(NowStateFlg, false);
        //}
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
        if (NowStateFlg != "GameClearRFlg")
        {
            Anim.SetBool(NowStateFlg, false);
        }
        Anim.SetBool("GameClearRFlg", true);
    }
    public void GameClearLightVer()
    {
        if (NowStateFlg != "GameClearLFlg")
        {
            Anim.SetBool(NowStateFlg, false);
        }

        Anim.SetBool("GameClearLFlg", true);
    }
}
