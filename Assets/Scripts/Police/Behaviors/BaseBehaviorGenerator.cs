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

		string[,] policeAnimData;

		private Vector3 sheetPosition;
		private Vector3 sheetDirection;

		public string behaviorName;
		public Vector3[] toPositions;
		public Vector3 animDirection;
		public Vector3[] fromPositions;


		void Start ()
		{
			baseBehaviorController = GetComponent<BaseBehaviorController> ();
			policeParams = GetComponent<PoliceParams> ();
			GetSheetPositions ();
		}

		void GetSheetPositions ()
		{
			string[,] policeSheetPosition = csvManager.ReadFilesInResources ("csv/PoliceSheetsPos");
			for (int i = 0; i < policeSheetPosition.GetLength (0); i++) {
				if (policeSheetPosition [i, 0] == policeParams.policeID.ToString ()) {
					sheetPosition = new Vector3 (float.Parse (policeSheetPosition [i, 1]), float.Parse (policeSheetPosition [i, 2]), float.Parse (policeSheetPosition [i, 3]));
					sheetDirection = new Vector3 (float.Parse (policeSheetPosition [i, 4]), float.Parse (policeSheetPosition [i, 5]), float.Parse (policeSheetPosition [i, 6]));
				}
			}
			Debug.Log (sheetPosition);
			Debug.Log (sheetDirection);
		}

		private void CreateRouteArray (int policeID, string animName)
		{
			behaviorName = animName;
			string[,] policeBasicBehaviorArray = csvManager.ReadFilesInResources ("csv/PolieBasicBehaviors");
			for (int i = 0; i < policeBasicBehaviorArray.GetLength (0); i++) {
				if (policeBasicBehaviorArray [i, 0] == policeParams.policeID.ToString ()) {
					if (policeBasicBehaviorArray [i, 1] == animName) {
						//配列のサイズを取得し、配列のサイズを指定
						int toPositionsLength = 0;
						int fromPositionsLength = 0;
						for (int j = 0; j < policeBasicBehaviorArray.GetLength (0) - i; j++) {
							if (policeBasicBehaviorArray [i + j, 1] == animName) {
								switch (policeBasicBehaviorArray [i + j, 2]) {
								case "ToPositions":
									toPositionsLength++;
									break;
								case "animDirection":
									break;
								case "FromPositions":
									fromPositionsLength++;
									break;
								}
							} else {
								break;
							}
						}
						toPositions = new Vector3[toPositionsLength];
						fromPositions = new Vector3[fromPositionsLength];

						//配列に値を代入
						int elementNum = 0;
						for (int j = 0; j < policeBasicBehaviorArray.GetLength (0) - i; j++) {
							if (policeBasicBehaviorArray [i + j, 1] == animName) {
								switch (policeBasicBehaviorArray [i + j, 2]) {
								case "ToPositions":
									toPositions [elementNum] = new Vector3 (float.Parse (policeBasicBehaviorArray [i + j, 3]), float.Parse (policeBasicBehaviorArray [i + j, 4]), float.Parse (policeBasicBehaviorArray [i + j, 5]));
									elementNum++;
									break;
								case "animDirection":
									animDirection = new Vector3 (float.Parse (policeBasicBehaviorArray [i + j, 3]), float.Parse (policeBasicBehaviorArray [i + j, 4]), float.Parse (policeBasicBehaviorArray [i + j, 5]));
									elementNum = 0;
									break;
								case "FromPositions":
									fromPositions [elementNum] = new Vector3 (float.Parse (policeBasicBehaviorArray [i + j, 3]), float.Parse (policeBasicBehaviorArray [i + j, 4]), float.Parse (policeBasicBehaviorArray [i + j, 5]));
									elementNum++;
									fromPositionsLength++;
									break;
								}
							} else {
								break;
							}
						}
						break;
					}
				}
			}
		}

		public void BeginBaseBehavior (int id)
		{
			switch (id) {
			case 0:
				break;
			case 1:
				StartCoroutine (StartBasicBehavior(0,"OpenCooler"));
				break;
			case 2:
				StartCoroutine (StartBasicBehavior(0,"Bartending"));
				break;
			case 3:
				StartCoroutine (StartBasicBehavior(0,"OpenLocker"));
				break;
			}
		}

		private IEnumerator StartBasicBehavior (int policeID, string animName)
		{
			CreateRouteArray(policeID, animName);

			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;

			for (int i = 0; i < toPositions.Length; i++) {
				yield return StartCoroutine (baseBehaviorController.FromStandToTarget (toPositions [i]));
				if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
					yield break;
			}

			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (animDirection, animName));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;

			for (int i = 0; i < fromPositions.Length; i++) {
				yield return StartCoroutine (baseBehaviorController.FromActionToSheet (fromPositions [i]));
				if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
					yield break;
			}

			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (sheetPosition));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			
			yield return StartCoroutine (baseBehaviorController.SitDown (sheetDirection));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
		}

		private IEnumerator Opening ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;

			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (-2.03f, 0, 0.8f)));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;

			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (-3f, 0, 0.8f), "OpenCooler"));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;

			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (sheetPosition));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;

			yield return StartCoroutine (baseBehaviorController.SitDown (sheetDirection));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
		}

		private IEnumerator OpeningLocker ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (2.13f, 0, 0.9f)));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (3f, 0, 0.9f), "OpenLocker"));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (sheetPosition));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.SitDown (sheetDirection));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
		}

		private IEnumerator GetTea ()
		{
			yield return StartCoroutine (baseBehaviorController.StandUp ());
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.FromStandToTarget (new Vector3 (-2.29f, 0, -0.13f)));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.StartActionAnimation (new Vector3 (-3f, 0, -0.13f), "Bartending"));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.FromActionToSheet (sheetPosition));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
			yield return StartCoroutine (baseBehaviorController.SitDown (sheetDirection));
			if (policeParams.policeStatus == PoliceStatus.PREFERENCIAL_BEHAVIOR)
				yield break;
		}


	}
}