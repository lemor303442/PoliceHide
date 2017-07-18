using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class WalkTo : PoliceAction
	{
		private Vector3 m_targetPos;
		private float m_speed;
		private bool m_isEnd;

		public void Init(Vector3 targetPos, float speed)
		{
			m_targetPos = targetPos;
			m_speed = speed;
			m_isEnd = false;
		}
		public override void Update(){
			var dir = m_targetPos - m_gameObject.transform.position;
			var step = m_speed * Time.deltaTime;
			if (Mathf.Sqrt(dir.sqrMagnitude) >= step)
			{
				dir.Normalize();
				dir *= step;
			}
			else
			{
				m_isEnd = true;
			}
			m_gameObject.transform.position += dir;
//			m_gameObject.transform.position += m_gameObject.transform.forward * m_speed * Time.deltaTime;
		}
		public override bool IsEnd(){
			return m_isEnd;
		}
	}
}
