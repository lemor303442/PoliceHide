using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class PoliceActionManager : MonoBehaviour
	{

		private List<PoliceAction> m_ActionList;
		[SerializeField]
		TextAsset commandCSV;

		void Start()
		{
			m_ActionList = new List<PoliceAction>();

			//PlayAnimation,walk
			//RotateTo,-3.71, 0, 4.417374, 3
			//WalkTo,-3.71, 0, 4.417374, 5;
			//WaitAction
			//ThinkNextAction

			string cmd = commandCSV.ToString();
			//string cmd = "PlayAnimation,walk,false\nRotateTo,-3.71, 0, 4.417374, 3\nWalkTo,-3.71, 0, 4.417374, 1\nWaitAction\nRotateTo,4.75, 0, 4.417374, 3\nWalkTo,4.75, 0, 4.417374, 1\nWaitAction";

			foreach(var cmdLine in cmd.Split('\n'))
			{
				string[] cmds = cmdLine.Split(',');
				switch (cmds[0])
				{
				case "PlayAnimation":
					PlayAnimation(cmds[1], System.Convert.ToBoolean(cmds[2]));
					break;
				case "WalkTo":
					WalkTo(new Vector3(float.Parse(cmds[1]), float.Parse(cmds[2]), float.Parse(cmds[3])), int.Parse(cmds[4]));
					break;
				case "RotateTo":
					RotateTo(new Vector3(float.Parse(cmds[1]), float.Parse(cmds[2]), float.Parse(cmds[3])), int.Parse(cmds[4]));
					break;
				case "WaitAction":
					WaitAction();
					break;
				case "WaitForSeconds":
					WaitForSeconds(float.Parse(cmds[1]));
					break;
				case "GoToDesk":
					RotateTo(new Vector3(-3.71f, 0, 4.417374f), 3);
					WalkTo(new Vector3(-3.71f, 0, 4.417374f), 1);
					WaitAction();
					break;
				}
			}
		}

		private void NextAction(int actNo)
		{
			PoliceAction act;
			switch (actNo)
			{
			case 0:
				act = new WalkTo();
				break;
			case 1:
				act = null;
				break;
			default:
				act = null;
				break;
			}
			act.SetGameObject(this.gameObject);
			m_ActionList.Add(act);
		}

		private void WaitAction()
		{
			var act = new WaitAction();
			act.SetGameObject(this.gameObject);
			m_ActionList.Add(act);
		}

		private void PlayAnimation(string anim, bool waitAnimEnd)
		{
			var act = new StartActionAnimation();
			act.SetGameObject(this.gameObject);
			act.Init(anim, waitAnimEnd);
			m_ActionList.Add(act);
		}

		private void RotateTo(Quaternion angle, float speed)
		{
			var act = new RotateTo();
			act.SetGameObject(this.gameObject);
			act.Init(angle, speed);
			m_ActionList.Add(act);
		}

		private void RotateTo(Vector3 destPos, float speed)
		{
			var act = new RotateTo();
			act.SetGameObject(this.gameObject);
			act.Init(destPos, speed);
			m_ActionList.Add(act);
		}

		private void WalkTo(Vector3 destPos, float speed)
		{
			var act = new WalkTo();
			act.SetGameObject(this.gameObject);
			act.Init(destPos, speed);
			m_ActionList.Add(act);
		}

		private void WaitForSeconds(float waitTime){
			var act = new WaitForSeconds();
			act.SetGameObject(this.gameObject);
			act.Init(waitTime);
			m_ActionList.Add(act);
		}

		public void Update()
		{
			if (m_ActionList.Count == 0)
			{
				return;
			}

			var isAllEnd = true;
			foreach (var act in m_ActionList)
			{
				if (act is WaitAction)
				{
					break;
				}
				act.Update();
				if(!act.IsEnd()){
					isAllEnd = false;
				}
			}
			if (isAllEnd)
			{
				if (!(m_ActionList[0] is WaitAction))
				{
					m_ActionList.RemoveAt(0);
				}
				m_ActionList.RemoveAt(0);
			}
		}
	}
}