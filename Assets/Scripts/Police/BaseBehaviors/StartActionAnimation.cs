using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class StartActionAnimation : PoliceAction {
		private Animator m_animator;

		public void Init(string anim)
		{
			m_animator = m_gameObject.GetComponent<Animator>();
			m_animator.Play(anim, 0);
		}

		// Update is called once per frame
		public override void Update(){
		
		}

		public override bool IsEnd(){
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
		}
	}
}
