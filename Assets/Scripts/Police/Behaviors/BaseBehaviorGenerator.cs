using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Polices;

namespace Polices.Behaviors
{
	[RequireComponent (typeof(BaseBehaviorController))]
	[RequireComponent (typeof(PoliceParams))]
	public class BaseBehaviorGenerator : MonoBehaviour
	{
		BaseBehaviorController baseBehaviorController;
		PoliceParams policeParams;
		//イベントを発行する核となるインスタンス
		//private Subject<int> timerSubject = new Subject<int> ();

		void Start ()
		{
			baseBehaviorController = GetComponent<BaseBehaviorController> ();
			policeParams = GetComponent<PoliceParams> ();
		}

		public void BeginBaseBehavior (int id)
		{
			switch (id) {
			case 0:
				break;
			case 1:
				StartCoroutine (Opening ());
				break;
			case 2:
				StartCoroutine (GetTea ());
				break;
			case 3:
				StartCoroutine (OpeningLocker ());
				break;
			}
		}

		private IEnumerator Opening ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (-1.82f, 0, -1.55f)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (-2.82f, 0, -1.55f), "Opening"));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (policeParams.sheetPosition));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.SitDown (policeParams.sheetDirection));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
		}

		private IEnumerator OpeningLocker ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (2.3f, 0, -1.55f)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (3.3f, 0, -1.55f), "Opening"));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (policeParams.sheetPosition));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.SitDown (policeParams.sheetDirection));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
		}

		private IEnumerator GetTea ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (-1.48f, 0, -0.66f)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (-1.97f, 0, -2.81f)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (-2.97f, 0, -2.81f), "Bartending"));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (policeParams.sheetPosition));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.SitDown (policeParams.sheetDirection));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
		}


	}
}