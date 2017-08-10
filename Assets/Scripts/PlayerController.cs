using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("p")){
			GameObject poop = new GameObject();
			poop.name = "dest_poop";
			poop.transform.position = this.transform.position;
			GameObject.FindObjectOfType<EventManager>().poopFlg = true;
		}
	}
}
