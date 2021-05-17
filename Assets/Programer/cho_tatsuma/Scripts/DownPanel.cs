using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPanel : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        //speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y > -10)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - Time.deltaTime * speed, this.transform.position.z);
        }
        else if (this.gameObject.transform.position.y < -10)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, -10, this.transform.position.z);
        }
    }
}
