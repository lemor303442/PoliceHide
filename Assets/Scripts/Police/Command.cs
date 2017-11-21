using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Command
{
	private string basicCommands;
	private string otherCommands;
	private string preferencialCommands;

	public string BasicCommands{ get { return basicCommands; } }

	public string OtherCommands{ get { return otherCommands; } }

	public string PreferencialCommands{ get { return preferencialCommands; } }

	public void Init (Action onComplete = null, Action onError = null)
	{
		basicCommands = CsvManager.ReadTextFile("Commands/BasicCommands.csv");
		otherCommands = CsvManager.ReadTextFile("Commands/OtherCommands.csv");
		preferencialCommands = CsvManager.ReadTextFile("Commands/PreferencialCommands.csv");
		CoroutineHandler.StartStaticCoroutine (LoadScenarioFromServer (onComplete, onError));
	}

	IEnumerator LoadScenarioFromServer (Action onComplete, Action onError = null)
	{
		string newBasicCommands = "";
		string newOtherCommands = "";
		string newPreferencialCommands = "";

		WWW request = new WWW ("https://docs.google.com/spreadsheets/d/1XArPYnCr47dGCuKDx-WHJQ3DBFO6pmw9i7qMwrIZclA/export?format=csv&gid=1564450559");
		yield return request;
		if (!string.IsNullOrEmpty (request.error)) {
			Debug.LogWarning ("エラー:" + request.error);
			if (onError != null)
				onError ();
			yield break;
		} else {
			newBasicCommands = request.text;
			if (newBasicCommands != basicCommands) {
				basicCommands = newBasicCommands;
				CsvManager.WriteData ("Commands/BasicCommands.csv", basicCommands);
			}
		}

		request = new WWW ("https://docs.google.com/spreadsheets/d/1XArPYnCr47dGCuKDx-WHJQ3DBFO6pmw9i7qMwrIZclA/export?format=csv&gid=859044375");
		yield return request;
		if (!string.IsNullOrEmpty (request.error)) {
			Debug.LogWarning ("エラー:" + request.error);
			if (onError != null)
				onError ();
			yield break;
		} else {
			newOtherCommands = request.text;
			if (newOtherCommands != otherCommands) {
				otherCommands = newOtherCommands;
				CsvManager.WriteData ("Commands/OtherCommands.csv", otherCommands);
			}
		}

		request = new WWW ("https://docs.google.com/spreadsheets/d/1XArPYnCr47dGCuKDx-WHJQ3DBFO6pmw9i7qMwrIZclA/export?format=csv&gid=1589268886");
		yield return request;
		if (!string.IsNullOrEmpty (request.error)) {
			Debug.LogWarning ("エラー:" + request.error);
			if (onError != null)
				onError ();
			yield break;
		} else {
			newPreferencialCommands = request.text;
			Debug.Log(newPreferencialCommands);
			Debug.Log(preferencialCommands);
			if (newPreferencialCommands != preferencialCommands) {
				preferencialCommands = newPreferencialCommands;
				CsvManager.WriteData ("Commands/PreferencialCommands.csv", preferencialCommands);
			}
		}

		if (onComplete != null)
			onComplete ();
	}
}


public class MyMonoBehaviour : MonoBehaviour
{
	public void CallStartCoroutine (IEnumerator iEnumerator)
	{
		Debug.Log (iEnumerator);
		StartCoroutine (iEnumerator); //ここで実際にMonoBehaviour.StartCoroutine()を呼ぶ
	}
}