using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    //int a;
    //public GameObject[] game;
    // Start is called before the first frame update
    GameObject scoreObj;
    public bool col;
    AudioSource audi;
    void Start()
    {
        col = false;
        scoreObj = GameObject.Find("Score");
        audi = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("すり抜けた！");
        //if (other.tag == "Wa")
        //{
        //    //a++;
        //    SupureManager.get_supure++;
        //  //  this.gameObject.SetActive(false);

        //}

    }

    public void Collision()
    {
        if (col) return ;
        if(this.gameObject.transform.parent != null)
        {
            this.gameObject.transform.parent = null;
        }
        SupureManager.get_supure++;
        // this.gameObject.SetActive(false);
        audi.Play();
        col = true;
    }
}
