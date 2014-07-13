using UnityEngine;
using System.Collections;

public class spawnGenerator : MonoBehaviour {


	public GameObject spawning;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void spawn( Vector3 position ) {
		StartCoroutine("createParticles", position);
	}

	public IEnumerator createParticles(Vector3 position){
		Object o = Instantiate( spawning, position-Vector3.up*0.7f, Quaternion.identity );
		yield return new WaitForSeconds(3);
		Destroy(o);
	}

}
