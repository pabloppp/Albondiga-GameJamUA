using UnityEngine;
using System.Collections;

public class userStates : MonoBehaviour {
		
	public enum modes{
		IDLE,
		WALKING
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
	
	}

	public float getSedPercent(){
		return (sed / sedMax);
	}

	IEnumerator AddSed(){

		while (true) {

			yield return new WaitForSeconds(loseEachSeconds); //wait

			switch(mode){
			case modes.IDLE:
				sed = Mathf.Clamp(sed+sedMax*(staticLosePcent/100), 0, sedMax);
				break;
			case modes.WALKING:
				sed = Mathf.Clamp(sed+sedMax*(dinamicLosePcent/100), 0, sedMax);
				break;
			}
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
