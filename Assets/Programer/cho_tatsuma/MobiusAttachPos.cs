// @file   MobiusAttachPos
// @brief  メビウスの輪が合体した時のモデルの座標・回転を設定するクラス
// @author T,Cho
// @date   2021/04/06 作成

//方向性
//プレイヤーが現在どのメビウスの輪に乗っているのかを随時確認し更新。
//単体の輪が当たったことを確認したあと１で求めた輪のスクリプトから二つの輪の中点を抽出
//メビウスの輪モデルの座標のを設定し、Meshをtrueに変更して出現させる。
using System;       //Enumを使えるようにする
using UnityEngine;

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

    float before_degree;                //前回の回転角を保持
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
    void FixedUpdate()
    {
        //現在の回転角度を格納するためにいったんトランスフォーム情報ごと取得
        Transform myTransform = transform;
        //プレイヤーが乗るメビウスの輪（単体）を探索
        m_nowMobiusNo = m_playerMoveScript.GetNowMobiusNum();

        //前回の入力キーを取得
        BeforeDownGetKey();

        if (!m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetMobiusStripFlag())
        {
            //松井君に送りたい部分（離れた場合）
         //   MobiusCollisionOff();
        }

        //メビウスの輪（単体）同士が当たったかどうかを取得
        if (m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetMobiusStripFlag() && !m_mobiusCol)
        {
            //松井君に送りたい部分（くっついた場合）
        //    MobiusCollisionOn();
            m_mobiusCol = true;
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
            //すべてのキーから探し当てる
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    m_keyCode = code;
                }
            }
        }
    }

    // @name   MobiusChileMeshRenderOff
    // @brief  メビウスの輪の子供のメッシュレンダーをオフ（透明化処理）
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

    // @name   MobiusChileMeshRenderOn
    // @brief  メビウスの輪の子供のメッシュレンダーをオン（透明化解除処理）
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

    // @name   MobiusRotateDegree
    // @brief  他のメビウスと当たった時の回転角の算出処理
    // @return 回転角
    float MobiusRotateDegree()
    {
        float degree;
        float x, y, radian;

        //三角形の底辺と高さを算出
        x = m_mobius[m_nowMobiusNo].GetComponent<Transform>().position.x - otherMobius.GetComponent<Transform>().position.x;
        y = m_mobius[m_nowMobiusNo].GetComponent<Transform>().position.y - otherMobius.GetComponent<Transform>().position.y;

        //ラジアンを引き出す
        radian = Mathf.Atan2(y, x);

        //最終角度を求める
        degree = radian * (180.0f / 3.141592f);

        if(degree < 0f)
        {
            degree += 360f;
        }
        //90度、0度の補正
        if ((degree >= 80f && degree <= 100f) || (degree >= 260f && degree <= 280f))
        {
            degree = 90f;
        }
        else if ((degree >= 170f && degree <= 190f))
        {
            degree = 0f;
        }

        //45度の補正
        if ((degree >= 35f && degree <= 55f) || (degree >= 215f && degree <= 235f))
        {
            degree = 45f;
        }
        else if ((degree >= 125f && degree <= 145f) || (degree >= 305f && degree <= 325f))
        {
            degree = 135f;
        }
        return degree;
    }

    // @name   MobiusCollisionOn
    // @brief  他のメビウスと当たった時に実装したい部分
    public void MobiusCollisionOn()
    {
        otherMobius = m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetColMobiusObj();
        MobiusChileMeshRenderOff(m_mobius[m_nowMobiusNo]);
        MobiusChileMeshRenderOff(otherMobius);
        before_degree = MobiusRotateDegree();
        //当たった二つの中間点を取得→メビウスの輪の座標に設定するため
        Vector3 pos = m_mobius[m_nowMobiusNo].GetComponent<MoveMobius>().GetColPos();
        //メビウスの輪のモデルを表示
        m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled = true;

        if (before_degree == 90f || before_degree == 270f)
        {
            this.gameObject.GetComponent<Transform>().position = new Vector3(pos.x + 15, pos.y, pos.z);
        }
        else if (before_degree == 180f || before_degree == 0f)
        {
            this.gameObject.GetComponent<Transform>().position = new Vector3(pos.x, pos.y - 15, pos.z);
        }
        else if(before_degree == 40f || before_degree == 135f)
        {
            this.gameObject.GetComponent<Transform>().position = new Vector3(pos.x + 15, pos.y - 15, pos.z);
        }
        transform.Rotate(new Vector3(0, 0, before_degree));
    }
    // @name   MobiusCollisionOff
    // @brief  他のメビウスと離れた時に実装したい部分
    public void MobiusCollisionOff()
    {
        if (m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled)
        {
            //メビウスの輪（二つつなぎ）のモデルをいったん隠す
            m_pCylinder.gameObject.GetComponent<MeshRenderer>().enabled = false;
            for (int i = 0; i < m_mobius.Length; i++)
            {
                MobiusChileMeshRenderOn(m_mobius[i]);
            }
            m_mobiusCol = false;
            transform.Rotate(new Vector3(0, 0, -before_degree));
        }
    }
}

