using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MoveMobiusから呼び出してもらうスクリプト
//動く線に乗っているときのメビウスの処理
public class LinePutMobius : MonoBehaviour
{
    public bool MoveLineFlag = false;//線が動いているかどうか
    public bool MoveLinePutFlag=false;//線に乗っているかどうか
    public Vector2 MoveLineVec;

    MoveMobius Mm;

    public List<Vector2> vec;
    public Vector2 ColVec;
    Vector3 OldPos;

    public List<GameObject> Line;

    bool Old2Flag;
    // Start is called before the first frame update
    void Start()
    {
        Mm = this.GetComponent<MoveMobius>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLineTrueStop();
        LinePutCheck();

        if (MoveLineFlag)//線が動いていたら
        {
            PutMobiusCol();
        }

        if (!MoveLineFlag)
        {
            Old2Flag = true;
        }
        else
        {
            
            if (Old2Flag)
            {
                OldPos = this.transform.position;
            }
            Old2Flag = !Old2Flag;
        }
    }

    private void LinePutCheck()
    {
        if (Mm.Getcl().Count != 0)
        {
            for(int i=0;i< Mm.Getcl().Count;i++)
            {
                if (Mm.Getcl()[i].MoveLineFlag)
                {
                    if (!Mm.Getcl()[i].MoveFlag)
                    {
                        MoveLinePutFlag = true;
                        break;
                    }                    
                }
                else
                {
                    MoveLinePutFlag = false;
                }
                
            }
        }
    }

