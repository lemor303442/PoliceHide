using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public class DestroyObj : PoliceAction
	{
		private string m_targetObjName;
		private bool m_isStart;

		public void Init(string targetObjName)
		{
			m_targetObjName = targetObjName;
			m_isStart = true;
		}

		public override void Update(){
			if(m_isStart){
				MonoBehaviour.Destroy(GameObject.Find(m_targetObjName));
				m_isStart = false;
			}
		}

		public override bool IsEnd(){
			return !m_isStart;
		}
	}
}
