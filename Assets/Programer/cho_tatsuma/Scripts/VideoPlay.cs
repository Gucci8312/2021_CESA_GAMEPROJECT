using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlay : MonoBehaviour
{
    //　VideoPlayerコンポーネント
    private VideoPlayer mPlayer;
    public bool endVideo;

    float m_time;
    public float inputInterval;
    bool skip;
    private GameMaster m_gameMaster;
    [SerializeField] Image m_circle = default;

    // Start is called before the first frame update
    void Start()
    {
        PauseManager.GameObjectFindInit();
        m_gameMaster = GameObject.Find("GameManeger").GetComponent<GameMaster>();
        mPlayer = GetComponent<VideoPlayer>();
        mPlayer.Stop();
        skip = false;
        Invoke("VideoPlayMethod", 1.5f);
    }


    // Update is called once per frame
    void Update()
    {
        if (!endVideo) PauseManager.OnPause();
        if (PushButton() && mPlayer.isPlaying) { }
        else
        {
            m_time = 0f;
            m_circle.fillAmount = 0;
        }
        if (mPlayer.time >= mPlayer.length - 1f && !endVideo)
        {
            endVideo = true;
            m_time = 0f;
            m_circle.fillAmount = 0;
        }

        if (skip && !endVideo)
        {
            endVideo = true;
        }

        if (Controler.GetMenuButtonFlg()) {
            if (mPlayer.isPlaying)
            {
                mPlayer.Pause();
            }
            else if(mPlayer.isPaused)
            {
                mPlayer.Play();
            }
        }
    }


    void VideoPlayMethod()
    {
        mPlayer.Play();
    }

    bool PushButton()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            m_time += Time.deltaTime;
            m_circle.fillAmount = m_time / inputInterval;
            //処理を書く
            if (m_time >= inputInterval)
            {
                m_circle.fillAmount = 0;
                skip = true;
            }
            return true;
        }
        else if (Input.GetKey("joystick button 0"))
        {
            m_time += Time.deltaTime;
            m_circle.fillAmount = m_time / inputInterval;
            //処理を書く
            if (m_time >= inputInterval)
            {
                m_circle.fillAmount = 0;
                skip = true;
            }
            return true;
        }
        return false;
    }
}
