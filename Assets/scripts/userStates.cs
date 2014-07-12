using UnityEngine;
using System.Collections;

public class userStates : MonoBehaviour {
		
	public enum modes{
		IDLE,
		WALKING,
		FULLSPEED,
		MIDSPEED,
		LOWSPEED
	}

	public float sed = 0;
	public float sedMax = 100;
	public float staticLosePcent = 1;//%
	public float dinamicLosePcent = 2;//%
	public float loseEachSeconds = 5;//seconds
	float _warningMin = 0.20f;

	public modes mode = modes.IDLE;

	public Texture thirstBar;
	public Color okColor;
	public Color badColor;

	public Texture warning;

	// Use this for initialization
	void Start () {
		StartCoroutine (AddSed());
	}
	
	// Update is called once per frame
	void Update () {
		if(this.GetComponent<player_movement>().movement==false){
			mode=modes.IDLE;
		}
		else{
			mode=modes.WALKING;
		}
	}

	public float getSedPercent(){
		return (sed / sedMax);
	}

	void addSedOasis(float quant){
		sed -= quant;
		if(sed < 0) sed = 0;
	}

	IEnumerator AddSed(){

		while (true) {

			yield return new WaitForSeconds(loseEachSeconds); //wait
			if(mode==modes.IDLE){
				sed = Mathf.Clamp(sed+sedMax*(staticLosePcent/100), 0, sedMax);
				mode=modes.IDLE;
			}
			else{
				sed = Mathf.Clamp(sed+sedMax*(dinamicLosePcent/100), 0, sedMax);
				mode=modes.WALKING;
			}
			/*switch(this.GetComponent<player_movement>().state){
			case playerStates.IDLE:
				sed = Mathf.Clamp(sed+sedMax*(staticLosePcent/100), 0, sedMax);
				break;
			case playerStates.FULLSPEED:
				sed = Mathf.Clamp(sed+sedMax*(dinamicLosePcent/100), 0, sedMax);
				break;
			case playerStates.MIDSPEED:
				sed = Mathf.Clamp(sed+sedMax*(dinamicLosePcent/100), 0, sedMax);
				break;
			case playerStates.LOWSPEED:
				sed = Mathf.Clamp(sed+sedMax*(dinamicLosePcent/100), 0, sedMax);
				break;

			
			}*/

			if(this.GetComponent<player_movement>())
				sed = Mathf.Clamp(sed+sedMax*(staticLosePcent/100), 0, sedMax);
		}

	}

	void OnGUI(){
		float wmax = Screen.width * 0.2f;
		float hmax = Screen.height * 0.05f;
		GUI.color = new Color (0, 0, 0, 0.2f);
		GUI.DrawTexture(new Rect (10, 20, wmax+10, hmax+10), thirstBar);
		GUI.color = okColor*(1-getSedPercent())+badColor*getSedPercent();
		//Draw sed level
		GUI.DrawTexture(new Rect (15, 25, (1-getSedPercent())*wmax, hmax), thirstBar);
		//---
		GUIStyle simple = new GUIStyle (GUIStyle.none);
		GUI.color = Color.white;
		simple.normal.textColor = Color.white;
		GUI.TextArea (new Rect(15,5,200,20),"Sed:",simple);

		//warning
		if (1-getSedPercent() <= _warningMin) {
			//Debug.Log("---");
			GUI.DrawTexture(new Rect (wmax+30, 15, hmax, wmax*0.31f), warning, ScaleMode.ScaleAndCrop);
		}

	}
}
