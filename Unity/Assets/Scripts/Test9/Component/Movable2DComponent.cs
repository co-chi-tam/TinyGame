using UnityEngine;
using System;
using System.Collections;

public class Movable2DComponent : Component {

	private Vector3 m_StartPosition;
	private Vector3 m_TargetPosition;

	private float m_Speed = 1f;
	private float m_MinDistance = 0.005f;

	public Movable2DComponent () : base()
	{
		
	}

	public bool Calculate(Vector3 current, out Vector3 result) {
		var needCalculate = false;
		result = current;
		m_TargetPosition.z = current.z = 0f;
		var direction = m_TargetPosition - current;
		var distance = direction.sqrMagnitude;
		if (distance > m_MinDistance) {
			result += direction.normalized * m_DeltaTime * m_Speed;
			needCalculate = true;
		}
		return needCalculate;
	}

	public bool Finish(Vector3 current) {
		m_TargetPosition.z = current.z = 0f;
		var direction = m_TargetPosition - current;
		var distance = direction.sqrMagnitude;
		return distance <= m_MinDistance;
	}

	public void SetSpeed(float speed) {
		m_Speed = speed;
	}

	public void SetDistance(float value) {
		m_MinDistance = value;
	}

	public void SetTargetPosition(Vector3 position) {
		m_TargetPosition = position;
	}

	public Vector3 GetCurrentPosition(Vector3 position) {
		var cameraOffset = Camera.main.orthographicSize * 2f;
		var ratio = position.y / 2f; //cameraOffset;
		position.z = -1f + ratio;
		return position;
	}

	public void SetStartPosition(Vector3 position) {
		m_StartPosition = position;
	}

	public Vector3 GetStartPosition() {
		return m_StartPosition;
	}


}
