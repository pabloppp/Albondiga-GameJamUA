using UnityEngine;
using System.Collections;

public class Illusion : MonoBehaviour {
	public float rangeViewDisappear = 10f;
	public bool isReal = false;

	public float showDistance = 50f;
	private Transform particulas;
	private Transform player;
	private explode exp;

	public KeyCode drinkKey = KeyCode.Space;
	public float waterQuantityPerUse;

	private bool keyWasPressed = false;
	private bool isDrinking = false;

	
	public Font f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;

		f=player.GetComponent<playerDrinks>().f;
		exp = GetComponent<explode>();
	}
	
	void FixedUpdate(){	
		if (!isReal) {
				Disappear ();
		} else if (Vector3.Distance (this.transform.position, player.position) < this.rangeViewDisappear) {
			if(Input.GetKeyUp(drinkKey) && !this.isDrinking)
				this.keyWasPressed = false;
			else if (Input.GetKeyDown(drinkKey) && !this.keyWasPressed)
			{
				this.keyWasPressed = true;

				player.GetComponent<userStates>().sed = Mathf.Max(player.GetComponent<userStates>().sed -
					this.waterQuantityPerUse, 0);
			}
		}
	}

	void OnGUI(){
		if (isReal && Vector3.Distance(this.transform.position, player.position) < this.rangeViewDisappear) {

		GUIStyle myStyle = new GUIStyle(GUI.skin.label);
		GUI.skin.font = f;
		myStyle.fontSize = 25;
		myStyle.normal.textColor = Color.black;
					
				GUI.Label (new Rect (Screen.width/2-125,Screen.height*0.8f,250,50), "Press SPACE to drink", myStyle);

		}
	}

	void Disappear()
	{	
		if (Vector3.Distance (transform.position, player.position) < rangeViewDisappear) {		
			exp.Explode(transform.position+Vector3.up*1.2f);
			Destroy(gameObject);
		}
	}
}