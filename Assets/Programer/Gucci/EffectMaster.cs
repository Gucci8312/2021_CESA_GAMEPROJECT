using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectMaster : MonoBehaviour
{
    GameObject[] Mobius;                                                        // メビウスの輪
    public GameObject[] CheckPoint;
    //GameObject[] CheckPoint;
    public VisualEffect vf;

    // Start is called before the first frame update
    void Start()
    {
        Mobius = GameObject.FindGameObjectsWithTag("Mobius");

        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }

        //CheckPoint = GameObject.FindGameObjectsWithTag("CheckPoint");

        //for (int i = 0; i < CheckPoint.Length; i++)
        //{
        //    CheckPoint[i] = GameObject.Find("CheckPoint (" + i + ")");                                        //全てのメビウス取得
        //}
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Mobius.Length; i++)
        {
            MoveMobius Script = Mobius[i].GetComponent<MoveMobius>();
            if (Script.GetMobiusColFlg())
            {
                CreateColMobiusEffect(Script);
            }
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    Target Script = CheckPoint[i].GetComponent<Target>();
        //    if (Script.GetColFlg())
        //    {
        //        CreateGetCheckPointEffect(Script);
        //    }
        //}
    }
    public static void CreateColMobiusEffect(MoveMobius _Script)
    {
        GameObject obj = (GameObject)Resources.Load("ColMobiusEffect");
        Instantiate(obj, _Script.GetMobiusColPos(), Quaternion.identity);
    }

    public static void CreateGetCheckPointEffect(Target _Script)
    {
        GameObject obj = (GameObject)Resources.Load("GetCheckPointEffect");
        Instantiate(obj, _Script.GetColPos(), Quaternion.identity);
    }
}
