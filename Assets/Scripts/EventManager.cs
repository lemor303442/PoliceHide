using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices;
using System;

public class EventManager : MonoBehaviour
{

	public List<PoliceActionManager> policeActionManagers = new List<PoliceActionManager> ();
	private PlayerController playerController;

	private bool poopFlg;

	void Update ()
	{
		
	}

	public void EventPoopTrigger (int poopId)
	{
		//一番近くて、暇してるポリスを探す
		//とりあえずデバックでpolcieId=1番のポリスに送ることにする。
		Debug.Log("poop trigger called");

		StartCoroutine (SearchPoliceBasicClosest ((PoliceActionManager pam) => pam.RecieveEvents ("Poop", poopId)));
	}
		
	IEnumerator SearchPoliceBasicClosest (Action<PoliceActionManager> onComplete)
	{
		for (;;) {
			Debug.Log("looking for SearchPoliceBasicClosest");
			List<PoliceActionManager> basicBehaviorPoliceList = new List<PoliceActionManager> ();
			foreach (var item in policeActionManagers) {
				if (item.PoliceParams.policeStatus == PoliceStatus.BASIC_BEHAVIOR)
					basicBehaviorPoliceList.Add (item);
			}
			Debug.Log(basicBehaviorPoliceList.Count);
			//確認する場所が、poopからではなく、playerの座標からになってしまう。poopの座標情報をゲットしたいな。
			if (basicBehaviorPoliceList.Count > 0) {
				float minDistance = 1000;
				int closestPoliceId = 0;
				foreach (var item in basicBehaviorPoliceList) {
					Debug.Log(minDistance);
					float newDistance = Vector3.Distance (item.gameObject.transform.position, playerController.gameObject.transform.position);
					if (minDistance > newDistance) {
						minDistance = newDistance;
						closestPoliceId = item.PoliceParams.policeID;
					}
				}
				foreach (var item in basicBehaviorPoliceList) {
					if (item.PoliceParams.policeID == closestPoliceId) {
						onComplete (item);
						break;
					}
				}
				yield break;
			} else {
				yield return new UnityEngine.WaitForSeconds (0.5f);
			}
		}
	}

	public void SetPoliceActionManager (PoliceActionManager pam)
	{
		policeActionManagers.Add (pam);
	}

	public void SetPlayerController (PlayerController pc)
	{
		playerController = pc;
	}
}
