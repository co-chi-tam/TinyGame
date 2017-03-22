using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnimatorEventComponent : Component {

	public Animator animator;

	private IAnimatorEvent m_CallBack;
	private string didEndAnimation = "DidEndAnimation";
	private Dictionary<string, Action<string>> m_AnimationEvent;

	public AnimatorEventComponent (Animator animControl, IAnimatorEvent eventCallback) : base()
	{
		animator = animControl;
		m_CallBack = eventCallback;
		m_AnimationEvent = new Dictionary<string, Action<string>> ();
		SetUpAnimationClip ();
	}

	public void AddEvent(string name, Action<string> function) {
		if (m_AnimationEvent.ContainsKey (name) == false) {
			m_AnimationEvent.Add (name, function);
		}
	}

	public void Invoke(string name, string value) {
		if (m_AnimationEvent.ContainsKey (name) == true) {
			m_AnimationEvent[name](value);
		}
	}

	private AnimationClip GetAnimationClipByName(string name) {
		var animationClips = animator.runtimeAnimatorController.animationClips;
		for (int i = 0; i < animationClips.Length; i++) {
			if (animationClips [i].name == name) {
				return animationClips [i];
			}
		}
		return null;
	}

	private void SetUpAnimationClip() {
		var animationClips = animator.runtimeAnimatorController.animationClips;
		for (int i = 0; i < animationClips.Length; i++) {
			var eventAnim = new AnimationEvent ();
			eventAnim.time = animationClips [i].length;
			eventAnim.functionName = didEndAnimation;
			eventAnim.stringParameter = animationClips [i].name;
			animationClips [i].AddEvent (eventAnim);
		}
	}

	private AnimationClip SetUpAnimationClipByName(string name, string function) {
		var animationClips = animator.runtimeAnimatorController.animationClips;
		for (int i = 0; i < animationClips.Length; i++) {
			if (animationClips [i].name == name) {
				var eventAnim = new AnimationEvent ();
				eventAnim.time = animationClips [i].length;
				eventAnim.functionName = function;
				eventAnim.stringParameter = animationClips [i].name;
				animationClips [i].AddEvent (eventAnim);
				return animationClips [i];
			}
		}
		return null;
	}

}
