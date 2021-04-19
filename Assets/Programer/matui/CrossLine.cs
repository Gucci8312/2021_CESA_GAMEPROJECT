using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLine : MonoBehaviour
{
    public List<Vector2> CrossPos = new List<Vector2>();//自身の交点
    private Vector2 LPos;//左端の点
    private Vector2 RPos;//右端の点

    private Vector2 Lvec;//左端の点に対してのベクトル
    private Vector2 Rvec;//右端の点に対してのベクトル

    public Vector3 RayHitPos;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;

        LPos = RotationfromPosition(this.transform.position, this.transform.localScale, this.transform.localEulerAngles.z, 0);//自分の左端の回転を含めた座標を取得
        RPos = RotationfromPosition(this.transform.position, this.transform.localScale, this.transform.localEulerAngles.z, 1);//自分の右端の回転を含めた座標を取得

        this.GetComponent<BoxCollider>().size = new Vector3(1.05f,1,1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //回転したときの座標を求める（横長の線を基準に回転）
    private Vector2 RotationfromPosition(Vector2 pos, Vector2 scale, float Angle,int tyouten)
    {
        scale.y = 0;//横長の棒の先端に点を置くために縦軸を0にする
        scale.x += 20;//多少の貫通していないのを無視させるためのスケールアップ

        float theta = Mathf.Atan((scale.y / 2) / (scale.x / 2)) * 180 / 3.14f;
        float sha = Mathf.Sqrt((scale.x / 2) * (scale.x / 2) + (scale.y / 2) * (scale.y / 2));

        
        float deg =0;

        switch (tyouten)
        {
            case 0:
                deg = theta + Angle;
                break;
            case 1:
                deg = 180-theta + Angle;
                break;
            //case 2:
            //    deg = 360-theta + Angle;
            //    break;
            //case 3:
            //    deg = 180+theta + Angle;
            //    break;
        }

        Vector2 outpos;
        outpos.x = pos.x + sha * Mathf.Cos(deg * 3.14f / 180);
        outpos.y = pos.y + sha * Mathf.Sin(deg * 3.14f / 180);


        return outpos;
    }

    //交点を求める（true時に交点の座標を返す）
    private bool CrossLinePosition(GameObject Line1, GameObject Line2, out Vector2 outvec)
    {
        outvec = Vector2.zero;//出力用
        Vector2 p1, p2, p3, p4;

        CrossLine cl1 = Line1.GetComponent<CrossLine>();//Line1のCrossLineスクリプトを取得　
        CrossLine cl2 = Line2.GetComponent<CrossLine>();//Line2のCrossLineスクリプトを取得　

        p1 = cl1.GetLPos();
        p2 = cl1.GetRPos();

        p3 = cl2.GetLPos();
        p4 = cl2.GetRPos();


        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        outvec.x = p1.x + u * (p2.x - p1.x);
        outvec.y = p1.y + u * (p2.y - p1.y);

        return true;
    }

    //メビウスが移動できる座標を与える
    public Vector2 CanMovePosition(Vector2 pos)
    {
        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < CrossPos.Count; i++)
        {
            distance.Add((pos - CrossPos[i]).magnitude);

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
                return CrossPos[i];
            }
        }

        return pos;
    }

    //交点と同じ座標があるかどうか
    public bool SameCrossPos(Vector2 _pos)
    {
        for (int i = 0; i < CrossPos.Count; i++)
        {
            float distance = (_pos - CrossPos[i]).magnitude;//交点と引数との差を求める
            if (distance <= 2) //差がほぼなければ
            {
                return true;
            }
        }
        return false;
    }

    //入力した方向と交点がある方向を調べる（true時に交点のある方向を返す）
    public bool CanInputMoveVec(Vector2 _vec, out Vector2 outvec)
    {

        float Radius = Mathf.Atan2(LPos.y - RPos.y, LPos.x - RPos.x); //自分と指定した座標とのラジアンを求める
        Lvec = new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);

        Radius = Mathf.Atan2(RPos.y - LPos.y, RPos.x - LPos.x); //自分と指定した座標とのラジアンを求める
        Rvec = new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);

        float gosa = 0.2f;
        float dis1 = (_vec - Lvec).magnitude;
        float dis2 = (_vec - Rvec).magnitude;

        if (dis1 <= gosa)
        {
            outvec = Lvec; Debug.Log("方向が同じLvec" + Lvec);
            return true;
        }
        else if(dis2 <= gosa)
        {
            outvec = Rvec; Debug.Log("方向が同じRvec"+ Rvec);
            return true;
        }

        outvec = _vec; Debug.Log("方向が違う");

        return false;
    }

    public List<Vector2> GetCrossPos()//交点を取得
    {
        return CrossPos;
    }
    public Vector2 GetRPos()
    {
        return RPos;
    }
    public Vector2 GetLPos()
    {
        return LPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            Vector2 vec;
            if (CrossLinePosition(this.gameObject, other.gameObject, out vec))
            {
                CrossPos.Add(vec);
            }
            else
            {
                Debug.Log("交点が求められなかった");
            }
        }
    }
}
