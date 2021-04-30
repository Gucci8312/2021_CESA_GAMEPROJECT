using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockType
    {
        NormalBlock = 0,
        MutekiBlock,
    }
    public BlockType Type;
    public int BlockHP = 3;

    GameObject ColObj;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //メビウスがぶつかった時の処理
    public void Collision(GameObject otherobj)
    {
        ColObj = otherobj;

        switch (Type)
        {
            case BlockType.NormalBlock:
                BlockHP--;
                if (BlockHP <= 0)//耐久値が0になったら
                {
                    BlockBraek();
                }
                break;

            case BlockType.MutekiBlock:
                break;
        }
    }

    //ブロックが壊れた時の処理
    private void BlockBraek()
    {
        switch (ColObj.tag)
        {
            case "Mobius":
                ColObj.GetComponent<MoveMobius>().GimicObjNull();//削除する際、オブジェクトの参照が出来なくなるのでこちらで処理する
                break;
        }


        Destroy(this.gameObject);//削除
    }


}

