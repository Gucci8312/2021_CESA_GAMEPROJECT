using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeX : MonoBehaviour
{
    public Animator camAnim;

    public void CamShakeX()
    {
        camAnim.SetTrigger("ShakeX");
    }
}
