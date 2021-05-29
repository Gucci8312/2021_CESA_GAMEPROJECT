using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MoveMobiusから呼び出してもらうスクリプト
//動く線に乗っているときのメビウスの処理
public class LinePutMobius : MonoBehaviour
{
    private bool MoveLineFlag = false;//線が動いているかどうか
    private bool MoveLinePutFlag = false;//線に乗っているかどうか
   [HideInInspector] public Vector2 MoveLineVec;     //線が移動した方向（MoveLine側で操作）

    MoveMobius Mm;

    Vector3 OldPos;

    public List<GameObject> Line;


    public List<Vector2> vec;
    public Vector2 ColVec;
    public Vector2 hanareruVec;
    public float Movedis;

    public bool LinePutMobiusColFlag;
    // Start is called before the first frame update
    void Start()
    {
        Mm = this.GetComponent<MoveMobius>();
    }

    // Update is called once per frame
    void Update()
    {
        Movedis = (this.transform.position - OldPos).magnitude;//位置の差を取得

        MoveLineTrueStop();
        MoveLineSetting();
        LinePutCheck();

        if (MoveLineFlag)//線が動いていたら
        {
            //PutMobiusRayCol();//当たり判定実行
            //for (int i = 0; i < 5; i++)
            //{
            //    if (!PutMobiusCol())
            //    {
            //        Debug.Log("colcount" + i);
            //        break;
            //    }
            //    else
            //    {
            //        Debug.Log("ぶつかった" + i);
            //        colcount++;
            //    }
            //}
        }

        //if (!MoveLineFlag)
        //{
        //    OldPos = this.transform.position;
        //}
        OldPos = this.transform.position;        
    }

    private void MoveLineSetting()
    {
        if (!MoveLineFlag)
        {

            int NotMoveLinecount = 0;//MoveLineじゃない線を数える用
            bool SameFlag = false;//登録したMoveLineと同じかどうか

            if (Mm.GetLine().Count != 0)
            {
                for (int i = 0; i < Mm.GetLine().Count; i++)
                {
                    if (Mm.Getcl()[i].MoveLineFlag)//MoveLineでなら
                    {
                        if (!Mm.Getcl()[i].MoveFlag && Mm.MoveLineObj == null)//線が動いていない　かつ　まだ動く線を登録してなければ
                        {
                            //MoveLineに登録
                            Mm.GetLine()[i].GetComponent<MoveLine>().PutMobiusOnOff(true, this.gameObject);
                            Debug.Log(Mm.MoveLineObj.name + "に乗った");
                        }
                    }
                    else//MoveLineでなければ
                    {
                        NotMoveLinecount++;
                    }

                    if (Mm.MoveLineObj == Mm.GetLine()[i])//登録したMoveLineと同じ奴なら
                    {
                        SameFlag = true;
                        //Debug.Log(Mm.GetLine()[i].name + "と同じ");
                    }
                }
            }


            if (((Mm.GetLine().Count == 0) || (NotMoveLinecount == Mm.GetLine().Count) || !SameFlag) && Mm.MoveLineObj != null)
            {
                //MoveLineに登録したやつを削除
                Debug.Log(Mm.MoveLineObj.name + "から離れた");
                Mm.MoveLineObj.GetComponent<MoveLine>().PutMobiusOnOff(false, this.gameObject);
            }

        }
    }

    //動く線に乗っているかどうか確認する
    private void LinePutCheck()
    {
        if (Mm.MoveLineObj != null)
        {
            MoveLinePutFlag = true;
        }
        else
        {
            MoveLinePutFlag = false;
        }
    }

    private void MoveLineTrueStop()
    {
        if (MoveLinePutFlag)//動く線に乗っているなら
        {
            if (MoveLineFlag)//線が動いていたら
            {
                if (Mm.GetFlickMoveFlag())
                {
                    this.transform.position = Mm.OldPos;
                    Mm.ZeroVelo();
                }
            }
        }
    }

    //線が進む方向をセットする用
    public void SetMoveLineVec(Vector2 vec)
    {
        MoveLineVec = vec;
    }

    public void SetMoveLineFlag(bool flag)
    {
        MoveLineFlag = flag;
    }

