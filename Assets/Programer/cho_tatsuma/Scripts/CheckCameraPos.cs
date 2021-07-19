using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CheckCameraPos : MonoBehaviour
{
    static private GameObject mainCamera;
    static private Vector3 m_oldPosition;
    static private float speed;
    static private float m_framCount;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = ((mainCamera.transform.position - m_oldPosition) / Time.deltaTime).magnitude;
        m_oldPosition = mainCamera.transform.position;
    }

    static public bool isStop()
    {
        if(speed <= 0.2f)
        {
            return true;
        }
        Debug.Log("false");
        return false;
    }
}
