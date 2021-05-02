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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HipDrop();
            OldState = NowState;
            NowState = "HipDrop";
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Run();
            OldState = NowState;
            NowState = "Run";
        }
    }
    public void HipDrop()
    {
        Anim.SetBool("HipDropFlg", true);
    }

    public void Walk()
    {
        Anim.SetBool("WalkFlg", true);
    }

    public void Run()
    {
        Anim.SetBool("RunFlg", true);
    }

    void StopHipDrop()
    {
        Anim.SetBool("HipDropFlg", false);
        if(OldState == "Walk")
        {
            Walk();
        }
        else if(OldState == "Run")
        {
            Run();
        }
    }
}
