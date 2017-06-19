using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	[SerializeField]
	Vector3 cameraPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.position = new Vector3 (player.transform.position.x, cameraPos.y, cameraPos.z);
		//this.transform.LookAt (player.transform, this.transform.forward);
		//this.transform.LookAt(player.transform);
		//this.transform.position += Vector3.forward * Time.deltaTime;
	}
}
