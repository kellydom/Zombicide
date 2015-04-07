using UnityEngine;
using System.Collections;

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

	public void HighlightNeighborsOf(GameObject zone){
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			GameObject testZone = BoardLayout.S.createdZones[i];
			if(IsNeighborOf(zone, testZone)){
				testZone.GetComponent<ZoneScript>().Highlight();
			}
		}
	}

	public bool IsNeighborOf(GameObject zone, GameObject possibleNeighbor){
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
}
