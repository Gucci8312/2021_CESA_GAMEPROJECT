using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵が乗っているメビウスの輪の挙動
public class EnemyMobius : MonoBehaviour
{
    public bool EnemyBeatFlag;                                 //ビートが指定した回数になったかどうか
    public int MaxBeatNum = 12;                                //ビート最大数指定
    public float BeatCount = 0;                                //現在のビート数
    bool BeatOnOffFlag = false;                                //一度だけrythmSendCheckFlagを取得用

    MoveMobius Mm;//MoveMobiusスクリプト

    private Vector3 PlayerVec;                                   //プレイヤーが乗っているメビウスへのベクトル
    private Vector2 TargetVec;                                   //最短距離へ移動するためのベクトル
    GameObject Player;

    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    // List<GameObject> Line = new List<GameObject>();                          //線のオブジェクト

    static bool StopFlag = false;//true:止める　false:動く

    // Start is called before the first frame update
    void Start()
    {
        Mm = this.GetComponent<MoveMobius>();
        Player = GameObject.Find("Player");

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!StopFlag)
        {
            EnemyMobiusUpdate();
        }
    }


    //EnemyMobiusの更新
    private void EnemyMobiusUpdate()
    {
        //if (Time.timeScale != 0)//時間が止まっていなければ
        //{
        BeatCounter();//ビート数チェック
        MobiusStripBeatReset();

        if (Mm.EnemyMoveFlag && !Mm.PlayerMoveFlg && Mm.Getcl().Count != 0) //エネミーが乗ってたら かつ　線に乗っていたら
        {
            if (GoToVectorFlag())
            {
                Mm.EnemyOnMoveFlag(EnemyBeatFlag, TargetVec);
                EnemyBeatFlag = false;

            }
        }


        //}
    }

    //何ビートか調べる
    private void BeatCounter()
    {
        if (BeatCount >= MaxBeatNum)
        {
            EnemyBeatFlag = true;
            BeatCount = 0;

        }
        //else
        //{
        //    BeatFlag = false;

        //}


        if (this.rythm.rythmSendCheckFlag && !BeatOnOffFlag)//ビートを刻んだら
        {
            //time += Time.deltaTime;
            //MoveLine.BeatCount++;
            BeatCount++;
            BeatOnOffFlag = true;
        }
        else if (!this.rythm.rythmSendCheckFlag && BeatOnOffFlag)
        {
            BeatOnOffFlag = false;
        }
    }

    //メビウスの輪の時にビートをリセット
    private void MobiusStripBeatReset()
    {
        bool MobiusStripCheckFlag = false;//どれかがメビウスの輪になっているかどうか

        //メビウスの輪になってないかチェック
        for (int i = 0; i < Mm.GetAllMm().Count; i++)
        {
            if (Mm.GetAllMm()[i].GetMobiusStripFlag() && Mm.GetAllMm()[i].GetPlayerMoveFlg())//プレイヤーが乗るメビウスが輪になれば
            {
                MobiusStripCheckFlag = true;
                break;
            }
        }

        if (MobiusStripCheckFlag)//全てのメビウスのどれかが輪になっていたら
        {
            BeatCount = 0;
            EnemyBeatFlag = false;
        }
    }

    //最短距離を求めて移動が出来るかどうか調べる
    private bool GoToVectorFlag()
    {
        List<Vector2> MoveVec = new List<Vector2>();
        for (int i = 0; i < Mm.Getcl().Count; i++)
        {
            if (!Mm.Getcl()[i].NearEndRCrossPosFlag(this.transform.position))
            {
                MoveVec.Add(Mm.Getcl()[i].GetRvec());/* Debug.Log("Rvecげっと！");*/
            }
            if (!Mm.Getcl()[i].NearEndLCrossPosFlag(this.transform.position))
            {
                MoveVec.Add(Mm.Getcl()[i].GetLvec());/* Debug.Log("Lvecげっと！");*/
            }

        }

        PlayerVec = Mm.SearchVector(this.transform.position, Player.transform.position);//プレイヤーが乗っているメビウスのベクトルを取得

        int Count = MoveVec.Count;
        for (int i = 0; i < Count; i++)
        {
            TargetVec = NearVector(MoveVec, PlayerVec);//目標への最短距離を取得
            if (SearchMobiusFlag(TargetVec))
            {
                return true;
            }
            else
            {
                MoveVec.Remove(TargetVec);
            }
        }

        return false;
    }

    //指定した方向が引数のリスト中から近いものを調べる
    private Vector2 NearVector(List<Vector2> _vec, Vector2 SerchVec)
    {
        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < _vec.Count; i++)
        {
            distance.Add((SerchVec - _vec[i]).magnitude);

            //if (distance[i] == 0)//差がない（同じ座標）場合
            //{
            //    distance[i] = 10000;//適当に大きい値を入れて最小の値として取得させないようにする
            //}

            if (distance[i] <= Min)//取得している最小の値より小さければ
            {
                Min = distance[i];//差が最小の値を取得
            }
        }

        for (int i = 0; i < distance.Count; i++)
        {
            if (distance[i] == Min)//差が最小の値を持った要素であれば
            {
                return _vec[i];
            }
        }

        return SerchVec;
    }

    //引数の方向にエネミーメビウスがあるかどうか
    private bool SearchMobiusFlag(Vector2 vec)
    {
        float distance = 0;//適切な長さを取得用

        for (int i = 0; i < Mm.Getcl().Count; i++)
        {
            distance = Mm.Getcl()[i].NearLRPosDistance(this.transform.position, vec);
            if (distance != 0)
            {
                break;
            }
        }

        Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                        new Vector3(vec.x * 1, vec.y * 1, 0));

        List<GameObject> ColObj = new List<GameObject>();               //すり抜けたメビウスオブジェクトを格納するリスト
        List<Vector3> HitPos = new List<Vector3>();                     //ヒットした座標

        //貫通レイキャスト
        foreach (RaycastHit hit in Physics.SphereCastAll(ray, Mm.GetThisR(), distance + 10))
        {
            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

            //if (hit.collider.gameObject.CompareTag("Mobius"))
            //{
            //    //if (hit.collider.gameObject.GetComponent<MoveMobius>().EnemyMoveFlag//メビウスにエネミーが乗っていたなら
            //    //    || hit.collider.gameObject.GetComponent<MoveMobius>().GetMobiusStripFlag())//メビウスの輪になっていたら 

            //    //if (!hit.collider.gameObject.GetComponent<MoveMobius>().GetPlayerMoveFlg())//メビウスにプレイヤーが乗っていないなら
            //    //{
            //    //    return false;
            //    //}
            //}

            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト
            GameObject hitObj = null;

            //レイが当たったオブジェクト
            switch (hit.collider.gameObject.tag)
            {
                case "Mobius":
                    hitObj = hit.collider.gameObject;
                    break;

                    //case "Block":
                    //    hitObj = hit.collider.gameObject;
                    //    break;
            }

            if (hitObj == this.gameObject || hit.point == Vector3.zero) { hitObj = null; }//ヒットした中に自身が含まれないようにする


            if (ColObj.Count == 0 && hitObj != null) //レイが当たったオブジェクトがあれば　かつ　リストが空なら
            {
                ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                HitPos.Add(hit.point);/* Debug.Log("HitPos" + hit.point);*/
            }
            else if (ColObj.Count != 0 && hitObj != null)
            {
                if (Mm.SameObjListSearch(ColObj, hitObj))//ColObjリストの中に当たったものがなければ
                {
                    ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                    HitPos.Add(hit.point);/* Debug.Log("HitPos" + hit.point);*/
                }
            }
        }


        if (ColObj.Count != 0)//リストの中に要素があれば
        {
            GameObject otherObj = Mm.NearObjSearch(ColObj, HitPos, this.transform.position);//リストの中から始点に近いオブジェクトを取得
                                                                                            //Debug.Log(otherObj.name + "とぶつかった~～");

            if (!otherObj.GetComponent<MoveMobius>().GetPlayerMoveFlg())//メビウスにプレイヤーが乗っていないなら
            {
                return false;
            }
        }

        return true;
    }

    static public void StopFlagSet(bool flag)
    {
        StopFlag = flag;
    }

}
