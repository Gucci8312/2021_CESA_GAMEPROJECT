// @file   TitleMoveMobius.cs
// @brief  タイトルでメビウスの輪がくっついたり離れたりする挙動クラス定義
// @author T,Cho
// @date   2021/04/29 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   TitleMoveMobius
// @brief  タイトルでメビウスの輪がくっついたり離れたりする挙動クラス
public class TitleMoveMobius : MonoBehaviour
{
    [SerializeField] GameObject　m_targetGameObj;                 //どこまで移動するかの目的地
   Vector3 m_InitPos;                                      //初期位置格納
    public float move_speed;                                      //移動するまでの速さ
   public bool move_flg_to;                                             //移動完了フラグ
   public bool move_flg_back;                                           //戻り完了
    // Start is called before the first frame update
   public void Start()
    {
        move_flg_to = false;
        move_flg_back = false;
    }

    private void OnEnable()
    {
        m_InitPos = this.gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(!move_flg_to)
        this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, m_targetGameObj.transform.position, move_speed);

        if(this.gameObject.transform.position == m_targetGameObj.transform.position)
        {
            move_flg_to = true;
        }

        if (move_flg_to)
        {
            Invoke("BackPos", 0.1f);
        }

        if (this.gameObject.transform.position == m_InitPos && move_flg_to)
        {
            move_flg_back = true;
        }
    }

    IEnumerator HorizonMobius()
    {
        yield return null;
    }

    IEnumerator VerticalMobius()
    {
        yield return null;
    }

    void BackPos()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, m_InitPos, move_speed);
    }
}
