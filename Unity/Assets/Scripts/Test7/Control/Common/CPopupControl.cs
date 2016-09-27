using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CPopupControl : CControl {

	[SerializeField]	protected GameObject m_Content;
	[SerializeField]	protected CanvasGroup m_ContentCanvasGroup;

	private WaitForFixedUpdate m_WaitForFixedUpdate;

	protected override void Start ()
	{
		base.Start ();
		if (m_ContentCanvasGroup == null) {
			m_ContentCanvasGroup = m_Content.AddComponent<CanvasGroup> ();
		}
	}

	public virtual void Show(bool value, float timeShow = 3f, Action complete = null, Action<float> processing = null, Func<bool> broken = null) {
		if (value) {
			CHandleEvent.Instance.AddEvent (timeShow, HandleActiveGameObject (m_Content, false), complete, processing, broken);
		}
	}

	public virtual void ShowFadeIn (bool value, Vector3 direction, float distance = 20f, float timeShow = 0.25f, Action end = null) {
		m_Content.SetActive (value);
		m_ContentCanvasGroup.alpha = 0f;
		if (value) {
			CHandleEvent.Instance.AddEvent (timeShow, HandleFadeInDirectionObject (m_ContentCanvasGroup, direction, distance), end);
		}
	}

	public virtual void ShowFadeOut (bool value, Vector3 direction, float distance = 20f, float timeShow = 0.25f, Action end = null) {
		m_Content.SetActive (value);
		m_ContentCanvasGroup.alpha = 1f;
		if (value) {
			CHandleEvent.Instance.AddEvent (timeShow, HandleFadeOutDirectionObject (m_ContentCanvasGroup, direction, distance), () => {
				m_Content.SetActive (false);
				if (end != null){
					end();
				}
			});
		}
	}

	public IEnumerator HandleFadeOutDirectionObject(CanvasGroup group, Vector3 direction, float distance) {
		var speed = Time.deltaTime * 5f;
		while (group.alpha >= speed) {
			var position = group.transform.position;
			position.x = Time.deltaTime * distance * direction.x;
			position.y = Time.deltaTime * distance * direction.y;
			position.z = Time.deltaTime * distance * direction.z;
			group.alpha -= speed;
			group.transform.position += position;
			yield return m_WaitForFixedUpdate;
		}
		m_ContentCanvasGroup.alpha = 0f;
	}

	public IEnumerator HandleFadeInDirectionObject(CanvasGroup group, Vector3 direction, float distance) {
		var speed = Time.deltaTime;
		while (group.alpha < 1f) {
			var position = group.transform.position;
			position.x = Time.deltaTime * distance * direction.x;
			position.y = Time.deltaTime * distance * direction.y;
			position.z = Time.deltaTime * distance * direction.z;
			group.alpha += speed;
			group.transform.position += position;
			yield return m_WaitForFixedUpdate;
		}
		m_ContentCanvasGroup.alpha = 1f;
	}

	public IEnumerator HandleActiveGameObject(GameObject obj, bool active) {
		yield return null;
		obj.SetActive (active);
	}

}
