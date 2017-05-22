using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour {
	bool pause = true;
	public GameObject joy_stick;
	public GameObject pause_image;
	public Text score_text;
	int score = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		score_text.text = "Score : " + score.ToString();
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
	public void CheckScore(int id,int count){
		switch (id) {
		case 1:
			score += 1000 * count;
			break;
		}
	}
}
