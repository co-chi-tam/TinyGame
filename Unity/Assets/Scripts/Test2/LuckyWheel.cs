using UnityEngine;
using System;
using System.Collections;

public enum EWheelType {
	None = 0,
	AlwayFail = 1,
	AlwayWin = 2
}

[Serializable]
public class LuckyWheel {

	[SerializeField]	private GameObject m_Wheel;
	[SerializeField]	private int m_CircleSegment = 16;
	[SerializeField]	private EWheelType m_WheelType = EWheelType.None;
	[SerializeField]	private int[] m_SegmentUnwant;

	private int m_WheelTarget = 3;
	private float m_Segment;
	private float m_Rotation;
	private Vector3 m_CurrentRotation;
	private WaitForFixedUpdate m_WaitForFixedUpdate = new WaitForFixedUpdate ();
	private bool m_OnRotation = false;

	public IEnumerator HandleRotation(int target, Action start = null, Action end = null) {
		m_Segment = 360f / m_CircleSegment;
		if (m_OnRotation == true)
			yield break;
		m_CurrentRotation = m_Wheel.transform.rotation.eulerAngles;
		m_WheelTarget = (m_CircleSegment * 3) + (target % m_CircleSegment);
		m_OnRotation = true;
		if (start != null) {
			start ();
		}
		switch (m_WheelType) {
		case EWheelType.AlwayFail:
			SetupWheelUnwant ();
			break;
		case EWheelType.AlwayWin:
			SetupWheelAlwayWin ();
			break;
		case EWheelType.None:
		default:
			//TODO
			break;
		}
		var rotationFix = -m_Segment * m_WheelTarget - (m_Segment / 2f);
		var maxRotate = rotationFix - m_CurrentRotation.z;
		while (m_CurrentRotation.z - rotationFix >= 2.5f) {
			m_CurrentRotation.z = Mathf.Lerp (m_CurrentRotation.z, rotationFix, 0.025f);
			m_Wheel.transform.rotation = Quaternion.Euler (m_CurrentRotation);
			yield return m_WaitForFixedUpdate;
		}
		if (end != null) {
			end ();
		}
		m_OnRotation = false;
	}

	public int GetWheelTarget() {
		return m_WheelTarget % m_CircleSegment;
	}

	private void SetupWheelUnwant() {
		var indexFake = 100;
		while (indexFake >= 0) {
			var wheelAvailable = m_WheelTarget % m_CircleSegment;
			for (int i = 0; i < m_SegmentUnwant.Length; i++) {
				var tmp = m_SegmentUnwant [i] - wheelAvailable;
				if (tmp == 0 || m_SegmentUnwant [i] > m_CircleSegment) {
					return;
				}
			}
			m_WheelTarget += 1;
			indexFake -= 1;
		}
	}

	private void SetupWheelAlwayWin() {
		var isWin = false;
		while (isWin == false) {
			var wheelAvailable = m_WheelTarget % m_CircleSegment + 1;
			isWin = true;
			for (int i = 0; i < m_SegmentUnwant.Length; i++) {
				var tmp = m_SegmentUnwant [i] - wheelAvailable;
				if (tmp == 0) {
					isWin = false;
				}
			}
			m_WheelTarget += 1;
		}
	}

}
