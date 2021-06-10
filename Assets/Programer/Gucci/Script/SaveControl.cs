using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveControl : MonoBehaviour
{
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
        sw.Flush();
        sw.Close();
    }
}
