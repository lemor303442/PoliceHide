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

		private string poopAnimIndex = "PoopAnimIndex";
		private bool AnimStateFinished = false;

		void Start ()
		{
			policeParams = GetComponent<PoliceParams>();

			_animator = GetComponent <Animator> ();
			_stateMachineObservables = _animator.GetBehaviour <StateMachineObservalbes> ();

			//animationState変更時にAnimStateFinishedをfalseに変更する
			_stateMachineObservables
				.OnStateEnterObservable
				.Subscribe (x => AnimStateFinished = false);

			//normalizedTimeの値を監視、1を超えたらAnimStteFinishedをtrueに変更する
			_stateMachineObservables
				.OnStateUpdateObservable            
				.Where (x => x.normalizedTime >= 1)			//ステート中を監視
				.Subscribe (x => AnimStateFinished = true);	//AnimatorのRestパラメータをTrueにする
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
			yield return StartCoroutine (WaitTillAnimFinish ());
		}
	}
}