    //引数の位置から離れた方のベクトルを返す
    private bool LeaveVector(Vector3 _Pos, out Vector2 outvec)
    {
        Vector2 disvec = Mm.SearchVector(this.transform.position, _Pos);//相手へのベクトルを取得

        ColVec= disvec;
        //Debug.Log("相手へのベクトルは" +disvec);
        // Vector2 disvec =-_vec;//相手へのベクトルを取得

        //disvec = -disvec;//相手へのベクトルを反対方向にする

        List<Vector2> MoveVec = new List<Vector2>();//移動できる候補となるベクトル
        for (int i = 0; i < Mm.Getcl().Count; i++)
        {
            if (MoveLinePutFlag != Mm.Getcl()[i].MoveLineFlag)//乗っている線と違う種類の線なら
            {
                continue;
            }

            if (Mm.Getcl()[i].MoveFlag)
            {
                Mm.Getcl()[i].ALLUpdate();
            }

            float Gosa = 0.9f;//移動できるベクトルを取得する際、選定する用

            if (!Mm.Getcl()[i].NearEndRPosFlag(this.transform.position, Mm.GetThisR() * 1.1f))//メビウスが右端に居なければ
            {
                float distance = (Mm.Getcl()[i].GetRvec() - disvec).magnitude;
                if (distance >= Gosa)
                {
                    MoveVec.Add(Mm.Getcl()[i].GetRvec());/* Debug.Log("Rvecげっと！");*/
                }
                else
                {
                    Debug.Log(Mm.GetLine()[i].name + "のRvecは" + Mm.Getcl()[i].GetRvec() + "distanceは" + distance);
                }
            }
            else
            {
                float dis = (new Vector2(this.transform.position.x, this.transform.position.y) - Mm.Getcl()[i].GetRPos()).magnitude;
                Debug.Log(Mm.GetLine()[i].name + "のRposは" + Mm.Getcl()[i].GetRPos() + "distanceは" + dis);
            }

            if (!Mm.Getcl()[i].NearEndLPosFlag(this.transform.position, Mm.GetThisR() * 1.1f))//メビウスが左端に居なければ
            {
                float distance = (Mm.Getcl()[i].GetLvec() - disvec).magnitude;
                if (distance >= Gosa)
                {
                    MoveVec.Add(Mm.Getcl()[i].GetLvec());/* Debug.Log("Lvecげっと！");*/
                }
                else
                {
                    Debug.Log(Mm.GetLine()[i].name + "のLvecは" + Mm.Getcl()[i].GetLvec() + "distanceは" + distance);
                }
            }
            else
            {
                float dis = (new Vector2(this.transform.position.x, this.transform.position.y) - Mm.Getcl()[i].GetLPos()).magnitude;
                Debug.Log(Mm.GetLine()[i].name + "のLposは" + Mm.Getcl()[i].GetLPos() + "distanceは" + dis);
            }
            //line.Add(Mm.GetLine()[i]);
        }

        vec = MoveVec;
        Line = Mm.GetLine();

        Vector2 LeaveVec = Vector2.zero;
        disvec = -disvec;//相手へのベクトルを反対方向にする
        if (MoveVec.Count != 0)
        {
            LeaveVec = NearVector(MoveVec, disvec);//取得したリストの中から相手の反対方向に近いベクトルを取得
            hanareruVec = LeaveVec;

            if (LeaveVecRaySerch(LeaveVec))//一番移動させたい方にメビウスなどのオブジェクトが無ければ
            {
                outvec = LeaveVec; Debug.Log("一番移動させたいLeaveVecは" + LeaveVec);
                return true;
            }
            else//それ以外の方向へ移動させる
            {
                for (int i = 0; i < MoveVec.Count; i++)
                {
                    if (LeaveVecRaySerch(MoveVec[i]))//移動させたい方にメビウスなどのオブジェクトが無ければ
                    {
                        outvec = MoveVec[i]; Debug.Log("次に移動させたいLeaveVecは" + LeaveVec);
                        return true;

                    }
                }
            }
        }

        outvec = Vector2.zero;
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

            //if (distance[i] <= 0.1f)//差がない（同じ座標）場合
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

    //離れさせる際にその方向にオブジェクトが無いかをレイで調べる（あればfalse,無ければtrue）
    private bool LeaveVecRaySerch(Vector2 _vec)
    {
        float distance = Mm.GetThisR();//適切な長さを取得用(メビウスの半径内にないかどうか調べる用)
        Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                        new Vector3(_vec.x * 1, _vec.y * 1, 0));
        //貫通レイキャスト
        foreach (RaycastHit hit in Physics.RaycastAll(ray, distance + 4))
        {
            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

            switch (hit.collider.gameObject.tag)
            {
                case "Mobius":
                    Debug.Log("ずらすところにMobiusがおる");
                    return false;

                case "Block":
                    return false;
            }
        }

        return true;
    }

    //メビウスが線に乗っているときの当たり判定
    private bool PutMobiusRayCol()
    {
        Vector3 Pos = this.transform.position;//レイを飛ばしすぎないようにするもの
        Vector2 OldVec = Mm.SearchVector(OldPos, Pos);//前のフレームの座標からのベクトル

        //Vector3 addpos = Pos - OldPos;
        //OldPos -= addpos; 

        Ray ray;
        //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 5;// プレイヤーのメビウスの輪の円の半径を取得
        float distance = (Pos - OldPos).magnitude;     //レイを飛ばす長さ

        List<GameObject> ColObj = new List<GameObject>();               //すり抜けたメビウスオブジェクトを格納するリスト
        List<Vector3> HitPos = new List<Vector3>();                     //ヒットした座標

        ray = new Ray(new Vector3(OldPos.x, OldPos.y, OldPos.z),    //Rayを飛ばす発射位置
         new Vector3(OldVec.x, OldVec.y, 0));                             //飛ばす方向

        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1000, false);

