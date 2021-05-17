using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameClear : MonoBehaviour
{

    PlayerMove player;

    public float TransformX;

    public float TransformY;
    public string BGM;
    bool GameStartFlg;
    Vector3 Scale;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        SoundManager.StopBGM();
        SoundManager.PlayBgmName(BGM);
        Invoke("StartGameClear", 1.9f);
    }

    void Update()
    {
        //transform.position = new Vector3(player.transform.position.x + TransformX, player.transform.position.y + TransformY, -30);
        if (GameStartFlg)
        {
            Scale.x += 0.15f;
            Scale.y += 0.15f;
            gameObject.transform.localScale = Scale;
            Invoke("EndGameClear",0.1f);
        }
    }
    public void StartGameClear()
    {
        GameStartFlg = true;
    }
    void EndGameClear()
    {
        GameStartFlg = false;
    }
}
