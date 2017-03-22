using UnityEngine;
using System.Collections;

public class UIResult : CBaseMonobehaviour, IResult {

	#region Properties

	[SerializeField]	protected string m_ResultString;
	[SerializeField]	protected int m_ResultInt;
	[SerializeField]	protected float m_ResultFloat;
	[SerializeField]	protected Vector3 m_ResultV3;
	[SerializeField]	protected bool m_ResultBool;

	#endregion

	#region IResult implementation

	public virtual void SetString (string value)
	{
		m_ResultString = value;
	}

	public virtual void SetString (string value, params object[] objs)
	{
		m_ResultString = string.Format (m_ResultString, objs);
	}

	public virtual string GetString ()
	{
		return m_ResultString;
	}

	public virtual void SetInt (int value)
	{
		m_ResultInt = value;
	}

	public virtual int GetInt ()
	{
		return m_ResultInt;
	}

	public virtual void SetFloat (float value)
	{
		m_ResultFloat = value;
	}

	public virtual float GetFloat ()
	{
		return m_ResultFloat;
	}

	public virtual void SetVector3 (Vector3 value)
	{
		m_ResultV3 = value;
	}

	public virtual Vector3 GetVector3 ()
	{
		return m_ResultV3;
	}

	public virtual void SetBool (bool value) {
		m_ResultBool = value;
	}

	public virtual bool GetBool() {
		return m_ResultBool;
	}

	public virtual void Clear() {
		m_ResultString = string.Empty;
		m_ResultInt = 0;
		m_ResultFloat = 0f;
		m_ResultV3 = Vector3.zero;
		m_ResultBool = false;
	}

	#endregion

}
