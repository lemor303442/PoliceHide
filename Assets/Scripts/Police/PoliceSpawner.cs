using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices;

public class PoliceSpawner : MonoBehaviour
{
	
	GameObject police;
	Vector3[] policeInstantiatePos = new Vector3[]{ new Vector3 (4, 0, 4), new Vector3 (-4, 0, 4) };
	int maxPoliceCount = 4;
	float policeInstantiateInterval = 1f;

	int policeId = 1;
	bool isInstantiateEnable;
	float timer;

	Command command = new Command();

	void Start ()
	{
		police = Resources.Load ("Prefabs/Police") as GameObject;

		command.Init (() => isInstantiateEnable = true, () => isInstantiateEnable = true);
	}

	void Update ()
	{
		if (isInstantiateEnable) {
			timer += Time.deltaTime;
			if (timer > policeInstantiateInterval) {
				InstantiatePolice ();
				timer = 0;
			}
		}
	}

	void InstantiatePolice ()
	{
		GameObject policeClone = Instantiate (police, policeInstantiatePos [policeId % 2], Quaternion.identity);
		policeClone.GetComponent<PoliceParams> ().policeID = policeId;
		PoliceActionManager pam = policeClone.GetComponent<PoliceActionManager> ();
		pam.BasicCommands = command.BasicCommands;
		pam.OtherCommands = command.OtherCommands;
		pam.PreferencialCommands = command.PreferencialCommands;
		policeId++;
		if (policeId - 1 == maxPoliceCount) {
			isInstantiateEnable = false;
		}
		GameObject.FindObjectOfType<EventManager> ().AddPolice (policeClone.GetComponent<PoliceActionManager> ());
	}
}
