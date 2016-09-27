using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UIPointer : UIMember, IPointerEnterHandler, IPointerExitHandler {

	public Action<Vector2> OnEventPointerEnter;
	public Action<Vector2> OnEventPointerStay;
	public Action<Vector2> OnEventPointerExit;

//	#region IPointerUpHandler implementation
//
//	public void OnPointerUp (PointerEventData eventData)
//	{
//		OnItemPointUp (eventData.position);
//		if (OnEventPointerUp != null) {
//			OnEventPointerUp (eventData.position);
//		}
//	}
//
//	#endregion
//
//	#region IPointerDownHandler implementation
//	public void OnPointerDown (PointerEventData eventData)
//	{
//		OnItemPointDown (eventData.position);
//		if (OnEventPointerDown != null) {
//			OnEventPointerDown (eventData.position);
//		}
//	}
//	#endregion
//
//	#region IPointerClickHandler implementation
//	public void OnPointerClick (PointerEventData eventData)
//	{
//		OnItemClick (eventData.position);
//		if (OnEventPointerClick != null) {
//			OnEventPointerClick (eventData.position);
//		}
//	}
//	#endregion

	#region IPointerHandler implementation

	private bool m_PointerStay = false;

	public void OnPointerEnter (PointerEventData eventData)
	{
		OnItemFocusEnter (eventData.position);
		m_PointerStay = true;
		if (OnEventPointerEnter != null) {
			OnEventPointerEnter (eventData.position);
		}
	}

	void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
	{
		OnItemFocusExit (eventData.position);
		m_PointerStay = false;
		if (OnEventPointerExit != null) {
			OnEventPointerExit (eventData.position);
		}
	}

	#endregion

	#region Monobehaviour

	protected override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
		if (m_PointerStay) {
			OnItemFocusStay (Input.mousePosition);
		}
	}

	protected void OnDestroy() {
		this.Clear ();
	}

	#endregion

	#region Main methods

	protected virtual void OnItemFocusEnter(Vector2 position) {

	}

	protected virtual void OnItemFocusStay(Vector2 position) {

	}

	protected virtual void OnItemFocusExit(Vector2 position) {

	}

//	protected virtual void OnItemClick(Vector2 position) {
//		
//	}
//
//	protected virtual void OnItemPointDown(Vector2 position) {
//		
//	}
//
//	protected virtual void OnItemPointUp(Vector2 position) {
//		
//	}

	#endregion
}
