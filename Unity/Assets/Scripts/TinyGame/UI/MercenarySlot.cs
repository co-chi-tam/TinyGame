using System;
using System.Collections;
using UnityEngine;

public class MercenarySlot : CBaseMonobehaviour, IResult {

	#region Properties

	[SerializeField]	private string m_MercenarySlotStr;
	[SerializeField]	private int m_MercenarySlotInt;
	[SerializeField]	private float m_MercenarySlotFlt;
	[SerializeField]	private Vector3 m_MercenarySlotV3;
	[SerializeField]	private bool m_ActiveSlot;

	#endregion

	#region IResult implementation

	public void SetString (string value)
	{
		m_MercenarySlotStr = value;
	}

	public void SetString (string value, params object[] objs)
	{
		m_MercenarySlotStr = string.Format (m_MercenarySlotStr, objs);
	}

	public string GetString ()
	{
		return m_MercenarySlotStr;
	}

	public void SetInt (int value)
	{
		m_MercenarySlotInt = value;
	}

	public int GetInt ()
	{
		return m_MercenarySlotInt;
	}

	public void SetFloat (float value)
	{
		m_MercenarySlotFlt = value;
	}

	public float GetFloat ()
	{
		return m_MercenarySlotFlt;
	}

	public void SetVector3 (Vector3 value)
	{
		m_MercenarySlotV3 = value;
	}

	public Vector3 GetVector3 ()
	{
		return m_MercenarySlotV3;
	}

	public void SetBool (bool value) {
		m_ActiveSlot = value;
	}

	public bool GetBool() {
		return m_ActiveSlot;
	}

	public void Clear() {
		m_MercenarySlotStr = string.Empty;
		m_ActiveSlot = false;
	}

	#endregion



}
