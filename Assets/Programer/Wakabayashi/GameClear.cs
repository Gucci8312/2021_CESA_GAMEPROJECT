using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameClear : MonoBehaviour
{

    PlayerMove player;

    public float TransformX;

    public float TransformY;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();

    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + TransformX, player.transform.position.y + TransformY, -30);
    }

}
