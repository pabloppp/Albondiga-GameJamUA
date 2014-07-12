using UnityEngine;
using System.Collections;

public class playerDrinks : MonoBehaviour {

	public KeyCode drinkKey = KeyCode.Space;
	public bool playerDrinking = false;
	private bool detectingOasis = false;
	public float oasisWater = 35f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnGUI(){
		if(detectingOasis && !playerDrinking){		
			GUI.Label (new Rect (Screen.width/2-50,Screen.height*0.8f,200,50), "Press SPACE to drink");
		}
	}

	void OnTriggerStay(Collider other){
		//print ("touching...");
		if(other.gameObject.tag == "water"){
			detectingOasis = true;
			if(Input.GetKeyDown(drinkKey) && !playerDrinking){
				print ("drinking...");
				StartCoroutine("drinking");
			}

		} 
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "water"){
			detectingOasis = false;
		}

	}

	IEnumerator drinking(){
	
		SendMessage("move_drinking", SendMessageOptions.DontRequireReceiver);
		playerDrinking = true;
		SendMessage("addSedOasis", oasisWater, SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(3);	
		SendMessage("move_idle",SendMessageOptions.DontRequireReceiver);
		playerDrinking = false;
	}


	
}
