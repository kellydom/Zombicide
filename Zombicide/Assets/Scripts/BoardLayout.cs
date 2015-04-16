using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardLayout : MonoBehaviour {
	[System.Serializable]
	public class Door{
		public int zoneOne;
		public int zoneTwo;

		public bool isOpened;
	}


	public static BoardLayout S;

	public List<Vector3> zonePositions;
	public List<Vector3> zoneSizes;
	public List<bool> isStreetZone;

	public List<Vector2> neighborZones;
	public GameObject door;
	public List<GameObject> doors;
	public List<Door> doorConnections;

	public GameObject zonePlanePrefab;
	public List<GameObject> createdZones;
	public List<List<int>> zoneGraph = new List<List<int>>();


	void Start(){
		//Singleton initialization
		if(S == null){
			S = this;
		}
		else
		{
			if(this != S)
				Destroy(this.gameObject);
		}
		StartCoroutine(CreateGraph());
	}

	void Awake(){
		createdZones = new List<GameObject>();

		for(int i = 0; i < zonePositions.Count; ++i){
			GameObject newZone = Instantiate(zonePlanePrefab, zonePositions[i] + new Vector3(0, 0.051f, 0), Quaternion.identity) as GameObject;
			newZone.transform.localScale = zoneSizes[i] + new Vector3(0, 0.01f, 0);

			newZone.GetComponent<ZoneScript>().zoneNum = i;

			newZone.GetComponent<ZoneScript>().addLinePoints(zoneSizes[i] + new Vector3(0, .01f, 0));

			createdZones.Add(newZone);

			/*Color c = new Color(1, 0, 0, 0.2f);

			if(isStreetZone[i]) c = new Color(0, 0, 1, 0.2f);
			newZone.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = c;*/

		}

		for(int i = 0; i < doors.Count; ++i){
			if(doorConnections[i].isOpened){
				doors[i].transform.Rotate(Vector3.right, 180);
			}
		}
	}
	
	IEnumerator CreateGraph(){
		while(ZoneSelector.S == null){
			yield return 0;
			continue;
		}

		int count = BoardLayout.S.createdZones.Count;
		for(int i = 0; i < count; ++i){
			List<int> temp = new List<int>();
			zoneGraph.Add (temp);
		}
		
		for(int i = 0; i < count; ++i){
			foreach(Vector2 vec in neighborZones){
				if((int)vec.x == i) zoneGraph[i].Add ((int)vec.y);
				if((int)vec.y == i) zoneGraph[i].Add ((int)vec.x);
			}
		}
	}

	public void UpdateGraph(int zoneOne, int zoneTwo){

		zoneGraph[zoneOne].Add (zoneTwo);
		zoneGraph[zoneTwo].Add (zoneOne);

	}

}
