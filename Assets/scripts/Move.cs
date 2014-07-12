using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public CharacterController controller;

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W))
			controller.Move (Vector3.forward);
		if (Input.GetKey (KeyCode.S))
			controller.Move (-Vector3.forward);
		if (Input.GetKey (KeyCode.D))
			controller.Move (Vector3.right);
		if (Input.GetKey (KeyCode.A))
			controller.Move (-Vector3.right);
	}
}
