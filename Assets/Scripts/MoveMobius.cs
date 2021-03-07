using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovePower;                 // 移動力
    bool MoveFlg;                           // 移動判定用
    bool m_colOtherMobius;
    GameObject player;

    Ray m_backRay;                           //後方にオブジェクトがあるかどうか
    Ray m_fowardRay;                         //前方にオブジェクトがあるかどうか
    Ray m_upRay;                         //前方にオブジェクトがあるかどうか
    Ray m_downRay;                         //前方にオブジェクトがあるかどうか
    Vector3 m_saveMobiusPos;
    RaycastHit m_hitup;
    RaycastHit m_hitdo;
    RaycastHit m_hitr;
    RaycastHit m_hitl;

    [SerializeField]
    private LayerMask m_layerMaskMobius = default;      //オブジェクトを特定のものに絞る

=======
    [SerializeField]
    private LayerMask m_layerMaskMobius = default;      //オブジェクトを特定のものに絞る

    Rigidbody rb;

    //test move;
    private float moveX;
    private float moveY;
    private float moveZ;
    public float speed = 3.0f;
    private Vector3 beforeVelocity;
>>>>>>> origin/長龍馬
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveX = Random.Range(-10.0f, 10.0f) * speed;
        moveY = Random.Range(-10.0f, 10.0f) * speed;
        moveZ = Random.Range(3.0f, 10.0f) * speed;
        rb.velocity = new Vector3(moveX, moveY, moveZ);//初期ベクトル
        player = GameObject.Find("Player");
        m_saveMobiusPos = this.gameObject.transform.position;
<<<<<<< HEAD
=======

>>>>>>> origin/長龍馬
    }

    // Update is called once per frame
    void Update()
    {
        m_backRay = new Ray(transform.position, new Vector3(-1, 0, 0));            //後ろに衝突するオブジェクトがあるかどうかの判断
        m_fowardRay = new Ray(transform.position, new Vector3(1, 0, 0));            //前に衝突するオブジェクトがあるかどうかの判断
        m_upRay = new Ray(transform.position, new Vector3(0, 1, 0));            //↑に衝突するオブジェクトがあるかどうかの判断
        m_downRay = new Ray(transform.position, new Vector3(0, -1, 0));            //↓に衝突するオブジェクトがあるかどうかの判断
        Debug.DrawRay(m_fowardRay.origin, m_fowardRay.direction * 38, Color.red, 1, false);

<<<<<<< HEAD
        // プレイヤーが乗っているとき
        if (MoveFlg)
        {
            if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + MovePower, 0.0f);
                //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
                if (Physics.Raycast(m_upRay, out m_hitup, 38.0f, m_layerMaskMobius))
                {
                    if (m_hitup.collider == null) return;//衝突しなければ例外処理
                    this.gameObject.transform.position = new Vector3(m_saveMobiusPos.x, m_saveMobiusPos.y, m_saveMobiusPos.z);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - MovePower, 0.0f);
                //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
                if (Physics.Raycast(m_downRay, out m_hitdo, 38.0f, m_layerMaskMobius))
                {
                    if (m_hitdo.collider == null) return;//衝突しなければ例外処理
                    this.gameObject.transform.position = new Vector3(m_saveMobiusPos.x, m_saveMobiusPos.y, m_saveMobiusPos.z);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x - MovePower, transform.position.y, 0f);
                //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
                if (Physics.Raycast(m_backRay, out m_hitl, 38.0f, m_layerMaskMobius))
                {
                    if (m_hitl.collider == null) return;//衝突しなければ例外処理
                    this.gameObject.transform.position = new Vector3(m_saveMobiusPos.x, m_saveMobiusPos.y, m_saveMobiusPos.z);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x + MovePower, transform.position.y, 0f);
                //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
                if (Physics.Raycast(m_fowardRay, out m_hitr, 38.0f, m_layerMaskMobius))
                {
                    if (m_hitr.collider == null) return;//衝突しなければ例外処理
                    this.gameObject.transform.position = new Vector3(m_saveMobiusPos.x, m_saveMobiusPos.y, m_saveMobiusPos.z);
                }
            }
            // Debug.Log(MoveFlg);
        }
        //MoveFlg = false;

        int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得

=======
        //// プレイヤーが乗っているとき
        //if (MoveFlg)
        //{
        //    if (Input.GetKey(KeyCode.W))
        //    {
        //        this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + MovePower, 0.0f);
        //        //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
        //        RayCastStop(m_upRay, m_hitup);
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - MovePower, 0.0f);
        //        //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
        //        RayCastStop(m_downRay, m_hitdo);
        //    }
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        this.gameObject.transform.position = new Vector3(transform.position.x - MovePower, transform.position.y, 0f);
        //        //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
        //        RayCastStop(m_backRay, m_hitl);
        //    }
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        this.gameObject.transform.position = new Vector3(transform.position.x + MovePower, transform.position.y, 0f);
        //        //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
        //        RayCastStop(m_fowardRay, m_hitr);
        //    }
        //    // Debug.Log(MoveFlg);
        //}
        //MoveFlg = false;

        //   int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得
        beforeVelocity = rb.velocity;
>>>>>>> origin/長龍馬
        //if (this.name == "Mobius (" + num + ")")//自分が対象のメビウスの輪なら
        //{
        //    //Debug.Log("Mobius (" + num + "):対象のメビウスの輪");
        //    MoveFlg = true;
        //}
        //else
        //{
        //    MoveFlg = false;
        //}

    }

    // 衝突時
    private void OnCollisionEnter(Collision other)
    {
        // プレイヤーに当たった時
        if (other.gameObject.tag == "Player")
        {
            // other.gameObject.transform.parent = this.gameObject.transform;
            MoveFlg = true;
        }

        // メビウスの輪同士がぶつかったとき
        if (other.gameObject.tag == "Mobius")
        {
            MoveFlg = false;
            m_saveMobiusPos = this.gameObject.transform.position;
            Debug.Log("MobiusPosition : " + m_saveMobiusPos);
            Debug.Log("メビウスの輪同士がぶつかった");
        }
<<<<<<< HEAD
    }

=======

        if (other.gameObject.tag == "Wall")
        {
            //Vector3 refrectVec = Vector3.Reflect(this.beforeVelocity, other.contacts[0].normal);//反射ベクトル計算
            var wallNormal = other.contacts[0].normal;
            var weight = Vector3.Dot(-this.beforeVelocity, wallNormal);

            var refrectVec = this.beforeVelocity + 2 * weight * wallNormal;
            this.rb.velocity = refrectVec;
        }
    }

>>>>>>> origin/長龍馬
    // 離れた時
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            MoveFlg = false;
            // other.gameObject.transform.parent = null;
            Debug.Log("プレイヤーから離れた");
        }
    }
<<<<<<< HEAD
=======

    private void RayCastStop(Ray _ray, RaycastHit _hit)
    {
        //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
        if (Physics.Raycast(_ray, out _hit, 38.0f, m_layerMaskMobius))
        {
            if (_hit.collider == null) return;//衝突しなければ例外処理
            this.gameObject.transform.position = new Vector3(m_saveMobiusPos.x, m_saveMobiusPos.y, m_saveMobiusPos.z);
        }
    }
>>>>>>> origin/長龍馬
}
