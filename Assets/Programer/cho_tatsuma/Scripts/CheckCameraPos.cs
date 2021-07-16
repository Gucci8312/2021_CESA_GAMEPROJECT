using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CheckCameraPos : MonoBehaviour
{
    static private GameObject mainCamera;

    static private Vector3 m_oldPosition;
    static private float m_framCount;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_framCount % 10 == 0)
        {
            m_oldPosition = mainCamera.transform.position;
        }
        m_framCount++;
        isStop();
    }

    static bool isStop()
    {
        if (Mathf.Abs(m_oldPosition.x - mainCamera.transform.position.x) < 0.1f)
        {
            Debug.Log("true");
            return true;
        }
        Debug.Log("false");
        return false;
    }
}
