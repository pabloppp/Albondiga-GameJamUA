using UnityEngine;
using System.Collections;

public class playerDrinks : MonoBehaviour {

	public KeyCode drinkKey = KeyCode.Space;
	public bool playerDrinking = false;
	private bool detectingOasis = false;
	public float oasisWater;

	public Font f;

	private bool keyPulsada = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		if (Input.GetKeyUp(drinkKey))
		{
			keyPulsada = false;
		}	
	}

	void OnGUI(){
		if (!f) {
			Debug.LogError("No font found, assign one in the inspector.");
			return;
		}

		GUIStyle myStyle = new GUIStyle(GUI.skin.label);
		GUI.skin.font = f;
		myStyle.fontSize = 25;
		myStyle.normal.textColor = Color.black;
		if(detectingOasis && !playerDrinking){		
			GUI.Label (new Rect (Screen.width/2-125,Screen.height*0.8f,250,50), "Press SPACE to drink", myStyle);
		}
	}

	void OnTriggerStay(Collider other){
		print ("touching...");
		if(other.gameObject.tag == "water"){
			detectingOasis = true;
			if(Input.GetKeyDown(drinkKey) && !playerDrinking && !keyPulsada){
				keyPulsada = true;
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
		yield return new WaitForSeconds(0);	
		SendMessage("move_idle",SendMessageOptions.DontRequireReceiver);
		playerDrinking = false;
	}
}
