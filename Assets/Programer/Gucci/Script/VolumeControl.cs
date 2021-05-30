using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public GameObject[] Sound=new GameObject[3];
    private bool SoundFlg;
    private int NowIdx;

    // Start is called before the first frame update
    void Start()
    {
        //  m_slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (SoundFlg)
        {
            if (Controler.GetUpButtonFlg())
            {
                NowIdx--;
                if (NowIdx < 0)
                {
                    NowIdx++;
                }
            }
            else if (Controler.GetDownButtonFlg())
            {
                NowIdx++;
                if (NowIdx > 3)
                {
                    NowIdx--;
                }
            }

            if(Controler.GetRightButtonFlg())
            {
                Sound[NowIdx].GetComponent<VolParam>().AddParam();
                SoundManager.MasterVolume = Sound[NowIdx].GetComponent<VolParam>().GetParam();
            }
            else if(Controler.GetLeftButtonFlg())
            {
                Sound[NowIdx].GetComponent<VolParam>().PullParam();
                SoundManager.MasterVolume = Sound[NowIdx].GetComponent<VolParam>().GetParam();
            }
        }
    }
}
