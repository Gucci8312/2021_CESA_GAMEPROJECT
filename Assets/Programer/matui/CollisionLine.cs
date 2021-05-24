using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メビウスの線の当たり判定
public class CollisionLine : MonoBehaviour
{

    List<GameObject> Line = new List<GameObject>();                          //線のオブジェクト
    List<CrossLine> cl = new List<CrossLine>();                              //CrossLineスクリプト

    GameObject parentObj;
    MoveMobius Mm;
    // Start is called before the first frame update
    void Start()
    {
        parentObj = this.transform.parent.gameObject;
        Mm = parentObj.GetComponent<MoveMobius>();
    }

    // Update is called once per frame
    void Update()
    {
        Mm.Line = Line;
        Mm.cl = cl;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            if (Line.Count == 0)//Lineリストに要素が無ければ
            {
                Line.Add(other.gameObject);//Lineリストに当たったものを追加
                cl.Add(other.GetComponent<CrossLine>());
            }
            else//Lineリストに要素があれば
            {
                if (Mm.SameObjListSearch(Line, other.gameObject))//Lineリストの中に当たったものがなければ
                {
                    Line.Add(other.gameObject);//Lineリストに当たったものを追加
                    cl.Add(other.GetComponent<CrossLine>());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            Line.Remove(other.gameObject);//登録したLineリストの中に該当する要素を削除する
            cl.Remove(other.GetComponent<CrossLine>());
        }

    }

}
