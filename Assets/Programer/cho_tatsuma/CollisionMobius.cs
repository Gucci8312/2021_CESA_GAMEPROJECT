// @file   CollisionMobius.cs
// @brief  メビウスの輪同士が当たった時にMobiusAttachPosの関数を呼び出すクラス定義
// @author T,Cho
// @date   2021/04/20 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   CollisionMobius
// @brief  メビウスの輪同士が当たった時にMobiusAttachPosの関数を呼び出すクラス
public class CollisionMobius : MonoBehaviour
{
    GameObject mobius;
    MobiusAttachPos m_mobiusPosScript;
    // Start is called before the first frame update
    void Start()
    {
        mobius = GameObject.Find("mebiusu");
        m_mobiusPosScript = mobius.GetComponent<MobiusAttachPos>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Mobius")
        {
            m_mobiusPosScript.MobiusCollisionOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mobius")
        {
            m_mobiusPosScript.MobiusCollisionOff();
        }
    }
}
