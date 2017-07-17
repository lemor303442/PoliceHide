using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public abstract class PoliceAction
	{
		protected int m_status;
		protected PoliceActionManager m_manager;
		protected GameObject m_gameObject;
		protected PlayerAnimation m_anim;

		public void SetGameObject (GameObject obj)
		{
			m_gameObject = obj;
		}

		public abstract void Update ();

		public abstract bool IsEnd ();


		private GameObject targetObj;
		public bool RotateTo (Vector3 targetPos)
		{
			if (targetObj == null) {
				targetObj = new GameObject ();
				targetObj.transform.position = targetPos;
			}
			Quaternion targetRotation = targetObj.transform.rotation;
			Quaternion firstRotation = m_gameObject.transform.rotation;
			float rotateSpeed = 50;
			//実際に回転させる
			Quaternion currentRotation = Quaternion.RotateTowards (m_gameObject.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			m_gameObject.transform.rotation = currentRotation;
			if ((m_gameObject.transform.eulerAngles.y - targetRotation.eulerAngles.y) % 360 == 0) {
				GameObject.Destroy (targetObj);
				targetObj = null;
				return true;
			}
			return false;
		}

		public bool WalkTo (Vector3 targetPos, float speed)
		{
			m_gameObject.transform.position += m_gameObject.transform.forward * speed * Time.deltaTime;
			if (Vector3.Distance (m_gameObject.transform.position, targetPos) < 0.1f)
				return true;
			return false;
		}

		public void PlayAnimation ()
		{
			if (m_anim == null) {
				m_anim = new PlayerAnimation ();
				m_anim.Init (m_gameObject);
			}
			m_anim.PlayAnim (0);
		}

		public bool IsAnimationEnd ()
		{
			return (m_anim == null) ? true : m_anim.IsAnimEnd ();
		}
	}
}
