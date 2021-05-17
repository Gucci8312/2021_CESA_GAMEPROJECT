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
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GetComponent<VideoPlayer>();
        Invoke("VideoPlayMethod", 0.5f);
    }


    // Update is called once per frame
    void Update()
    {       
        if(mPlayer.time >= mPlayer.length - 1f && !endVideo)
        {
            endVideo = true;
        }

        if (Controler.SubMitButtonFlg() && !endVideo)
        {
            endVideo = true;
        }
    }

    void VideoPlayMethod()
    {
        mPlayer.Play();

    }
}
