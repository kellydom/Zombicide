using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ZoneSelector : MonoBehaviour {
	public static ZoneSelector S;

	GameObject currZone;
	public GameObject CurrZone{
		get{return currZone;}
	}

	public Image zombAttRem;
	bool selectingEnemy = false;

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

	void GetEnemy(Vector3 pos){
		GameObject closestEnemy = null;
		for(int i = 0; i < GameController.S.allZombies.Count; ++i){
			GameObject zomb = GameController.S.allZombies[i].gameObject;
			Bounds oldBounds = zomb.GetComponent<BoxCollider>().bounds;
			Bounds newBounds = new Bounds(oldBounds.center, oldBounds.size + new Vector3(0, 2, 0));
			
			if(newBounds.Contains(pos)){
				closestEnemy = zomb;
				break;
			}
		}
		print (closestEnemy);
		if(closestEnemy != null){
			Enemy enemy = closestEnemy.GetComponent<Enemy>();
			if(ActionWheel.S.mouseInWheel || ActionWheel.S.mouseInWheelButton) return;
			
			int speed = 1;
			int damage = 1;
			string zombType = "Walker";
			List<GameObject> zoneZombies = new List<GameObject>();
			if(enemy.type == Enemy.EnemyType.Walker){
				zoneZombies = enemy.currZone.GetComponent<ZoneScript>().walkersInZone;
			}
			if(enemy.type == Enemy.EnemyType.Runner){
				zombType = "Runner";
				speed = 2;
				zoneZombies = enemy.currZone.GetComponent<ZoneScript>().runnersInZone;
			}
			if(enemy.type == Enemy.EnemyType.Fatty){
				zombType = "Fatty";
				damage = 2;
				zoneZombies = enemy.currZone.GetComponent<ZoneScript>().fattiesInZone;
			}
			if(enemy.type == Enemy.EnemyType.Abomination){
				zombType = "Abomination";
				damage = 3;
				zoneZombies = enemy.currZone.GetComponent<ZoneScript>().abombInZone;
			}
			
			Vector3 avgPos = Vector3.zero;
			
			foreach(GameObject zombie in zoneZombies){
				avgPos += zombie.transform.position;
			}

			avgPos /= zoneZombies.Count;
			GameController.S.SetZombieNumText(avgPos, zoneZombies.Count, zombType, damage, speed);
			if(!selectingEnemy){
				selectingEnemy = true;
			}
		}
		else{
			GameController.S.MoveZombieNumOff();
			if(selectingEnemy){
				selectingEnemy = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Plane boardPlane = new Plane(Vector3.up, new Vector3(0,0.05f,0));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hit;

		if(boardPlane.Raycast(ray, out hit)){
			Vector3 hitPos = ray.GetPoint(hit);
			GetEnemy(hitPos);
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

	public List<int> GetNeighborsOfInt(int zoneNum){
		List<int> neighbors = new List<int>();
		neighbors.AddRange(BoardLayout.S.zoneGraph[zoneNum]);
		return neighbors;
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
		List<int> neighbors = GetNeighborsOfInt(zone.GetComponent<ZoneScript>().zoneNum);
		foreach(int n in neighbors){
			BoardLayout.S.createdZones[n].GetComponent<ZoneScript>().Highlight();
		}

	}

	public void HighlightZonesInRange(GameObject zone, int close, int far){
		foreach(GameObject nextZone in BoardLayout.S.createdZones){
			int dist = ZoneDistance(zone, nextZone);

			if(dist >= close && dist <= far){
				nextZone.GetComponent<ZoneScript>().Highlight();
			}
		}
	}

	//much better performance-wise than the above
	public void HighlightMoveableZonesUpTo(GameObject zone, int range){
		List<int> zonesChecked = new List<int>();

		List<int> zonesToCheckNow = new List<int>();
		zonesToCheckNow.Add (zone.GetComponent<ZoneScript>().zoneNum);
		List<int> zonesToCheckNext = new List<int>();

		while(range > 0){
			while(zonesToCheckNow.Count > 0){
				int nextZone = zonesToCheckNow[0];
				zonesChecked.Add (nextZone);
				zonesToCheckNow.RemoveAt(0);

				List<int> neighbors = GetNeighborsOfInt(nextZone);
				foreach(int neigh in neighbors){
					if(zonesChecked.Contains(neigh)) continue;
					if(zonesToCheckNow.Contains (neigh)) continue;
					if(zonesToCheckNext.Contains(neigh)) continue;

					zonesToCheckNext.Add (neigh);
					BoardLayout.S.createdZones[neigh].GetComponent<ZoneScript>().Highlight();
				}
			}
			zonesToCheckNow.AddRange(zonesToCheckNext);
			zonesToCheckNext.Clear();

			range--;
		}
	}

	public bool IsNeighborOf(GameObject zone, GameObject possibleNeighbor){
		if(zone.GetComponent<ZoneScript>() == null) return false;
		if(possibleNeighbor.GetComponent<ZoneScript>() == null) return false;

		int zoneOne = zone.GetComponent<ZoneScript>().zoneNum;
		int zoneTwo = possibleNeighbor.GetComponent<ZoneScript>().zoneNum;

		if(BoardLayout.S.zoneGraph[zoneOne].Contains(zoneTwo)) return true;
		if(BoardLayout.S.zoneGraph[zoneTwo].Contains(zoneOne)) return true;
		
		return false;
	}

	public List<GameObject> GetZonesCanSeeFrom(GameObject zone){
		if(zone.GetComponent<ZoneScript>() == null) return new List<GameObject>();
		List<int> zonesCanSee = new List<int>();
		zonesCanSee.Add (zone.GetComponent<ZoneScript>().zoneNum);

		List<int> originalNeighbors = new List<int>();
		List<string> sightDirections = new List<string>();
		
		float x = zone.transform.position.x;
		float z = zone.transform.position.z;
		
		originalNeighbors = GetNeighborsOfInt(zone.GetComponent<ZoneScript>().zoneNum);
		for(int i = 0; i < originalNeighbors.Count; ++i){
			ZoneScript newZone = BoardLayout.S.createdZones[originalNeighbors[i]].GetComponent<ZoneScript>();
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
		
		List<int> zonesChecked = new List<int>();
		zonesChecked.AddRange(originalNeighbors);
		zonesChecked.Add (zone.GetComponent<ZoneScript>().zoneNum);
		
		//for each neighbor, look along sight directions
		for(int i = 0; i < originalNeighbors.Count; ++i){
			//the keyvaluepair is so we know where the next neighbor came from
			List<KeyValuePair<int, int>> neighbors = new List<KeyValuePair<int,int>>();
			List<int> firstNeighbors = GetNeighborsOfInt(originalNeighbors[i]);
			
			for(int j = 0; j < firstNeighbors.Count; ++j){
				KeyValuePair<int, int> newPair = new KeyValuePair<int, int>(originalNeighbors[i], firstNeighbors[j]);
				neighbors.Add(newPair);
			}
			while(neighbors.Count > 0){
				if(zonesChecked.Contains(neighbors[0].Value)){
					neighbors.RemoveAt(0);
					continue;
				}
				
				int newZone = neighbors[0].Value;
				
				float ogX = BoardLayout.S.createdZones[neighbors[0].Key].transform.position.x;
				float ogZ = BoardLayout.S.createdZones[neighbors[0].Key].transform.position.z;
				
				float newX = BoardLayout.S.createdZones[newZone].transform.position.x;
				float newZ = BoardLayout.S.createdZones[newZone].transform.position.z;
				
				float xDiff = Mathf.Abs(newX - ogX);
				float zDiff = Mathf.Abs(newZ - ogZ);
				
				if(sightDirections[i] == "x" && xDiff > zDiff){
					
					//newZone.GetComponent<ZoneScript>().Highlight();
					zonesCanSee.Add (newZone);
					if(BoardLayout.S.isStreetZone[newZone]){
						
						List<int> nextNeighbors = GetNeighborsOfInt(newZone);
						for(int j = 0; j < nextNeighbors.Count; ++j){
							KeyValuePair<int, int> newPair = new KeyValuePair<int, int>(newZone, nextNeighbors[j]);
							neighbors.Add(newPair);
						}
					}
				}
				else if(sightDirections[i] == "z" && zDiff >= xDiff){
					//newZone.GetComponent<ZoneScript>().Highlight();
					zonesCanSee.Add (newZone);
					if(BoardLayout.S.isStreetZone[newZone]){
						
						List<int> nextNeighbors = GetNeighborsOfInt(newZone);
						for(int j = 0; j < nextNeighbors.Count; ++j){
							KeyValuePair<int, int> newPair = new KeyValuePair<int, int>(newZone, nextNeighbors[j]);
							neighbors.Add(newPair);
						}
					}
				}
				zonesChecked.Add (newZone);
				neighbors.RemoveAt(0);
			}
		}

		List<GameObject> returnList = new List<GameObject>();
		foreach(int n in zonesCanSee){
			returnList.Add (BoardLayout.S.createdZones[n]);
		}

		return returnList;
	}

	public void HighlightZonesCanSeeFrom(GameObject zone){
		List<GameObject> zonesCanSee = GetZonesCanSeeFrom(zone);
		foreach(GameObject zoneGO in zonesCanSee){
			zoneGO.GetComponent<ZoneScript>().Highlight();
		}
	}

	public List<GameObject> SeeableZonesWithSurvivors(GameObject startingZone){
		List<GameObject> zonesCanSee = GetZonesCanSeeFrom(startingZone);
		List<GameObject> zonesWithSurvivors = new List<GameObject>();
		
		foreach(GameObject zone in zonesCanSee){
			foreach(Survivor surv in GameController.S.survivors){
				if(surv.CurrZone == zone){
					zonesWithSurvivors.Add (zone);
					break;
				}
			}
		}

		return zonesWithSurvivors;
	}

	public List<GameObject> ClosestSeeableZonesWithSurvivor(GameObject startingZone){
		List<GameObject> zonesWithSurvivors = new List<GameObject>();
		zonesWithSurvivors = SeeableZonesWithSurvivors(startingZone);

		return ClosestZonesFromList(zonesWithSurvivors, startingZone);
	}

	public List<GameObject> NoisiestSeeableZonesWithSurvivor(GameObject startingZone){
		List<GameObject> zonesWithSurvivors = new List<GameObject>();
		zonesWithSurvivors = SeeableZonesWithSurvivors(startingZone);
		
		return NoisiestZonesFromList(zonesWithSurvivors);
	}

	public List<GameObject> ClosestZonesFromList(List<GameObject> zoneList, GameObject startingZone){
		List<GameObject> closestZones = new List<GameObject>();

		int closestDist = int.MaxValue;
		foreach(GameObject zone in zoneList){
			if(ZoneDistance(startingZone, zone) < closestDist) closestDist = ZoneDistance(startingZone, zone);
		}
		foreach(GameObject zone in zoneList){
			if(ZoneDistance(startingZone, zone) == closestDist) closestZones.Add (zone);
		}

		return closestZones;
	}

	public List<GameObject> NoisiestZonesFromList(List<GameObject> zoneList){
		List<GameObject> noisyZones = new List<GameObject>();
		
		int mostNoise = 0;
		foreach(GameObject zone in zoneList){
			if(zone.GetComponent<ZoneScript>().ZoneNoise() > mostNoise){
				mostNoise = zone.GetComponent<ZoneScript>().ZoneNoise();
			}
		}
		
		foreach(GameObject zone in zoneList){
			if(zone.GetComponent<ZoneScript>().ZoneNoise() == mostNoise){
				noisyZones.Add (zone);
			}
		}
		
		return noisyZones;
	}

	public List<GameObject> GetNoisiestZones(){
		List<GameObject> noisyZones = NoisiestZonesFromList(BoardLayout.S.createdZones);

		return noisyZones;
	}

	public int ZoneDistance(GameObject zoneOne, GameObject zoneTwo){
		if(zoneOne == zoneTwo) return 0;

		List<int> zonesChecked = new List<int>();
		zonesChecked.Add(zoneOne.GetComponent<ZoneScript>().zoneNum);

		List<int> neighborZones = GetNeighborsOfInt(zoneOne.GetComponent<ZoneScript>().zoneNum);
		List<KeyValuePair<int, int>> neighbors = new List<KeyValuePair<int, int>>();
		foreach(int go in neighborZones){
			KeyValuePair<int, int> newPair = new KeyValuePair<int, int>(go, 1);
			neighbors.Add (newPair);
		}
		zonesChecked.AddRange(neighborZones);

		while(neighbors.Count > 0){
			int currZone = neighbors[0].Key;
			int dist = neighbors[0].Value;

			if(currZone == zoneTwo.GetComponent<ZoneScript>().zoneNum) return dist;

			neighborZones = GetNeighborsOfInt(currZone);
			foreach(int go in neighborZones){
				if(zonesChecked.Contains(go)) continue;
				KeyValuePair<int, int> newPair = new KeyValuePair<int, int>(go, 1 + dist);
				zonesChecked.Add (go);
				neighbors.Add (newPair);
			}
			neighbors.RemoveAt(0);
		}

		return -1;
	}

	public List<GameObject> StepTowardsZone(GameObject startZone, GameObject goalZone, int distToZone){
		List<int> possiblePaths = new List<int>();

		//Yeah, I know this is a stupid way to do it, but whatever
		List<List<int>> currentPaths = new List<List<int>>();
		List<int> neighbors = GetNeighborsOfInt(startZone.GetComponent<ZoneScript>().zoneNum);
		foreach(int go in neighbors){
			List<int> temp = new List<int>();
			temp.Add (go);
			currentPaths.Add (temp);
		}

		while(currentPaths.Count > 0 && currentPaths[0].Count <= distToZone){
			List<int> thisPath = currentPaths[0];
			int currZone = thisPath[thisPath.Count - 1];


			if(currZone == goalZone.GetComponent<ZoneScript>().zoneNum){
				possiblePaths.Add (thisPath[0]);
				currentPaths.RemoveAt(0);
				continue;
			}

			neighbors = GetNeighborsOfInt(currZone);
			foreach(int go in neighbors){
				List<int> newPath = new List<int>();
				newPath.AddRange(thisPath);
				if(newPath.Contains(go)) continue;

				newPath.Add (go);
				currentPaths.Add (newPath);
			}
			currentPaths.RemoveAt(0);

			//Fun sorting algorithm!
			List<List<int>> newCurrentPaths = new List<List<int>>();
			while(currentPaths.Count > 0){
				int lowCount = int.MaxValue;
				int lowNum = -1;
				for(int i = 0; i < currentPaths.Count; ++i){
					if(currentPaths[i].Count < lowCount){
						lowCount = currentPaths[i].Count;
						lowNum = i;
					}
				}
				newCurrentPaths.Add (currentPaths[lowNum]);
				currentPaths.RemoveAt(lowNum);
			}
			currentPaths.AddRange(newCurrentPaths);

		}
		List<GameObject> returnList = new List<GameObject>();

		foreach(int n in possiblePaths){
			returnList.Add (BoardLayout.S.createdZones[n]);
		}


		return returnList;
	}

	public List<GameObject> GetZonesInRange(GameObject startZone, int close, int far){
		List<GameObject> rangeZones = new List<GameObject>();

		foreach(GameObject zone in BoardLayout.S.createdZones){
			int zoneDist = ZoneDistance(startZone, zone);
			if(zoneDist >= close && zoneDist <= far) rangeZones.Add(zone);
		}

		return rangeZones;
	}

	public List<GameObject> GetZonesCanSeeFromInRange(GameObject startZone, int close, int far){
		List<GameObject> zonesCanSee = GetZonesCanSeeFrom(startZone);

		for(int i = zonesCanSee.Count - 1; i >= 0; --i){
			int dist = ZoneDistance(startZone, zonesCanSee[i]);
			if(dist < close || dist > far){
				zonesCanSee.RemoveAt(i);
			}
		}

		return zonesCanSee;
	}

	public bool IsPlayerZone(GameObject zone){
		foreach(Survivor surv in GameController.S.survivors){
			if(surv.CurrZone == zone) return true;
		}
		return false;
	}

	public List<GameObject> BuildingNeighbors(GameObject startZone){
		List<int> buildingNeighbors = new List<int>();
		buildingNeighbors.Add (startZone.GetComponent<ZoneScript>().zoneNum);
		List<int> zonesChecked = new List<int>();

		List<int> zonesToCheck = new List<int>();
		zonesToCheck.Add (startZone.GetComponent<ZoneScript>().zoneNum);

		while(zonesToCheck.Count > 0){
			int zone = zonesToCheck[0];
			if(zonesChecked.Contains(zone)){
				zonesToCheck.RemoveAt(0);
				continue;
			}

			zonesChecked.Add (zone);

			List<int> neighbors = GetNeighborsOfInt(zone);
			foreach(int neigh in neighbors){
				if(zonesChecked.Contains(neigh)) continue;
				if(BoardLayout.S.isStreetZone[neigh]){
					zonesChecked.Add (neigh);
					continue;
				}
				if(zonesToCheck.Contains(neigh)){
					continue;
				}
				buildingNeighbors.Add (neigh);
				zonesToCheck.Add (neigh);
			}
			zonesToCheck.RemoveAt(0);
		}

		List<GameObject> returnList = new List<GameObject>();
		foreach(int n in buildingNeighbors){
			returnList.Add (BoardLayout.S.createdZones[n]);
		}

		return returnList;
	}
}
