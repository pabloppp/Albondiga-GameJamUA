using UnityEngine;
using System.Collections;

public class Illusion : MonoBehaviour {
	public float rangeViewDisappear = 10f;
	public bool isReal = false;

	public float showDistance = 50f;
	private Transform particulas;
	private Transform player;
	private explode exp;

	public KeyCode drinkKey = KeyCode.Space;
	public float waterQuantityPerUse;

	private bool keyWasPressed = false;
	private bool isDrinking = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		
		exp = GetComponent<explode>();
	}
	
	void FixedUpdate(){	
		if (!isReal) {
				Disappear ();
		} else if (Vector3.Distance (this.transform.position, player.position) < this.rangeViewDisappear) {
			if(Input.GetKeyUp(drinkKey) && !this.isDrinking)
				this.keyWasPressed = false;
			else if (Input.GetKeyDown(drinkKey) && !this.keyWasPressed)
			{
				this.keyWasPressed = true;

				player.GetComponent<userStates>().sed -= this.waterQuantityPerUse;
			}
		}
	}

	void Disappear()
	{	
		if (Vector3.Distance (transform.position, player.position) < rangeViewDisappear) {		
			exp.Explode(transform.position+Vector3.up*1.2f);
			Destroy(gameObject);
		}
	}
}