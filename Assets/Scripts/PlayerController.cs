using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	int poopId = 1;



	
	void Update ()
	{
		if (Input.GetKeyDown ("p")) {
			InstantiatePoop ();
		}
	}

	public void InstantiatePoop ()
	{
		GameObject obj = Instantiate(Resources.Load("Prefabs/Poop")) as GameObject;
		obj.name = "dest_poop" + poopId.ToString ();
		obj.transform.position = this.transform.position;
		GameObject.FindObjectOfType<EventManager> ().EventPoopTrigger(poopId);
		poopId++;
	}
}
