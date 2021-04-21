// @file   Rythm.cs
// @brief  リズム定義用スクリプト　（主に音と合わせたノーツの移動でBPMを図る）
// @author T,Cho
// @date   2021/03/10 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   Rythm
// @brief  リズムを定義するクラス
public class Rythm : MonoBehaviour
{
    [SerializeField]
    GameObject m_sphere;

    public int BPM;                         //リズム拍指定

    Vector3 m_currentPos;
    Vector3 m_targetPos;

    float m_startTime;                      //初期タイムを設定（０でいい）
    float m_time;                           //１BPMに対する秒計算

    public bool rythmCheckFlag;            //円が指定されたリズムに触れたことを返す
    public bool rythmSendCheckFlag;          //円が指定されたリズムに触れたことを返す
    public bool checkPlayerMove;                //リズムに合わせた反応が成功したかどうか
    public bool checkMoviusMove;                //リズムに合わせた反応が成功したかどうか

    //  public float SetSuccessInputTime;                    //入力待ち時間の引き伸ばし決め

    public float distance;                                  //円との距離を図る

    private int m_beatCount;                                //ビートの回数を取得
    public int EnemyTroughRing;                             //敵が何ビートによって進むのか（実装するかどうかわからない）
    static float tansu;                                     //音による誤差調整用

    [SerializeField] AudioClip SE = default;
    AudioSource audioSource;
    [SerializeField] AudioSource stageBGM = default;

    [SerializeField] GameObject MobiusObj = default;                                                                            //リズムオブジェクト
    MoveMobius mobius_script;                                                                                    //リズムスクリプト取得用

    private bool OneLRTriggerFlag;  //LRトリガー押し込みによる連続入力させない用


    GameObject m_frameManager;                              //ポストエフェクトのフレーム用
    ChangeFlameColor m_changeColorScript;                   //ポストエフェクトのフレーム用スクリプト
    // Start is called before the first frame update
    void Start()
    {
        //変数の初期化
        tansu = 0.0f;
        rythmCheckFlag = false;
        checkPlayerMove = false;
        checkMoviusMove = false;
        m_startTime = Time.timeSinceLevelLoad;

        //Componentを取得
        audioSource = GetComponent<AudioSource>();
        this.mobius_script = MobiusObj.GetComponent<MoveMobius>();                                                  //リズムのコード
        StartCoroutine("SuccessCheck");

        //音を再生・ループにする
        stageBGM.Play();
        stageBGM.loop = true;

        //フレームマネージャーからソースを取得
        m_frameManager = GameObject.Find("FrameManager");
        m_changeColorScript = m_frameManager.GetComponent<ChangeFlameColor>();
    }

    // @name   OnEnable
    // @brief  インスペクタービューから情報を取得
    private void OnEnable()
    {
        //入力されたBPMから一分間によるビート回数を取得
        m_time = (60.0f / (float)BPM);
        //目的地を設定
        m_targetPos = new Vector3(-m_sphere.transform.position.x, m_sphere.transform.position.y, m_sphere.transform.position.z);
        //現在位置を設定
        m_currentPos = m_sphere.transform.position;
    }

    // @name   FixedUpdate
    // @brief  一定フレームで呼び出し（Updateだと一定じゃないためずれがどうしても生じるため）
    private void FixedUpdate()
    {
		m_changeColorScript.Flame_Color_Attenuation();
		//音の始まりを調整
		//音のループによる読み込み時の誤差を調整
		if (stageBGM.time <= 0.05f)       
        {
            m_startTime = Time.timeSinceLevelLoad;
            m_sphere.transform.position = new Vector3(m_currentPos.x, m_currentPos.y,m_currentPos.z);
            return;
        }
        //徐々に移動するように設定
        float diff = Time.timeSinceLevelLoad - m_startTime;
        float rate = (diff / m_time) + tansu;
        //ノーツの現在位置を更新
        m_sphere.transform.position = Vector3.Lerp(m_currentPos, m_targetPos, rate);

        //越えたら　m_startTimeをその時の時間に設定
        if (rate >= 1.0f)
        {
            m_startTime = Time.timeSinceLevelLoad;
            tansu = rate - 1.0f;
        }
        else
        {
            tansu = 0.0f;
        }

        //ゴール点に達した時
        if (m_sphere.transform.position.x == m_targetPos.x)
        {
            m_targetPos = new Vector3(-m_sphere.transform.position.x, m_sphere.transform.position.y, m_sphere.transform.position.z);
            m_currentPos = m_sphere.transform.position;
        }

    }

    // @name   CheckDistanceWall
    // @brief  両端にある壁との距離を取得（距離で成功・失敗のタイミングを計る）
    void CheckDistanceWall()
    {
        GameObject[] flagWall;
        flagWall = GameObject.FindGameObjectsWithTag("Flag");


        float diffLeft = Mathf.Abs(flagWall[0].transform.position.x) - Mathf.Abs(m_sphere.transform.position.x);
        float diffRight = Mathf.Abs(flagWall[1].transform.position.x) - Mathf.Abs(m_sphere.transform.position.x);

        if ((diffLeft < distance) || (diffRight < distance))
        {
            if (rythmCheckFlag) return;
            rythmCheckFlag = true;
        }
        else if (rythmCheckFlag)
        {
            rythmCheckFlag = false;
        }

    }

    // @name   SuccessCheck
    // @brief  成功したかどうかを非同期処理で調べる
    IEnumerator SuccessCheck()
    {
        while (true)
        {
            CheckDistanceWall();

            if (rythmCheckFlag)
            {
                //Entarキーで成功かどうかを判断する
                if(Input.GetKeyDown(KeyCode.Return) || LRTrigger())
                {
                    checkPlayerMove = true;
                    rythmCheckFlag = false;
                    Debug.Log("距離：" + distance);
                }
                else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) || mobius_script.StickFlickInputFlag())
                {
                    checkMoviusMove = true;
                    rythmCheckFlag = false;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //壁に当たった＝ビートのタイミングが来た
        if (collision.gameObject.tag == "Flag")
        {
            //   Debug.Log("TriggerOn");
            m_beatCount++;
            m_changeColorScript.ChangeColor_Flame();
            //rythmCheckFlag = true;
            if (m_beatCount >= EnemyTroughRing)
            {
                //田中くんのスクリプトにおくるよう
                Invoke("TurnRythmSendCheckFlagTrue", 0.4f);
            }
            //     Invoke("TurnFalseSuccessCheck", SetSuccessInputTime);
        }
    }
    // @name   TurnRythmSendCheckFlagTrue
    // @brief  敵の移動処理に使用
    private void TurnRythmSendCheckFlagTrue()
    {
        rythmSendCheckFlag = true;
        m_beatCount = 0;
    }

    // @name   TurnRythmSendCheckFlagTrue
    // @brief   リングがキー入力受付判定を越えた時 
    private void TurnFalseSuccessCheck()
    {
        if (rythmCheckFlag)
            rythmCheckFlag = false;
    }

    // @name   LRTrigger
    // @brief  LRトリガー処理　（松井君実装） 
    private bool LRTrigger()
    {
        float LTrigger = Input.GetAxis("L_Trigger");//０～１
        float RTrigger = Input.GetAxis("R_Trigger");//０～１


        if (!OneLRTriggerFlag)
        {
            if (LTrigger == 1 || RTrigger == 1)
            {
                OneLRTriggerFlag = true;
                return true;
            }
        }
        else
        {
            if (LTrigger == 0 && RTrigger == 0)
            {
                OneLRTriggerFlag = false;
            }
        }

        return false;
    }

}
