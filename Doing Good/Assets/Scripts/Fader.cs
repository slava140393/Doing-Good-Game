using System;
using UnityEngine;

public class Fader : MonoBehaviour
{
	public const string Fader_Path = "Fader";
	[SerializeField] private Animator _animator;
	private Action _fadedInCallback;
	private Action _fadedOutCallback;
	private readonly int Faded = Animator.StringToHash("Faded");

	public void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public bool isFading { get; private set; }

	public void FadeIn(Action fadedInCallback)
	{
		if(isFading)
			return;

		isFading = true;
		_fadedInCallback = fadedInCallback;
		_animator.SetBool(Faded, true);
	}
	public void FadeOut(Action fadedOutCallback)
	{
		if(isFading)
			return;

		isFading = true;
		_fadedInCallback = fadedOutCallback;
		_animator.SetBool(Faded, false);
	}


	private void Handle_FadeInAnimationOver()
	{
		_fadedInCallback?.Invoke();
		_fadedInCallback = null;
		isFading = false;
	}
	private void Handle_FadeOutAnimationOver()
	{
		_fadedOutCallback?.Invoke();
		_fadedOutCallback = null;
		isFading = false;
	}
}