using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    public GameObject SceneManeger;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch(SceneManeger.GetComponent<SceneMove>().ThisArea)
        {
            case 1:
                //this.transform.rotation = Quaternion.Euler(0,-90,0);
                this.transform.localEulerAngles = new Vector3(0,-180,0);
                break;
            case 2:
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 3:
                this.transform.rotation = Quaternion.Euler(0, -180, 0);
                break;
            case 4:
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 5:
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }
}
