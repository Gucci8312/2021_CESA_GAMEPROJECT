using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveControl : MonoBehaviour
{
    struct SAVEDATA
    {
        public bool ClearFlg;
        public int ClearPercent;
        public bool ClearTimeAtackFlg;
    }

    static private SAVEDATA[] SaveData = new SAVEDATA[25];

    // Start is called before the first frame update
    void Start()
    {
        //string SaveData;
        //SaveData = File.ReadAllText("a");
        // StageControl.(SaveData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void Load()
    {
        string Data;
        Data = File.ReadAllText("./Assets/Data/SaveData.txt");
        StageControl.ReleaseStage(int.Parse(Data));
        Debug.Log("Load");
    }

    static public void Save()
    {
        StreamWriter sw = new StreamWriter("./Assets/Data/SaveData.txt");
        sw.Write(StageControl.OpenStageNum());
        //Debug.Log("Save");
        //for (int Idx = 0; Idx < 25; Idx++)
        //{
        //    sw.Write(SaveData[Idx].ClearFlg);
        //    sw.Write(SaveData[Idx].ClearPercent);
        //    sw.Write(SaveData[Idx].ClearTimeAtackFlg);
        //}

        sw.Flush();
        sw.Close();
    }

    bool GetClearFlg(int _Idx)
    {
        return SaveData[_Idx].ClearFlg;
    }

    int GetClearPercent(int _Idx)
    {
        return SaveData[_Idx].ClearPercent;
    }

    bool GetClearTimeAtackFlg(int _Idx)
    {
        return SaveData[_Idx].ClearTimeAtackFlg;
    }

    bool SetClearFlg(int _Idx, bool _Flg)
    {
        return SaveData[_Idx].ClearFlg = _Flg;
    }

    int SetClearPercent(int _Idx, int _Parcent)
    {
        return SaveData[_Idx].ClearPercent = _Parcent;
    }

    bool SetClearTimeAtackFlg(int _Idx, bool _Flg)
    {
        return SaveData[_Idx].ClearTimeAtackFlg = _Flg;
    }
}
