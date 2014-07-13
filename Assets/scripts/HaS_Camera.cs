using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngineInternal;

/// <summary>
/// Script para controlar la camara en 3ª persona, configurable
/// </summary>

/// <description>
/// You can add it directly to the camera but it's beter that you
/// create an empty object, we'll call it the 'cameraMan' then assign this script to it
/// finally make the camera a child of the cameraMan, set up your preferences and enjoy
/// </description>

public class HaS_Camera : MonoBehaviour {
	
	public enum modes{
		follow,
		cinematic
	}
	
	public Transform target;
	public modes mode = modes.follow;
	public bool fixedCamera = false;
	public bool restrictFloor = true;
	public float transitionSpeed = 10;
	public float rotationSpeed = 100;
	public float mouseSensitivity = 5;
	public float zoomSensitivity = 10;
	public float minZoomDist = 2;
	public float maxZoomDist = 20;
	public float currentZoom = 4;
	public Vector2 angleToTarget;
	public Vector3 tiltCamera = new Vector3(0,0,0);
	public Vector3 targetDisplacement = new Vector3(0,0,0);
	
	public Transform realCamera;
	
	// Use this for initialization
	void Start () {
		
		initialize();
		//gameObject.tag = "CameraMan";
		
	}
	
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(mode == modes.follow && target != null){
			
			placeCamera(false);
			
			//transform.position = target.position+desf;
			if(!fixedCamera){
				
				if(Input.GetAxis("Mouse ScrollWheel") != 0){
					currentZoom = Mathf.Clamp( currentZoom-Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime*zoomSensitivity, minZoomDist, maxZoomDist);
				}
				
				if(Input.GetMouseButton(1) ){
					angleToTarget.y = Mathf.Clamp(angleToTarget.y+Input.GetAxis("Mouse Y")*Time.deltaTime*mouseSensitivity, -Mathf.PI/2, Mathf.PI/2);
					angleToTarget.x = angleToTarget.x+Input.GetAxis("Mouse X")*Time.deltaTime*mouseSensitivity; // Mathf.Clamp(*Time.deltaTime*mouseSensitivity, -Mathf.PI/2, Mathf.PI/2);
				}
				
			}

			if(restrictFloor){
				//angleToTarget.y = Mathf.Clamp(angleToTarget.y+Input.GetAxis("Mouse Y")*Time.deltaTime*mouseSensitivity, -Mathf.PI/2, 0);
				RaycastHit hit = new RaycastHit();
				
				if (Physics.Raycast (transform.position-Vector3.up, Vector3.up, out hit, 100)) {
					if(hit.collider.gameObject.tag == "floor"){
						Vector3 floorCast = transform.position;
						floorCast.y = hit.point.y+1;
						transform.position = floorCast;
						Vector2 newAngle = getAnglesXY(transform.position, target.position);
						angleToTarget.y = newAngle.y;
					}
				}
				
				
			} 
			
			
		}
		else if(mode == modes.cinematic){
			//no se que hacer aqui...
		}


		
		
	}
	
	public void initialize(){
		if(target != null) angleToTarget = getAnglesXY(transform.position, target.position);
		
		if(transform.GetChild(0).CompareTag("MainCamera")){
			realCamera = transform.GetChild(0);
		}
	}
	
	public void placeCamera(bool force){
		Vector3 whereToGo = new Vector3(0,0,0);
		
		//float distance = Vector3.Distance(transform.position, target.position);
		
		//Debug.Log("angle: "+Mathf.Rad2Deg*angleToTarget.y);
		
		whereToGo.y = -Mathf.Sin(angleToTarget.y)*currentZoom;
		//Debug.Log(angleToTarget.y);
		
		float rectDist = Mathf.Sqrt(Mathf.Pow(currentZoom, 2)-Mathf.Pow(whereToGo.y, 2));
		
		whereToGo.y = target.position.y+whereToGo.y;
		
		whereToGo.x = target.position.x-Mathf.Sin(angleToTarget.x)*rectDist;
		
		whereToGo.z = target.position.z-Mathf.Cos(angleToTarget.x)*rectDist;
		
		//Debug.Log("angles: "+angleToTarget.x+" "+angleToTarget.y);
		//Debug.Log("wheretogo: "+whereToGo.x+" "+whereToGo.y+" "+whereToGo.z);D
		
		if(force || transitionSpeed == 0) transform.position = whereToGo;
		else transform.position = Vector3.Lerp(transform.position, whereToGo, Time.deltaTime*transitionSpeed);
		
		
		//Mirar hacia el objetivo
		//transform.LookAt(target.position);
		Vector3 pos = target.position-transform.position;
		Quaternion newRot = Quaternion.LookRotation(pos);
		Quaternion tiltRot = Quaternion.Euler(tiltCamera);
		newRot *= tiltRot;
		if(force || rotationSpeed == 0) transform.rotation = newRot;
		else transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime*rotationSpeed);
		//transform.Rotate(tiltCamera);
		
		if(realCamera != null){
			realCamera.position = transform.position+targetDisplacement;
			Debug.DrawRay(target.position, targetDisplacement);
			Debug.DrawRay(transform.position, targetDisplacement);
		}
	}
	
	public Vector2 getAnglesXY(Vector3 a, Vector3 b){
		
		Vector2 angulos = new Vector2(0,0);
		float distX = b.x - a.x;
		float distY = b.y - a.y;
		float distZ = b.z - a.z;
		
		float hipote = Mathf.Sqrt(Mathf.Pow(distX, 2)+Mathf.Pow(distZ, 2));
		
		//float tangente = ditsX/distZ;
		//angulos.x = Mathf.Atan(hipoH);
		
		angulos.x = Mathf.Atan2(distX, distZ);
		
		angulos.y = Mathf.Atan2(distY, hipote);
		
		return angulos;
		
	}
	
}

