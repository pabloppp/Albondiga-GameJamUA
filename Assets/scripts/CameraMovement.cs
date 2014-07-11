using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public int distance;
	public int min;
	public int max;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") < 0 && min<distance) // back
		{
			distance--;
			
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && max>distance)  // forward
		{
			distance++;
		}
		Vector3 positionplayer= GameObject.Find("Player").transform.position;
		Camera.main.transform.position= new Vector3(positionplayer.x,positionplayer.y,positionplayer.z-distance);
	}
}
