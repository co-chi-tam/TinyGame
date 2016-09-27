using System;
using System.Collections;
using UnityEngine;

public interface IGroup: IResult {

	void CalculateResult();
	string[] GetStringResults ();
	int[] GetIntResults ();
	float[] GetFloatResults ();
	Vector3[] GetVector3Results();
	bool[] GetBoolResults();

}
