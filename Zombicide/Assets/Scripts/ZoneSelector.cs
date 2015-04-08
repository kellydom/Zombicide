using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneSelector : MonoBehaviour {
	public static ZoneSelector S;

	GameObject currZone;
	public GameObject CurrZone{
		get{return currZone;}
	}

	// Use this for initialization
	void Start () {
		if(S == null){
			S = this;
		}
		else
		{
			if(this != S)
				Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Plane boardPlane = new Plane(Vector3.up, new Vector3(0,0.05f,0));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hit;

		if(boardPlane.Raycast(ray, out hit)){
			Vector3 hitPos = ray.GetPoint(hit);
			GameObject zone = GetZoneAt(hitPos);
			HighlightZone(zone);
		}

		if(Input.GetMouseButtonDown(0)){
			if(currZone != null){
				GameController.S.ClickedCurrZone();
			}
		}
	
	}

	public GameObject GetZoneAt(Vector3 pos){
		GameObject closestZone = null;
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			GameObject zone = BoardLayout.S.createdZones[i];
			Bounds oldBounds = zone.GetComponent<BoxCollider>().bounds;
			Bounds newBounds = new Bounds(oldBounds.center, oldBounds.size + new Vector3(0, 2, 0));
			
			if(newBounds.Contains(pos)){
				closestZone = zone;
				break;
			}
		}
		return closestZone;
	}

	public void HighlightZone(GameObject zone){
		if(ActionWheel.S.mouseInWheel || ActionWheel.S.mouseInWheelButton){
			if(currZone != null){
				//currZone.GetComponent<ZoneScript>().Unhighlight();
				currZone = null;
			}
			return;
		}

		if(currZone == zone) return;
		if(zone == null){
			if(currZone != null){
				//currZone.GetComponent<ZoneScript>().Unhighlight();
			}
			currZone = null;
			return;
		}
		
		//zone.GetComponent<ZoneScript>().Highlight();
		if(currZone != null){
			//currZone.GetComponent<ZoneScript>().Unhighlight();
		}
		currZone = zone;
	}

	public List<GameObject> GetNeighborsOf(GameObject zone){
		List<GameObject> neighbors = new List<GameObject>();
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			GameObject testZone = BoardLayout.S.createdZones[i];
			if(IsNeighborOf(zone, testZone)){
				neighbors.Add (testZone);
			}
		}

		return neighbors;
	}

	public void HighlightNeighborsOf(GameObject zone){
		List<GameObject> neighbors = GetNeighborsOf(zone);
		for(int i = 0; i < neighbors.Count; ++i){
			neighbors[i].GetComponent<ZoneScript>().Highlight();
		}

	}

	public bool IsNeighborOf(GameObject zone, GameObject possibleNeighbor){
		if(zone.GetComponent<ZoneScript>() == null) return false;
		if(possibleNeighbor.GetComponent<ZoneScript>() == null) return false;

		int zoneOne = zone.GetComponent<ZoneScript>().zoneNum;
		int zoneTwo = possibleNeighbor.GetComponent<ZoneScript>().zoneNum;


		for(int i = 0; i < BoardLayout.S.neighborZones.Count; ++i){
			int one = (int)BoardLayout.S.neighborZones[i].x;
			int two = (int)BoardLayout.S.neighborZones[i].y;

			if( (zoneOne == one && zoneTwo == two) || (zoneOne == two && zoneTwo == one) ){
				return true;
			}
		}
		for(int i = 0; i < BoardLayout.S.doorConnections.Count; ++i){
			if(!BoardLayout.S.doorConnections[i].isOpened) continue;

			int one = BoardLayout.S.doorConnections[i].zoneOne;
			int two = BoardLayout.S.doorConnections[i].zoneTwo;
			
			if( (zoneOne == one && zoneTwo == two) || (zoneOne == two && zoneTwo == one) ){
				return true;
			}

		}


		return false;
	}

	public List<GameObject> GetZonesCanSeeFrom(GameObject zone){
		if(zone.GetComponent<ZoneScript>() == null) return new List<GameObject>();
		List<GameObject> zonesCanSee = new List<GameObject>();
		zonesCanSee.Add (zone);

		List<GameObject> originalNeighbors = new List<GameObject>();
		List<string> sightDirections = new List<string>();
		
		float x = zone.transform.position.x;
		float z = zone.transform.position.z;
		
		originalNeighbors = GetNeighborsOf(zone);
		for(int i = 0; i < originalNeighbors.Count; ++i){
			ZoneScript newZone = originalNeighbors[i].GetComponent<ZoneScript>();
			//newZone.Highlight();
			zonesCanSee.Add (originalNeighbors[i]);

			if(BoardLayout.S.isStreetZone[newZone.zoneNum]){
				float newX = newZone.transform.position.x;
				float newZ = newZone.transform.position.z;
				
				float xDiff = Mathf.Abs (newX - x);
				float zDiff = Mathf.Abs (newZ - z);
				
				if(xDiff > zDiff) sightDirections.Add ("x");
				else sightDirections.Add( "z");
			}
			else{
				originalNeighbors.RemoveAt(i);
				i--;
			}
		}
		
		List<GameObject> zonesChecked = new List<GameObject>();
		zonesChecked.AddRange(originalNeighbors);
		zonesChecked.Add (zone);
		
		//for each neighbor, look along sight directions
		for(int i = 0; i < originalNeighbors.Count; ++i){
			//the keyvaluepair is so we know where the next neighbor came from
			List<KeyValuePair<GameObject, GameObject>> neighbors = new List<KeyValuePair<GameObject,GameObject>>();
			List<GameObject> firstNeighbors = GetNeighborsOf(originalNeighbors[i]);
			
			for(int j = 0; j < firstNeighbors.Count; ++j){
				KeyValuePair<GameObject, GameObject> newPair = new KeyValuePair<GameObject, GameObject>(originalNeighbors[i], firstNeighbors[j]);
				neighbors.Add(newPair);
			}
			while(neighbors.Count > 0){
				if(zonesChecked.Contains(neighbors[0].Value)){
					neighbors.RemoveAt(0);
					continue;
				}
				
				GameObject newZone = neighbors[0].Value;
				
				float ogX = neighbors[0].Key.transform.position.x;
				float ogZ = neighbors[0].Key.transform.position.z;
				
				float newX = newZone.transform.position.x;
				float newZ = newZone.transform.position.z;
				
				float xDiff = Mathf.Abs(newX - ogX);
				float zDiff = Mathf.Abs(newZ - ogZ);
				
				if(sightDirections[i] == "x" && xDiff > zDiff){
					
					//newZone.GetComponent<ZoneScript>().Highlight();
					zonesCanSee.Add (newZone);
					if(BoardLayout.S.isStreetZone[newZone.GetComponent<ZoneScript>().zoneNum]){
						
						List<GameObject> nextNeighbors = GetNeighborsOf(newZone);
						for(int j = 0; j < nextNeighbors.Count; ++j){
							KeyValuePair<GameObject, GameObject> newPair = new KeyValuePair<GameObject, GameObject>(newZone, nextNeighbors[j]);
							neighbors.Add(newPair);
						}
					}
				}
				else if(sightDirections[i] == "z" && zDiff >= xDiff){
					//newZone.GetComponent<ZoneScript>().Highlight();
					zonesCanSee.Add (newZone);
					if(BoardLayout.S.isStreetZone[newZone.GetComponent<ZoneScript>().zoneNum]){
						
						List<GameObject> nextNeighbors = GetNeighborsOf(newZone);
						for(int j = 0; j < nextNeighbors.Count; ++j){
							KeyValuePair<GameObject, GameObject> newPair = new KeyValuePair<GameObject, GameObject>(newZone, nextNeighbors[j]);
							print (newPair);
							neighbors.Add(newPair);
						}
					}
				}
				zonesChecked.Add (newZone);
				neighbors.RemoveAt(0);
			}
		}

		return zonesCanSee;
	}

	public void HighlightZonesCanSeeFrom(GameObject zone){
		List<GameObject> zonesCanSee = GetZonesCanSeeFrom(zone);
		foreach(GameObject zoneGO in zonesCanSee){
			zoneGO.GetComponent<ZoneScript>().Highlight();
		}

	}
}
