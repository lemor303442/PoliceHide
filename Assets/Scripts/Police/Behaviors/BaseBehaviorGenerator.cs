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
			}
		}

		private IEnumerator Opening ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (3, 0, 3)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (0, 0, 6)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (0, 0, 7), "Opening"));
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
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (-3, 0, 3)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (0, 0, 6)));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (0, 0, 7), "Bartending"));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (policeParams.sheetPosition));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
			yield return StartCoroutine (baseBehaviorController.SitDown (policeParams.sheetDirection));
			if(policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)yield break;
		}
	}
}