        //貫通のレイキャスト
        foreach (RaycastHit hit in Physics.SphereCastAll(ray, Mm.GetThisR(), distance * 1.0f))
        {
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
        }//foreach

        if (ColObj.Count != 0)//リストの中に要素があれば
        {
            GameObject otherObj = Mm.NearObjSearch(ColObj, HitPos, OldPos);//リストの中から始点に近いオブジェクトを取得
            //Debug.Log(otherObj.name + "とぶつかった~～");

            if (/*Mm.MoveLineObj != */!otherObj.GetComponent<LinePutMobius>().MoveLinePutFlag)//ぶつかった相手が動く線に乗っていない
            {

                //otherObj.transform.position = HitPos[Mm.ListNumberSearch(ColObj, otherObj)];//メビウスの座標を例が当たった座標にする（計算をしやすくするため）
                //otherObj.GetComponent<MoveMobius>().Collision(this.gameObject);

                Collision(otherObj);
            }


            return true;
        }
        else
        {
            Debug.Log("すり抜けてない");
            return false;
        }
    }

    //当たった時の処理（引数は相手のメビウス）
    private void Collision(GameObject ColObj)
    {
        float ColR = ColObj.GetComponent<MoveMobius>().GetThisR();

        Vector2 disvec;
        //Vector2 OldVec = Mm.SearchVector(OldPos, this.transform.position);
        if (ColObj.GetComponent<LinePutMobius>().LeaveVector(OldPos, out disvec))
        {
            //ColObj.GetComponent<MoveMobius>().MobiusCol(Mm.GetThisR() /*+ ColR*/ + 4, -disvec);

            float PosDis = (OldPos - ColObj.transform.position).magnitude;//位置の差を取得
            float dis = (Mm.GetThisR() + ColR) - PosDis;//半径の合計と位置の差との差を取得

            //ColObj.GetComponent<MoveMobius>().MobiusCol(dis + PosDis+4, -disvec);
            ColObj.GetComponent<MoveMobius>().MobiusCol(dis + PosDis/6, -disvec);

            ColObj.GetComponent<MoveMobius>().ZeroVelo();
            Debug.Log("移動床によってぶつかった！！");
        }
        else
        {
            Debug.Log("移動床によってぶつかっても移動しなかった…！！");
        }
        //this.transform.position = other.gameObject.transform.position;
        //Mm.MobiusCol(ThisR + ColR, -MoveLineVec);
        //Debug.Log("移動床によってぶつかった！！");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mobius"))
        {
            //if (!MoveLineFlag)
            //{
            LinePutMobius OtherLpm= other.GetComponent<LinePutMobius>();

            if ((MoveLinePutFlag && !OtherLpm.MoveLinePutFlag)//相手が動く線に乗ってなく自分が乗っていたら
                || ((MoveLinePutFlag&& OtherLpm.MoveLinePutFlag)&& (!MoveLineFlag&& !OtherLpm.MoveLineFlag)))//お互いに動く線に乗って動いてなかったら
            {
                Collision(other.gameObject);
                LinePutMobiusColFlag = true;
                OtherLpm.LinePutMobiusColFlag = true;
            }
            else if (MoveLinePutFlag == OtherLpm.MoveLinePutFlag &&
                (LinePutMobiusColFlag && OtherLpm.LinePutMobiusColFlag))
            {
                Collision(other.gameObject);
                LinePutMobiusColFlag = false;
                OtherLpm.LinePutMobiusColFlag = false;
            }
            //}
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Line"))
    //    {
    //        if (other.GetComponent<CrossLine>().MoveLineFlag)
    //        {
    //            //if (Mm.MoveLineObj == other.gameObject)
    //            //{
    //            //    MoveLine Ml = other.GetComponent<MoveLine>();
    //            //    Ml.PutOnMobius.Remove(this.gameObject);
    //            //    Ml.Mm.Remove(Mm);
    //            //    Ml.Lpm.Remove(this);

    //            //    Mm.MoveLineObj = null;
    //            //    Debug.Log("移動床と離れた！！" + this.name);
    //            //}

    //        }
    //    }
    //}


    public bool GetMoveLineFlag()
    {
        return MoveLineFlag;
    }
    public bool GetMoveLinePutFlag()
    {
        return MoveLinePutFlag;
    }

}
