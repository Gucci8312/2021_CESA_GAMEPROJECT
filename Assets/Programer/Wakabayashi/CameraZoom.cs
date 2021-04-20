using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public bool ZoomActive;
    public Vector3[] Target;

    public Camera Cam;

    public float Speed;

    PlayerMove player;

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
       // playerpos = player.transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        if (ZoomActive)
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 5, Speed);
            //Cam.transform.position = player.transform.position;
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, Target[1], Speed);

        }
        else
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 3, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, Target[0], Speed);
        }
    }
}
