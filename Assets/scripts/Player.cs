using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}

	void move(){
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		float v= Input.GetAxis("Vertical");
		//Debug.Log("hhh"+h);
		//Debug.Log("vv"+v);
		float x=this.transform.position.x;
		float y=this.transform.position.z;
		float x2=Camera.main.transform.position.x;
		float y2=Camera.main.transform.position.z;
			y=y+v*speed;
			x=x+h*speed;
			y2=y2+v*speed;
			x2=x2+h*speed;


		Camera.main.transform.position= new Vector3(x2,Camera.main.transform.position.y,y2);
		this.transform.position= new Vector3(x,this.transform.position.y,y);


		
		
	}
}
