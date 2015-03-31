using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;


[CustomEditor(typeof(BoardLayout))]
public class BoardEditor : Editor {

	BoardLayout boardLayout;
	bool showZones;


	void Start(){
		showZones = false;
	}

	public override void OnInspectorGUI ()
	{
		
		//This is the pathMap script this is a custom editor for
		if(boardLayout == null){
			boardLayout = (BoardLayout)target;
		}

		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("zonePositions"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("zoneSizes"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("isStreetZone"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("zonePlanePrefab"));

		serializedObject.ApplyModifiedProperties();

		if(showZones){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width/3);
			if (GUILayout.Button("Hide Zones")){
				
				showZones = false;
				
				//This is needed anytime anything might change in the sceneview
				SceneView.RepaintAll();
			}			
			GUILayout.Space(Screen.width/3);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width/3);
			if (GUILayout.Button("Add Zone")){
				
				boardLayout.zonePositions.Add (boardLayout.zonePositions[boardLayout.zonePositions.Count - 1]);
				boardLayout.zoneSizes.Add (boardLayout.zoneSizes[boardLayout.zoneSizes.Count - 1]);
				boardLayout.isStreetZone.Add (boardLayout.isStreetZone[boardLayout.isStreetZone.Count - 1]);
				
				//This is needed anytime anything might change in the sceneview
				SceneView.RepaintAll();
			}			
			GUILayout.Space(Screen.width/3);
			EditorGUILayout.EndHorizontal ();
		}
		else {
			//not editing
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width/3);
			if (GUILayout.Button("Show Zones")){
				showZones = true;
				SceneView.RepaintAll();
			}
			GUILayout.Space(Screen.width/3);
			EditorGUILayout.EndHorizontal ();
		}

	}

	void OnSceneGUI()
	{
		if(!showZones) {SceneView.RepaintAll(); return;}
		if(boardLayout.zonePositions.Count != boardLayout.zoneSizes.Count || boardLayout.zonePositions.Count != boardLayout.isStreetZone.Count){
			return;
		}
		for(int i = 0; i < boardLayout.zonePositions.Count; ++i){
			Vector3[] vert = new Vector3[4];
			vert[0] = boardLayout.transform.position + boardLayout.zonePositions[i] + new Vector3(boardLayout.zoneSizes[i].x, 0, boardLayout.zoneSizes[i].z);
			vert[1] = boardLayout.transform.position + boardLayout.zonePositions[i] + new Vector3(-boardLayout.zoneSizes[i].x, 0, boardLayout.zoneSizes[i].z);
			vert[2] = boardLayout.transform.position + boardLayout.zonePositions[i] + new Vector3(-boardLayout.zoneSizes[i].x, 0, -boardLayout.zoneSizes[i].z);
			vert[3] = boardLayout.transform.position + boardLayout.zonePositions[i] + new Vector3(boardLayout.zoneSizes[i].x, 0, -boardLayout.zoneSizes[i].z);

			Color c = new Color(1, 0, 0, 0.1f);
			if(boardLayout.isStreetZone[i]) c = new Color(0, 0, 1, 0.1f);

			Handles.DrawSolidRectangleWithOutline(vert, c, Color.white);
		}
		
		
		SceneView.RepaintAll();
	}
}
