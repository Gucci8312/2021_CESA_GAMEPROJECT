using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreExpantion : MonoBehaviour
{
    private int scoreUp = 0;
    GameObject ScoreObj;
    AudioSource ScoreObjAudio;

    // Start is called before the first frame update
    void Start()
    {
        ScoreObj = GameObject.Find("Score");
        ScoreObj.GetComponent<ExpantionShrink>().musicOn = false;
        ScoreObjAudio = ScoreObj.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (scoreUp < (int)SupureManager.GetScore())
        {
            scoreUp++;
            ScoreObj.GetComponent<ExpantionShrink>().isExpantion = true;
            ScoreObjAudio.Play();
            NumControl.DrawScore(scoreUp);
        }
    }
}
