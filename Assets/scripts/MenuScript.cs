using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	

	public Font f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
	if(Input.GetKey (KeyCode.Space))
		   Application.LoadLevel("main");
	}


	
	void OnGUI(){
		if (!f) {
			Debug.LogError("No font found, assign one in the inspector.");
			return;
		}
		
		GUIStyle myStyle = new GUIStyle(GUI.skin.label);
		GUI.skin.font = f;
		myStyle.fontSize = 25;
		myStyle.normal.textColor = Color.red;	
			GUI.Label (new Rect (Screen.width/2-150,Screen.height*0.8f,600,50), "Press SPACE to START", myStyle);

		GUIStyle myStyle2 = new GUIStyle(GUI.skin.label);
		GUI.skin.font = f;
		myStyle2.fontSize = 50;
		myStyle2.normal.textColor = Color.white;	
		GUI.Label (new Rect (Screen.width/2-100,Screen.height*0.05f,600,50), "MIRAGE", myStyle2);

		GUIStyle myStyle3 = new GUIStyle(GUI.skin.label);
		GUI.skin.font = f;
		myStyle.fontSize = 45;
		myStyle.normal.textColor = Color.white;	
		GUI.Label (new Rect (Screen.width/2+120,Screen.height*0.5f,600,50), "FIND THE TOWER", myStyle);


	}
	

}
