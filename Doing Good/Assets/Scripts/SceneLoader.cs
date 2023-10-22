using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
	private readonly ICoroutineRunner _coroutineRunner;
	//private readonly Fader _fader;
	private bool _isLoading;
	public SceneLoader(ICoroutineRunner coroutineRunner)
	{
		_coroutineRunner = coroutineRunner;
		//_fader = fader;
	}

	public void LoadScene(string sceneName, Action onLoaded)
	{
		if(_isLoading)
			return;
		string currentSceneName = SceneManager.GetActiveScene().name;

		if(currentSceneName == sceneName)
		{
			throw new Exception($"You are trying to load already loaded scene {sceneName}");
		}

		_coroutineRunner.StartCoroutine(LoadSceneRoutine(sceneName, onLoaded));
	}
	private IEnumerator LoadSceneRoutine(string sceneName, Action onLoaded)
	{
		AsyncOperation asyncOperator = SceneManager.LoadSceneAsync(sceneName);
		asyncOperator.allowSceneActivation = false;

		while(asyncOperator.progress < 0.9f)
			yield return null;

		asyncOperator.allowSceneActivation = true;

		_isLoading = true;
		onLoaded?.Invoke();
	}
}