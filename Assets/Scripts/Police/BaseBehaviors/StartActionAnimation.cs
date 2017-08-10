using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public class StartActionAnimation : PoliceAction
	{
		private Animator m_animator;
		private string m_anim;
		private bool m_repeat;

		private bool m_startAnim;
		private float preNormalizedTime;
		private bool isAnimSwitched;

		public void Init (string anim, bool repeat)
		{
			m_animator = m_gameObject.GetComponent<Animator> ();
			m_anim = anim;
			m_repeat = repeat;
			m_startAnim = true;
		}
			
		public override void Resume(){
			float duration = 0.3f / m_animator.GetCurrentAnimatorStateInfo (0).length;
			//				duration = 0;
			m_animator.CrossFade (m_anim, duration);
			m_startAnim = false;
			//これちょっと実装微妙、、
			if (m_animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.3f) {
				isAnimSwitched = false;
			}else{
				isAnimSwitched = true;
			}
		}

		// Update is called once per frame
		public override void Update ()
		{
			if (m_startAnim) {
				float duration = 0.3f / m_animator.GetCurrentAnimatorStateInfo (0).length;
//				duration = 0;
				m_animator.CrossFade (m_anim, duration);
				m_startAnim = false;
				//これちょっと実装微妙、、
				if (m_animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.3f) {
					isAnimSwitched = false;
				}else{
					isAnimSwitched = true;
				}
			}
		}

		public override bool IsEnd ()
		{
//			Debug.Log (m_anim + " repeat? " + m_repeat + " time? " + m_animator.GetCurrentAnimatorStateInfo (0).normalizedTime + " isSwitched? " + isAnimSwitched);
			if (m_repeat) {
				return true;
			}

			float normalizedTime = m_animator.GetCurrentAnimatorStateInfo (0).normalizedTime;

			if (!isAnimSwitched) {
				if (normalizedTime - preNormalizedTime < 0) {
					isAnimSwitched = true;
				}
				preNormalizedTime = normalizedTime;
				return false;
			} else {
				return normalizedTime >= 1.0f;
			}
		}
	}
}
