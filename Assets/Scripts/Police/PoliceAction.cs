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

		public void SetGameObject (GameObject obj)
		{
			m_gameObject = obj;
		}

		public abstract void Update ();

		public abstract bool IsEnd ();
	}
}
