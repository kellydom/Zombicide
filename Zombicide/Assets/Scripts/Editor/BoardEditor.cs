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
	bool showNeighbors;


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
		EditorGUILayout.PropertyField(serializedObject.FindProperty("neighborZones"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("door"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("doors"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("doorConnections"), true);

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

		if(showNeighbors){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width/3);
			if (GUILayout.Button("Hide Neighbors")){
				
				showNeighbors = false;
				
				//This is needed anytime anything might change in the sceneview
				SceneView.RepaintAll();
			}			
			GUILayout.Space(Screen.width/3);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width/3);
			if (GUILayout.Button("Add Neighbors")){
				
				boardLayout.neighborZones.Add(Vector2.zero);
				
				//This is needed anytime anything might change in the sceneview
				SceneView.RepaintAll();
			}			
			GUILayout.Space(Screen.width/3);
			EditorGUILayout.EndHorizontal ();
		}
		else{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width/3);
			if (GUILayout.Button("Show Neighbors")){
				showNeighbors = true;
				SceneView.RepaintAll();
			}
			GUILayout.Space(Screen.width/3);
			EditorGUILayout.EndHorizontal ();
		}
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(Screen.width/3);
		if (GUILayout.Button("Add Door")){
			
			GameObject newDoor = Instantiate(boardLayout.door, Vector3.zero, Quaternion.identity) as GameObject;
			boardLayout.doors.Add(newDoor);
			BoardLayout.Door d = new BoardLayout.Door();
			boardLayout.doorConnections.Add(d);

			
			//This is needed anytime anything might change in the sceneview
			SceneView.RepaintAll();
		}			
		GUILayout.Space(Screen.width/3);
		EditorGUILayout.EndHorizontal ();

	}

	void OnSceneGUI()
	{
		if(boardLayout == null) return;

		if(boardLayout.zonePositions.Count != boardLayout.zoneSizes.Count || boardLayout.zonePositions.Count != boardLayout.isStreetZone.Count){
			return;
		}
		if(showZones) {
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
		}
		if(showNeighbors){
			for(int i = 0; i < boardLayout.neighborZones.Count; ++i){
				int i1 = (int)boardLayout.neighborZones[i].x;
				int i2 = (int)boardLayout.neighborZones[i].y;
				if(i1 >= boardLayout.zonePositions.Count || i2 >= boardLayout.zonePositions.Count){
					continue;
				}

				Vector3 pos1 = boardLayout.zonePositions[i1];
				Vector3 pos2 = boardLayout.zonePositions[i2];

				Vector3 realPos1 = Vector3.MoveTowards(pos1, pos2, 0.5f);
				Vector3 realPos2 = Vector3.MoveTowards(pos2, pos1, 0.5f);

				Handles.color = Color.yellow;
				//Handles.DrawLine(pos1, pos2);
				Vector3[] vert = new Vector3[2];
				vert[0] = realPos1;
				vert[1] = realPos2;
				Handles.DrawAAPolyLine(10, vert);

			}

			for(int i = 0; i < boardLayout.doors.Count; ++i){
				int i1 = (int) boardLayout.doorConnections[i].zoneOne;
				int i2 = (int) boardLayout.doorConnections[i].zoneTwo;
				
				if(i1 >= boardLayout.zonePositions.Count || i2 >= boardLayout.zonePositions.Count){
					continue;
				}
				
				
				Vector3 pos1 = boardLayout.zonePositions[i1];
				Vector3 pos2 = boardLayout.zonePositions[i2];
				Vector3 doorPos = boardLayout.doors[i].transform.position;


				Handles.color = Color.green;
				//Handles.DrawLine(pos1, pos2);
				Vector3[] vert = new Vector3[3];
				vert[0] = pos1;
				vert[1] = doorPos;
				vert[2] = pos2;
				Handles.DrawAAPolyLine(10, vert);
			}
		}
		
		SceneView.RepaintAll();
	}
}
