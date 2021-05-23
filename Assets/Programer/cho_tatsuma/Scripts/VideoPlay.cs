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
    [SerializeField] Image m_circle = default;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GetComponent<VideoPlayer>();
        mPlayer.Stop();
        skip = false;
        Invoke("VideoPlayMethod", 1.5f);
    }


    // Update is called once per frame
    void Update()
    {
        if (PushButton()) { } else
        {
            m_time = 0f;
            m_circle.fillAmount = 0;
        }
        if (mPlayer.time >= mPlayer.length - 1f && !endVideo)
        {
            endVideo = true;
        }

        if ( skip && !endVideo)
        {
            endVideo = true;
        }

        if (Controler.GetMenuButtonFlg())
        {
            if (mPlayer.isPaused)
            {
                mPlayer.Play();
            }
            else
            {
                mPlayer.Pause();
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
        else if(Input.GetKey("joystick button 0"))
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
