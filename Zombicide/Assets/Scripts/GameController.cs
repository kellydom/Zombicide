using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static GameController S;

	public List<Survivor> survivors = new List<Survivor>();
	public Survivor currSurvivor;
	public Survivor closestSurvivor;
	public Deck deck;
	//public Card picked;

	public bool mouseInWheel = false;
	public bool mouseInWheelButton = false;

	bool playerTurn;
	bool playerGoing;

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
		playerTurn = true;
		playerGoing = false;

		GameObject[] survivorsTEMP = GameObject.FindGameObjectsWithTag("Survivor");
		for(int i = 0; i < survivorsTEMP.Length; ++i){
			survivors.Add(survivorsTEMP[i].GetComponent<Survivor>());
		}
		deck = GameObject.Find("Main Camera").GetComponent<Deck>();
	}

	public void TakeObjSetup(){

	}

	public void ReorganizeInvSetup(){

	}

	public void SearchSetup(){


	}

	public void OpenDoorSetup(){

	}

	public void MakeNoiseSetup(){

	}

	public void ChangeSeatsSetup(){

	}

	public void GetIntoOutOfCarSetup(){

	}

	public void DriveCarSetup(){

	}

	public void RangedSetup(){

	}

	public void MeleeSetup(){

	}

	public void DoNothingSetup(){
		currSurvivor.DoNothing();
	}

	public void MoveSetup(){
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			BoardLayout.S.createdZones[i].GetComponent<ZoneScript>().Unhighlight();
		}
		ZoneSelector.S.HighlightNeighborsOf(currSurvivor.CurrZone);
	}

	IEnumerator PlayerTurn(){
		playerGoing = true;

		while(true){
			if(currSurvivor == null){
				//check to see if all survivors have finished - otherwise, keep looping
				bool allSurvivorsDone = true;
				for(int i = 0; i < survivors.Count; ++i){
					if(!survivors[i].HasGone) allSurvivorsDone = false;
				}
				if(allSurvivorsDone) break;

				Plane boardPlane = new Plane(Vector3.up, new Vector3(0,0.05f,0));
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float hit;
				
				if(boardPlane.Raycast(ray, out hit)){
					Vector3 hitPos = ray.GetPoint(hit);
					closestSurvivor = GetClosestSurvivorTo(hitPos);
					if(closestSurvivor != null){
						closestSurvivor.Highlight();
						if(Input.GetMouseButton(0)){
							
							currSurvivor = closestSurvivor;
							currSurvivor.currTurn = true;
						}

					}

				}


				yield return 0;
				continue;
			}

			if(currSurvivor.numActions == 0){
				ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
				currSurvivor.currTurn = false;
				currSurvivor.Unhighlight();
				currSurvivor = null;
				yield return 0;
				continue;
			}

			switch(ActionWheel.S.CurrAction){
			case "Move":
				if(Input.GetMouseButton(0) && !ActionWheel.S.mouseInWheel && !ActionWheel.S.mouseInWheelButton){
					GameObject clickedZone = ZoneSelector.S.CurrZone;
					GameObject survZone = currSurvivor.CurrZone;
					if(ZoneSelector.S.IsNeighborOf(clickedZone, survZone)){
						currSurvivor.MoveTo(clickedZone, 1);
					}
				}
				break;
			}
			
			yield return 0;
		}

		playerTurn = false;
		playerGoing = false;
	}

	Survivor GetClosestSurvivorTo(Vector3 pos){
		Survivor surv = null;

		float distance = 0.03f;
		for(int i = 0; i < survivors.Count; ++i){
			Survivor temp = survivors[i];
			if(temp.HasGone) continue;

			if(Vector3.Distance(pos, temp.transform.position) < distance){
				distance = Vector3.Distance(pos, temp.transform.position);
				surv = temp;
			}
		}
		return surv;
	}


	void Update(){
		//picked = deck.draw ();
		//print (picked.cardName);
		if(playerTurn){
			if(!playerGoing){
				//I'm using coroutines because I think it looks cleaner here
				StartCoroutine(PlayerTurn());
			}
		}
		else{

		}

	}

	public void SpawnSurvivors(GameObject startingZone){
		for(int i = 0; i < survivors.Count; ++i){
			survivors[i].SetZone(startingZone);
		}
	}

	public void SelectSurvivor(Survivor surv){
		if(currSurvivor != null) return;

		surv.Highlight();

		currSurvivor = surv;
		currSurvivor.currTurn = true;
	}

	public void ClickedCurrZone(){
		if(currSurvivor == null) return;
	}



}
