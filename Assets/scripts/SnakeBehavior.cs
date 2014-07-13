using UnityEngine;
using System.Collections;

public class SnakeBehavior : MonoBehaviour {
	public float speed;
	private GameObject player;

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
		if (this.player != null && this.player.GetComponent<userStates> ().sed < this.player.GetComponent<userStates> ().sedMax)
						UpdateMovement ();
		else {
			rigidbody.velocity = Vector3.zero;

			rigidbody.useGravity = false;
		}
	}

	void UpdateMovement(){
		transform.LookAt (player.transform.position);

		rigidbody.AddForce(transform.forward * speed , ForceMode.VelocityChange);

		rigidbody.velocity = transform.forward * speed;

		float x, y, z;

		x = rigidbody.velocity.x;
		y = rigidbody.velocity.y;
		z = rigidbody.velocity.z;

		rigidbody.velocity = new Vector3(Mathf.Clamp (x, -speed, speed),
		                                 Mathf.Clamp (y, -speed, speed),
		                                 Mathf.Clamp (z, -speed, speed));


	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player") {
			collider.GetComponent<userStates>().sed = collider.GetComponent<userStates>().sedMax;
		}
	}
}
