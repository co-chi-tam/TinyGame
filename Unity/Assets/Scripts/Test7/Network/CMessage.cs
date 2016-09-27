using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CMessage : MessageBase {

	public string content;

	public override void Serialize (NetworkWriter writer)
	{
		base.Serialize (writer);
		writer.Write (content);
	}

	public override void Deserialize (NetworkReader reader)
	{
		base.Deserialize (reader);
		content = reader.ReadString ();
	}

}
