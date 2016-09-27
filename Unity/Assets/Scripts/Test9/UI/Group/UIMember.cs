using UnityEngine;
using System.Collections;

public class UIMember : CBaseMonobehaviour, IMember {

	public virtual IResult GetResult ()
	{
		return null;
	}

	public virtual IResult GetResultObject () {
		return null;
	}

	public virtual void Clear() {
		
	}

}
