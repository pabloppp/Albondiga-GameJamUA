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

	public GameObject explosion;


	//animations
	private Animator animator;
	//KEY BINDING 
	//--- MOVING
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public KeyCode reset = KeyCode.Space;

	
	public bool mimicCameraDir = true; 

	public bool rootMotion = false;
	public bool rootRotation = false;
	public bool isColision= false;
	public int isTexto= 0;// 0 no pongo nada  //1 es pocima //2 piedra
	public int vision= 0;// porcentaje de certeza

	public float walkSpeed = 5;
	public float runSpeed = 10;
	public float FullSpeed = 7;
	public float MidSpeed = 5;
	public float LowSpeed = 3.5f;
	public float penalizacion= 0.4f;
	public float rotationSpeed = 0;
	public float life = 100;
	public float limitsup=100;
	public float limitlow=90;
	public float distanceTower=60;
	//that means the FORWARD MOVEMENT will always be in the direction of the camera
	
	//--- ACTIONS
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode attackKey = KeyCode.Mouse0;
	
	//-------
	
	public bool allowRun = true;
	public bool allowAtack = true;
	public bool allowJump = true;
	public bool sendStateMessages = true;
	public bool movement=false;
	
	//Components
	
	Rigidbody myRigidbody; 
	public GameObject inventario;
	//public GameObject cantimplora;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody>();
		StartCoroutine("FSM");
		animator= this.GetComponentInChildren<Animator>();

	}

	void Animate(){
	
	if (movement == true) {


						if (state == playerStates.FULLSPEED) {
								animator.SetInteger ("RunSpeed", 1);
								myRigidbody.drag = 0;
						} else
								animator.SetInteger ("RunSpeed", 2);


	
				} else {

						animator.SetInteger ("RunSpeed", 0);
			myRigidbody.drag = 10;
				}

	}


	void TareaVerdadera(){
		if(inventario.GetComponent<Inventory>().type==0 && inventario.GetComponent<Inventory>().esPocima==true){
			//	Destroy(inventario);
			inventario=GameObject.Find ("cantiplora_standard");
			GameObject.Find ("stone_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
			inventario.GetComponent<Inventory>().esPocima=true;
			inventario.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+4);
		}
		
		if(inventario.GetComponent<Inventory>().type==1 && inventario.GetComponent<Inventory>().esPocima==false){
			//	Destroy(inventario);
			inventario=GameObject.Find ("stone_standard");
			GameObject.Find ("cantiplora_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
			inventario.GetComponent<Inventory>().esPocima=false;
			inventario.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
		}



	}

	void TareaMiente(){
		if(inventario.GetComponent<Inventory>().type==1 && inventario.GetComponent<Inventory>().esPocima==true){
			//	Destroy(inventario);
			inventario=GameObject.Find ("stone_standard");
			GameObject.Find ("cantiplora_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
			inventario.GetComponent<Inventory>().esPocima=true;
			inventario.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
		}
		
		if(inventario.GetComponent<Inventory>().type==0 && inventario.GetComponent<Inventory>().esPocima==false){
			//	Destroy(inventario);
			inventario=GameObject.Find ("cantiplora_standard");
			GameObject.Find ("stone_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
			inventario.GetComponent<Inventory>().esPocima=false;
			inventario.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+4);
		}
		
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Animate();
			
		RaycastHit hit = new RaycastHit();
		
		if (Physics.Raycast (transform.position, -Vector3.up, out hit, 100)) {
			Vector3 floorCast = transform.position;
			floorCast.y = hit.point.y;
			transform.position = floorCast;
		}

		life=this.GetComponent<userStates>().sed;
		if(state != playerStates.FREEZED ){
			state=playerStates.FULLSPEED;
			/*
			if(life>=0 && life<=limitlow) 
				state=playerStates.FULLSPEED;
			if(life<limitsup && life>=limitlow) 
				state=playerStates.MIDSPEED;
			if(life>limitsup) 
				state=playerStates.LOWSPEED;
				*/
		}
		if(life==100) 
			state=playerStates.FALLING;
		isColision=false;
		if(inventario!=null){
			//modifico las velocidades por llevar peso


			/*	if(life>=0 && life<=limitlow) 
					state=playerStates.FULLSPEED;
				if(life<limitsup && life>=limitlow) 
					state=playerStates.MIDSPEED;
				if(life>limitsup) 
					state=playerStates.LOWSPEED;*/


			//Es real todo
			if(life>=0 && life<=limitlow){
				TareaVerdadera();


			}
			// al 66 por ciento es real
			if(life<limitsup && life>=limitlow){

				if(vision>33){
					TareaVerdadera();

				}
				else{
					TareaMiente();
//					
				}

			}
			// al 33 por ciento es real
			
			if(life>limitsup){ 
				if(vision>66){
					TareaVerdadera();
					
				}
				else{
					TareaMiente();
					//					
				}
			}

		}
	
	
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


	void OnTriggerEnter (Collider other) {
		//other.gameObject.parent=this.gameObject.transform;
		if(other.gameObject.tag=="stone" && inventario==null){
			//inventario=other.gameObject;
		//	Debug.Log(other.gameObject.tag);
			isColision=true;
			if(Input.GetKey(reset)){
				vision=Random.Range(1,100);
				GameObject.Find ("stone_standard").GetComponent<Inventory>().esPocima=other.gameObject.GetComponent<Inventory>().esPocima;
				Destroy(other.gameObject);
				inventario=GameObject.Find ("stone_standard");
				//cojo peso
				FullSpeed=FullSpeed*penalizacion;
				MidSpeed=MidSpeed*penalizacion;
				LowSpeed=LowSpeed*penalizacion;
			}
		}
		//Destroy(other.gameObject);
	}
	void OnTriggerStay (Collider other) {
		//other.gameObject.parent=this.gameObject.transform;
		if(other.gameObject.tag=="stone" && inventario==null){
			//
		//	Debug.Log(other.gameObject.tag);
			isColision=true;
			if(Input.GetKey(reset)){
				vision=Random.Range(1,100);
				GameObject.Find ("stone_standard").GetComponent<Inventory>().esPocima=other.gameObject.GetComponent<Inventory>().esPocima;
				Destroy(other.gameObject);
				inventario=GameObject.Find ("stone_standard");
				//cojo peso
				FullSpeed=FullSpeed*penalizacion;
				MidSpeed=MidSpeed*penalizacion;
				LowSpeed=LowSpeed*penalizacion;
			}

		}
		//Destroy(other.gameObject);
	}
	
	//STATE BIT SCRIPTS
	
	//IDLE
	void idle(){
		movement=false;
	
		/*if(movementKey()){
			if(life>=0 && life<=limitlow) 
				state=playerStates.FULLSPEED;
			if(life<limitsup && life>=limitlow) 
				state=playerStates.MIDSPEED;
			if(life>limitsup) 
				state=playerStates.LOWSPEED;*/
		//	else state = playerStates.WALKING; //If any movement key is pressed while idle it goes to walking
		/*	if(life=>100 && life<75)
				state=playerStates.WALKING;
			/*if(life<75 && life>=25)
				state=playerStates.MIDSPEED;
			if(life<25)
				state=playerStates.LOWSPEED;*/
		//}
	// END IDLE
	}
	
	//RUNNING AND WALKING
	void moving(float vel){
		movement=true;
		if(!movementKey()){ state = playerStates.IDLE;
			movement=false;
		}
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

		if(inventario!=null){
			if(inventario.GetComponent<Inventory>().type==0)
				inventario.transform.position= new Vector3(this.transform.position.x,this.transform.position.y+1,this.transform.position.z);
			else
				inventario.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+4);
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

		if(Input.GetKey(reset)){
			int angulo=Random.Range(1,360);
			Debug.Log (angulo);
			//		this.transform.position= new Vector3(GameObject.Find("tower").transform.x+Mathf.Sin(angulo)*60f,GameObject.Find("tower").transform.y,GameObject.Find("tower").transform.z+Mathf.Cos(angulo)*60);
			this.transform.position= new Vector3(GameObject.Find("tower").transform.position.x+Mathf.Sin(angulo)*distanceTower,this.transform.position.y,GameObject.Find("tower").transform.position.z+Mathf.Cos(angulo)*distanceTower);
			//if(grounded) 
			Instantiate(explosion, transform.position, Quaternion.identity );
			state = playerStates.IDLE;
			this.GetComponent<userStates>().sed=0;
		}
		else{
			Debug.Log ("muertooooo");
		}


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
	
	void move_drinking(){
		state = playerStates.FREEZED;

	}

	void move_idle(){
		if(state == playerStates.FREEZED){		
			state = playerStates.IDLE;
		}
	}
	
	
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

	IEnumerator time(float duration)
		
	{
		
		yield return new WaitForSeconds(duration); //Wait
			isTexto=0;

	
		
		// Debug.Log("End Wait() function and the time is: "+Time.time);
		
	}

	void OnGUI(){
		//---
		if(state==playerStates.FALLING){
		GUIStyle simple = new GUIStyle (GUIStyle.none);
		GUI.color = Color.red;
		simple.normal.textColor = Color.red;
		GUI.TextArea (new Rect(275,275,200,20),"You lose pulsa Space",simple);
		}
		if(isColision==true){
			GUIStyle simple = new GUIStyle (GUIStyle.none);
			GUI.color = Color.red;
			simple.normal.textColor = Color.red;
			GUI.TextArea (new Rect(275,275,200,20),"Para coger la piedra pulsa Space",simple);
		}
		if(Input.GetKey(attackKey) && inventario!=null){
			if(inventario.GetComponent<Inventory>().esPocima==true){
				isTexto=1;
				StartCoroutine(time(1f));
				inventario=null;
				FullSpeed=FullSpeed/penalizacion;
				MidSpeed=MidSpeed/penalizacion;
				LowSpeed=LowSpeed/penalizacion;
				GameObject.Find ("stone_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
				GameObject.Find ("cantiplora_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);

			}

		}
		if(Input.GetKey(attackKey) && inventario!=null ){
			if(inventario.GetComponent<Inventory>().esPocima==false){
				isTexto=2;
				StartCoroutine(time(1f));
				inventario=null;
				//cojo peso
				FullSpeed=FullSpeed/penalizacion;
				MidSpeed=MidSpeed/penalizacion;
				LowSpeed=LowSpeed/penalizacion;
				GameObject.Find ("stone_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
				GameObject.Find ("cantiplora_standard").transform.position=new Vector3(this.transform.position.x,0,this.transform.position.z);
			}
			
		}

		if(isTexto==1){
			GUIStyle simple = new GUIStyle (GUIStyle.none);
			GUI.color = Color.red;
			simple.normal.textColor = Color.red;
			this.GetComponent<userStates>().sed=0;
			GUI.TextArea (new Rect(275,275,200,20),"Has recuperado salud",simple);

		}
		if(isTexto==2){
			GUIStyle simple = new GUIStyle (GUIStyle.none);
			GUI.color = Color.red;
			simple.normal.textColor = Color.red;
			GUI.TextArea (new Rect(275,275,200,20),"Es una triste piedra",simple);
			
		}


		
	}


	
}
