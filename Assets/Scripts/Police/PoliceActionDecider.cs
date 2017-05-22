using UnityEngine;
using Polices.Decider;
using Polices;
using Polices.Behaviors;


namespace Polices.Decider
{
	[RequireComponent(typeof(BaseBehaviorGenerator))]
	[RequireComponent(typeof(PrefentialBehaviorGenerator))]
	[RequireComponent(typeof(PoliceParams))]
	public class PoliceActionDecider : MonoBehaviour
	{
		private BaseBehaviorGenerator baseBehaviorGenerator;
		private PrefentialBehaviorGenerator preferentialBehaviorGenerator;
		private PoliceParams policeParams;

		Rigidbody rigidbody;

		private float idleTimer;

		float policeStatusChangedTime;

		public void Init(){
			baseBehaviorGenerator = GetComponent<BaseBehaviorGenerator>();
			baseBehaviorGenerator.Init();
			preferentialBehaviorGenerator = GetComponent<PrefentialBehaviorGenerator>();
			preferentialBehaviorGenerator.Init();
			policeParams = GetComponent<PoliceParams>();
			policeParams.policeStatus = PoliceStatus.IDLE;
		}

		void Update(){
			if(policeParams.policeStatus == PoliceStatus.IDLE)
				idleTimer += Time.deltaTime;
			if(idleTimer > 3){
				StartActionRandomly();
				idleTimer = 0;
			}
			
		}


		public void StartActionRandomly(){
			baseBehaviorGenerator.BeginBaseBehavior(Random.Range(1,4));
		}
//
//		public void StartPrefentialBehavior(){
//			StartCoroutine(preferentialBehaviorGenerator.Poop(new Vector3(1,0,1)));
//		}

		public void SpawnedOutside(int id){
			StartCoroutine(baseBehaviorGenerator.GoToSheet(id));

		}
	}
}
