using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour {
	public GameObject player;
	[SerializeField]
	Vector3 cameraPos;
	public bool checker = true;
	float dist ;
	public Text gameover;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (checker) {
			this.gameObject.transform.position = new Vector3 (player.transform.position.x, cameraPos.y, cameraPos.z);
		}else{
			dist = Vector3.Distance(player.transform.position, this.transform.position);
			if(dist>3.5f){
				this.transform.LookAt(player.transform);
				this.transform.position += Vector3.forward * Time.deltaTime;
				gameover.enabled = true;
			}
		}
	}
}
