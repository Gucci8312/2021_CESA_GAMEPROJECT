﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//線の処理
public class CrossLine : MonoBehaviour
{
    public enum GimicType//線のギミックの種類
    {
       NONE = 0,
       MoveLine,
       RootationLine,
    }
    public GimicType Type;

    //MoveLineで変更したりするのでpublicにしてる
    [HideInInspector] public List<Vector2> CrossPos = new List<Vector2>();                            //自身の交点
    [HideInInspector] public List<GameObject> Line = new List<GameObject>();                          //線のオブジェクト
    [HideInInspector] public List<CrossLine> cl = new List<CrossLine>();                              //CrossLineスクリプト

    private Vector2 LPos;//左端の点
    private Vector2 RPos;//右端の点

    private Vector2 Lvec;//左端の点に対してのベクトル
    private Vector2 Rvec;//右端の点に対してのベクトル

    [HideInInspector] public Vector3 RayHitPos;

    [HideInInspector] public bool GimicLineFlag=false;             //動く床かどうか（MoveLineから操作）
    [HideInInspector] public bool GimicOnFlag = false;             //動いてるかどうか（MoveLineから操作）

    private bool GotoLineFlag = false; //通ったかどうか

    Transform ThisTransform;
    // Start is called before the first frame update
    void Start()
    {
        ThisTransform = this.GetComponent<Transform>();
        this.GetComponent<BoxCollider>().isTrigger = true;
        this.GetComponent<BoxCollider>().size = new Vector3(1.05f, 1, 1);

        LRPosVecUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        ALLUpdate();
    }

    //全ての更新処理（publicなのはギミックの移動時のずれをなくすため）
    public void ALLUpdate()
    {
        LRPosVecUpdate();
        CrossPosUpdate();
    }

    //交点の更新
    private void CrossPosUpdate()
    {
        if (Line.Count != 0)
        {
            for (int i = 0; i < Line.Count; i++)
            {
                Vector2 vec;
                CrossLinePosition(this.gameObject, Line[i], out vec);
                CrossPos[i] = vec;
            }
        }

        if (Line.Count < CrossPos.Count)//線同士が離れた際、無駄な交点が残ってしまった場合
        {
            CrossPos.RemoveRange(Line.Count, CrossPos.Count - Line.Count);
        }
    }

    //端の座標と方向の更新
    private void LRPosVecUpdate()
    {
        LPos = RotationfromPosition(ThisTransform.position, ThisTransform.localScale, ThisTransform.localEulerAngles.z, 1);//自分の左端の回転を含めた座標を取得
        RPos = RotationfromPosition(ThisTransform.position, ThisTransform.localScale, ThisTransform.localEulerAngles.z, 0);//自分の右端の回転を含めた座標を取得

        float Radius = Mathf.Atan2(LPos.y - RPos.y, LPos.x - RPos.x); //自分と指定した座標とのラジアンを求める
        Lvec = new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);

