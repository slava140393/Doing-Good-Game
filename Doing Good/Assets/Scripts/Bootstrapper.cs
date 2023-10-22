using System;
using System.Collections;
using System.Runtime.Serialization.Json;
using Data;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Bootstrapper : MonoBehaviour, ICoroutineRunner
{
	//[SerializeField] private Fader _faderPrefab;
	private const string Main_Scene_Name = "MainScene";
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	private IEnumerator Start()
	{
		SceneLoader sceneLoader = new SceneLoader(this);
	

		yield return LoadScene(sceneLoader);
	

	}
	private IEnumerator LoadScene(SceneLoader sceneLoader)
	{
		bool onLoaded = false;
		sceneLoader.LoadScene(Main_Scene_Name, () => onLoaded = true);

		while(!onLoaded)
			yield return null;
	}
}