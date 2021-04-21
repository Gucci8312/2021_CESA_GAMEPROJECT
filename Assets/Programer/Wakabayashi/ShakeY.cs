using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeY : MonoBehaviour
{
    public Animator camAnim;

    public void CamShakeY()
    {
        camAnim.SetTrigger("ShakeY");
    }
}
