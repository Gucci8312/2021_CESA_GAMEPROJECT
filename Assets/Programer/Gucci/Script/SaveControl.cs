using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveControl : MonoBehaviour
{
    static List<string[]> csvDatas = new List<string[]>();
    static TextAsset csvFile;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void Load()
    {
        StreamReader sr = new StreamReader("./Assets/Data/SaveData.txt");
        Debug.Log("Load");
        for (int Idx = 0; Idx < 25; Idx++)
        {
            string Line = sr.ReadLine();                                                            // 一行を格納
            string[] TempFileData = Line.Split(',');

            bool ClearFlg = System.Convert.ToBoolean(TempFileData[0]);                              // クリアしたかのフラグを変換して格納
            int Parcent = System.Convert.ToInt32(TempFileData[1]);                                  // スプレー取得％を変換して格納
            bool TimeAttackClearFlg = System.Convert.ToBoolean(TempFileData[2]);                    // タイムアタックをクリアしたかのフラグを変換して格納

            StageControl.ReleaseStage(Idx, ClearFlg, Parcent, TimeAttackClearFlg);                  // ステージ解放
        }
    }

    static public void NewGame()
    {
        StreamWriter sw = new StreamWriter("./Assets/Data/SaveData.txt");
        Debug.Log("NewGame");

        for (int Idx = 0; Idx < 25; Idx++)
        {
            sw.Write(false);                                                 // クリアしているか
            sw.Write(",");
            sw.Write(0);                                            // スプレー取得パーセント
            sw.Write(",");
            sw.Write(false);                                      // タイムアタッククリアしているか
            sw.Write("\n");
        }

        sw.Flush();
        sw.Close();
    }

    static public void Save()
    {
        StreamWriter sw = new StreamWriter("./Assets/Data/SaveData.txt");
        Debug.Log("Save");

        for (int Idx = 0; Idx < 25; Idx++)
        {
            sw.Write(StageControl.GetClearFlg(Idx));                                                 // クリアしているか
            sw.Write(",");
            sw.Write(StageControl.GetStageParsent(Idx));                                            // スプレー取得パーセント
            sw.Write(",");
            sw.Write(StageControl.GetTimeAttackClearFlg(Idx));                                      // タイムアタッククリアしているか
            sw.Write("\n");
        }

        sw.Flush();
        sw.Close();
    }

    static public void ResetSaveData()
    {
        for (int Idx = 0; Idx < 25; Idx++)
        {
            StageControl.ReleaseStage(Idx, false, 0, false);                  // ステージ解放
        }
        StageControl.ReleaseStage(0, true, 0, false);
    }
}
