using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices.Behaviors
{
	[RequireComponent (typeof(PreferentialBehaviorController))]
	[RequireComponent (typeof(PoliceParams))]
	public class PrefentialBehaviorGenerator : MonoBehaviour
	{
		PreferentialBehaviorController preferentialBehaviorController;
		PoliceParams policeParams;

		void Start ()
		{
			preferentialBehaviorController = GetComponent<PreferentialBehaviorController> ();
			policeParams = GetComponent<PoliceParams> ();
		}

		public IEnumerator Poop (Vector3 targetPos)
		{
			yield return StartCoroutine (preferentialBehaviorController.StartLookAround ());
			yield return StartCoroutine (preferentialBehaviorController.WalkToTarget (targetPos));
			yield return StartCoroutine (preferentialBehaviorController.StartPickUpAnimation ());
			yield return StartCoroutine (preferentialBehaviorController.FromActionToSheet (policeParams.sheetPosition));
			yield return StartCoroutine (preferentialBehaviorController.SitDown(policeParams.sheetDirection));
		}
	}
}
