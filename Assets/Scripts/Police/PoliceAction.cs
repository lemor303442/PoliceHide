using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public abstract class PoliceAction
	{
		protected bool m_isStart;
		protected int m_status;
		protected PoliceActionManager m_manager;
		protected GameObject m_gameObject;

		public void SetGameObject (GameObject obj)
		{
			m_gameObject = obj;
		}

		public bool IsStart()
		{
			return m_isStart;
		}
		public void SetStart()
		{
			m_isStart = true;
		}
		public virtual void Start () { }
		public virtual void Resume () { }
		public virtual void Suspend () { }

		public abstract void Update ();

		public abstract bool IsEnd ();
	}
}
