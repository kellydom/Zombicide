using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject boardObject;
	public GameObject spawnZone;

	bool hasSpawned = false;

	// Use this for initialization
	void Start () {
		float closestDist = int.MaxValue;

		BoardLayout bl = boardObject.GetComponent<BoardLayout>();
		for(int i = 0; i < bl.zonePositions.Count; ++i){
			Vector3 pos = bl.zonePositions[i];
			float dist = Vector3.Distance(transform.position, pos);
			if(dist < closestDist){
				closestDist = dist;
				spawnZone = bl.createdZones[i];
			}
		}

	}
	
	// Update is called once per frame
	void Update () {	
		if(!hasSpawned && GameController.S != null){
			GameController.S.SpawnSurvivors(spawnZone);
			hasSpawned = true;

		}
	}
}
