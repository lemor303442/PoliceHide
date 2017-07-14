using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Police
{
	public class WalkTo : PoliceAction
	{
		public Vector3 targetPos;
		public void Init(Vector3 targetPos){
			targetPos = targetPos;
		}
		public override void Update(){
			
		}
		public override bool IsEnd(){
			return WalkTo(targetPos, 5);
		}
	}
}
