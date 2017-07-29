using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public class RotateTo : PoliceAction
	{
		private	Vector3	m_destPos;
		private	Quaternion m_destRot;
		private bool m_posMode;
		private float m_time;
		private float m_timer;

		public void Init(Quaternion destRot, float time)
		{
			m_destRot = destRot;
			m_time = time;
			m_timer = 0;
			m_posMode = false;
		}
		public void Init(Vector3 targetPos, float time)
		{
			m_destPos = targetPos;
			m_time = time;
			m_timer = 0;
			m_posMode = true;
		}
		public void Init(string destObj, float time, int policeId)
		{
			if(destObj == "dest_sheet" || destObj == "dest_sheetDir" || destObj == "dest_sheetEnterPos"){
				destObj = destObj + policeId.ToString();
			}
			m_destPos = GameObject.Find(destObj).transform.position;
			if(m_destPos == null){
				Debug.Log(destObj + "が見つかりませんでした。");
				return;
			}
			m_time = time;
			m_timer = 0;
			m_posMode = true;
		}
		public override void Start(){
			if (m_posMode)
			{
				var dirPos = m_destPos - m_gameObject.transform.position;
				var angle = Mathf.Atan2(dirPos.x, dirPos.z);
				m_destRot = Quaternion.Euler(0, (angle * 180 / Mathf.PI) + 0, 0);
			}
		}

		public override void Resume(){
			var dirPos = m_destPos - m_gameObject.transform.position;
			var angle = Mathf.Atan2(dirPos.x, dirPos.z);
			m_destRot = Quaternion.Euler(0, (angle * 180 / Mathf.PI) + 0, 0);
		}


		public override void Update(){
			m_timer += Time.deltaTime;
			m_timer = (m_timer > m_time)? m_time : m_timer;
			var dir = Quaternion.Lerp(m_gameObject.transform.rotation, m_destRot, m_timer / m_time);
			m_gameObject.transform.rotation = dir;
		}
		public override bool IsEnd(){
			return m_timer >= m_time;
		}
	}
}
