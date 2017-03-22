using UnityEngine;
using System.Collections;

public interface IResult {

	// STRING
	void SetString(string value);
	void SetString(string value, params object[] objs);
	string GetString();

	// INT
	void SetInt(int value);
	int GetInt();

	// FLOAT
	void SetFloat (float value);
	float GetFloat();

	// Vector 3
	void SetVector3 (Vector3 value);
	Vector3 GetVector3();

	// BOOL
	void SetBool (bool valule);
	bool GetBool();

	// CLEAR
	void Clear();
}
