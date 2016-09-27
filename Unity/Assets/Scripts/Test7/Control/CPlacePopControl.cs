using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CPlacePopControl : CPopupControl {

	[Header ("Place info")]
	[SerializeField]	private Image m_PlaceInfoImage;
	[SerializeField]	private Button m_BuyPlaceButton;
	[SerializeField]	private Button m_NextTurnButton;
	[SerializeField]	private Text m_PlacePriceText;
	[SerializeField]	private Text m_PlaceInfoText;
	[SerializeField]	private Text m_PlaceOwnerText;
	[SerializeField]	private Image m_PlaceWaitingImage;

	private bool m_BrokenCountDown = false;

	protected override void Start ()
	{
		base.Start ();
		m_BrokenCountDown = false;
	}

	public virtual void ShowInfo<T> (bool value, float time, T place = null, Action buy = null, Action next = null) where T : CPlace{
		m_Content.SetActive (value);
		if (value) {
			if (place != null) {
				m_PlaceInfoImage.sprite = place.GetImageInfo ();
				m_PlaceInfoText.text = place.ToString ();
				m_PlacePriceText.text = place.GetPrice ().ToString ();
				m_PlaceWaitingImage.type = Image.Type.Filled;
				m_PlaceWaitingImage.fillMethod = Image.FillMethod.Horizontal;
				m_PlaceWaitingImage.fillOrigin = 1;
				m_PlaceWaitingImage.fillAmount = 1f;
				m_BrokenCountDown = false;
				if (place.GetOwner () != null) {
					m_PlaceOwnerText.text = place.GetOwner ().GetPlayerName ();
				} else {
					m_PlaceOwnerText.text = "Free";
				}
				m_BuyPlaceButton.onClick.RemoveAllListeners ();
				m_BuyPlaceButton.onClick.AddListener (() => {
					if (buy != null) {
						buy ();
					}
					ShowInfo<T> (false, time);
					m_BrokenCountDown = true;
				});
				m_NextTurnButton.onClick.RemoveAllListeners ();
				m_NextTurnButton.onClick.AddListener (() => {
					if (next != null) {
						next ();
					}
					ShowInfo<T> (false, time);
					m_BrokenCountDown = true;
				});
			}
			ShowFadeIn (value, Vector3.zero);
			Show (value, 3f, next, SetPlaceWaitingImage, () => {
				return m_BrokenCountDown;
			});
		}
	}

	public void SetPlaceWaitingImage(float value) {
		m_PlaceWaitingImage.fillAmount = 1f - value;
	}

}
