﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onpu : MonoBehaviour
{
    Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {
        Pos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Pos.x *= Random.value;
        Pos.y *= Random.value;
        transform.Translate(Pos);
    }
}
