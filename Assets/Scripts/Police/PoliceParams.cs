using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices;

namespace Polices
{
	public class PoliceParams : MonoBehaviour
	{
		public PoliceStatus policeStatus = PoliceStatus.NONE;
		public PoliceStatus prePoliceStatus = PoliceStatus.NONE;
		public PoliceStatus policeStatusTransfer = PoliceStatus.NONE;

		public Vector3 sheetPosition;
		public Vector3 sheetDirection;

		void Start ()
		{
			StartCoroutine(CheckPoliceStatus());
		}

		IEnumerator CheckPoliceStatus ()
		{
			for (;;) {
				if(policeStatus != policeStatusTransfer){
					prePoliceStatus = policeStatusTransfer;
					policeStatusTransfer = policeStatus;
				}
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
}