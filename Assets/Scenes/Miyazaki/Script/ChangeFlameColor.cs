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

	public void ChangeColor_Flame()
	{
		_material.color = Color.red;
		
	}
	public void Flame_Color_Attenuation()
	{
		
		Color color =_material.color;
		color.r -=0.05f;
		color.g -= 0.05f;
		color.b -= 0.05f;
		_material.color = color;
	}
}
