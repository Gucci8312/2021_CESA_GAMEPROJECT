using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effect : MonoBehaviour
{
    public bool StartFlg;
    VisualEffect vf;

    // Start is called before the first frame update
    void Start()
    {
        vf = this.gameObject.GetComponent<VisualEffect>();
        vf.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(StartFlg)
        {
            vf.Play();
        }
    }
}
