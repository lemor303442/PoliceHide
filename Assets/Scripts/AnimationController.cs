using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AnimationController : MonoBehaviour {
	private Animator _animator;
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

	public void ButtonClick(int id){
		count = 0;
		isButtonPressed = !isButtonPressed;
		//どのボタンが押されたかを判断
		_animator.SetBool("Dance",true);
	}

	public void ButtonClickUp(int id){
		_animator.SetBool("Dance",false);
		playSceneController.CheckScore(id,count);
		count = 0;
		//hashと何回連続で行ったか
	}
}
