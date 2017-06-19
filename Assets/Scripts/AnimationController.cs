using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour {
	private Animator _animator;
	AnimatorStateInfo animInfo;
	private StateMachineObservalbes _stateMachineObservables;
	[SerializeField]
	private bool isAnimStateChanging = true;
	[SerializeField]
	private bool AnimStateFinished = false;
	private bool isButtonPressed = false;
	[SerializeField]
	private PlaySceneController playSceneController;
	float preNormalizedTime;
	int count = 1;
	int NowAnimation = 0;
	public GameObject slider_canvas;
	public Slider slider;
	public GameObject main_camera;

	void Start(){
		_animator = GetComponent <Animator> ();
		_stateMachineObservables = _animator.GetBehaviour <StateMachineObservalbes> ();

		//start
		_stateMachineObservables
			.OnStateEnterObservable
			.Subscribe (x => {
				AnimStateFinished = false;
				isAnimStateChanging = true;
				Debug.Log(x.fullPathHash);
				Debug.Log("開始");
			});

		//animationState変更時にAnimStateFinishedをfalseに変更する
		_stateMachineObservables
			.OnStateExitObservable
			.Subscribe (x => {
				AnimStateFinished = false;
				isAnimStateChanging = false;
			});

		//normalizedTimeの値を監視、1を超えたらAnimStteFinishedをtrueに変更する
		_stateMachineObservables
			.OnStateUpdateObservable 
			.Where(x => x.normalizedTime >= 1)
			.Where(x => !AnimStateFinished)
			.Where(x => !isAnimStateChanging)
			.Subscribe (x => {
				AnimStateFinished = true;
			});	//AnimatorのRestパラメータをTrueにする

		_stateMachineObservables
			.OnStateUpdateObservable
			.Subscribe (x => {
				if (Mathf.FloorToInt (x.normalizedTime) - Mathf.FloorToInt (preNormalizedTime) == 1 && x.fullPathHash != -648720422) {
				//整数値が増えた時の処理
					count++;
					//playSceneController.CheckScore(x.fullPathHash.ToString(),count);
			}
			preNormalizedTime = x.normalizedTime;
		});
		
	}
	void Update(){
		slider_canvas.transform.LookAt(main_camera.transform);
		animInfo = _animator.GetCurrentAnimatorStateInfo(0);
		if (animInfo.nameHash != Animator.StringToHash ("Base Layer.Grounded")) {
			slider.value = animInfo.normalizedTime - count;
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Eyesite") {
			if (animInfo.nameHash != Animator.StringToHash ("Base Layer.Grounded")) {
				_animator.SetTrigger ("pray");
				slider.enabled = false;
				//playSceneController.GameOver ();
			}
		}
	}

	public void ButtonClick(int id){
		count = 0;
		isButtonPressed = !isButtonPressed;
		NowAnimation = id;
		//どのボタンが押されたかを判断
		switch (NowAnimation) {
		case 1:
			_animator.SetBool ("Dance", true);
			break;
		case 2:
			_animator.SetBool ("JoyJump", true);
			break;
		case 3:
			_animator.SetBool ("HeadSpin", true);
			break;
		case 4:
			_animator.SetBool ("FallFlat", true);
			break;
		}
	}

	public void ButtonClickUp(int id){
		switch (NowAnimation) {
		case 1:
			_animator.SetBool ("Dance", false);
			break;
		case 2:
			_animator.SetBool ("JoyJump", false);
			break;
		case 3:
			_animator.SetBool ("HeadSpin", false);
			break;
		case 4:
			_animator.SetBool ("FallFlat", false);
			break;
		}
		playSceneController.CheckScore(NowAnimation,count);
		count = 0;
		NowAnimation = 0;
		//hashと何回連続で行ったか
	}
}
