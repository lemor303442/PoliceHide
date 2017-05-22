using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Polices.Behaviors
{
	public class BaseBehaviorController : MonoBehaviour
	{
		private PoliceParams policeParams;

		private Animator _animator;
		private StateMachineObservalbes _stateMachineObservables;

		private string baseAnimIndex = "BaseAnimIndex";
		private bool isAnimStateChanging = true;
		private bool AnimStateFinished = false;
		private float AnimStateNormalized;

		public void Init ()
		{
			policeParams = GetComponent<PoliceParams> ();

			_animator = GetComponent <Animator> ();
			_stateMachineObservables = _animator.GetBehaviour <StateMachineObservalbes> ();

			//
			_stateMachineObservables
				.OnStateEnterObservable
				.Subscribe (x => {
					AnimStateFinished = false;
					isAnimStateChanging = true;
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
				.Where(x => x.normalizedTime > 1)
				.Where(x => !AnimStateFinished)
				.Where(x => !isAnimStateChanging)
				.Subscribe (x => {
					AnimStateFinished = true;
			});	//AnimatorのRestパラメータをTrueにする


		}


		private IEnumerator WaitTillAnimFinish ()
		{
			for (;;) {
				if (AnimStateFinished == true)
					break;
				if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
					yield break;
				yield return null;
			}
		}

		private IEnumerator WalkTo (Vector3 targetPos, float speed)
		{
			for (;;) {
				this.transform.position += transform.forward * speed * Time.deltaTime;
				if (Vector3.Distance (this.transform.position, targetPos) < 0.1f)
					break;
				if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
					yield break;
				yield return null;
			}
		}

		private IEnumerator LookAt (Vector3 targetPos)
		{
			GameObject target = new GameObject ();
			target.transform.position = targetPos;
			GameObject targetLooker = new GameObject ();
			targetLooker.transform.position = this.transform.position;
			targetLooker.transform.LookAt (target.transform);
			Quaternion targetRotation = targetLooker.transform.rotation;
			Quaternion firstRotation = this.transform.rotation;
			float rotateSpeed = 50;
			for (;;) {
				Quaternion currentRotation = Quaternion.RotateTowards (this.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
				this.transform.rotation = currentRotation;
				//Debug.Log("this = " + this.transform.rotation + ", targetRotation = " + targetRotation );
				if ((this.transform.eulerAngles.y - targetRotation.eulerAngles.y)%360 == 0) {
					break;
				}
				if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
					break;
				yield return null;
			}
			Destroy (target);
			Destroy (targetLooker);
		}


		public IEnumerator StandUp ()
		{
			policeParams.policeStatus = PoliceStatus.BASIC_BEHAVIOR_TO;
			//立ち上がる
			_animator.SetInteger (baseAnimIndex, 1);
			//待機
			AnimStateFinished = false;
			yield return StartCoroutine (WaitTillAnimFinish ());
		}

		public IEnumerator FromStandToTarget (Vector3 targetPos)
		{
			//回転アニメーションスタート
			_animator.SetInteger (baseAnimIndex, 2);
			//回転する
			yield return StartCoroutine (LookAt (targetPos));
			//WALKアニメーションスタート
			_animator.SetInteger (baseAnimIndex, 3);
			//歩く
			yield return StartCoroutine (WalkTo (targetPos, 1));
		}

		public IEnumerator StartActionAnimation (Vector3 targetPos, string animName)
		{
			policeParams.policeStatus = PoliceStatus.DURING_BASIC_BEHAVIOR;
			//回転アニメーションスタート
			_animator.SetInteger (baseAnimIndex, 4);
			//回転する
			yield return StartCoroutine (LookAt (targetPos));
			//アニメーション開始
			_animator.SetBool (animName, true);
			//待機
			AnimStateFinished = false;
			yield return StartCoroutine (WaitTillAnimFinish ());
			//アニメーション終了
			_animator.SetBool (animName, false);
		}

		public IEnumerator FromActionToSheet (Vector3 targetPos)
		{
			if(policeParams.policeStatus != PoliceStatus.TO_SHEET)
			policeParams.policeStatus = PoliceStatus.BASIC_BEHAVIOR_FROM;
			//回転アニメーションスタート
			_animator.SetInteger (baseAnimIndex, 4);
			//回転する
			yield return StartCoroutine (LookAt (targetPos));
			//WALKアニメーションスタート
			_animator.SetInteger (baseAnimIndex, 5);
			//歩く
			yield return StartCoroutine (WalkTo (targetPos, 1));
		}

		public IEnumerator SitDown (Vector3 targetPos)
		{
			//回転アニメーションスタート
			_animator.SetInteger (baseAnimIndex, 6);
			//回転する
			yield return StartCoroutine (LookAt (targetPos));
			//座るアニメーションスタート
			_animator.SetInteger (baseAnimIndex, 7);
			//待機
			AnimStateFinished = false;
			yield return StartCoroutine (WaitTillAnimFinish ());
			//workアニメーションスタート
			_animator.SetInteger (baseAnimIndex, 8);
			policeParams.policeStatus = PoliceStatus.IDLE;
		}

		public void SpawnedOutside(){
			policeParams.policeStatus = PoliceStatus.TO_SHEET;
			_animator.SetTrigger("PrefentialAnimFinished");
		}
	}
}