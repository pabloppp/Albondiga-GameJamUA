using UnityEngine;
using System.Collections;

using UnityEditor;
using UnityEngineInternal;using UnityEditor;
using UnityEngineInternal;

[CustomEditor(typeof(HaS_Camera))]
public class HaS_CameraEditor : Editor
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