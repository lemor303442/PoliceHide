using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class PlayerAnimation : MonoBehaviour
	{
		private Animator _animator;
		private StateMachineObservalbes _stateMachineObservables;

		private string baseAnimIndex = "BaseAnimIndex";
		private bool isAnimStateChanging = true;
		private bool AnimStateFinished = false;
		private float AnimStateNormalized;

		public void Init (GameObject gameObj)
		{

		}
		public bool IsAnimEnd()
		{
			return AnimStateFinished;
		}
		public void PlayAnim(int index){
		}
	}
}