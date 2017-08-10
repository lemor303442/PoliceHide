using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices;

public class EventManager : MonoBehaviour {

	public List<PoliceActionManager> policeActionManagers = new List<PoliceActionManager>();

	[SerializeField]
	public bool poopFlg;

	void Update(){
		if(poopFlg){
			SendEventToPolices("Poop");
			poopFlg = false;
		}
	}

	void SendEventToPolices(string name){
		foreach(var item in policeActionManagers){
			item.RecieveEvents(name);
		}
	}

	public void AddPolice(PoliceActionManager pam){
		policeActionManagers.Add(pam);
	}
}
