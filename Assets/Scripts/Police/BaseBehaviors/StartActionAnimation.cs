using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class StartActionAnimation : PoliceAction {
		private Animator m_animator;
		private string m_anim;
		private bool m_waitAnimEnd;
		private bool m_startAnim;

		public void Init(string anim, bool waitAnimEnd)
		{
			m_animator = m_gameObject.GetComponent<Animator>();
			m_anim = anim;
			m_waitAnimEnd = waitAnimEnd;
			m_startAnim = true;
		}

		// Update is called once per frame
		public override void Update(){
			if(m_startAnim){
				m_animator.Play(m_anim, 0);
				m_startAnim = false;
			}
		}

		public override bool IsEnd(){
			if(!m_waitAnimEnd){
				return true;
			}
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
		}
	}
}
