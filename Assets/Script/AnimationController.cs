using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
	
	public Animator PlayerAnimator;
	AnimatorStateInfo animInfo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		animInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
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
	}
}
