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
    float Size;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        SoundManager.StopBGM();
        SoundManager.PlayBgmName(BGM);
        Invoke("StartGameClear", 1.9f);
    }

    void FixedUpdate()
    {
        //transform.position = new Vector3(player.transform.position.x + TransformX, player.transform.position.y + TransformY, -30);
        if (GameStartFlg)
        {
            if (Size < 0.8f)
            {
                Size += 0.1f;
            }
            Scale.x = Size;
            Scale.y = Size;

            //Scale.x += 0.1f;
            //Scale.y += 0.1f;

            gameObject.transform.localScale = Scale;
            Invoke("EndGameClear", 0.2f);
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
