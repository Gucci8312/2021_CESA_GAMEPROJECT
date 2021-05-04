using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MoveMobiusから呼び出してもらうスクリプト
public class LinePutMobius : MonoBehaviour
{
    bool MoveLineFlag = false;
    Vector2 MoveLineVec;

    MoveMobius Mm;
    // Start is called before the first frame update
    void Start()
    {
        Mm = this.GetComponent<MoveMobius>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLineTrueStop();
    }

    private void MoveLineTrueStop()
    {
        if (Mm.MoveLineObj != null)
        {
            if (Mm.MoveLineObj.GetComponent<MoveLine>().GetMoveFlag())
            {
                this.transform.position = Mm.StartMovePos;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mobius"))
        {
            if (MoveLineFlag && !other.GetComponent<LinePutMobius>().MoveLineFlag)
            {
                float ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;// プレイヤーのメビウスの輪の円の半径を取得
                float ColR = (other.GetComponent<SphereCollider>().bounds.size.x + other.GetComponent<SphereCollider>().bounds.size.y) / 4;

                other.gameObject.transform.position = this.transform.position;
                other.GetComponent<MoveMobius>().MobiusCol(ThisR + ColR, -MoveLineVec);
                Debug.Log("移動床によってぶつかった！！");
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Mobius"))
    //    {
    //    }
    //}


}
