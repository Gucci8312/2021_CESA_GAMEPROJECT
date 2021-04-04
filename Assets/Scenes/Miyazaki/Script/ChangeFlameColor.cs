using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlameColor : MonoBehaviour
{
	public Material _material;
    // Start is called before the first frame update
    void Start()
    {
		_material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ChangeColor_Flame();
		}
		
	}

	void ChangeColor_Flame()
	{
		if(_material.color == Color.red)
		{
			_material.color = Color.blue;
		}
		else if (_material.color == Color.blue)
		{
			_material.color = Color.red;
		}
	}
}
