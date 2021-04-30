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
        Vector3 pos = player.transform.position;
        pos.z = pos.z - 100;
        //player.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    public void Update()
    {
        if (ZoomActive)
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 5, Speed);
            //Cam.transform.position = player.transform.position;
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, player.transform.position, Speed);

        }
        else
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 3, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, Target[0], Speed);
        }
    }
}
