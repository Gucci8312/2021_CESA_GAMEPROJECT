// @file   MobiusAttachPos
// @brief  メビウスの輪が合体した時のモデルの座標・回転を設定するクラス
// @author T,Cho
// @date   2021/04/06 作成

//方向性
//プレイヤーが現在どのメビウスの輪に乗っているのかを随時確認し更新。
//単体の輪が当たったことを確認したあと１で求めた輪のスクリプトから二つの輪の中点を抽出
//メビウスの輪モデルの座標のを設定し、Meshをtrueに変更して出現させる。
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;       //Enumを使えるようにする

public class MobiusAttachPos : MonoBehaviour
{
    GameObject m_player;            //プレイヤーゲームオブジェクトを取得
    PlayerMove m_playerMoveScript;　//プレイヤーの移動スクリプトを取得（NowMobiusを見つけるため）
    private int m_nowMobiusNo;      //プレイヤーが現在どのメビウスに乗っているかの情報

    GameObject[] m_mobius;                  //すべてのメビウスの輪（単体の輪）を格納
    GameObject m_pCylinder;              //MeshRendererが設定されているメビウスの輪のモデルオブジェクトを設定
    Vector3 m_mobiusPosition;           //合体後のメビウスモデルの座標変数

    KeyCode m_keyCode;                  //どのキーを入力したかを保存
    Vector2 StickInput;                   //スティック入力時の値を取得用(-1～1)
    GameObject otherMobius;              //キャラのいる位置とは違った当たった邦のメビウスの格納変数
    bool m_mobiusCol;                   //メビウスがくっついたかどうかを一回だけ判定。
    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーからNowMobiusNumを取得するためコンポーネントを取得
        m_player = GameObject.Find("Player");
        m_playerMoveScript = m_player.GetComponent<PlayerMove>();

        //メビウスの輪（単体）から当たった情報と中点を取得するためコンポーネントを取得
        m_mobius = GameObject.FindGameObjectsWithTag("Mobius");

        //すべてのメビウスの輪（単体）を取得
        for (int i = 0; i < m_mobius.Length; i++)
        {
            m_mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        
        }

        m_pCylinder = GameObject.Find("pCylinder2");
        //メビウスの輪（二つつなぎ）のモデルをいったん隠す
        m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled = false;

        //一度くっついたとき回転しない用にするため。
        m_mobiusCol = false;
    }

    // Update is called once per frame
    void Update()
    {
        //現在の回転角度を格納するためにいったんトランスフォーム情報ごと取得
        Transform myTransform = transform;
        //プレイヤーが乗るメビウスの輪（単体）を探索
        m_nowMobiusNo = m_playerMoveScript.GetNowMobiusNum();
       
        //前回の入力キーを取得
        BeforeDownGetKey();

        //メビウスの輪（単体）同士が当たったかどうかを取得
        if (m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetMobiusStripFlag() && !m_mobiusCol)
        {
            otherMobius = m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetColMobiusObj();
            MobiusChileMeshRenderOff(m_mobius[m_nowMobiusNo]);
            MobiusChileMeshRenderOff(otherMobius);
            //当たった二つの中間点を取得→メビウスの輪の座標に設定するため
            Vector3 pos = m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetColPos();
            //メビウスの輪のモデルを表示
            m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled = true;
           
            if (m_keyCode.ToString() == "S" || m_keyCode.ToString() == "W" || StickInput.y != 0)
            {
                this.gameObject.GetComponent<Transform>().position = new Vector3(pos.x + 15, pos.y, pos.z);
                if (myTransform.eulerAngles.z == 0)
                    transform.Rotate(new Vector3(0, 0, 90));
            }
            else if(m_keyCode.ToString() == "A" || m_keyCode.ToString() == "D" || StickInput.x != 0)
            {
                this.gameObject.GetComponent<Transform>().position = new Vector3(pos.x, pos.y - 15, pos.z);
                if (myTransform.eulerAngles.z == 90)
                    transform.Rotate(new Vector3(0, 0, -90));
            }
            m_mobiusCol = true;
        }
        else if (m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled && !m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetMobiusStripFlag())
        {
            //メビウスの輪（二つつなぎ）のモデルをいったん隠す
            m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled = false;           
            for (int i = 0; i < m_mobius.Length; i++)
            {
                MobiusChileMeshRenderOn(m_mobius[i]);
            }
            m_mobiusCol = false;
        }
    }

    // @name   BeforeDownGetKey
    // @brief  前回どのキーを押したかを記憶
    void BeforeDownGetKey()
    {
        StickInput.x = Input.GetAxis("Horizontal");
        StickInput.y = Input.GetAxis("Vertical");
        // 何かのキーが押下されたら
        if (Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    m_keyCode = code;
                }
            }
        }
    }

    void MobiusChileMeshRenderOff(GameObject _gameObj)
    {
        //メビウスの輪（単体）のモデルのメッシュレンダーを取得（trueになっているもののみ）→子オブジェクト探索
        Component[] meshComponent = _gameObj.GetComponentsInChildren(typeof(MeshRenderer), true);
        //foreach (MeshRenderer mesh in meshComponent)
        //{
        //    //メビウスの輪（単体）のモデルのメッシュレンダーを非表示に設定
        //    mesh.enabled = false;
        //}
        meshComponent[0].gameObject.GetComponent<MeshRenderer>().enabled = false;
        meshComponent[1].gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    void MobiusChileMeshRenderOn(GameObject _gameObj)
    {
        //メビウスの輪（単体）のモデルのメッシュレンダーを取得（trueになっているもののみ）→子オブジェクト探索
        Component[] meshComponent = _gameObj.GetComponentsInChildren(typeof(MeshRenderer), false);
        foreach (MeshRenderer mesh in meshComponent)
        {
            //メビウスの輪（単体）のモデルのメッシュレンダーを非表示に設定
            mesh.enabled = true;
        }

    }
}
