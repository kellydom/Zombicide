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
	public Card picked;
	public Button pickedImage;

	public bool mouseInWheel = false;
	public bool mouseInWheelButton = false;

	public bool playerTurn;
	bool playerGoing;
	bool zombieGoing;
	List<GameObject> zombieSpawnZones = new List<GameObject>();
	public Text turnText;
	public Sprite gemEmpty;
	public Sprite gemFull;
	public List<Image> gems;

	public Image zombieNumImg;
	public Text zombieNum;

	//for now, I'm just going to have a list of each zombie type, and 
	//spawn from that randomly (until cards are done)
	public List<GameObject> enemyTypes;

	List<Enemy> allZombies = new List<Enemy>();

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
		zombieGoing = false;

		GameObject[] survivorsTEMP = GameObject.FindGameObjectsWithTag("Survivor");
		for(int i = 0; i < survivorsTEMP.Length; ++i){
			survivors.Add(survivorsTEMP[i].GetComponent<Survivor>());
		}
		deck = GameObject.Find("Main Camera").GetComponent<Deck>();

		foreach (Survivor guy in survivors) {
			guy.front1 = deck.empty;
			guy.front2 = deck.empty;
			guy.back1 = deck.empty;
			guy.back2 = deck.empty;
			guy.back3 = deck.empty;
		}

		//survivors [1].front1 = deck.pan;

		foreach(Image img in gems){
			img.sprite = null;
		}

		StartCoroutine(GetZombieSpawnZones());
	}

	IEnumerator GetZombieSpawnZones(){
		yield return new WaitForEndOfFrame();

		GameObject[] zombieSpawn = GameObject.FindGameObjectsWithTag("ZombieSpawn");
		foreach(GameObject zSpawn in zombieSpawn){
			GameObject zone = ZoneSelector.S.GetZoneAt(zSpawn.transform.position);
			zombieSpawnZones.Add (zone);
		}
	}

	public void TakeObjSetup(){
		if(currSurvivor == null) return;
		if(currSurvivor.CurrZone.GetComponent<ZoneScript>().objectiveInRoom == null) return;

		Destroy(currSurvivor.CurrZone.GetComponent<ZoneScript>().objectiveInRoom);
		currSurvivor.CurrZone.GetComponent<ZoneScript>().objectiveInRoom = null;
		
		ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
		currSurvivor.numActions--;
	}

	public void ReorganizeInvSetup(){

	}

	public void SearchSetup(){
		picked = deck.draw ();
		print (picked.cardName);
		currSurvivor.hasSearched = true;
		currSurvivor.numActions--;
		//pickedImage.image.sprite = picked.but.image.sprite;
		//pickedImage.transform.position = new Vector3 (0, 0, 0);
		
		ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
	}

	public void OpenDoorSetup(){
		if(currSurvivor == null) return;

		int pZone = currSurvivor.CurrZone.GetComponent<ZoneScript>().zoneNum;
		for(int i = 0; i < BoardLayout.S.doorConnections.Count; ++i){
			BoardLayout.Door door = BoardLayout.S.doorConnections[i];
			if(door.zoneOne == pZone || door.zoneTwo == pZone){
				if(!door.isOpened){
					BoardLayout.S.doors[i].transform.Rotate(Vector3.right, 180);
					Vector3 newPos = BoardLayout.S.doors[i].transform.position + Vector3.up / 3.0f;
					BoardLayout.S.doors[i].transform.position = newPos;
					door.isOpened = true;
				}
			}
		}

		currSurvivor.numActions--;
	}

	public void MakeNoiseSetup(){
		ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
		if(currSurvivor == null) return;
		currSurvivor.CurrZone.GetComponent<ZoneScript>().AddNoiseToken();
		currSurvivor.numActions--;

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
		AttackScript.S.CreateAttackWheels();
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
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			BoardLayout.S.createdZones[i].GetComponent<ZoneScript>().Unhighlight();
		}

		while(true){
			if(currSurvivor == null){
				turnText.text = "Choose a Survivor";
				foreach(Image img in gems){
					img.sprite = null;
				}
				//check to see if all survivors have finished - otherwise, keep looping
				bool allSurvivorsDone = true;
				for(int i = 0; i < survivors.Count; ++i){
					if(!survivors[i].HasGone) allSurvivorsDone = false;
				}
				if(allSurvivorsDone) break;
/*
				Plane boardPlane = new Plane(Vector3.up, new Vector3(0,0.05f,0));
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float hit;
				
				if(boardPlane.Raycast(ray, out hit)){
					Vector3 hitPos = ray.GetPoint(hit);
					closestSurvivor = GetClosestSurvivorTo(hitPos);
					if(closestSurvivor != null){
						if(!closestSurvivor.HasGone){
							closestSurvivor.Highlight();
							if(Input.GetMouseButton(0)){
								
								currSurvivor = closestSurvivor;
								currSurvivor.currTurn = true;
							}

						}

					}

				}*/


				yield return 0;
				continue;
			}
			currSurvivor.Highlight();

			if(currSurvivor.numActions == 0){
				ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
				currSurvivor.currTurn = false;
				currSurvivor.Unhighlight();
				currSurvivor.HasGone = true;
				SurvivorToken.S.tokenOnClicked(currSurvivor.name);
				currSurvivor = null;
				yield return 0;
				continue;
			}
			turnText.text = "Actions Remaining: " + currSurvivor.numActions;
			/*int ctr = 0;
			foreach(Image gem in gems){
				if(ctr < currSurvivor.numActions){
					gem.sprite = gemFull;
				}
				else{
					gem.sprite = gemEmpty;
				}
				ctr++;
			}*/

			switch(ActionWheel.S.CurrAction){
			case "Move":
				if(Input.GetMouseButton(0) && !ActionWheel.S.mouseInWheel && !ActionWheel.S.mouseInWheelButton){
					GameObject clickedZone = ZoneSelector.S.CurrZone;
					GameObject survZone = currSurvivor.CurrZone;
					if(ZoneSelector.S.IsNeighborOf(clickedZone, survZone)){
						ZoneScript zs = currSurvivor.CurrZone.GetComponent<ZoneScript>();

						int actionCount = 1;
						
						if(zs.walkersInZone.Count > 0) actionCount = 2;
						if(zs.runnersInZone.Count > 0) actionCount = 2;
						if(zs.fattiesInZone.Count > 0) actionCount = 2;
						if(zs.abombInZone.Count > 0) actionCount = 2;

						currSurvivor.MoveTo(clickedZone, actionCount);
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

	IEnumerator ZombieTurn(){
		zombieGoing = true;
		turnText.text = "Zombies' Turn!";
		foreach(Image img in gems){
			img.sprite = null;
		}

		//Move Zombies on board
		CameraController.S.ZoomOut(0.5f);
		yield return new WaitForSeconds(0.5f);

		foreach(GameObject zone in BoardLayout.S.createdZones){
			if(allZombies.Count == 0) continue;

			zone.GetComponent<ZoneScript>().DoZombieActions();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1);

		//Spawn Zombies
		foreach(GameObject zone in zombieSpawnZones){
			CameraController.S.MoveTo(zone.transform.position + new Vector3(0, 1, 0) + Vector3.back / 3, 1);
			yield return new WaitForSeconds(1);

			SpawnZombiesAt(zone);
			yield return new WaitForSeconds(1);

		}


		yield return 0;

		foreach(Survivor surv in survivors){
			surv.HasGone = false;
			surv.numActions = 3;
		}
		foreach(Enemy zomb in allZombies){
			zomb.hasDoneAction = false;
		}

		zombieGoing = false;
		playerTurn = true;
	}

	void SpawnZombiesAt(GameObject zone){

		//Random zombie spawn while we don't have cards fully implemented yet
		int zombieType = 0;
		float ran = Random.Range(0.0f, 100.0f);
		if(ran < 50) zombieType = 0;
		else if(ran < 85) zombieType = 1;
		else zombieType = 2;

		int numToSpawn = 0;
		if(zombieType == 0) numToSpawn = Random.Range(1, 5);
		else if(zombieType == 1) numToSpawn = Random.Range(1, 3);
		else numToSpawn = 1;

		Vector3 topRightCorner = zone.GetComponent<BoxCollider>().bounds.max;
		Vector3 topLeftCorner = topRightCorner;
		topLeftCorner.x = zone.GetComponent<BoxCollider>().bounds.min.x;
		topRightCorner.z += -0.03f;
		topLeftCorner.z += -0.03f;


		Vector3 spawnPos = Vector3.Lerp (topLeftCorner, topRightCorner, (zombieType + 1) / 5.0f);


		if(zombieType == 2){
			//Have to spawn 2 walkers per fatty as well)
			Vector3 walkerSpawnPos = Vector3.Lerp (topLeftCorner, topRightCorner, 1/5.0f);

			int walkersToSpawn = numToSpawn * 2;
			
			
			while(walkersToSpawn > 0){
				GameObject zombie = Instantiate(enemyTypes[0], walkerSpawnPos + Vector3.up / 5 * walkersToSpawn, Quaternion.identity) as GameObject;
				zone.GetComponent<ZoneScript>().AddZombieToZone(zombie);
				zombie.GetComponent<Enemy>().currZone = zone;
				allZombies.Add (zombie.GetComponent<Enemy>());
				walkersToSpawn--;
			}
		}

		while(numToSpawn > 0){
			GameObject zombie = Instantiate(enemyTypes[zombieType], spawnPos + Vector3.up / 5 * numToSpawn, Quaternion.identity) as GameObject;
			zone.GetComponent<ZoneScript>().AddZombieToZone(zombie);
			zombie.GetComponent<Enemy>().currZone = zone;
			allZombies.Add (zombie.GetComponent<Enemy>());
			numToSpawn--;
		}
		
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
			if(!zombieGoing){
				StartCoroutine(ZombieTurn());
			}
		}

	}

	public void SpawnSurvivors(GameObject startingZone){
		for(int i = 0; i < survivors.Count; ++i){
			survivors[i].survNum = i;
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

	public void SetZombieNumText(Vector3 pos, int num){
		zombieNum.text = "x"+num;
		Vector2 viewportPoint = Camera.main.WorldToScreenPoint(pos - new Vector3(0, 0, 0.05f)); //convert game object position to VievportPoint
		// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
		zombieNumImg.transform.position = viewportPoint;
	}

	public void MoveZombieNumOff(){
		zombieNum.text = "";
		zombieNumImg.transform.position = new Vector2(-1, -1);
	}
}
