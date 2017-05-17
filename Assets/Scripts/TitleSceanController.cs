using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceanController : MonoBehaviour {
	public Text gametitle;
	// Use this for initialization
	void Start () {
		if (Application.systemLanguage == SystemLanguage.Japanese) {
			gametitle.text = "警察早くやめてぇー";
		} else {
			gametitle.text = "I am Police";
		}
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