    private void MoveLineTrueStop()
    {
        if (Mm.MoveLineObj != null)//動く線に乗っているなら
        {
            if (Mm.MoveLineObj.GetComponent<MoveLine>().GetMoveFlag())//線が動いていたら
            {
               // this.transform.position = Mm.StartMovePos;
                Mm.ZeroVelo();
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
       // Vector2 disvec =-_vec;//相手へのベクトルを取得

        //disvec = -disvec;//相手へのベクトルを反対方向にする

        List<Vector2> MoveVec = new List<Vector2>();//移動できる候補となるベクトル
        for (int i = 0; i < Mm.Getcl().Count; i++)
        {
            //if (MoveLinePutFlag != Mm.Getcl()[i].MoveLineFlag)
            //{
            //    continue;
            //}

            if (Mm.Getcl()[i].MoveFlag)
            {
                Mm.Getcl()[i].ALLUpdate();
            }

            float Gosa = 0.8f;//移動できるベクトルを取得する際、選定する用

            if (!Mm.Getcl()[i].NearEndRPosFlag(this.transform.position,Mm.GetThisR()))//メビウスが右端に居なければ
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

            if (!Mm.Getcl()[i].NearEndLPosFlag(this.transform.position, Mm.GetThisR()))//メビウスが左端に居なければ
            {
                float distance = (Mm.Getcl()[i].GetLvec() - disvec).magnitude;
                if (distance >= Gosa)
                {
                    MoveVec.Add(Mm.Getcl()[i].GetLvec());/* Debug.Log("Lvecげっと！");*/
                }
                else
                {
                    Debug.Log(Mm.GetLine()[i].name + "のLvecは" + Mm.Getcl()[i].GetLvec()+ "distanceは" + distance);
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

        Vector2 LeaveVec=Vector2.zero;
        disvec = -disvec;//相手へのベクトルを反対方向にする
        if (MoveVec.Count != 0)
        {
            LeaveVec = NearVector(MoveVec, disvec);//取得したリストの中から相手の反対方向に近いベクトルを取得
            ColVec = LeaveVec;

            if (LeaveVecRaySerch(LeaveVec))//一番移動させたい方にメビウスなどのオブジェクトが無ければ
            {
                outvec = LeaveVec; Debug.Log("一番移動させたいLeaveVecは" + LeaveVec);
                return true;
            }
            else//それ以外の方向へ移動させる
            {
                for(int i = 0; i < MoveVec.Count; i++)
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

            if (distance[i] <= 0.1f)//差がない（同じ座標）場合
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

    //離れさせる際にその方向にオブジェクトが無いかをレイで調べる（あればfalse,無ければtrue）
    private bool LeaveVecRaySerch(Vector2 _vec)
    {
        float distance = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;//適切な長さを取得用
        Ray ray = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                        new Vector3(_vec.x * 1, _vec.y * 1, 0));
        //貫通レイキャスト
        foreach (RaycastHit hit in Physics.RaycastAll(ray, distance + 4))
        {
            // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

            switch (hit.collider.gameObject.tag)
            {
                case "Mobius":
                    return false;

                case "Block":
                    return false;
            }
        }

        return true;
    }

    //メビウスが線に乗っているときの当たり判定
    private bool PutMobiusCol()
    {
        Vector3 Pos =this.transform.position;//レイを飛ばしすぎないようにするもの
        Vector2 ColVec = Mm.SearchVector(OldPos, Pos);//前のフレームの座標からのベクトル

        Ray ray;
        //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 5;// プレイヤーのメビウスの輪の円の半径を取得
        float distance = (Pos - OldPos).magnitude;     //レイを飛ばす長さ

        List<GameObject> ColObj = new List<GameObject>();               //すり抜けたメビウスオブジェクトを格納するリスト
        List<Vector3> HitPos = new List<Vector3>();                     //ヒットした座標

        ray = new Ray(new Vector3(OldPos.x, OldPos.y, OldPos.z),    //Rayを飛ばす発射位置
         new Vector3(ColVec.x, ColVec.y, 0));                             //飛ばす方向

        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1000, false);

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
                HitPos.Add(hit.point); Debug.Log("HitPos" + hit.point);
            }
            else if (ColObj.Count != 0 && hitObj != null)
            {
                if (Mm.SameObjListSearch(ColObj, hitObj))//ColObjリストの中に当たったものがなければ
                {
                    ColObj.Add(hitObj);//レイで当たったオブジェクトをリストに格納
                    HitPos.Add(hit.point); Debug.Log("HitPos" + hit.point);
                }
            }
        }//foreach

        if (ColObj.Count != 0)//リストの中に要素があれば
        {
            GameObject otherObj = Mm.NearObjSearch(ColObj, HitPos, OldPos);//リストの中から始点に近いオブジェクトを取得

            if (/*Mm.MoveLineObj != */!otherObj.GetComponent<MoveMobius>().MoveLineObj)//ぶつかった相手が動く線に乗っていない
            {
                //float ColR = otherObj.GetComponent<MoveMobius>().GetThisR();

                //Vector2 disvec;
                //if (otherObj.GetComponent<LinePutMobius>().LeaveVector(OldPos, out disvec))
                //{
                //    otherObj.GetComponent<MoveMobius>().MobiusCol(Mm.GetThisR() + ColR * 1.2f, -disvec);
                //    //if(MoveLinePutFlag && otherObj.GetComponent<LinePutMobius>().MoveLinePutFlag)//お互いに動く線に乗っていたら
                //    //{
                //    //    Mm.MobiusCol(ThisR + ColR * 1.2f, disvec);
                //    //}
                //    Debug.Log("移動床によってぶつかった！！");
                //}
                //else
                //{
                //    Debug.Log("移動床によってぶつかっても移動しなかった…！！");
                //}
                //this.transform.position = other.gameObject.transform.position;
                //Mm.MobiusCol(ThisR + ColR, -MoveLineVec);
                //Debug.Log("移動床によってぶつかった！！");

                Collision(otherObj);
            }

            Debug.Log(otherObj.name + "とぶつかった~～");
            return true;
        }
        else
        {
            Debug.Log("すり抜けてない");
            return false;
        }
    }

    private void Collision(GameObject ColObj)
    {
        float ColR = ColObj.GetComponent<MoveMobius>().GetThisR();

        Vector2 disvec;
        //Vector2 OldVec = Mm.SearchVector(OldPos, this.transform.position);
        if (ColObj.GetComponent<LinePutMobius>().LeaveVector(OldPos, out disvec))
        {
            ColObj.GetComponent<MoveMobius>().MobiusCol(Mm.GetThisR() + ColR * 1.2f, -disvec);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mobius"))
        {
            if (MoveLinePutFlag && other.GetComponent<LinePutMobius>().MoveLinePutFlag)//ぶつかった相手が動く線に乗っていたら
            {
                //float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
                //float ColR = other.GetComponent<MoveMobius>().GetThisR();

                //Vector2 disvec;
                ////Vector2 OldVec = Mm.SearchVector(OldPos, this.transform.position);
                //if (other.GetComponent<LinePutMobius>().LeaveVector(OldPos, out disvec))
                //{
                //    other.GetComponent<MoveMobius>().MobiusCol(Mm.GetThisR() + ColR * 1.2f, -disvec);
                //    Debug.Log("移動床によってぶつかった！！");
                //}
                //else
                //{
                //    Debug.Log("移動床によってぶつかっても移動しなかった…！！");
                //}
                //this.transform.position = other.gameObject.transform.position;
                //Mm.MobiusCol(ThisR + ColR, -MoveLineVec);
                //Debug.Log("移動床によってぶつかった！！");
                Collision(other.gameObject);

            }
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


}
