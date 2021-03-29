using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagWallPos : MonoBehaviour
{
    public enum WALLPOSITION{
        LEFT,
        RIGHT
    }
    public WALLPOSITION wallType;            //壁2つ別々の場所に保持するためのタイプ選択
    [SerializeField] GameObject rythm_sphere;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        switch (wallType)
        {
            case WALLPOSITION.LEFT:
                this.transform.position = rythm_sphere.GetComponent<Transform>().position;
                this.transform.position = new Vector3(-this.transform.position.x, this.transform.position.y, this.transform.position.z);
                break;
            case WALLPOSITION.RIGHT:
                this.transform.position = rythm_sphere.GetComponent<Transform>().position;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
