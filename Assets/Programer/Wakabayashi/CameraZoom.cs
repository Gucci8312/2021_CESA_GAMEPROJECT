using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public bool ZoomActive = false;
    // private bool ZoomActive = false;
    public Vector3[] Target;

    public Camera Cam;

    public float Speed;　        //拡大する速度

    public float distanceZ;     //Z軸の距離

    PlayerMove player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    public void Update()
    {
        if (ZoomActive)
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 5, Speed);
            Vector3 posP;
            posP = new Vector3(player.transform.position.x, player.transform.position.y, distanceZ);     //プレイヤーの座標取得
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, posP, Speed);                 //カメラの座標とプレイヤーの座標をLerp

        }
        else
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 3, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, Target[0], Speed);
        }
    }

    public void OnZoom()
    {
        ZoomActive = true;
    }

    public void OffZoom()
    {
        ZoomActive = false;
    }
}

