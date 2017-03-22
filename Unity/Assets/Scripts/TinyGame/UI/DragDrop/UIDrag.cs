using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UIDrag : UIPointer, IBeginDragHandler, IDragHandler, IEndDragHandler {

	#region Properties

	[SerializeField]	public GameObject root;
	[SerializeField]	public GameObject content;
	[SerializeField]	public GameObject target;
	[SerializeField]	private EDragState m_DragState = EDragState.Free;
	[SerializeField]	private GameObject m_Result;
	[SerializeField]	private UIGroup m_Group;

	public enum EDragState : int {
		Free 		= 0,
		BeginDrag 	= 1,
		Drag 		= 2,
		EndDrag 	= 3
	}

	private Vector3 m_StartPosition;
	private Transform m_TargetContent;
	private IResult m_IResultRepair;

	public UIDrop dropableObject;
	public Action<Vector2, IResult> OnEventBeginDrag;
	public Action<Vector2, IResult> OnEventDrag;
	public Action<Vector2, IResult> OnEventEndDrag;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
		Init ();
	}

	private void OnEnable() {
		Init ();
	}

	private void Init() {
		m_StartPosition = target.transform.localPosition;
		m_TargetContent = target.transform.parent;
		m_IResultRepair = m_Result.GetComponent<IResult> ();
	}

	#endregion

	#region IDragHandler implementation

	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{
		OnItemBeginDrag (eventData.position, m_IResultRepair);
		if (OnEventBeginDrag != null) {
			OnEventBeginDrag(eventData.position, m_IResultRepair);
		}
		m_DragState = EDragState.BeginDrag;
	}

	public void OnDrag (PointerEventData eventData)
	{
		OnItemDrag (eventData.position, m_IResultRepair);
		if (OnEventDrag != null) {
			OnEventDrag(eventData.position, m_IResultRepair);
		}
		m_DragState = EDragState.Drag;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		OnItemEndDrag (eventData.position, m_IResultRepair);
		if (OnEventEndDrag != null) {
			OnEventEndDrag(eventData.position, m_IResultRepair);
		}
		m_DragState = EDragState.EndDrag;
	}

	#endregion

	#region IMember implementation

	public override IResult GetResult ()
	{
		if (m_IResultRepair != null && m_DragState == EDragState.EndDrag) {
			return m_IResultRepair;
		}
		return null;
	}

	public override IResult GetResultObject ()
	{
		return m_IResultRepair;
	}

	public override void Clear ()
	{
		base.Clear ();
		DestroyImmediate (target);
	}

	#endregion

	#region Main methods

	protected virtual void OnItemBeginDrag(Vector2 position, IResult result) {
		target.transform.SetParent (root.transform);
		target.transform.position = position;
	}

	protected virtual void OnItemDrag(Vector2 position, IResult result) {
		target.transform.position = position;
	}

	protected virtual void OnItemEndDrag(Vector2 position, IResult result) {
		target.transform.SetParent (m_TargetContent.transform);
		target.transform.localPosition = m_StartPosition;
	}

	public void SetState(EDragState state) {
		m_DragState = state;
	}

	#endregion
}
