using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityStandardAssets.CinematicEffects;

public class AnimationController : MonoBehaviour
{
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
	int count;
	int NowAnimation = 0;
	public GameObject slider_canvas;
	public Slider slider;
	public GameObject main_camera;
	bool isPlaying = true;

	void Start ()
	{
		_animator = GetComponent <Animator> ();
		_stateMachineObservables = _animator.GetBehaviour <StateMachineObservalbes> ();

		//start
		_stateMachineObservables
			.OnStateEnterObservable
			.Subscribe (x => {
			AnimStateFinished = false;
			isAnimStateChanging = true;
			Debug.Log (x.fullPathHash);
			Debug.Log ("開始");
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
			.Where (x => x.normalizedTime >= 1)
			.Where (x => !AnimStateFinished)
			.Where (x => !isAnimStateChanging)
			.Subscribe (x => {
			AnimStateFinished = true;
		});	//AnimatorのRestパラメータをTrueにする

		_stateMachineObservables
			.OnStateUpdateObservable
			.Subscribe (x => {
			if (Mathf.FloorToInt (x.normalizedTime) - Mathf.FloorToInt (preNormalizedTime) == 1 && x.fullPathHash != -648720422) {
				//整数値が増えた時の処理
				count++;
				// poopのanimation終了場合
				if (x.normalizedTime - preNormalizedTime < 1 && animInfo.fullPathHash == -1220629825) {
					this.GetComponent<PlayerController> ().InstantiatePoop ();
				}
			}	
			preNormalizedTime = x.normalizedTime;
		});
		
	}

	void Update ()
	{
		slider_canvas.transform.LookAt (main_camera.transform);
		animInfo = _animator.GetCurrentAnimatorStateInfo (0);
		if (animInfo.nameHash != Animator.StringToHash ("Base Layer.Grounded")) {
			slider.value = animInfo.normalizedTime - count;
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Eyesite") {
			if (animInfo.nameHash != Animator.StringToHash ("Base Layer.Grounded")) {
				isPlaying = false;
				slider.gameObject.SetActive (false);
				main_camera.GetComponent<TonemappingColorGrading> ().enabled = true;
				main_camera.GetComponent<CameraController> ().checker = false;
				Time.timeScale = 0.5f;
				_animator.SetTrigger ("pray");
				playSceneController.GameOver ();
			}
		}
	}

	public void ButtonClick (int id)
	{
		count = 0;
		isButtonPressed = !isButtonPressed;
		NowAnimation = id;
		slider_canvas.SetActive (true);
		//どのボタンが押されたかを判断
		switch (NowAnimation) {
		case 1:
			_animator.SetBool ("Dance", true);
			break;
		case 2:
			_animator.SetBool ("JoyJump", true);
			break;
		case 3:
			_animator.SetBool ("can_can", true);
			break;
		case 4:
			_animator.SetBool ("FallFlat", true);
			break;
		case 5:
			_animator.SetBool ("Poop", true);
			break;
		}
	}

	public void ButtonClickUp (int id)
	{
		if (isPlaying) {
			switch (NowAnimation) {
			case 1:
				_animator.SetBool ("Dance", false);
				break;
			case 2:
				_animator.SetBool ("JoyJump", false);
				break;
			case 3:
				_animator.SetBool ("can_can", false);
				break;
			case 4:
				_animator.SetBool ("FallFlat", false);
				break;
			case 5:
				_animator.SetBool ("Poop", false);
				break;
			}
		}
		slider_canvas.SetActive (false);
		playSceneController.CheckScore (NowAnimation, count);
		count = 0;
		NowAnimation = 0;
		//hashと何回連続で行ったか
	}
}
