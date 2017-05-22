using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polices.Decider;

namespace Polices
{
	public class PoliceSpawner : MonoBehaviour
	{

		GameObject police;
		int policeID = 0;

		float timer;
		int nextInterval;


		private Vector3[] spawnPosition = new Vector3[2];

		// Use this for initialization
		void Start ()
		{
			police = Resources.Load ("Prefabs/Police") as GameObject;
			spawnPosition [0] = new Vector3 (-4, 0, 4);
			spawnPosition [1] = new Vector3 (4, 0, 4);

			nextInterval = Random.Range (3, 8);
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (policeID < 4) {
				timer += Time.deltaTime;
				if (timer > nextInterval) {
					timer = 0;
					nextInterval = Random.Range (10, 30);
					SpawnPolice ();
				}
			}
		
		}

		public void SpawnPolice ()
		{
			GameObject policeClone = Instantiate (police) as GameObject;
			policeClone.transform.position = spawnPosition [Random.Range (0, 2)];
			policeClone.GetComponent<PoliceParams> ().policeID = policeID;
			policeClone.GetComponent<PoliceActionDecider> ().Init ();
			policeClone.GetComponent<PoliceActionDecider> ().SpawnedOutside (policeID);
			policeID += 1;
		}
	}
}