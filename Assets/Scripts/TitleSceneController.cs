using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour {
	public Text gametitle;
	public Text highscore;
	// Use this for initialization


	void Start (){
		SceneManager.LoadScene("Stage", LoadSceneMode.Additive); 
		
		if (Application.systemLanguage == SystemLanguage.Japanese) {
			gametitle.text = "POLICE\nSimulator";
		} else {
			gametitle.text = "I am Police";
		}
		highscore.text = DataManager.instance.Score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartButton(){
		SceneManager.LoadScene ("Play");
	}

	public void SendSns(){
		SocialConnector.SocialConnector.Share("#ChristmasSimulator", "http:", null);
	}

	public void GameSound(){
		if (AudioListener.volume == 0) {
			AudioListener.volume = 1f;
		} else {
			AudioListener.volume = 0;
		}
	}

}
