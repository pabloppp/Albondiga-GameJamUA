using UnityEngine;
using System.Collections;

public class meta : MonoBehaviour {

	bool finished = false;
	HaS_Camera camscript;
	public Transform mimeta;
	// Use this for initialization
	void Start () {
		camscript = Camera.main.GetComponentInParent<HaS_Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (finished) {
			camscript.angleToTarget.x+=Time.deltaTime*0.5f;
			camscript.angleToTarget.y = 
				Mathf.Lerp(camscript.angleToTarget.y, -15*Mathf.Deg2Rad, Time.deltaTime);
			camscript.currentZoom = 
				Mathf.Clamp(camscript.currentZoom+Time.deltaTime*10, camscript.minZoomDist, camscript.maxZoomDist );
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("FIN");
		other.GetComponent<player_movement> ().state = 
			player_movement.playerStates.IDLE;

		other.GetComponent<player_movement> ().enabled = false;
		other.GetComponentInChildren<Animator>().SetInteger ("RunSpeed", 0);
		other.GetComponent<playerDrinks> ().enabled = false;
		other.GetComponent<userStates> ().enabled = false;
		other.attachedRigidbody.velocity = Vector3.zero;
    	camscript.fixedCamera = true;
		camscript.rotationSpeed = 2;
		camscript.transitionSpeed = 0.1f;
		camscript.target = mimeta;
		camscript.maxZoomDist *= 20;
		finished = true;
	}
}
