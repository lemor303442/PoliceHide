using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class PoliceActionManager : MonoBehaviour
	{

		private List<PoliceAction> m_ActionList;

		public void Update()
		{
			PoliceAction act;
			switch (actNo)
			{
			case 0:
				act = new WalkTo();
				break;
			case 1:
				//act = new OpenDoor();
				break;
			default:
				act = null;
				break;
			}
			act.SetGameObject(this.gameObject);
			m_ActionList.Add(act);

			m_ActionList[0].Update();
			if (m_ActionList[0].IsEnd())
			{
				m_ActionList.RemoveAt(0);
			}
		}
	}
}