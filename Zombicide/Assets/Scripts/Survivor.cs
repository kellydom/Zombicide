using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Survivor : MonoBehaviour {

	public int survNum;

	GameObject currZone;
	public GameObject CurrZone{
		get{return currZone;}
	}
	Color baseColor;

	bool hasGone;
	public bool HasGone{
		get{return hasGone;}
		set{hasGone = value;}
	}
	public string name;
	public int numActions;
	public bool slippery = false;
	public bool extraAction = false;
	public bool freeSearch = false;
	public bool doubleMove = false;
	public Card front1;
	public Card front2;
	public Card back1;
	public Card back2;
	public Card back3;

	public bool currTurn = false;

	public bool hasSearched = false;


	// Use this for initialization
	void Start () {
		numActions = 3;
		baseColor = gameObject.GetComponent<Renderer>().material.color;
	}

	public void SetZone(GameObject newZone){
		currZone = newZone;
		Vector3 basePos = currZone.transform.position;
		ZoneScript zone = currZone.GetComponent<ZoneScript>();
		int index = zone.zoneNum;
		Vector3 size = BoardLayout.S.zoneSizes[index];

		Vector3 bottomLeft = newZone.GetComponent<BoxCollider>().bounds.min;
		Vector3 bottomRight = bottomLeft;
		bottomRight.x = newZone.GetComponent<BoxCollider>().bounds.max.x;
		bottomLeft.z += .03f;
		bottomRight.z += .03f;

		if(survNum >= 3) {
			bottomLeft.z = (bottomLeft.z + newZone.GetComponent<BoxCollider>().bounds.max.z) / 2;
			bottomRight.z = bottomLeft.z;
		}

		int xValue = survNum;
		if(xValue >= 3) xValue -= 3;

		transform.position = Vector3.Lerp (bottomLeft, bottomRight, (xValue + 1) / 4.0f ) + Vector3.up / 5;
	}

	public void DoNothing(){
		numActions = 0;
	}

	public void MoveTo(GameObject newZone, int actionCost){
		numActions -= actionCost;
		SetZone(newZone);
		GameController.S.MoveSetup();
	}
	public bool CanMove(){
		ZoneScript zs = currZone.GetComponent<ZoneScript>();
		
		if(numActions > 1) return true;
		
		if(zs.walkersInZone.Count > 0) return false;
		if(zs.runnersInZone.Count > 0) return false;
		if(zs.fattiesInZone.Count > 0) return false;
		if(zs.abombInZone.Count > 0) return false;
		
		return true;
	}

	public bool CanDoMelee(){
		if(front1 != null){
			if(front1.melee){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				
				if(zs.walkersInZone.Count > 0) return true;
				if(zs.runnersInZone.Count > 0) return true;
				if(zs.fattiesInZone.Count > 0) return true;
				if(zs.abombInZone.Count > 0) return true;

			}
		}
		if(front2 != null){
			if(front2.melee){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				
				if(zs.walkersInZone.Count > 0) return true;
				if(zs.runnersInZone.Count > 0) return true;
				if(zs.fattiesInZone.Count > 0) return true;
				if(zs.abombInZone.Count > 0) return true;
				
			}
		}

		return false;
	}
	public bool CanDoRanged(){
		if(front1 != null){
			if(front1.ranged){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				List<GameObject> frontRange1 = new List<GameObject>();
				frontRange1 = ZoneSelector.S.GetZonesInRange(currZone, front1.closeRange, front1.farRange);
				
				foreach(GameObject zone in frontRange1){
					
					if(zone.GetComponent<ZoneScript>().walkersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().runnersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().fattiesInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().abombInZone.Count > 0) return true;
				}
			}
		}
		if(front2 != null){
			if(front2.ranged){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				List<GameObject> frontRange2 = new List<GameObject>();
				frontRange2 = ZoneSelector.S.GetZonesInRange(currZone, front2.closeRange, front2.farRange);
				
				foreach(GameObject zone in frontRange2){
					
					if(zone.GetComponent<ZoneScript>().walkersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().runnersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().fattiesInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().abombInZone.Count > 0) return true;
				}
			}
		}
		
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.playerTurn) {Unhighlight(); return;}

		/*if(GameController.S.currSurvivor != null) {
			if(GameController.S.currSurvivor != this) Unhighlight();

			return;
		}
		if(GameController.S.closestSurvivor == this) Highlight();
		else Unhighlight();*/
	}

	void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			//GameController.S.SelectSurvivor(this);
		}
	}

	public void Highlight(){
		gameObject.GetComponent<Renderer>().material.color = new Color(1,1,0,0.2f);

	}
	public void Unhighlight(){
		gameObject.GetComponent<Renderer>().material.color = baseColor;

	}

	void OnMouseEnter(){
		if(hasGone) return;
		if(currTurn) return;
		//Highlight();
	}
	
	void OnMouseExit(){
		if(currTurn) return;
		//Unhighlight();
	}

}
