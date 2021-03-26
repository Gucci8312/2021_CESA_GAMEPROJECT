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

    float m_frameCount;                     //リズムスフィア補間移動用フレーム更新
    public bool rythmCheckFlag;            //円が指定されたリズムに触れたことを返す
    public bool rythmSendCheckFlag;          //円が指定されたリズムに触れたことを返す
    public bool successFlag;                //リズムに合わせた反応が成功したかどうか

    public float SetSuccessInputTime;                    //入力待ち時間の引き伸ばし決め

    private  int m_beatCount;
    public  int EnemyTroughRing;
    [SerializeField] AudioClip SE;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_frameCount = 0.0f;
        rythmCheckFlag = false;
        successFlag = false;
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("SuccessCheck");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        m_time = (60.0f / BPM);
        m_targetPos = new Vector3(-m_sphere.transform.position.x, m_sphere.transform.position.y, m_sphere.transform.position.z);
        m_startTime = 0.0f;
        m_currentPos = m_sphere.transform.position;
    }

    private void FixedUpdate()
    {
        var diff = Time.timeSinceLevelLoad - m_startTime;
        var rate = (diff / m_time);
        m_sphere.transform.position = Vector3.Lerp(m_currentPos, m_targetPos, rate);

        //越えたら　m_startTimeをその時の時間に設定
        if (rate >= 1.0f) m_startTime = Time.timeSinceLevelLoad;

        //ゴール点に達した時
        if (m_sphere.transform.position.x == m_targetPos.x)
        {
            m_targetPos = new Vector3(-m_sphere.transform.position.x, m_sphere.transform.position.y, m_sphere.transform.position.z);
            m_currentPos = m_sphere.transform.position;
            m_frameCount = 0.0f;
        }
    }

    IEnumerator SuccessCheck()
    {
        while (true)
        {
            //Entarキーで成功かどうかを判断する
            if (Input.GetKeyDown(KeyCode.Return) && rythmCheckFlag)
            {
                successFlag = true;
                Debug.Log("Suceeded!!!");
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
        if(rythmCheckFlag)
        rythmCheckFlag = false;
    }
}
