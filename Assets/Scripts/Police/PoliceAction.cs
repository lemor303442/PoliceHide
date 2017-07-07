using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public abstract class PoliceAction
	{
		protected int m_status;
		protected PoliceActionManager m_manager;
		protected GameObject m_gameObject;
		protected PlayerAnimation m_anim;

		public void SetGameObject(GameObject obj)
		{
			m_gameObject = obj;
		}
		public abstract void Update();
		public abstract bool IsEnd();
		public void PlayAnimation()
		{
			if (m_anim == null)
			{
				m_anim = new PlayerAnimation();
				m_anim.Init(m_gameObject);
			}
			m_anim.PlayAnim(0);
		}
		public bool IsAnimationEnd()
		{
			return (m_anim == null)? true : m_anim.IsAnimEnd();
		}
	}
}
