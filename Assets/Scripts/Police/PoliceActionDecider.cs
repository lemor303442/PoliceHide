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

		float policeStatusChangedTime;

		void Start(){
			baseBehaviorGenerator = GetComponent<BaseBehaviorGenerator>();
			preferentialBehaviorGenerator = GetComponent<PrefentialBehaviorGenerator>();
			policeParams = GetComponent<PoliceParams>();
			policeParams.policeStatus = PoliceStatus.IDLE;
		}


		public void StartActionRandomly(){
			baseBehaviorGenerator.BeginBaseBehavior(Random.Range(1,4));
		}

		public void StartPrefentialBehavior(){
			StartCoroutine(preferentialBehaviorGenerator.Poop(new Vector3(1,0,1)));
		}
	}
}
