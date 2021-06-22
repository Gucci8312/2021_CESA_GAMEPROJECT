using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メビウスがスプレーに対しての当たり判定を取るスクリプト
public class SupureCollision : MonoBehaviour
{
    Vector3 OldPos;
    MoveMobius Mm;
    
    bool MoveFlag;
    // Start is called before the first frame update
    void Start()
    {
        Mm = this.GetComponent<MoveMobius>();
    }

    // Update is called once per frame
    void Update()
    {
        RayCollision();
        MoveFlag = Mm.GetFlickMoveFlag();
        if (!Mm.GetFlickMoveFlag()) OldPos = this.transform.position;
    }

    private void RayCollision()
    {
        if (MoveFlag&&Mm.GetPlayerMoveFlg())
        {
            float distance = (this.transform.position - OldPos).magnitude;
            Vector2 vec = Mm.SearchVector(OldPos, this.transform.position);
            Ray ray = new Ray(new Vector3(OldPos.x, OldPos.y, OldPos.z),
                       new Vector3(vec.x * 1, vec.y * 1, 0));

            //Debug.DrawRay(ray.origin, ray.direction*distance, Color.blue, distance,false);
            //貫通レイキャスト
            foreach (RaycastHit hit in Physics.SphereCastAll(ray,Mm.GetThisR(),distance))
            {
                // Debug.Log(hit.collider.gameObject.name);//レイキャストが当たったオブジェクト

                switch (hit.collider.gameObject.tag)
                {
                    case "Supure":
                        hit.collider.gameObject.GetComponent<Score>().Collision();
                        break;
                }
            }
        }
    }
}
