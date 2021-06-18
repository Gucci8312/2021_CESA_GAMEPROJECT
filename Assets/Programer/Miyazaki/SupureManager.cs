using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupureManager : MonoBehaviour
{
	public int max_supure;
	static public float supure_num;
	static public float get_supure;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnEnable()
	{
		supure_num = max_supure;
	}

	static public float GetScore()
	{
		return (get_supure/ supure_num)*100;
	}

}
