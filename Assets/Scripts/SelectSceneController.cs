using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSceneController : MonoBehaviour {
	public Animator _animator;
	int[] scoreArray = new int[] {0, 0, 0, 0, 0};
	int[] animationArray = new int[] {100, 200, 300, 400, 500};
	int animation_id;
	public Text point_text;
	public int point;
	public GameObject BuyButtuon;
	// Use this for initialization
	void Start () {
		point = PlayerPrefs.GetInt("point", 0);
		point_text.text = point.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void BackPlayScene(){
		SceneManager.LoadScene ("Play");
	}
	public void OnClick(int id){
		scoreArray = PlayerPrefsX.GetIntArray("score");      //配列データの読み込み
		//まだ解禁されていなかったらボタン表示
		if (scoreArray [id] == 0) {
			BuyButtuon.SetActive (true);
			animation_id = id;
		} else {
			BuyButtuon.SetActive (false);
		}

		switch (id) {
		case 0:
			_animator.SetBool ("Dance", true);
			break;
		case 1:
			_animator.SetBool ("JoyJump", true);
			break;
		case 2:
			_animator.SetBool ("can_can", true);
			break;
		case 3:
			_animator.SetBool ("FallFlat", true);
			break;
		}
	}

	public void BuyAnimation(){//購入ボタン
		if (point > animationArray [animation_id]) {
			point -= animationArray [animation_id];
			PlayerPrefs.SetInt("point", point);
			scoreArray[animation_id] = 1;
			PlayerPrefsX.SetIntArray("score",scoreArray); //配列データの保存
			point_text.text = point.ToString();
			BuyButtuon.SetActive (false);
			Debug.Log (point);
		} else {
			Debug.Log ("金がない");
		}

	}
}
