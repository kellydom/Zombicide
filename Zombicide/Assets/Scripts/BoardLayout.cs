using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardLayout : MonoBehaviour {
	public static BoardLayout S;

	public List<Vector3> zonePositions;
	public List<Vector3> zoneSizes;
	public List<bool> isStreetZone;


	public GameObject zonePlanePrefab;
	public List<GameObject> createdZones;

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
	}

	void Awake(){
		createdZones = new List<GameObject>();

		for(int i = 0; i < zonePositions.Count; ++i){
			GameObject newZone = Instantiate(zonePlanePrefab, zonePositions[i] + new Vector3(0, 0.051f, 0), Quaternion.identity) as GameObject;
			newZone.transform.localScale = zoneSizes[i] + new Vector3(0, .01f, 0);

			newZone.GetComponent<ZoneScript>().zoneNum = i;

			newZone.GetComponent<ZoneScript>().addLinePoints(zoneSizes[i] + new Vector3(0, .01f, 0));

			createdZones.Add(newZone);

			/*Color c = new Color(1, 0, 0, 0.2f);

			if(isStreetZone[i]) c = new Color(0, 0, 1, 0.2f);
			newZone.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = c;*/

		}
	}

}
