using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices;

public class EventManager : MonoBehaviour {

	public List<PoliceActionManager> policeActionManagers = new List<PoliceActionManager>();

	[SerializeField]
//	public bool poopFlg;

	void Update(){
		
	}

	private void SendEventToPolices(string name){
		foreach(var item in policeActionManagers){
			item.RecieveEvents(name);
		}
	}

	public void EventPoopTrigger(int id){
		//一番近くて、暇してるポリスを探す
		//とりあえずデバックでpolcieId=1番のポリスに送ることにする。
		GameObject.Find("Police(Clone)").GetComponent<PoliceActionManager>().RecieveEvents("Poop", id);
	}



	public void AddPolice(PoliceActionManager pam){
		policeActionManagers.Add(pam);
	}
}
