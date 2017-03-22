using UnityEngine;
using System.Collections;

public class UIGroup : CBaseMonobehaviour, IGroup {

	#region Properties

	[SerializeField]	private string m_GroupName = "group 1";
	[SerializeField]	public UIMember[] members;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
	}

	#endregion

	#region IGroup implementation

	public virtual void CalculateResult ()
	{
		for (int i = 0; i < members.Length; i++) {
			if (members [i].GetResultObject () == null) {
				Debug.LogError ("SOMETHING WRONG " + members [i].name);
				return;
			}
		}
	}

	public virtual string[] GetStringResults ()
	{
		var strResults = new string[members.Length];
		for (int i = 0; i < members.Length; i++) {
			strResults [i] = members [i].GetResult ().GetString ();
		}
		return strResults;
	}

	public virtual int[] GetIntResults ()
	{
		var intResults = new int[members.Length];
		for (int i = 0; i < members.Length; i++) {
			intResults [i] = members [i].GetResult ().GetInt ();
		}
		return intResults;
	}

	public virtual float[] GetFloatResults ()
	{
		var floatResults = new float[members.Length];
		for (int i = 0; i < members.Length; i++) {
			floatResults [i] = members [i].GetResult ().GetFloat ();
		}
		return floatResults;
	}

	public virtual Vector3[] GetVector3Results ()
	{
		var v3Results = new Vector3[members.Length];
		for (int i = 0; i < members.Length; i++) {
			v3Results [i] = members [i].GetResult ().GetVector3 ();
		}
		return v3Results;
	}

	public virtual bool[] GetBoolResults ()
	{
		var boolResults = new bool[members.Length];
		for (int i = 0; i < members.Length; i++) {
			boolResults [i] = members [i].GetResult ().GetBool ();
		}
		return boolResults;
	}

	#endregion

	#region Result

	public virtual void SetString (string value)
	{
		
	}
	public virtual void SetString (string value, params object[] objs)
	{
		
	}
	public virtual string GetString ()
	{
		return string.Empty;
	}
	public virtual void SetInt (int value)
	{
		
	}
	public virtual int GetInt ()
	{
		return 0;
	}
	public virtual void SetFloat (float value)
	{
		
	}
	public virtual float GetFloat ()
	{
		return 0f;
	}
	public virtual void SetVector3 (Vector3 value)
	{
		
	}
	public virtual Vector3 GetVector3 ()
	{
		return Vector3.zero;
	}
	public virtual void SetBool (bool valule)
	{
		
	}
	public virtual bool GetBool ()
	{
		return false;
	}
	public virtual void Clear ()
	{
		
	}

	#endregion
}
