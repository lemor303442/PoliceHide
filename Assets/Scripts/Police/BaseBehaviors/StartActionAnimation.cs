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

		public void Init (string anim, bool repeat)
		{
			m_animator = m_gameObject.GetComponent<Animator> ();
			m_anim = anim;
			m_repeat = repeat;
			m_startAnim = true;
		}

		// Update is called once per frame
		public override void Update ()
		{
			if (m_startAnim) {
				float duration = 0.3f / m_animator.GetCurrentAnimatorStateInfo (0).length;
				m_animator.CrossFade (m_anim, duration);
				m_startAnim = false;
			}
		}

		public override bool IsEnd ()
		{
			if (m_repeat) {
				return true;
			}
			return m_animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f;
		}
	}
}
