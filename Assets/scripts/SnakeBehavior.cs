using UnityEngine;
using System.Collections;

public class SnakeBehavior : MonoBehaviour {
	public float speed;
	public Transform cola;

	private GameObject player;
	private spawnGenerator sp;
	private Vector3 initPos;

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag ("Player");
		sp = GetComponent<spawnGenerator>();
		sp.spawn(transform.position);
		initPos = transform.position;
		//StartCoroutine ("SpawnBody");
	}

	IEnumerator SpawnBody()
	{
		while(true)
		{
			var clon = (GameObject)Instantiate (
							Resources.Load ("bodyFragment"),
							transform.position,
				transform.rotation);

			yield return new WaitForSeconds(0.4f);
		}
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

		/*Vector3 randomPosition = new Vector3(transform.position.x, 200, transform.position.z);
		
		RaycastHit hit = new RaycastHit();
		
		if (Physics.Raycast (randomPosition, -Vector3.up, out hit, 200)) {
			transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);*/

	}

	public void resetSnake(){
		transform.position = initPos;

	}
}
