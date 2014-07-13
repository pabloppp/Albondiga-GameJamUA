using UnityEngine;
using System.Collections;

public class explode : MonoBehaviour {

	public GameObject explosion;
	bool poofed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Explode( Vector3 position ) {
		if(!poofed){
			Instantiate( explosion, position, Quaternion.identity );
			poofed = true;
		}
	}
}
