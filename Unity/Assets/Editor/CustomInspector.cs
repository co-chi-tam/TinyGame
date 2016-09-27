using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UITextMovable))]
public class CustomInspector : Editor  {

	public override void OnInspectorGUI()
	{
		var myTarget = (UITextMovable)target;
		DrawDefaultInspector ();
		if (GUILayout.Button ("Fix")) {
			myTarget.FitPosition ();
		}
		if (GUILayout.Button ("Move To Center")) {
			myTarget.MoveToCenterX (null);
		}
	}
}
