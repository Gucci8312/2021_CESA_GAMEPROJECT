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

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        SoundManager.StopBGM();
        SoundManager.PlayBgmName(BGM);
    }

    void Update()
    {
        //transform.position = new Vector3(player.transform.position.x + TransformX, player.transform.position.y + TransformY, -30);
    }

}
