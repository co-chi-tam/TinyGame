using UnityEngine;
using System.Collections;

public class CLog {

	public enum ELogMode : byte {
		None = 0
	}

	public static void LogDebug(string text, ELogMode mode = ELogMode.None) {
		Debug.Log (text);
	}

	public static void LogError(string text, ELogMode mode = ELogMode.None) {
		Debug.LogError (text);
	}

	public static void LogWarning(string text, ELogMode mode = ELogMode.None) {
		Debug.LogWarning (text);
	}

}
