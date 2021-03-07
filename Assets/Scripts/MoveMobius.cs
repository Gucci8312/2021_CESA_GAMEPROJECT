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

    public Vector2 StickInput;          //スティック入力時の値を取得用(-1～1)
    public Vector2 FlickVec;            //弾いた時のベクトル格納用
    bool FlickMoveFlag=false;           //弾き移動をさせるかどうか
    float Speed;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが乗っているとき
        if (MoveFlg)
        {
            StickFlick();

            if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + MovePower, 0.0f);
                // Debug.Log(MoveFlg);
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - MovePower, 0.0f);
                // Debug.Log(MoveFlg);
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x - MovePower, transform.position.y, 0f);
                // Debug.Log(MoveFlg);
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x + MovePower, transform.position.y, 0f);
                // Debug.Log(MoveFlg);
            }

            //StickInput.x = Input.GetAxis("Horizontal");
            //StickInput.y = Input.GetAxis("Vertical");
            //if (Horizontal != 0 || Vertical != 0)//スティック入力されていたら
            //{
            //    this.gameObject.transform.position = new Vector3(
            //        transform.position.x + (Horizontal*MovePower), transform.position.y + (Vertical*MovePower), 0f); //スティックを倒した分だけ移動
            //}
        }
        else
        {
            FlickVec.x = 0;
            FlickVec.y = 0;
            FlickMoveFlag = false;//弾き移動を止める
        }


        MoveFlg = false;

        int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得

        if (this.name == "Mobius (" + num + ")")//自分が対象のメビウスの輪なら
        {
            //Debug.Log("Mobius (" + num + "):対象のメビウスの輪");
            MoveFlg = true;
        }
        //else
        //{
        //    MoveFlg = false;
        //}

    }

    //スティックの弾き移動処理
    private void StickFlick() 
    {
        StickInput.x = Input.GetAxis("Horizontal");
        StickInput.y = Input.GetAxis("Vertical");

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
                    Speed = 2.0f;
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

    //// 衝突時
    //private void OnCollisionEnter(Collision other)
    //{
    //    // プレイヤーに当たった時
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = true;
    //    }

    //    // メビウスの輪同士がぶつかったとき
    //    if (other.gameObject.tag == "Mobius")
    //    {
    //        MoveFlg = false;
    //        Debug.Log("メビウスの輪同士がぶつかった");
    //    }
    //}

    //// 離れた時
    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        MoveFlg = true;
    //    }
    //}
}
