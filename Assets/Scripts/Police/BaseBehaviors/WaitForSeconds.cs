using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public class WaitForSeconds : PoliceAction
	{
		private float m_waitTime;
		private float timer;

		public void Init(float waitTime)
		{
			m_waitTime = waitTime;
		}
		public override void Update(){
			timer += Time.deltaTime;
		}
		public override bool IsEnd(){
			return m_waitTime < timer;
		}
	}
}
