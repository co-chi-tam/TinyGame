using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UITextMovable : MonoBehaviour {

	[SerializeField]	private float m_Speed = 200f;
	[SerializeField]	private RectTransform m_Content;
	[SerializeField]	private Text m_Text;
	[SerializeField]	private EMoveDirection m_MoveDirection = EMoveDirection.RightToLeft;

	private float halfWidth = 0f;
	private bool moved = false;
	private int direction = 1;
	private Queue<string> queueString = new Queue<string> ();

	public string text {
		get { return queueString.Peek(); }
		set { queueString.Enqueue (value); }
	}

	public enum EMoveDirection: byte {
		RightToLeft = 0,
		LeftToRight = 1
	}

	protected void Awake ()
	{
		switch (m_MoveDirection) {
		default:
		case EMoveDirection.RightToLeft:
			direction = -1;
			break;
		case EMoveDirection.LeftToRight:
			direction = 1;
			break;
		}
		queueString.Clear ();
		FitPosition ();
	}

	protected void Update() {
		if (queueString.Count > 0 && moved == false) {
			m_Text.text = queueString.Dequeue();
			MoveToCenterX (() => {
				// TODO
			});
		}
	}

	public void MoveToCenterX(Action complete = null) {
		CHandleEvent.Instance.AddEvent(HandleMove(complete));
	}

	public void FitPosition ()
	{
		halfWidth = m_Text.preferredWidth / 2f;
		var fitX = -direction * (halfWidth + m_Content.sizeDelta.x);
		var anchorX = direction == 1 ? 1 : 0;
		m_Text.rectTransform.anchorMin = new Vector2 (anchorX, 0.5f);
		m_Text.rectTransform.anchorMax = new Vector2 (anchorX, 0.5f);
		m_Text.rectTransform.sizeDelta = new Vector2 (m_Text.preferredWidth, m_Text.preferredHeight);
		m_Text.rectTransform.anchoredPosition = new Vector2 (fitX, 0f);
	}

	private IEnumerator HandleMove(Action complete) {
		if (moved) {
			yield break;
		}
		FitPosition ();
		moved = true;
		var movePosition = m_Text.rectTransform.anchoredPosition;
		while (MoveCondition(movePosition.x)) {
			movePosition.x += direction * m_Speed * Time.fixedDeltaTime;
			m_Text.rectTransform.anchoredPosition = movePosition;
			yield return WaitHelper.WaitFixedUpdate;
		}
		moved = false;
		if (complete != null) {
			complete ();
		}
	}

	private bool MoveCondition(float x) {
		switch (m_MoveDirection) {
		default:
		case EMoveDirection.RightToLeft:
			return x >= halfWidth * direction;
		case EMoveDirection.LeftToRight:
			return x <= halfWidth * direction;
		}
		return false;
	}

}
