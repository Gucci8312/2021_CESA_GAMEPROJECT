using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlameColor : MonoBehaviour
{
	public Material _material;
	public GameObject _DirectionLight;
	public GameObject _PointLight1;
	public GameObject _PointLight2;
	public GameObject _PointLight3;
	//public GameObject _PointLight4;

	public float DirectionLightPower = 0.05f;
	public float PointLightPower = 2000;
	int cnt = 0;
	Color mat;

	[System.Serializable]
	struct Change_Color
	{
		public Material _material;
		public Color color;
		public float Max_Color_Vlue;
		public float Min_Color_Vlue;
	}

	[System.Serializable]
	struct Change_PointLight
	{
		public Color color;
		public GameObject _PointLight1;
		public float Max_Color_Vlue;
		public float Min_Color_Vlue;
	}

	[System.Serializable]
	struct Ring
	{
		public Change_Color color;
		public Change_PointLight pointlight;
	}

	[SerializeField] private List<Ring> ring;

	// Start is called before the first frame update
	void Start()
	{

		//ring.Add(new Ring());
		//ring[0].color._material.SetColor("_Color", ring[0].color.color);
		//ring[1].color._material.SetColor("_Color", ring[1].color.color);
		//ring[2].color._material.SetColor("_Color", ring[2].color.color);
		//ring[3].color._material.SetColor("_Color", ring[3].color.color);
		

		//_material.color = Color.cyan;
		//mat = Color.green;
		//_material.SetColor("_Color", mat);

		_PointLight1.GetComponent<Light>().intensity = PointLightPower;
		_PointLight2.GetComponent<Light>().intensity = PointLightPower;
		_PointLight3.GetComponent<Light>().intensity = PointLightPower;
	}
	
	// Update is called once per frame
	void Update()
    {
		
		
	}

	public void ChangeColor_Flame()
	{

		//_material.color = mat;
		
		cnt = 0;
		//_DirectionLight.GetComponent<Light>().color = mat;
		//_DirectionLight.GetComponent<Light>().intensity = DirectionLightPower;
		_material.SetColor("_Color", new Color(0.0f, 0.7f, 0.0f));
		_PointLight1.GetComponent<Light>().intensity = PointLightPower;
		_PointLight2.GetComponent<Light>().intensity = PointLightPower;
		_PointLight3.GetComponent<Light>().intensity = PointLightPower;
		//_PointLight4.GetComponent<Light>().intensity = PointLightPower;

	}
	public void Flame_Color_Attenuation()
	{
		
		Color color = new Color(0.0f,0.2f,0.0f);
	

		if(cnt==10)
		{
			_material.SetColor("_Color", new Color(0.0f, 0.3f, 0.0f));
			_PointLight1.GetComponent<Light>().intensity = 1000;
			_PointLight2.GetComponent<Light>().intensity = 1000;
			_PointLight3.GetComponent<Light>().intensity = 1000;
			//_PointLight4.GetComponent<Light>().intensity = 0;
			cnt = 0;
		}
		cnt++;
		//_material.color = color;
	}
}
