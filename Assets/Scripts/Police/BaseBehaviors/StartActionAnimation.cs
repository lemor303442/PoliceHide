using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class StartActionAnimation : PoliceAction {
		private Animator m_animator;
		private bool m_waitAnimEnd;

		public void Init(string anim, bool waitAnimEnd)
		{
			m_animator = m_gameObject.GetComponent<Animator>();
			m_animator.Play(anim, 0);
			m_waitAnimEnd = waitAnimEnd;
		}

		// Update is called once per frame
		public override void Update(){
		
		}

		public override bool IsEnd(){
			if(!m_waitAnimEnd){
				return true;
			}
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
		}
	}
}
