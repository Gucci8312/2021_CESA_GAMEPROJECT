using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵が乗っているメビウスの輪の挙動
public class EnemyMobius : MonoBehaviour
{
    private bool EnemyBeatFlag;                                 //ビートが指定した回数になったかどうか
    public int MaxBeatNum = 4;                                  //ビート最大数指定
    float BeatCount = 0;

    MoveMobius Mm;//MoveMobiusスクリプト

    private Vector3 PlayerVec;                                   //プレイヤーが乗っているメビウスへのベクトル
    private Vector2 TargetVec;                                   //最短距離へ移動するためのベクトル
    MoveMobius[] Mobius;                                         //すべてのメビウスの輪（単体の輪）を格納
    GameObject Player;

    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用

    // Start is called before the first frame update
    void Start()
    {
        Mm = this.GetComponent<MoveMobius>();
        Player = GameObject.Find("Player");

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)//時間が止まっていなければ
        {
            BeatCounter();//ビート数チェック

            if (Mm.EnemyMoveFlag && !Mm.PlayerMoveFlg && Mm.Getcl().Count != 0) //エネミーが乗ってたら かつ　線に乗っていたら
            {
                if (GoToVectorFlag())
                {
                    Mm.EnemyOnMoveFlag(EnemyBeatFlag, TargetVec);
                }
            }
        }
    }

    //何ビートか調べる
    private void BeatCounter()
    {
        if (BeatCount>=MaxBeatNum)
        {
            EnemyBeatFlag = true;
            BeatCount = 0;
        }
        else
        {
            EnemyBeatFlag = false;

        }

        if (this.rythm.m_EmobiusBeatFlag)//ビートを刻んだら
        {
            //time += Time.deltaTime;
            BeatCount++;
        }
    }

    //最短距離を求めて移動が出来るかどうか調べる
    private bool GoToVectorFlag()
    {
        List<Vector2> MoveVec = new List<Vector2>();
        for(int i=0;i< Mm.Getcl().Count; i++)
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

        TargetVec = NearVector(MoveVec, PlayerVec);//目標への最短距離を取得

        if (SearchMobiusFlag(TargetVec))
        {
            return true;
        }

        return false;
    }

    //指定した方向が引数のリスト中から近いものを調べる
    private Vector2 NearVector(List<Vector2> _vec,Vector2 SerchVec)
    {
        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < _vec.Count; i++)
        {
            distance.Add((SerchVec - _vec[i]).magnitude);

            if (distance[i] == 0)//差がない（同じ座標）場合
            {
                distance[i] = 10000;//適当に大きい値を入れて最小の値として取得させないようにする
            }

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
        float distance=0;//適切な長さを取得用

        for(int i=0;i < Mm.Getcl().Count; i++)
        {
            distance=Mm.Getcl()[i].NearLRPosDistance(this.transform.position,vec);
            if (distance != 0)
            {
                break;
            }
        }

        Ray ray = new Ray(new Vector3(this.transform.position.x , this.transform.position.y, this.transform.position.z),
                        new Vector3(vec.x * 1, vec.y * 1, 0));
        //貫通レイキャスト
        foreach (RaycastHit hit in Physics.RaycastAll(ray, distance+100))
        {
            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

            if (hit.collider.gameObject.CompareTag("Mobius"))
            {
                if (hit.collider.gameObject.GetComponent<MoveMobius>().EnemyMoveFlag//メビウスにエネミーが乗っていたなら
                    || hit.collider.gameObject.GetComponent<MoveMobius>().GetMobiusStripFlag())//メビウスの輪になっていたら 
                {
                    return false;
                }
            }
        }

        return true;
    }
}