[CustomEditor(typeof(HaS_Camera))]
public class MyScriptEditor : Editor
{
	
	HaS_Camera.modes mode;
	
	public override void OnInspectorGUI()
	{
		HaS_Camera context = (HaS_Camera)target;
		GUILayout.Space(10);
		//mode = EditorGUILayout.Popup("> Camera Mode", mode, options);
		mode = (HaS_Camera.modes)EditorGUILayout.EnumPopup("> Camera Mode", context.mode);
		
		GUILayout.Space(10);
		if(mode == HaS_Camera.modes.follow){
			context.mode = HaS_Camera.modes.follow;
			
			context.target = EditorGUILayout.ObjectField("Target", context.target, typeof(Transform), true) as Transform;
			
			if(context.transform.GetChild(0).CompareTag("MainCamera")){
				GUILayout.Label(new GUIContent("Target displacement:"));
				GUILayout.BeginHorizontal();
				GUILayout.Label(new GUIContent("x:"));
				context.targetDisplacement.x = EditorGUILayout.FloatField(context.targetDisplacement.x);
				GUILayout.Label(new GUIContent("y:"));
				context.targetDisplacement.y = EditorGUILayout.FloatField(context.targetDisplacement.y);
				GUILayout.Label(new GUIContent("z:"));
				context.targetDisplacement.z = EditorGUILayout.FloatField(context.targetDisplacement.z);
				EditorGUILayout.EndHorizontal();
			}
			
			
			context.transitionSpeed = EditorGUILayout.FloatField("Camera Speed",context.transitionSpeed);
			context.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed",context.rotationSpeed);
			if(context.fixedCamera)context.fixedCamera = GUILayout.Toggle(context.fixedCamera, "Fixed Camera", "Button");
			else context.fixedCamera = GUILayout.Toggle(context.fixedCamera, "Free Camera", "Button");
			GUILayout.Space(10);
			GUILayout.Label("- Camera Position:");
			context.angleToTarget.x = EditorGUILayout.FloatField("Horizontal angle",context.angleToTarget.x);
			context.angleToTarget.y = EditorGUILayout.FloatField("Vertical angle",context.angleToTarget.y);
			
			GUILayout.Label(new GUIContent("Camera Tilt:"));
			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("x:"));
			context.tiltCamera.x = EditorGUILayout.FloatField(context.tiltCamera.x);
			GUILayout.Label(new GUIContent("y:"));
			context.tiltCamera.y = EditorGUILayout.FloatField(context.tiltCamera.y);
			GUILayout.Label(new GUIContent("z:"));
			context.tiltCamera.z = EditorGUILayout.FloatField(context.tiltCamera.z);
			EditorGUILayout.EndHorizontal();
			
			context.currentZoom = EditorGUILayout.Slider("Current Zoom", context.currentZoom, context.minZoomDist, context.maxZoomDist);
			GUILayout.BeginHorizontal();
			context.minZoomDist = EditorGUILayout.FloatField(context.minZoomDist);
			GUILayout.Label(new GUIContent("< zoom <"));
			context.maxZoomDist = EditorGUILayout.FloatField(context.maxZoomDist);
			GUILayout.EndHorizontal();
			
			if(!context.fixedCamera){
				GUILayout.Space(10);
				GUILayout.Label("- Free Camera Options:");
				context.mouseSensitivity = EditorGUILayout.FloatField("Mouse Sensitivity",context.mouseSensitivity);
				context.zoomSensitivity = EditorGUILayout.FloatField("Zoom Sensitivity",context.zoomSensitivity);
				context.restrictFloor = GUILayout.Toggle(context.restrictFloor, "Limit angle to floor");
			}
			
			GUILayout.Space(30);
			
			if(GUILayout.Button("Focus Camera")){
				context.initialize();
				context.placeCamera(true);
			}
			
			if(context.GetComponent("Animation") != null){
				Animation anim = (Animation)context.GetComponent("Animation");
				anim.enabled = false;
			}
		}
		else if(mode == HaS_Camera.modes.cinematic){
			context.mode = HaS_Camera.modes.cinematic;
			
			if(context.GetComponent("Animation") == null) GUILayout.Label("Add an Animation componen ---");
			else{
				Animation anim = (Animation)context.GetComponent("Animation");
				anim.enabled = true;
			}
		}
	}
}