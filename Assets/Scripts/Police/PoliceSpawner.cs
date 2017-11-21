using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices;

public class PoliceSpawner : MonoBehaviour
{

	GameObject police;
	Vector3[] policeInstantiatePos = new Vector3[]{new Vector3(4, 0, 4), new Vector3(-4, 0, 4)};
	int maxPoliceCount = 4;
	float policeInstantiateInterval = 1f;

	int policeId = 1;
	bool isInstantiateEnable;
	float timer;

	[SerializeField]
	private string basicCommands;
	[SerializeField]
	private string otherCommands;
	[SerializeField]
	private string preferencialCommands;


	void Start ()
	{
		police = Resources.Load ("Prefabs/Police") as GameObject;
		StartCoroutine(LoadScenarioFromServer());
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
		PoliceActionManager pam = policeClone.GetComponent<PoliceActionManager>();
		pam.BasicCommands = basicCommands;
		pam.OtherCommands = otherCommands;
		pam.PreferencialCommands = preferencialCommands;
		policeId++;
		if (policeId - 1 == maxPoliceCount) {
			isInstantiateEnable = false;
		}
		GameObject.FindObjectOfType<EventManager>().AddPolice(policeClone.GetComponent<PoliceActionManager>());
	}

	IEnumerator LoadScenarioFromServer ()
	{
		WWW request = new WWW ("https://docs.google.com/spreadsheets/d/1XArPYnCr47dGCuKDx-WHJQ3DBFO6pmw9i7qMwrIZclA/export?format=csv&gid=1564450559");
		yield return request;
		if (!string.IsNullOrEmpty (request.error)) {
			Debug.LogWarning ("エラー:" + request.error);
		} else {
			basicCommands = request.text;
		}

		request = new WWW ("https://docs.google.com/spreadsheets/d/1XArPYnCr47dGCuKDx-WHJQ3DBFO6pmw9i7qMwrIZclA/export?format=csv&gid=859044375");
		yield return request;
		if (!string.IsNullOrEmpty (request.error)) {
			Debug.LogWarning ("エラー:" + request.error);
		} else {
			otherCommands = request.text;
		}

		request = new WWW ("https://docs.google.com/spreadsheets/d/1XArPYnCr47dGCuKDx-WHJQ3DBFO6pmw9i7qMwrIZclA/export?format=csv&gid=1589268886");
		yield return request;
		if (!string.IsNullOrEmpty (request.error)) {
			Debug.LogWarning ("エラー:" + request.error);
		} else {
			preferencialCommands = request.text;
		}
		isInstantiateEnable = true;	
	}
}
