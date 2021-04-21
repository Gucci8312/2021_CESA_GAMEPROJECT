using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlameColor : MonoBehaviour
{
	public Material _material;

	public GameObject _DirectionLight;

	float LightPower = 0.1f;
	float LightAt = -0.0001f;
	Color mat;
    // Start is called before the first frame update
    void Start()
    {
		_material.color = Color.red;
		mat = _material.color;

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
		//Color color= _material.color;
		//color.r = 255;
		//color.g = 0;
		//color.b = 0;
		_material.color = mat;
		//_DirectionLight.GetComponent<Light>().color = mat;
		//_DirectionLight.GetComponent<Light>().intensity = LightPower;
		
	}
	public void Flame_Color_Attenuation()
	{
		
		Color color =_material.color;
		color.r -=0.05f;
		color.g -= 0.05f;
		color.b -= 0.05f;
		//_DirectionLight.GetComponent<Light>().intensity = LightAt;
		_material.color = color;
	}
}
