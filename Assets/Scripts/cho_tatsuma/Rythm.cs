// @file   Rythm.cs
// @brief  
// @author T,Cho
// @date   2021/03/10 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float SetSuccessInputTime;                    //入力待ち時間の引き伸ばし決め

    private int m_beatCount;
    public int EnemyTroughRing;
    static float bpm_time;
    static float tansu;
    [SerializeField] AudioClip SE;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        tansu = 0.0f;
        rythmCheckFlag = false;
        checkPlayerMove = false;
        checkMoviusMove = false;
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("SuccessCheck");
        m_startTime = Time.timeSinceLevelLoad;
    }

    private void OnEnable()
    {
        m_time = (60.0f / (float)BPM);
        Debug.Log(m_time);
        m_targetPos = new Vector3(-m_sphere.transform.position.x, m_sphere.transform.position.y, m_sphere.transform.position.z);
        m_currentPos = m_sphere.transform.position;
    }

    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad <= (m_time / 2.0f))
        {
            m_startTime = Time.timeSinceLevelLoad;
            return;
        }
        float diff = Time.timeSinceLevelLoad - m_startTime;
        float rate = (diff / m_time) + tansu;
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

        bpm_time = Time.timeSinceLevelLoad;
    }

    IEnumerator SuccessCheck()
    {
        while (true)
        {
            //Entarキーで成功かどうかを判断する
            if (Input.GetKeyDown(KeyCode.Return) && rythmCheckFlag)
            {
                checkPlayerMove = true;
                rythmCheckFlag = false;
                //  Debug.Log("Suceeded!!!");
            }

            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && rythmCheckFlag)
            {
                rythmCheckFlag = false;
                checkMoviusMove = true;
                checkPlayerMove = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Flag")
        {
            //   Debug.Log("TriggerOn");
            audioSource.PlayOneShot(SE);
            Debug.Log("音の感覚 : " + bpm_time);
            rythmCheckFlag = true;
            m_beatCount++;
            if (m_beatCount >= EnemyTroughRing)
            {
                //田中くんのスクリプトにおくるよう
                Invoke("TurnRythmSendCheckFlagTrue", 0.4f);
            }
            Invoke("TurnFalseSuccessCheck", SetSuccessInputTime);
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
}
