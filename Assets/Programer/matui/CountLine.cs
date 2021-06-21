using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//回数制限のある線
public class CountLine : MonoBehaviour
{
    public enum LineStatus
    {
        Live = 0,
        Dest,
    }
    /*public*/ LineStatus Status;

    public float Count = 3;
    float InitCount;
    //bool isflag = true; //存在するかどうか（falseで消す）
    CrossLine Cl;
    /*public*/ float Wariai;

    public float RecoveryMaxTime = 10;
    float time = 0;

    Renderer LineColor;//色のマテリアル

    Vector3 InitPos;

    MeshRenderer MeshRend;
    BoxCollider BoxCol;
    // Start is called before the first frame update
    void Start()
    {
        InitCount = Count;
        Cl = this.GetComponent<CrossLine>();
        LineColor = this.GetComponent<Renderer>();
        BoxCol = this.GetComponent<BoxCollider>();
        MeshRend = this.GetComponent<MeshRenderer>();


        //LineColor.material.EnableKeyword("_EMISSION");
        CountState();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (Status)
        {
            case LineStatus.Live:
                //DestroyLine();
                LineShowOnOff(true);
                CountDown();
                CountState();
                break;

            case LineStatus.Dest:
                LineShowOnOff(false);
                RecoveryTimer();
                break;
        }
    }

    //通れる回数を減らす関数
    private void CountDown()
    {
        if (Cl.GetGotoLineFlag())
        {
            Count--;
            Cl.SetGotoLineFlag(false);
            //CountState();
        }
    }

    //今の回数によって状態を変化させる回数
    private void CountState()
    {
        Wariai = (Count / InitCount) * 100;
        float Wariai2 = (100 / InitCount);

        //isflag = true;

        //回数を三段階で変化する
        if (100 >= Wariai && Wariai > Wariai2 * 2)//3/3～3/2なら
        {
            LineColor.material.SetColor("_EmissionColor", new Vector4(0, 1, 0, 0));
        }
        else if (Wariai2 * 2 >= Wariai && Wariai > Wariai2 * 1)//3/2～3/1なら
        {
            LineColor.material.SetColor("_EmissionColor", new Vector4(1, 1, 0, 0));
        }
        else if (Wariai2 >= Wariai && Wariai > 0)//3/1～0以上なら
        {
            LineColor.material.SetColor("_EmissionColor", new Vector4(1, 0, 0, 0));
        }
        else if (Wariai == 0)//０の場合は消す
        {
            //isflag = false;
            InitPos = this.transform.position;
            this.transform.position = new Vector3(2000, 0, 0);
            //LineShowOnOff(false);
            Status = LineStatus.Dest;
        }
    }

    //線が復活するまでの時間
    private void RecoveryTimer()
    {
        if (time >= RecoveryMaxTime)
        {
            time = 0;
            Count = InitCount;
            Status = LineStatus.Live;
            this.transform.position = InitPos;
            //LineShowOnOff(true);

        }

        time += Time.deltaTime;
    }

    private void LineShowOnOff(bool flag)
    {
        BoxCol.enabled = flag;
        MeshRend.enabled = flag;
        //this.gameObject.SetActive(flag);
    }

    //private void DestroyLine()
    //{
    //    if (!isflag)
    //    {
    //        if(time!=0) Destroy(this.gameObject);
    //        time += Time.deltaTime;
    //    }
    //}
}
