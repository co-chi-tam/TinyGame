using UnityEngine;
using System;
using System.Collections;
using System.Text;

public class CUtil {

	public static Vector3 V3Parser (string value) {
		var resultV3 = Vector3.zero;
		value = value.Replace ("(", "").Replace(")", ""); // (x,y,z)
		var splits = value.Split (','); // x,y,z
		resultV3.x = float.Parse (splits[0].ToString());
		resultV3.y = float.Parse (splits[1].ToString());
		resultV3.z = float.Parse (splits[2].ToString());
		return resultV3;
	}

	public static string V3StrParser (Vector3 value) {
		var result = new StringBuilder ("(");
		result.Append (value.x + ",");
		result.Append (value.y + ",");
		result.Append (value.z + "");
		result.Append (")");
		return result.ToString();
	}

}
