using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovePower;                 // 移動力
    bool MoveFlg;                           // 移動判定用
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

    public Vector2 StickInput;          //スティック入力時の値を取得用(-1～1)
    public Vector3 FlickVec;            //弾いた時のベクトル格納用
    bool FlickMoveFlag=false;           //弾き移動をさせるかどうか
    float Speed;

    void Start()
    {
        player = GameObject.Find("Player");
        m_saveMobiusPos = this.gameObject.transform.position;

        m_backRay = new Ray(transform.position, new Vector3(-1, 0, 0));            //後ろに衝突するオブジェクトがあるかどうかの判断
        m_fowardRay = new Ray(transform.position, new Vector3(1, 0, 0));            //前に衝突するオブジェクトがあるかどうかの判断
        m_upRay = new Ray(transform.position, new Vector3(0, 1, 0));            //↑に衝突するオブジェクトがあるかどうかの判断
        m_downRay = new Ray(transform.position, new Vector3(0, -1, 0));            //↓に衝突するオブジェクトがあるかどうかの判断
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが乗っているとき
        if (MoveFlg)
        {

            StickFlick();

            //if (Input.GetKey(KeyCode.W))
            //{
            //    this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + MovePower, 0.0f);
            //    //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
            //    RayCastStop(m_upRay, m_hitup);
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - MovePower, 0.0f);
            //    //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
            //    RayCastStop(m_downRay, m_hitdo);
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    this.gameObject.transform.position = new Vector3(transform.position.x - MovePower, transform.position.y, 0f);
            //    //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
            //    RayCastStop(m_backRay, m_hitl);
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    this.gameObject.transform.position = new Vector3(transform.position.x + MovePower, transform.position.y, 0f);
            //    //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
            //    RayCastStop(m_fowardRay, m_hitr);
            //}
            // Debug.Log(MoveFlg);
        }
                else
        {
            FlickVec.x = 0;
            FlickVec.y = 0;
            FlickMoveFlag = false;//弾き移動を止める
        }


        //MoveFlg = false;

        //   int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得
        // beforeVelocity = rb.velocity;
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
    //// 衝突時
    //private void OnCollisionEnter(Collision other)
    //{
    //    // プレイヤーに当たった時
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = true;
    //    }

        // メビウスの輪同士がぶつかったとき
        if (other.gameObject.tag == "Mobius")
        {
            MoveFlg = false;
            m_saveMobiusPos = this.gameObject.transform.position;
            Debug.Log("MobiusPosition : " + m_saveMobiusPos);
            Debug.Log("メビウスの輪同士がぶつかった");
        }

        if (other.gameObject.tag == "Wall")
        {
            var wallNormal = other.contacts[0].normal;
            var weight = Vector3.Dot(-FlickVec, wallNormal);

            var refrectVec = this.FlickVec + 2 * weight * wallNormal;
            FlickVec = refrectVec;
        }
    }

    // 離れた時
    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = false;
    //        // other.gameObject.transform.parent = null;
    //        Debug.Log("プレイヤーから離れた");
    //    }
    //}

    private void RayCastStop(Ray _ray, RaycastHit _hit)
    {
        //レイキャストを飛ばして指定先に衝突オブジェクトがあるかを確認
        if (Physics.Raycast(_ray, out _hit, 38.0f, m_layerMaskMobius))
        {
            if (_hit.collider == null) return;//衝突しなければ例外処理
            this.gameObject.transform.position = new Vector3(m_saveMobiusPos.x, m_saveMobiusPos.y, m_saveMobiusPos.z);
        }
    }

    //スティックの弾き移動処理
    private void StickFlick()
    {
        StickInput.x = Input.GetAxis("Horizontal");
        StickInput.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W)) { StickInput.y = 1; }
        if (Input.GetKey(KeyCode.S)) { StickInput.y = -1; }
        if (Input.GetKey(KeyCode.D)) { StickInput.x = 1; }
        if (Input.GetKey(KeyCode.A)) { StickInput.x = -1; }

        if (!FlickMoveFlag)
        {
            if (StickInput.x != 0 || StickInput.y != 0)//スティック入力されていたら
            {
                Vector2 stickmax = StickInput;//スティックを端まで倒したときの値を格納用

                //計算をしやすくするために正数に変化
                if (stickmax.x < 0) { stickmax.x = -stickmax.x; }
                if (stickmax.y < 0) { stickmax.y = -stickmax.y; }

                if (stickmax.x + stickmax.y >= 1)//スティックを端まで倒した場合
                {
                    FlickVec = -StickInput; ;//倒した方向の逆方向の値を代入
                }
            }
            else//スティックを倒していなければ（離したとき）
            {
                if (FlickVec.x != 0 || FlickVec.y != 0)//端まで倒したときのベクトルを持っていれば
                {
                    Speed = 20.0f;
                    FlickMoveFlag = true;
                }
            }
        }
        else
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x + FlickVec.x * Speed, transform.position.y + FlickVec.y * Speed, 0f); //弾いたほうへ移動

            Speed -= Time.deltaTime;//減速させる

            if (Speed < 0) //止まったら
            {
                FlickVec.x = 0;
                FlickVec.y = 0;
                FlickMoveFlag = false;
            }
        }
    }


}
