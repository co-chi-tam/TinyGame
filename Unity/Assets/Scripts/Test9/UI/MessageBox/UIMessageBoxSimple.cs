using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIMessageBoxSimple : UIMessageBox {

	#region Properties

	[SerializeField]	private Text m_TitleText;
	[SerializeField]	private Text m_ContentText;

	public override string title {
		get { 
			if (m_TitleText != null) {
				return m_TitleText.text;
			}
			return base.title;
		}
		set { 
			base.title = value;
			if (m_TitleText != null) {
				m_TitleText.text = value;
			}
		}
	}

	public override string content {
		get { 
			if (m_ContentText != null) {
				return m_ContentText.text;
			} 
			return base.content;
		}
		set { 
			base.content = value;
			if (m_ContentText != null) {
				m_ContentText.text = value;
			}
		}
	}

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
	}

	#endregion
}
