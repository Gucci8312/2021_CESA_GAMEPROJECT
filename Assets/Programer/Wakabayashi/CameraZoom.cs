using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public bool Zoom;
    public Vector3[] Target;

    public Camera Cam;

    public float Speed;

    PlayerMove player;

    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        player.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Zoom)
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 5, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, player.transform.position, Speed);
        }
        else
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 3, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, Target[0], Speed);
        }
    }

    public bool GetZoom()
    {
        return Zoom;
    }
}
