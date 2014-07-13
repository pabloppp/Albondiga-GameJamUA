using UnityEngine;
using System.Collections;

public class meta : MonoBehaviour {

	bool finished = false;
	HaS_Camera camscript;
	public Transform mimeta;
	public Texture youwinTex;
	float cartelopacity = 0;
	// Use this for initialization
	void Start () {
		camscript = Camera.main.GetComponentInParent<HaS_Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (finished) {
			if(cartelopacity < 1f) cartelopacity += Time.deltaTime*0.1f;
			camscript.angleToTarget.x+=Time.deltaTime*0.5f;
			camscript.angleToTarget.y = 
				Mathf.Lerp(camscript.angleToTarget.y, -15*Mathf.Deg2Rad, Time.deltaTime);
			camscript.currentZoom = 
				Mathf.Clamp(camscript.currentZoom+Time.deltaTime*10, camscript.minZoomDist, camscript.maxZoomDist );
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("FIN");
		WWW challenge = new WWW("http://api.gamejamua.com/challenge/a1a1616d9b8b561/complete"); //- See more at: http://gamejamua.com/compos/IV-gamejam-edicion-solidaria/challenges/?challenge=a1a1616d9b8b561#sthash.9q28oQSg.dpuf
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
	

	public void OnGUI(){
		if (finished) {
			GUI.color = new Color(1,1,1,cartelopacity);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), youwinTex, ScaleMode.ScaleToFit);
		}
	}
	
}
