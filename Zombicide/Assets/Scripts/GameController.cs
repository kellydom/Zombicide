using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static GameController S;

	public List<Survivor> survivors = new List<Survivor>();
	Survivor currSurvivor;
	GameObject currZone;

	bool mouseInWheel = false;
	bool mouseInWheelButton = false;

	// Use this for initialization
	void Start () {

		//Singleton initialization
		if(S == null){
			S = this;
		}
		else
		{
			if(this != S)
				Destroy(this.gameObject);
		}

		GameObject[] survivorsTEMP = GameObject.FindGameObjectsWithTag("Survivor");
		for(int i = 0; i < survivorsTEMP.Length; ++i){
			survivors.Add(survivorsTEMP[i].GetComponent<Survivor>());
		}
	
	}

	public void SpawnSurvivors(GameObject startingZone){
		for(int i = 0; i < survivors.Count; ++i){
			survivors[i].setZone(startingZone);
		}
	}

	public void HighlightZone(Vector3 pos){
		if(mouseInWheel || mouseInWheelButton){
			if(currZone != null){
				currZone.GetComponent<ZoneScript>().Unhighlight();
				currZone = null;
			}
			return;
		}

		float dist = int.MaxValue;

		GameObject closestZone = null;
		for(int i = 0; i < BoardLayout.S.zonePositions.Count; ++i){
			Vector3 zonePos = BoardLayout.S.zonePositions[i];
			if(Vector3.Distance(zonePos, pos) < dist){
				dist = Vector3.Distance(zonePos, pos);
				closestZone = BoardLayout.S.createdZones[i];
			}
		}
		if(currZone == closestZone) return;

		closestZone.GetComponent<ZoneScript>().Highlight();
		if(currZone != null){
			currZone.GetComponent<ZoneScript>().Unhighlight();
		}
		currZone = closestZone;


	}

	public void SelectSurvivor(Survivor surv){

		if(currSurvivor == surv) return;

		surv.IncreaseSize(2);

		if(currSurvivor != null) currSurvivor.IncreaseSize(0.5f);

		currSurvivor = surv;
	}

	public GameObject GetCurrZone(){
		return currZone;
	}

	public void ClickedCurrZone(){
		if(currSurvivor == null) return;

		currSurvivor.setZone(currZone);
	}

	public void MouseEnterWheel(){
		mouseInWheel = true;
	}

	public void MouseExitWheel(){
		mouseInWheel = false;

	}
	
	public void MouseEnterButton(){
		mouseInWheelButton = true;
	}
	
	public void MouseExitButton(){
		mouseInWheelButton = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
