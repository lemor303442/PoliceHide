using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Polices.Behaviors
{
	public class PreferentialBehaviorController : MonoBehaviour
	{
		private PoliceParams policeParams;

		private Animator _animator;
		private StateMachineObservalbes _stateMachineObservables;

		private string baseAnimIndex = "BaseAnimIndex";
		private string poopAnimIndex = "PoopAnimIndex";

		public bool isAnimStateChanging = true;
		public bool AnimStateFinished = false;
		private float AnimStateNormalized;
		private float preNormalizedTime;

		void Start ()
		{
			policeParams = GetComponent<PoliceParams>();

			_animator = GetComponent <Animator> ();
			_stateMachineObservables = _animator.GetBehaviour <StateMachineObservalbes> ();

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
					//				Debug.LogWarning ("AnimStateChanged : " + AnimStateFinished);
				});

			//normalizedTimeの値を監視、1を超えたらAnimStteFinishedをtrueに変更する
			_stateMachineObservables
				.OnStateUpdateObservable 
				.Where(x => x.normalizedTime > 1)
				.Where(x => isAnimStateChanging == false)
				.Subscribe (x => {
					AnimStateFinished = true;
					//					Debug.LogWarning ("AnimStateFinished : " + AnimStateFinished);
					preNormalizedTime = x.normalizedTime;
				});	//AnimatorのRestパラメータをTrueにする
		}


			
		private IEnumerator WaitTillAnimFinish ()
		{
			for (;;) {
				if (AnimStateFinished == true)
					break;
				yield return null;
			}
		}

		private IEnumerator WalkTo (Vector3 targetPos, float speed)
		{
			for (;;) {
				this.transform.position += transform.forward * speed * Time.deltaTime;
				if (Vector3.Distance (this.transform.position, targetPos) < 0.1f)
					break;
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
				if (this.transform.rotation == targetRotation) {
					break;
				}
				yield return null;
			}
			Destroy (target);
			Destroy (targetLooker);
		}

		public IEnumerator StartLookAround(){
			policeParams.policeStatus = PoliceStatus.PREFERENCIAL_BEHAVIOR;
			_animator.SetInteger(poopAnimIndex,1);
			yield return null;
			_animator.SetInteger(poopAnimIndex,2);
			isAnimStateChanging = false;
			AnimStateFinished = false;
			yield return StartCoroutine (WaitTillAnimFinish ());
		}

		public IEnumerator WalkToTarget(Vector3 targetPos){
			yield return null;
			//回転アニメーション
			_animator.SetInteger(poopAnimIndex,3);
			//回転する
			yield return StartCoroutine (LookAt (targetPos));
			//歩くアニメーション
			_animator.SetInteger(poopAnimIndex,4);
			//歩く
			yield return StartCoroutine (WalkTo (targetPos, 1));
		}

		public IEnumerator StartPickUpAnimation(){
			//拾うアニメーション
			_animator.SetInteger(poopAnimIndex,5);
			//アニメーション終了待ち
			AnimStateFinished = false;
			yield return StartCoroutine (WaitTillAnimFinish ());
			//baseAnim系列にアニメーションへ戻す
			_animator.SetTrigger("PrefentialAnimFinished");
		}

		public IEnumerator FromActionToSheet (Vector3 targetPos)
		{
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
	}
}
