using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class PoliceActionManager : MonoBehaviour
	{
		private List<List<PoliceAction>> m_ActionLists;
		string m_NowAction;
		[SerializeField]
		TextAsset commandCSV;

		void Start()
		{
			m_ActionLists = new List<List<PoliceAction>>();
		}

		public string GetNowAction()
		{
			return m_NowAction;
		}

		private void ThinkNextAction()
		{
			string cmd = commandCSV.ToString();

			var actionList = new List<PoliceAction>();
			var lines = cmd.Split('\n');
			foreach(var cmdLine in lines)
			{
				if (string.IsNullOrEmpty(cmdLine) || cmdLine.StartsWith("//"))
				{
					continue;
				}
				if (cmdLine[0] == ':')
				{
					m_NowAction = lines[0];
					continue;
				}
				string[] cmds = cmdLine.Split(',');
				switch (cmds[0])
				{
				case "PlayAnimation":
					PlayAnimation(actionList, cmds[1], System.Convert.ToBoolean(cmds[2]));
					break;
				case "WalkTo":
					WalkTo(actionList, cmds[1],int.Parse(cmds[2]));
					break;
				case "RotateTo":
					RotateTo(actionList, cmds[1], int.Parse(cmds[2]));
					break;
				case "WaitAction":
					WaitAction(actionList);
					break;
				case "WaitForSeconds":
					WaitForSeconds(actionList, float.Parse(cmds[1]));
					break;
				}
			}
			NextAction(actionList);
		}

		private void NextAction(List<PoliceAction> actionList)
		{
			if (m_ActionLists.Count == 0)
			{
				m_ActionLists.Add(actionList);
			}
			else
			{
				m_ActionLists[0] = actionList;
			}
		}

		private void InterruptAction(List<PoliceAction> actionList)
		{
			if (m_ActionLists.Count != 0)
			{
				var activeAction = m_ActionLists[m_ActionLists.Count - 1];
				foreach (var act in activeAction)
				{
					if (act is WaitAction)
					{
						break;
					}
					act.Suspend();
				}
			}
			m_ActionLists.Add(actionList);
		}

		private void WaitAction(List<PoliceAction> list)
		{
			var act = new WaitAction();
			act.SetGameObject(this.gameObject);
			list.Add(act);
		}

		private void PlayAnimation(List<PoliceAction> list, string anim, bool waitAnimEnd)
		{
			var act = new StartActionAnimation();
			act.SetGameObject(this.gameObject);
			act.Init(anim, waitAnimEnd);
			list.Add(act);
		}

		private void RotateTo(List<PoliceAction> list, Quaternion angle, float speed)
		{
			var act = new RotateTo();
			act.SetGameObject(this.gameObject);
			act.Init(angle, speed);
			list.Add(act);
		}

		private void RotateTo(List<PoliceAction> list, Vector3 destPos, float speed)
		{
			var act = new RotateTo();
			act.SetGameObject(this.gameObject);
			act.Init(destPos, speed);
			list.Add(act);
		}

		private void RotateTo(List<PoliceAction> list, string destObj, float speed)
		{
			var act = new RotateTo();
			act.SetGameObject(this.gameObject);
			act.Init(destObj, speed);
			list.Add(act);
		}

		private void WalkTo(List<PoliceAction> list, Vector3 destPos, float speed)
		{
			var act = new WalkTo();
			act.SetGameObject(this.gameObject);
			act.Init(destPos, speed);
			list.Add(act);
		}

		private void WalkTo(List<PoliceAction> list, string destObj, float speed)
		{
			var act = new WalkTo();
			act.SetGameObject(this.gameObject);
			act.Init(destObj, speed);
			list.Add(act);
		}

		private void WaitForSeconds(List<PoliceAction> list, float waitTime){
			var act = new WaitForSeconds();
			act.SetGameObject(this.gameObject);
			act.Init(waitTime);
			list.Add(act);
		}

		public void Update()
		{
			if (m_ActionLists.Count != 0)
			{
				var activeAction = m_ActionLists[m_ActionLists.Count - 1];
				var isAllEnd = true;
				foreach (var act in activeAction)
				{
					if (act is WaitAction)
					{
						break;
					}
					if (!act.IsStart())
					{
						act.Start();
						act.SetStart();
					}
					act.Update();
					if(!act.IsEnd()){
						isAllEnd = false;
					}
				}
				if (isAllEnd)
				{
					while (activeAction.Count > 0 && !(activeAction[0] is WaitAction))
					{
						activeAction.RemoveAt(0);
					}
					activeAction.RemoveAt(0);
					if (activeAction.Count == 0)
					{
						if (m_ActionLists.Count != 0)
						{
							m_ActionLists.Remove(activeAction);
							activeAction = m_ActionLists[m_ActionLists.Count - 1];
							foreach (var act in activeAction)
							{
								if (act is WaitAction)
								{
									break;
								}
								act.Resume();
							}
						}
					}
				}
			}
			if (m_ActionLists.Count == 0)
			{
				ThinkNextAction();
			}
		}
	}
}