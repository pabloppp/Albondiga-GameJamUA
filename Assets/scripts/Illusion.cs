using UnityEngine;
using System.Collections;

public class Illusion : MonoBehaviour {
	public float rangeViewDisappear = 10f;
	public bool isReal = false;
	public float showDistance = 50f;
	private Transform particulas;
	private Transform player;
	private explode exp;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		exp = GetComponent<explode>();

		/*this.particulas = gameObject.transform.FindChild("particulas");

		if(this.particulas != null)
			this.particulas.gameObject.SetActive(false);*/
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isReal) {
		   Disappear ();
		}
		if (Vector3.Distance (transform.position, player.position) < showDistance) {	

		}
	}

	void Disappear()
	{
		if (Vector3.Distance (transform.position, player.position) < rangeViewDisappear) {		
			Debug.Log("Puff...");
			exp.Explode(transform.position+Vector3.up*1.2f);
			Destroy(gameObject);

		}
	}
}
