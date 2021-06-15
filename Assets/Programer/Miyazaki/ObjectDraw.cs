using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDraw : MonoBehaviour
{

	[System.Serializable]
	struct Object_Score_Drow
	{
		public GameObject gameobject;
		public int threshold;
	}
	[SerializeField] private List<Object_Score_Drow> Score_Obj;



	void Object_Draw_Update(int nowsocre)
    {
		for (int i=0;i<Score_Obj.Count;i++)
		{
			if (nowsocre < Score_Obj[i].threshold)
			{
				Score_Obj[i].gameobject.SetActive(true);
			}
		}
	}



}
