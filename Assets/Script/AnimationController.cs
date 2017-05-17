using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
	
	public Animator PlayerAnimator;
	AnimatorStateInfo animInfo;
	int animation_play;
	int score = 0;

	// Use this for initialization
	void Start () {
		animation_play = Animator.StringToHash ("Base Layer.Grounded");
	}
	
	// Update is called once per frame
	void Update () {
		animInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
		if (animation_play != animInfo.nameHash) {
			score += 1;
			animation_play = animInfo.nameHash;
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Eyesite") {
			if (animInfo.nameHash != Animator.StringToHash ("Base Layer.Grounded")) {
				Debug.Log ("GameOver");
			}
		}
	}
	public void DanceAnimation(){
		PlayerAnimator.SetTrigger ("Dance");
		animation_play = animInfo.nameHash;
	}
}
