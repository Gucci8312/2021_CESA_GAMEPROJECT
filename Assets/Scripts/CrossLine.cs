using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLine : MonoBehaviour
{
    public List<Vector2> CrossPos=new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 RotationfromPosition(Vector2 pos,Vector2 scale,float Angle)//回転したときの座標を求める
    {
        Vector2 Center;
        Center.x = pos.x + scale.x / 2;
        Center.y = pos.y + scale.y / 2;
        float theta = Mathf.Atan((scale.y / 2) / (scale.x / 2)) * 180 / 3.14f;
        float sha = Mathf.Sqrt((scale.x / 2) * (scale.x / 2) + (scale.y / 2) * (scale.y / 2));


        float deg1 = theta + Angle;
        //float deg2 = 180 - theta + kakudo;
        //float deg3 = 360 - theta + kakudo;
        //float deg4 = 180 + theta + kakudo;

        Vector2 outpos;
        outpos.x = Center.x + sha * Mathf.Cos(deg1 * 3.14f / 180);
        outpos.y = Center.y + sha * Mathf.Sin(deg1 * 3.14f / 180);


        return outpos;
    }

    private  bool CrossLinePosition(GameObject Line1, GameObject Line2,out Vector2 outvec)//交点を求める
    {
       outvec = Vector2.zero;
        Vector2 p1, p2, p3, p4;

        if (Line1.transform.localScale.x> Line1.transform.localScale.y)//横に長ければ
        {
            p1 = new Vector2(Line1.transform.position.x - Line1.transform.localScale.x / 2, Line1.transform.position.y);
            p2 = new Vector2(Line1.transform.position.x + Line1.transform.localScale.x / 2, Line1.transform.position.y);
        }
        else//縦に長ければ
        {
            p1 = new Vector2(Line1.transform.position.x, Line1.transform.position.y - Line1.transform.localScale.y / 2);
            p2 = new Vector2(Line1.transform.position.x, Line1.transform.position.y + Line1.transform.localScale.y / 2);
        }

        if (Line2.transform.localScale.x > Line2.transform.localScale.y)//横に長ければ
        {
            p3 = new Vector2(Line2.transform.position.x - Line2.transform.localScale.x / 2, Line2.transform.position.y);
            p4 = new Vector2(Line2.transform.position.x + Line2.transform.localScale.x / 2, Line2.transform.position.y);
        }
        else//縦に長ければ
        {
            p3 = new Vector2(Line2.transform.position.x, Line2.transform.position.y - Line2.transform.localScale.y / 2);
            p4 = new Vector2(Line2.transform.position.x, Line2.transform.position.y + Line2.transform.localScale.y / 2);
        }



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

    public Vector2 CanMovePosition(Vector2 pos)//メビウスが移動できる座標を与える
    {
        List<float> distance = new List<float>();//引数の座標と交点との差
        float Min = 10000;//最小値
        for(int i=0;i< CrossPos.Count; i++)
        {
            distance.Add((pos - CrossPos[i]).magnitude);

            if (distance[i] == 0)//差がない（同じ座標）場合
            {
                distance[i]=10000;//適当に大きい値を入れて最小の値として取得させないようにする
            }

            if (distance[i] <= Min)//取得している最小の値より小さければ
            {
                Min = distance[i];//差が最小の値を取得
            }
     }

        for(int i = 0; i < distance.Count; i++)
        {
            if (distance[i] == Min)//差が最小の値を持った要素であれば
            {
                return CrossPos[i];
            }
        }

        return pos;
    }

    public bool SameCrossPos(Vector2 _pos)//交点と同じ座標があるかどうか
    {
        for (int i = 0; i < CrossPos.Count; i++)
        {
            float distance=(_pos - CrossPos[i]).magnitude;//交点と引数との差を求める
            if (distance <= 1) //差がほぼなければ
            {
                return true;
            }
        }
        return false;
    }

    public List<Vector2> GetCrossPos()//交点を取得
    {
        return CrossPos;
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
