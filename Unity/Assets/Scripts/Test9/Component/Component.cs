using System;
using System.Collections;

public class Component {

	protected float m_TotalTime;
	protected float m_DeltaTime;

	public Component ()
	{
		
	}

	public virtual void UpdateComponent(float dt) {
		m_TotalTime += dt;
		m_DeltaTime = dt;
	}

	public virtual void Clear() {
		m_TotalTime = 0f;
	}

}
