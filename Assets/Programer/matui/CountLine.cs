using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//回数制限のある線
public class CountLine : MonoBehaviour
{
    public float Count = 3;
    float InitCount;
    bool isflag = true; //存在するかどうか（falseで消す）
    CrossLine Cl;
    public float Wariai;

    float time = 0;

    Renderer LineColor;//色のマテリアル

    // Start is called before the first frame update
    void Start()
    {
        InitCount = Count;
        Cl = this.GetComponent<CrossLine>();
        LineColor = this.GetComponent<Renderer>();
        //LineColor.material.EnableKeyword("_EMISSION");
        CountState();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DestroyLine();
        CountDown();
        CountState();
    }

    private void CountDown()
    {
        if (Cl.GetGotoLineFlag())
        {
            Count--;
            Cl.SetGotoLineFlag(false);
            //CountState();
        }
    }

    private void CountState()
    {
        Wariai = (Count / InitCount) * 100;
        float Wariai2 = (100 / InitCount);

        isflag = true;

        if (100 >= Wariai && Wariai > Wariai2 * 2)
        {
            LineColor.material.SetColor("_EmissionColor", new Vector4(0, 1, 0, 0));
        }
        else if (Wariai2 * 2 >= Wariai && Wariai > Wariai2 * 1)
        {
            LineColor.material.SetColor("_EmissionColor", new Vector4(1, 1, 0, 0));
        }
        else if (Wariai2  >= Wariai && Wariai > 0)
        {
            LineColor.material.SetColor("_EmissionColor", new Vector4(1, 0, 0, 0));
        }
        else if (Wariai == 0)
        {
            isflag = false;
            this.transform.position = new Vector3(2000, 0, 0);
        }


        //if (Count == 0)
        //{
        //    isflag = false;
        //    this.transform.position = new Vector3(2000, 0, 0);
        //}
    }

    private void DestroyLine()
    {
        if (!isflag)
        {
            if(time!=0) Destroy(this.gameObject);
            time += Time.deltaTime;
        }
    }
}
