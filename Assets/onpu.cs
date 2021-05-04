using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onpu : MonoBehaviour
{
    Vector3 Pos;
    public bool DownFlg;
    Rigidbody rb;
    float DestroyTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Pos = this.gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
        Pos.x = 0.0f;
        Pos.y = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Pos.x *= Random.value;
        //Pos.y *= Random.value;
        transform.Translate(Pos);
        if (EffectControl.GetOnpuDownFlg())
        {
            this.gameObject.transform.parent = null;
            rb.AddForce(new Vector3(0.0f, -2000.0f, 0.0f));
            DestroyTime -= Time.deltaTime;
            if (DestroyTime <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
