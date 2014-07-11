using UnityEngine;
using System.Collections;

/// <summary>
/// Script for controlling, ina very general way, the player's movement and animation.
/// </summary>

[RequireComponent (typeof(Rigidbody))]
public class player_movement : MonoBehaviour {
	
	
	//STATES IN WHICH THE PLAYER CAN BE
	public enum playerStates{
		ERROR,   //Should never enter here
		IDLE,
		WALKING,
		FULLSPEED,
		MIDSPEED,
		LOWSPEED,
		RUNNING,
		JUMPING,
		FALLING,
		ATTACKING,
		HIT,
		FREEZED,
		DEAD
	} 
	//------
	
	public playerStates state = playerStates.IDLE;  //CURRENT STATE
	
	public bool grounded = true;
	
	public string[] groundTags = {"floor", "ground"};
	
	//KEY BINDING 
	//--- MOVING
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	
	public bool mimicCameraDir = true; 
	public bool rootMotion = false;
	public bool rootRotation = false;
	public float walkSpeed = 5;
	public float runSpeed = 10;
	public float FullSpeed = 7;
	public float MidSpeed = 5;
	public float LowSpeed = 3.5f;
	public float rotationSpeed = 0;
	public float life = 100;
	public float limitsup=75;
	public float limitlow=25;
	//that means the FORWARD MOVEMENT will always be in the direction of the camera
	
	//--- ACTIONS
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode attackKey = KeyCode.Mouse0;
	
	//-------
	
	public bool allowRun = true;
	public bool allowAtack = true;
	public bool allowJump = true;
	public bool sendStateMessages = true;
	
	
	//Components
	
	Rigidbody myRigidbody; 
	
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody>();
		StartCoroutine("FSM");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
	
	public IEnumerator FSM(){
		
		while(state != playerStates.DEAD){
			
			switch(state){
			case playerStates.IDLE:
				if(sendStateMessages) SendMessage("move_idle");
				idle();
				break;
				
			case playerStates.WALKING:
				if(sendStateMessages) SendMessage("move_walking");
				moving(walkSpeed);
				break;

			case playerStates.FULLSPEED:
				if(sendStateMessages) SendMessage("move_fullspeed");
				moving(FullSpeed);
				break;
			
			case playerStates.MIDSPEED:
				if(sendStateMessages) SendMessage("move_midspeed");
				moving(MidSpeed);
				break;
				
			case playerStates.LOWSPEED:
				if(sendStateMessages) SendMessage("move_lowspeed");
				moving(LowSpeed);
				break;
				
			case playerStates.FALLING:
				if(sendStateMessages) SendMessage("move_falling");
				falling();
				break;
				
			}
			
			yield return null;
		}
		
	}
	
	
	//STATE BIT SCRIPTS
	
	//IDLE
	void idle(){
		if(movementKey()){
			if(life<=100 && life>=limitsup) 
				state=playerStates.FULLSPEED;
			if(life<limitsup && life>=limitlow) 
				state=playerStates.MIDSPEED;
			if(life<limitlow) 
				state=playerStates.LOWSPEED;
		//	else state = playerStates.WALKING; //If any movement key is pressed while idle it goes to walking
		/*	if(life=>100 && life<75)
				state=playerStates.WALKING;
			/*if(life<75 && life>=25)
				state=playerStates.MIDSPEED;
			if(life<25)
				state=playerStates.LOWSPEED;*/
		}
	}// END IDLE
	
	
	//RUNNING AND WALKING
	void moving(float vel){
		
		if(!movementKey()) state = playerStates.IDLE;
		if(!grounded) state = playerStates.FALLING;
		
		//keys
		Vector3 moveDir = Vector3.zero;
		if(Input.GetKey(forwardKey)){
			if(mimicCameraDir) moveDir += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
			else moveDir += Vector3.forward;
		}
		if(Input.GetKey(backwardKey)){
			if(mimicCameraDir) moveDir -= new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
			else moveDir += Vector3.back;
		}
		if(Input.GetKey(leftKey)){
			if(mimicCameraDir) moveDir -= new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
			else moveDir += Vector3.left;
		}
		if(Input.GetKey(rightKey)){
			if(mimicCameraDir) moveDir += new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
			else moveDir += Vector3.right;
		}
		
		/*
		if(mimicCameraDir){
			moveDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
		}*/
		
		moveDir = moveDir.normalized;
		
		//If NOT rootMotion, move player
		if(!rootMotion){  
			
			//Debug movement
			//Debug.Log("Momentum: "+myRigidbody.velocity);
			
			
			//velocity management	
			if(moveDir != Vector3.zero) myRigidbody.velocity = vel*moveDir;
			
		}
		
		//If NOT rootRotation, rotate player
		if(!rootRotation){
			
			if(moveDir != Vector3.zero){
				if(rotationSpeed == 0) transform.rotation = Quaternion.LookRotation(moveDir);
				else transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime*rotationSpeed);
			}
			
		}
		else{
			if(mimicCameraDir){
				transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));
			}
		}
	}//END RUNNING
	
	//JUMPING
	void jumping(){
		
	}//END JUMPING
	
	//FALLING
	void falling(){
		if(grounded) state = playerStates.IDLE;
	}//END FALLING
	
	//ATTACKING
	void attacking(){
		
	}//END ATTACKING
	
	//HIT
	void hit(){
		
	}//END HIT
	
	//FREEZED
	void freezed(){
		
	}//END FREEZED
	
	//DEAD
	void dead(){
		
	}//END DEAD
	
	
	
	
	//Helping Functions
	bool movementKey(){
		if(Input.GetKey(forwardKey) || Input.GetKey(backwardKey) || Input.GetKey(leftKey) || Input.GetKey(rightKey)) return true;
		else return false;
	}
	
	bool hasGroundTag(Transform tr){		
		foreach(string s in groundTags) if(tr.CompareTag(s)) return true;
		return false;
	}
	
	//COLLISION FUNCTIONS
	
	void OnCollisionEnter(Collision col){
		
		//ground detection
		if(hasGroundTag(col.transform)){
			foreach(ContactPoint contact in col.contacts){
				//Debug.DrawRay(contact.point, contact.normal, Color.red);
				//Debug.Log(Vector3.Angle(contact.normal, transform.up));
				if(Vector3.Angle(contact.normal, transform.up) < 45){
					grounded = true;
					Debug.Log("On Ground --");
				}
			}
		}
		
	}
	
	void OnCollisionStay(Collision col){
		if(hasGroundTag(col.transform)){
			foreach(ContactPoint contact in col.contacts){
				//Debug.DrawRay(contact.point, contact.normal, Color.red);
				//Debug.Log(Vector3.Angle(contact.normal, transform.up));
				if(Vector3.Angle(contact.normal, transform.up) < 45){
					grounded = true;
					//Debug.Log("On Ground --");
				}
			}
		}
	}
	
	void OnCollisionExit(Collision col){
		
		//leaving ground
		if(hasGroundTag(col.transform)){
			grounded = false;
			Debug.Log("NOT On Ground --");
		}
		
	}
	
}
