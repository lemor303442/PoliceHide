using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySceanController : MonoBehaviour {
	bool pause = true;
	public GameObject joy_stick;
	public GameObject pause_image;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PauseButton(){
		if (pause) {
			pause = false;
			pause_image.SetActive (true);
			joy_stick.SetActive (false);
			Time.timeScale = 0;
		} else {
			pause = true;
			Time.timeScale = 1;
			pause_image.SetActive (false);
			joy_stick.SetActive (true);
		}
	}

	public void GameOver(){
		
	}
	public void Retry(){
		SceneManager.LoadScene ("Play");
	}
	public void Home(){
		SceneManager.LoadScene ("Title");
	}
}
