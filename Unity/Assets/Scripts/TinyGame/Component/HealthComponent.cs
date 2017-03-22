using System;
using System.Collections;

public class HealthComponent : Component {

	private int m_TotalDamage;
	private int m_TotalBuff;

	public HealthComponent () : base ()
	{
		m_TotalDamage = 0;
	}

	public void ApplyDamage(int value) {
		m_TotalDamage += value;
	}

	public void ApplyBuff(int value) {
		m_TotalBuff += value;
	}

	public bool Calculate(int current, out int result) {
		var needCalculate = false;
		result = current;
		if (m_TotalDamage != 0) {
			result = result - m_TotalDamage;
			m_TotalDamage = 0;
			needCalculate |= true;
		}
		if (m_TotalBuff != 0) {
			result = result + m_TotalBuff;
			m_TotalBuff = 0;
			needCalculate |= true;
		}
		return needCalculate;
	}

	public override void Clear ()
	{
		base.Clear ();
		m_TotalDamage = 0;
		m_TotalBuff = 0;
	}

}
