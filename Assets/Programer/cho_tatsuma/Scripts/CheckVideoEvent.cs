using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVideoEvent : MonoBehaviour
{
    public bool checkCollider;
    bool onece = false;
    // Start is called before the first frame update
    void Start()
    {
        checkCollider = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !onece)
        {
            checkCollider = true;
            onece = true;
        }
    }
}
