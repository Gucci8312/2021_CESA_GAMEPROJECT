using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onpu : MonoBehaviour
{
    Vector3 Pos;
   public bool DownFlg;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Pos = this.gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Pos.x *= Random.value;
        Pos.y *= Random.value;
        transform.Translate(Pos);
        if(DownFlg)
        {
            rb.AddForce(new Vector3(0.0f,-200.0f,0.0f));
        }
    }
}
