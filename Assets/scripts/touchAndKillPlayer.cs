using UnityEngine;
using System.Collections;

public class touchAndKillPlayer : MonoBehaviour {
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<userStates>().sed = other.GetComponent<userStates>().sedMax; 
		}
	}
}
