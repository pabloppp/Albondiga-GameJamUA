using UnityEngine;
using System.Collections;

public class birdBehavior : MonoBehaviour {
	
	public float speed = 100;
	public float rotSpeed = 0f;
	public float mediumHeight = 10;
	public float heightDif = 5;
	public float heightFq = 5;


	// Use this for initialization

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-Vector3.forward*Time.deltaTime*speed);
		Vector3 tempPos = transform.position;
		tempPos.y = mediumHeight+Mathf.Cos(Time.deltaTime*heightFq)*heightDif;
		transform.position = tempPos;
		transform.Rotate(Vector3.up*Time.deltaTime*rotSpeed);
	}
}
