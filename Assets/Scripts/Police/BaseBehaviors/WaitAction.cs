using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polices
{
	public class WaitAction : PoliceAction
	{
		public override void Update(){
		}
		public override bool IsEnd(){
			return false;
		}
	}
}
