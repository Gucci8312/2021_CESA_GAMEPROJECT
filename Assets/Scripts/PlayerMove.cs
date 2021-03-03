using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの挙動
public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Mobius = new GameObject[4];                                                        // メビウスの輪
    public float MovePower;                                                                         // 移動力
    public int NowMobius;                                                                           //現在のメビウスの添え字　
    int SaveMobius;                                                                                 //１つ前にいたメビウスの添え字
    public float InsideLength;                                                                      //内側に入ったときの位置を調整する値
    public bool RotateLeftFlg;                                                                      //回転方向が左右のどちらかを判定　true:左　false:右
    public bool InsideFlg;                                                                          //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    public float MobiusCollisonLength;                                                              //メビウスの輪同士で当たったときの判定用の長さ
    int SideCnt;                                                                                    //メビウスの動きにするためメビウスの輪を何回切り替えたかをカウント  2以上で外側内側入れ替える
    float counter;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納

        for (int i = 0; i < 4; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }
        SideCnt = 2;
        SaveMobius = -1;
    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {

        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 22;
        }
        else//外側
        {
            InsideLength = 0;
        }

        if (Mobius[NowMobius] != null)
        {

            Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得
            float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

            Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得
            float PlayHankei= this.GetComponent<SphereCollider>().bounds.size.x / 2 +                　　　　　// プレイヤーの円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

            Vector2 vecY = MobiusPos - PlayerPos;                                                              //プレイヤーの位置から対象のメビウスへのベクトルを求める(Y軸ベクトル)

            Vector2 vecX;//(X軸ベクトル)
            vecX.x = 0.0f * 0.0f - 1.0f * vecY.y;                                                              //Y軸とZ軸からX軸を求める
            vecX.y = 1.0f * vecY.x - 0.0f * 0.0f;

            float veclength = Mathf.Sqrt(vecY.x * vecY.x + vecY.y * vecY.y);                                   //Yベクトルの長さ計算
            //単位ベクトルにする
            vecY.x = vecY.x / veclength;
            vecY.y = vecY.y / veclength;

            veclength = Mathf.Sqrt(vecX.x * vecX.x + vecX.y * vecX.y);                                         //Xベクトルの長さ計算
            //単位ベクトルにする
            vecX.x = vecX.x / veclength;
            vecX.y = vecX.y / veclength;


            //対象のメビウスの軌道に乗せる
            if (veclength-2 > hankei - InsideLength)//対象のメビウスから離れている場合
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x + vecY.x * MovePower * Time.deltaTime, this.transform.position.y + vecY.y * MovePower * Time.deltaTime, 0);     //対象のメビウスに近づける

            }
            else if(veclength < hankei - InsideLength)//対象のメビウスに近づきすぎている
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x - vecY.x * MovePower * Time.deltaTime, this.transform.position.y - vecY.y * MovePower * Time.deltaTime, 0);     //対象のメビウスから離す
            }

            //軌道に沿って左右移動
            if (RotateLeftFlg)//左回転
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x - vecX.x * MovePower * Time.deltaTime, this.transform.position.y - vecX.y * MovePower * Time.deltaTime, 0);         //対象のメビウスの周りをまわる
            }
            else//右回転
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x + vecX.x * MovePower * Time.deltaTime, this.transform.position.y + vecX.y * MovePower * Time.deltaTime, 0);         //対象のメビウスの周りをまわる
            }


            CollisonMobius();


            if (counter > 0)
            {
                counter+=Time.deltaTime;
                if (counter > 2)
                {
                    SaveMobius = NowMobius;
                    counter = 0;
                }
            }
        }
    }


    private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    {

        Vector2 NowMobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // 現在のメビウスの輪の位置を取得
        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得

        Vector2 NowVec = NowMobiusPos - PlayerPos;
        float NowLength = Mathf.Sqrt(NowVec.x * NowVec.x + NowVec.y * NowVec.y);

        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

        Vector2 NextMobiusPos;//次のメビウスの場所
        Vector2 NextVec;
        float NextLength = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            NextMobiusPos = Mobius[i].GetComponent<SphereCollider>().bounds.center;

            Vector2 Vec = NextMobiusPos - NowMobiusPos;//対象のメビウスの輪と
            float VecLength = Mathf.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y);
            if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定
            {
                Vec.x = Vec.x / VecLength;
                Vec.y = Vec.y / VecLength;

                Vector2 CollisonPos;//当たっている場所を計算(接点)
                CollisonPos.x = NowMobiusPos.x + Vec.x * hankei;
                CollisonPos.y = NowMobiusPos.y + Vec.y * hankei;

                NextVec = CollisonPos - PlayerPos;//メビウスの輪同士の接点とプレイヤーの位置のベクトルを計算
                NextLength = Mathf.Sqrt(NextVec.x * NextVec.x + NextVec.y * NextVec.y);//メビウスの輪同士の接点とプレイヤーの位置の長さ計算


                if ((hankei/2)+(InsideLength/2) > NextLength)
                {
                    SaveMobius = NowMobius;
                    NowMobius = i;
                    counter = 1;


                    if (SideCnt>=2)//2回切り替えると
                    {
                        //内側と外側を反転させる
                        if (InsideFlg)
                        {
                            InsideFlg = false;
                        }
                        else
                        {
                            InsideFlg = true;
                        }

                        
                        SideCnt = 0;
                    }
                    //メビウスの輪を切り替えると左右移動を反転させる
                    if (RotateLeftFlg)
                    {
                        RotateLeftFlg = false;
                    }
                    else
                    {
                        RotateLeftFlg = true;
                    }

                    //Debug.Log("メビウスの輪を切り替えた");
                    SideCnt++;

                    break;
                }
            }

        }


    }


    public int GetNowMobiusNum()
    {
        return NowMobius;
    }

    // 衝突時
    // private void OnTriggerEnter(Collider other)
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("敵と当たった");
        }
    }

    // 離れた時
    private void OnTriggerExit(Collider other)
    //private void OnCollisionExit(Collision other)
    {
       
    }

}
