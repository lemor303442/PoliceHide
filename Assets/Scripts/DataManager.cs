using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
	public static DataManager instance;
	private int score = 0;
	public int Score{
		get{return score;}
		set{
			score = value;
		}
	}

	void Awake ()
	{
		if (instance == null) {

			instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		else {

			Destroy (this.gameObject);
		}

	}
}
