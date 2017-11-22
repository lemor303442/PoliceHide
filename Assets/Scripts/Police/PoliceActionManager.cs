using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public class PoliceActionManager : MonoBehaviour
	{
		//自分の情報
		PoliceParams policeParams;
		private List<List<PoliceAction>> m_ActionLists;
		public string m_nowAction;
		public string m_beforeAction;

		private string basicCommands;

		public string BasicCommands{ set { basicCommands = value; } }

		private string otherCommands;

		public string OtherCommands{ set { otherCommands = value; } }

		private string preferencialCommands;

		public string PreferencialCommands{ set { preferencialCommands = value; } }

		[SerializeField]
		List<string> basicBehaviors;

		//その他オブジェクトの情報
		EventManager eventManager;


		void Start ()
		{
			policeParams = GetComponent<PoliceParams> ();
			eventManager = GameObject.FindObjectOfType<EventManager> ();
			m_ActionLists = new List<List<PoliceAction>> ();
			GetBasicBehaviors ();

			CleanLog();
			OutputLog("Played Date:" + System.DateTime.Now.ToString ());
			OutputLog ("[Start()] => Instantiated");
			SetPoliceStatus (PoliceStatus.BASIC_BEHAVIOR);
			m_beforeAction = "Work";
			SetNextAction ("InstantiatedToMyDesk", 0);
		}

		public string GetNowAction ()
		{
			return m_nowAction;
		}

		private void GetBasicBehaviors ()
		{
			string cmd = basicCommands;

			var lines = cmd.Split ('\n');
			foreach (var cmdLine in lines) {
				string[] cmds = cmdLine.Split (',');
				if (cmds [0] == ":") {
					basicBehaviors.Add (cmds [1]);
					continue;
				}
			}
		}

		public void RecieveEvents (string name)
		{
			if (name == "Poop") {
				SetNextAction ("CleanPoop", 1);
			}
		}

		private void ThinkNextAction ()
		{
			string actionName;
			if (m_nowAction == "Work") {
				actionName = basicBehaviors [Random.Range (0, basicBehaviors.Count)];
				//もし現在このアクションをしているPoliceがいた場合、"Work"をする
				//イベントマネジャーの関数に移行予定
				foreach (var item in eventManager.policeActionManagers) {
					if (item.policeParams.policeStatus == PoliceStatus.BASIC_BEHAVIOR) {
						if (item.m_nowAction == actionName) {
							actionName = "Work";
							break;
						}
					} else if (item.policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR) {
						if (item.m_beforeAction == actionName) {
							actionName = "Work";
							break;
						}
					}
				}
			} else {
				actionName = "Work";
			}

			SetPoliceStatus (PoliceStatus.BASIC_BEHAVIOR);
			string log = "[ThinkNextAction()] action decided => " + actionName;
			OutputLog (log);
			SetNextAction (actionName, 0);
		}

		/// <summary>
		/// Sets the next action.(Action type -- 1:BasicAction, 2:PreferencialAction)
		/// </summary>
		private void SetNextAction (string actionName, int actionType)
		{
			string cmd = basicCommands + otherCommands + preferencialCommands;

			var actionList = new List<PoliceAction> ();
			var lines = cmd.Split ('\n');
			bool actionFound = false;

			foreach (var cmdLine in lines) {
				string[] cmds = cmdLine.Split (',');
				if (!actionFound) {
					if (string.IsNullOrEmpty (cmdLine) || cmds [0] != ":") {
						continue;
					} else {
						if (cmds [1] == actionName) {
							m_beforeAction = m_nowAction;
							m_nowAction = actionName;
							actionFound = true;
							OutputLog ("[SetNextAction()] => " + actionName);
							continue;
						}
					}
				} else {
					if (cmds [0] == "::") {
						break;
					}
					switch (cmds [0]) {
					case "PlayAnimation":
						PlayAnimation (actionList, cmds [1], System.Boolean.Parse (cmds [2]));
						OutputLog ("[SetNextAction()] => new action added to list : PlayAnimation, " + cmds [1] + ", " + System.Boolean.Parse (cmds [2]));
						break;
					case "WalkTo":
						WalkTo (actionList, cmds [1], int.Parse (cmds [2]), policeParams.policeID);
						OutputLog ("[SetNextAction()] => new action added to list : WalkTo, " + cmds [1] + ", " + int.Parse (cmds [2]) + policeParams.policeID.ToString ());
						break;
					case "RotateTo":
						RotateTo (actionList, cmds [1], float.Parse (cmds [2]), policeParams.policeID);
						OutputLog ("[SetNextAction()] => new action added to list : RotateTo, " + cmds [1] + ", " + float.Parse (cmds [2]) + policeParams.policeID.ToString ());
						break;
					case "WaitAction":
						WaitAction (actionList);
						OutputLog ("[SetNextAction()] => new action added to list : WaitAction");
						break;
					case "WaitForSeconds":
						WaitForSeconds (actionList, float.Parse (cmds [1]));
						OutputLog ("[SetNextAction()] => new action added to list : WaitForSeconds, " + float.Parse (cmds [1]));
						break;
					}
				}
			}
			if (actionType == 0) {
				NextAction (actionList);
			} else if (actionType == 1) {
				InterruptAction (actionList);
			}
		}

		private void NextAction (List<PoliceAction> actionList)
		{
			OutputLog ("[NextAction()] => m_ActionLists.Count = " + m_ActionLists.Count.ToString ());
			string currentListData = "";
			for (int i = 0; i < m_ActionLists.Count; i++) {
				for (int j = 0; j < m_ActionLists [i].Count; j++) {
					currentListData += m_ActionLists [i] [j].ToString () + "\n";
				}
			}
			OutputLog ("[NextAction()] => currentListData = \n" + currentListData);
			if (m_ActionLists.Count == 0) {
				m_ActionLists.Add (actionList);
			} else {
				m_ActionLists [0] = actionList;
			}
			currentListData = "";
			for (int i = 0; i < m_ActionLists.Count; i++) {
				for (int j = 0; j < m_ActionLists [i].Count; j++) {
					currentListData += m_ActionLists [i] [j].ToString () + "\n";
				}
			}
			OutputLog ("[NextAction()] => addedListData = \n" + currentListData);
		}

		private void InterruptAction (List<PoliceAction> actionList)
		{
			OutputLog ("[InterruptAction()] => m_ActionLists.Count = " + m_ActionLists.Count.ToString ());
			string currentListData = "";
			for (int i = 0; i < m_ActionLists.Count; i++) {
				for (int j = 0; j < m_ActionLists [i].Count; j++) {
					currentListData += m_ActionLists [i] [j].ToString () + "\n";
				}
			}
			OutputLog ("[InterruptAction()] => currentListData = \n" + currentListData);
			if (m_ActionLists.Count != 0) {
				var activeAction = m_ActionLists [m_ActionLists.Count - 1];
				foreach (var act in activeAction) {
					if (act is WaitAction) {
						break;
					}
					act.Suspend ();
				}
			}
			SetPoliceStatus (PoliceStatus.PREFERENCIAL_BEHAVIOR);
			m_ActionLists.Add (actionList);
			currentListData = "";
			for (int i = 0; i < m_ActionLists.Count; i++) {
				for (int j = 0; j < m_ActionLists [i].Count; j++) {
					currentListData += m_ActionLists [i] [j].ToString () + "\n";
				}
			}
			OutputLog ("[InterruptAction()] => addedListData = \n" + currentListData);
		}

		public void Update ()
		{
			if (m_ActionLists.Count != 0) {
				var activeAction = m_ActionLists [m_ActionLists.Count - 1];
				var isAllEnd = true;
				// waitActionまでのactionを行う
				// waitActionまでの処理がすべて終わったら、次のif文へGO
				foreach (var act in activeAction) {
					if (act is WaitAction) {
						break;
					}
					if (!act.IsStart ()) {
						act.Start ();
						act.SetStart ();
					}
					act.Update ();
					if (!act.IsEnd ()) {
						isAllEnd = false;
					}
				}
				if (isAllEnd) {
					// waitActionまでのactionを削除
					while (activeAction.Count > 0 && !(activeAction [0] is WaitAction)) {
						activeAction.RemoveAt (0);
					}
					// waitActionを削除
					activeAction.RemoveAt (0);
					OutputLog ("[Update()] => currentActiveAction.Count = " + activeAction.Count.ToString ());
					string currentActiveAction = "";
					for (int i = 0; i < activeAction.Count; i++) {
						currentActiveAction += activeAction [i].ToString () + "\n";
					}
					OutputLog ("[Update()] => currentActiveAction = \n" + currentActiveAction);
					// 現在のBehaviorがすべて処理終わった場合（終わってない場合は、また上に戻る）
					if (activeAction.Count == 0) {
						if (m_ActionLists.Count != 0) {
							OutputLog ("[Update()] => a behavior finished = \n" + currentActiveAction);
							//現在のコマンドを削除
							m_ActionLists.Remove (activeAction);

							OutputLog ("[NextAction()] => m_ActionLists.Count = " + m_ActionLists.Count.ToString ());
							string currentListData = "";
							for (int i = 0; i < m_ActionLists.Count; i++) {
								for (int j = 0; j < m_ActionLists [i].Count; j++) {
									currentListData += m_ActionLists [i] [j].ToString () + "\n";
								}
							}
							OutputLog ("[NextAction()] => currentListData = \n" + currentListData);
							//コマンドリストにまだコマンドが残っていたら、次のコマンドをセット
							if (m_ActionLists.Count > 0) {
								activeAction = m_ActionLists [m_ActionLists.Count - 1];
							}
							currentActiveAction = "";
							for (int i = 0; i < activeAction.Count; i++) {
								currentActiveAction += activeAction [i].ToString () + "\n";
							}
							OutputLog ("[Update()] => ResumeActions = \n" + currentActiveAction);
							foreach (var act in activeAction) {
								if (act is WaitAction) {
									break;
								}
								act.Resume ();
							}
							// まったく不明
							// 再開あるactionがない場合かな？
//							if (activeAction.Count == 0) {
								
							if (m_beforeAction == "Work") {
								OutputLog ("[Update()] => No action to resume. Set back to my sheet action.");
								SetPoliceStatus (PoliceStatus.BASIC_BEHAVIOR);
								//自分のシートに帰るアクションを追加する
								SetNextAction ("BackToMySheet", 0);
								string str = m_nowAction;
								m_nowAction = m_beforeAction;
								m_beforeAction = str;
							}

//							}
						}
					}
				}
			} else {
				ThinkNextAction ();
			}
		}


//		List<string[]> logList = new List<string[]> ();

		private void OutputLog (string log)
		{
			string path = "Logs/PoliceLog" + policeParams.policeID.ToString () + ".csv";
			string currentTime = "<" + Time.time.ToString("f3") + ">   ";
			LogManager.SaveText (path, currentTime + log, true);
		}

		private void CleanLog(){
			string path = "Logs/PoliceLog" + policeParams.policeID.ToString () + ".csv";
			LogManager.SaveText (path, "", false);
		}


		private void SetPoliceStatus (PoliceStatus _policeStatus)
		{
			policeParams.policeStatus = _policeStatus;
			string log = "[SetPoliceStatus()] policeStatus changed => " + _policeStatus.ToString ();
		}


		private void WaitAction (List<PoliceAction> list)
		{
			var act = new WaitAction ();
			act.SetGameObject (this.gameObject);
			list.Add (act);
		}

		private void PlayAnimation (List<PoliceAction> list, string anim, bool repeat)
		{
			var act = new StartActionAnimation ();
			act.SetGameObject (this.gameObject);
			act.Init (anim, repeat);
			list.Add (act);
		}

		private void RotateTo (List<PoliceAction> list, Quaternion angle, float speed)
		{
			var act = new RotateTo ();
			act.SetGameObject (this.gameObject);
			act.Init (angle, speed);
			list.Add (act);
		}

		private void RotateTo (List<PoliceAction> list, Vector3 destPos, float speed)
		{
			var act = new RotateTo ();
			act.SetGameObject (this.gameObject);
			act.Init (destPos, speed);
			list.Add (act);
		}

		private void RotateTo (List<PoliceAction> list, string destObj, float speed, int policeId)
		{
			var act = new RotateTo ();
			act.SetGameObject (this.gameObject);
			act.Init (destObj, speed, policeId);
			list.Add (act);
		}

		private void WalkTo (List<PoliceAction> list, Vector3 destPos, float speed)
		{
			var act = new WalkTo ();
			act.SetGameObject (this.gameObject);
			act.Init (destPos, speed);
			list.Add (act);
		}

		private void WalkTo (List<PoliceAction> list, string destObj, float speed, int policeId)
		{
			var act = new WalkTo ();
			act.SetGameObject (this.gameObject);
			act.Init (destObj, speed, policeId);
			list.Add (act);
		}

		private void WaitForSeconds (List<PoliceAction> list, float waitTime)
		{
			var act = new WaitForSeconds ();
			act.SetGameObject (this.gameObject);
			act.Init (waitTime);
			list.Add (act);
		}


	}
}