        Radius = Mathf.Atan2(RPos.y - LPos.y, RPos.x - LPos.x); //自分と指定した座標とのラジアンを求める
        Rvec = new Vector3(Mathf.Cos(Radius), Mathf.Sin(Radius), 0);
    }

    //回転したときの座標を求める（横長の線を基準に回転）
    private Vector2 RotationfromPosition(Vector2 pos, Vector2 scale, float Angle, int tyouten)
    {
        scale.y = 0;//横長の棒の先端に点を置くために縦軸を0にする
        scale.x = scale.x*1.1f;//多少の貫通していないのを無視させるためのスケールアップ

        float theta = Mathf.Atan((scale.y / 2) / (scale.x / 2)) * 180 / 3.14f;
        float sha = Mathf.Sqrt((scale.x / 2) * (scale.x / 2) + (scale.y / 2) * (scale.y / 2));


        float deg = 0;

        switch (tyouten)
        {
            case 0:
                deg = theta + Angle;
                break;
            case 1:
                deg = 180 - theta + Angle;
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

    //引数のベクトルが線のベクトルに近いかどうか（true時に交点のある方向を返す）
    public bool CanInputMoveVec(Vector2 _vec, out Vector2 outvec)
    {
        float gosa = 0.6f;//許容範囲（0～0.9）
        float dis1 = (_vec - Lvec).magnitude;
        float dis2 = (_vec - Rvec).magnitude;

        if (dis1 <= gosa)
        {
            outvec = Lvec;/* Debug.Log("方向が同じLvec" + Lvec);*/
            return true;
        }
        else if (dis2 <= gosa)
        {
            outvec = Rvec;/* Debug.Log("方向が同じRvec" + Rvec);*/
            return true;
        }

        outvec = _vec;/* Debug.Log("方向が違う");*/

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
    public Vector2 GetRvec()
    {
        return Rvec;
    }
    public Vector2 GetLvec()
    {
        return Lvec;
    }

    public void SetGotoLineFlag(bool flag)
    {
        GotoLineFlag = flag;
    }
    public bool GetGotoLineFlag()
    {
        return GotoLineFlag;
    }

    //調べたい座標が右端の交点にいるかどうか
    public bool NearEndRCrossPosFlag(Vector2 SerchPos)
    {
        Vector2 Endpos = NearCrossPos(RPos);

        float distance = (Endpos - SerchPos).magnitude;
        if (distance <= 5)
        {
            return true;
        }

        return false;
    }

    //調べたい座標が右端の交点にいるかどうか(Gosaは許容値)
    public bool NearEndRCrossPosFlag(Vector2 SerchPos,float Gosa)
    {
        Vector2 Endpos = NearCrossPos(RPos);

        float distance = (Endpos - SerchPos).magnitude;
        if (distance <= Gosa)
        {
            return true;
        }

        return false;
    }


    //調べたい座標が左端の交点にいるかどうか
    public bool NearEndLCrossPosFlag(Vector2 SerchPos)
    {
        Vector2 Endpos = NearCrossPos(LPos);

        float distance = (Endpos - SerchPos).magnitude;
        if (distance <= 5)
        {
            return true;
        }

        return false;
    }

    //調べたい座標が左端の交点にいるかどうか(Gosaは許容値)
    public bool NearEndLCrossPosFlag(Vector2 SerchPos,float Gosa)
    {
        Vector2 Endpos = NearCrossPos(LPos);

        float distance = (Endpos - SerchPos).magnitude;
        if (distance <= Gosa)
        {
            return true;
        }

        return false;
    }


    //調べ対座標が右端にあるかどうか
    public bool NearEndRPosFlag(Vector2 SerchPos)
    {
        float distance = (RPos - SerchPos).magnitude;
        if (distance <= 5)
        {
            return true;
        }
        return false;
    }

    //調べ対座標が右端にあるかどうか(Gosaは許容値)
    public bool NearEndRPosFlag(Vector2 SerchPos,float Gosa)
    {
        float distance = (RPos - SerchPos).magnitude;
        if (distance <= Gosa)
        {
            return true;
        }
        return false;
    }

    //調べ対座標が左端にあるかどうか
    public bool NearEndLPosFlag(Vector2 SerchPos)
    {
        float distance = (LPos - SerchPos).magnitude;
        if (distance <= 5)
        {
            return true;
        }
        return false;
    }

    //調べ対座標が左端にあるかどうか(Gosaは許容値)
    public bool NearEndLPosFlag(Vector2 SerchPos,float Gosa)
    {
        float distance = (LPos - SerchPos).magnitude;
        if (distance <= Gosa)
        {
            return true;
        }
        return false;
    }

    public bool SameLRvec(Vector2 _Lvec, Vector2 _Rvec)
    {
        if (_Lvec == Lvec && _Rvec == Rvec)
        {
            return true;
        }
        return false;
    }

    //引数の方向にある端との距離を返す
    public float NearLRPosDistance(Vector2 pos, Vector2 vec)
    {
        Vector2 LRPos = Vector2.zero;
        if (vec == Rvec)
        {
            LRPos = RPos;
        }
        else if (vec == Lvec)
        {
            LRPos = LPos;
        }

        float distance = (pos - LRPos).magnitude;
        return distance;
    }

    //交点のリストの中から引数に近い交点を返す
    public Vector2 NearCrossPos(Vector2 SerchPos)
    {
        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < CrossPos.Count; i++)
        {
            distance.Add((SerchPos - CrossPos[i]).magnitude);

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

        return SerchPos;
    }

    //交点のリストの中から引数に近い交点を返す(outは交点の要素番号を渡す)
    public Vector2 NearCrossPos(Vector2 SerchPos, out int outnum)
    {
        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for (int i = 0; i < CrossPos.Count; i++)
        {
            //if (CrossPos[i] == Vector2.zero) continue;

            distance.Add((SerchPos - CrossPos[i]).magnitude);

            if (distance[i] == 0 ||CrossPos[i] == Vector2.zero)//差がない（同じ座標）場合か　交点が０なら
            {
                distance[i] = 10000;//適当に大きい値を入れて最小の値として取得させないようにする
                if (CrossPos[i] == Vector2.zero) Debug.Log("交点" + CrossPos[i]);
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
                outnum = i; //メビウスに渡す番号
                return CrossPos[i];
            }
        }
        outnum = 0;
        return SerchPos;
    }

    //引数のリストの要素と近い交点を返す
    public Vector2 NearListCrossPos(List<Vector2> SerchPos)
    {
        for (int i = 0; i < CrossPos.Count; i++)
        {
            for (int j = 0; j < SerchPos.Count; j++)
            {
                float distance = (CrossPos[i] - SerchPos[j]).magnitude;
                if (distance < 10)
                {
                    return CrossPos[i];
                }
            }
        }

        return Vector2.zero;
    }

    //オブジェクトのリストの中の要素を検索する
    private bool SameObjListSearch(List<GameObject> ListObj, GameObject SearchObj)
    {
        int Count = 0;
        for (int i = 0; i < ListObj.Count; i++)
        {
            if (ListObj[i] != SearchObj)//リストの要素と検索したいオブジェクトが違うなら
            {
                Count++;
            }
        }
        if (ListObj.Count == Count)//リストの中に検索したいオブジェクトが無ければ
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool NearRvecFlag(Vector2 SerchVec)
    {
        float distance = (Rvec - SerchVec).magnitude;
        if (distance <= 0.1f)
        {
            return true;
        }
        return false;
    }

    private bool NearLvecFlag(Vector2 SerchVec)
    {
        float distance = (Lvec - SerchVec).magnitude;
        if (distance <= 0.1f)
        {
            return true;
        }
        return false;
    }

    //メビウスが線に乗っているかどうかの情報を与える関数
    public void PutMobiusOnOff(bool flag, GameObject _obj)
    {
        switch (Type)
        {
            case GimicType.MoveLine:
                this.GetComponent<MoveLine>().PutMobiusOnOff(flag, _obj);
                break;

            case GimicType.RootationLine:
                this.GetComponent<RotationLine>().PutMobiusOnOff(flag, _obj);
                break;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            if (Line.Count == 0)//Lineリストに要素が無ければ
            {
                Line.Add(other.gameObject);//Lineリストに当たったものを追加
                cl.Add(other.GetComponent<CrossLine>());
                CrossPos.Add(Vector2.zero);
            }
            else//Lineリストに要素があれば
            {
                if (SameObjListSearch(Line, other.gameObject))//Lineリストの中に当たったものがなければ
                {
                    Line.Add(other.gameObject);//Lineリストに当たったものを追加
                    cl.Add(other.GetComponent<CrossLine>());
                    CrossPos.Add(Vector2.zero);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            CrossLine otherCL = other.GetComponent<CrossLine>();//相手のCrossLineスクリプトを取得

            if (!SameObjListSearch(Line, other.gameObject))
            {
                //お互いの交点の座標を取得（ほんのわずかなずれがあるので二つとも取得する）
                Vector2 pos1 = NearListCrossPos(otherCL.GetCrossPos());
                Vector2 pos2 = otherCL.NearListCrossPos(CrossPos);

                //お互いに該当する交点を削除する
                CrossPos.Remove(pos1);
                otherCL.CrossPos.Remove(pos2);

                //登録したリストの中に該当する要素を削除する
                Line.Remove(other.gameObject);
                cl.Remove(otherCL);

                //相手の該当するリストの中に該当する要素を削除
                otherCL.Line.Remove(this.gameObject);
                otherCL.cl.Remove(this.GetComponent<CrossLine>());

                //Debug.Log("線同士が離れた");
            }
        }
    }
}
