using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testScript : MonoBehaviour {

	NavMeshAgent agent;
	int phase;

	void Start () {
		agent = GetComponent<NavMeshAgent>();

	}
	
	void Update () {
		switch(phase){
		case 0:
			agent.SetDestination(new Vector3(-2,0,3.75f));
			phase++;
			break;
		case 1:
			if(Vector3.Distance(this.transform.position, new Vector3(0,0,3.75f)) < 0.2f){
				phase++;
			}
			break;
		case 2:
			agent.SetDestination(new Vector3(0,0,0f));
			phase++;
			break;
		case 3:
			if(Vector3.Distance(this.transform.position, new Vector3(0,0,2f)) < 0.2f){
				phase++;
			}
			break;
		case 4:
			agent.SetDestination(new Vector3(2.3f,0,2.3f));
			phase++;
			break;
		}
	}
}
