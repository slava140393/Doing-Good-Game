using System;
using Data;
using UI;
using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
	private void Start()
	{
		JsonReadWriteService jsonReadWriteService = new JsonReadWriteService();
		UILoader uiLoader = Instantiate(Resources.Load<UILoader>("UIDocument"));
		uiLoader.Initialize(jsonReadWriteService);
	}



	private void Update()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			if(Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
				return;
			}
		}
	}
}