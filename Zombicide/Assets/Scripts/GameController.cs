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
	public Text survTurnText;
	public Image survTurnImg;
	public Text zombTurnText;
	public Image zombTurnImg;

	public Image zombieNumImg;
	public Text zombieNum;
	public Image zombieDescription;

	public Button deleteForSearch;
	public Button finishOrganizing;
	public bool playerSearching = false;
	public bool playerTrading = false;
	public bool playerOrganizing = false;


	//public bool playerJustStarted = false;

	//for now, I'm just going to have a list of each zombie type, and 
	//spawn from that randomly (until cards are done)
	public List<GameObject> enemyTypes;

	public List<Enemy> allZombies = new List<Enemy>();

	bool needToChooseWeapon = false;
	Card clickedCard = null;
	Card attackingWeapon = null;

	public bool zombiesAttacking = false;
	public bool waitForAaahhSpawn = false;
	public bool spawningIndoors = false;

	public Button stopAction;

	string btnShowingReload;
	bool reloadImgOut = false;
	public Image reloadImg;

	bool shouldUseFlashlight = true;
	

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

		StartCoroutine(GetZombieSpawnZones());

		float currHigh = zombieDescription.rectTransform.sizeDelta.y;
		float high = Screen.width/8f;

		zombieDescription.rectTransform.localScale = new Vector2(high / currHigh, high / currHigh);
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
		currSurvivor.GiveEXP(5);

		ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
		currSurvivor.numActions--;
	}

	public void ReorganizeInvSetup(){
		playerOrganizing = true;
		ActionWheel.S.MoveWheelUp ();
		finishOrganizing.transform.position = new Vector3 (Screen.width/2, Screen.height / 2, 0);
		GameController.S.stopAction.transform.position = new Vector3(Screen.width/2, Screen.height/4, 0);
		SurvivorToken.S.changeInventory ("Setup");
	}

	IEnumerator SpawnAaahhCard(){
		waitForAaahhSpawn = true;
		
		zombTurnText.text = "New Zombie Spawning!";
		StartCoroutine(MoveZombTurnImgOut());

		yield return new WaitForSeconds(1);

		GameController.S.pickedImage.transform.position = new Vector3 (-3000, 0, 0);

		CameraController.S.MoveTo(currSurvivor.CurrZone.transform.position + new Vector3(0, 1, 0) + Vector3.back / 3, 0.5f);
		yield return new WaitForSeconds(0.5f);

		
		Vector3 topRightCorner = currSurvivor.CurrZone.GetComponent<BoxCollider>().bounds.max;
		Vector3 topLeftCorner = topRightCorner;
		topLeftCorner.x = currSurvivor.CurrZone.GetComponent<BoxCollider>().bounds.min.x;
		topRightCorner.z += -0.03f;
		topLeftCorner.z += -0.03f;
		Vector3 walkerSpawnPos = Vector3.Lerp (topLeftCorner, topRightCorner, 1/5.0f);
		GameObject zombie = Instantiate(enemyTypes[0], topLeftCorner + Vector3.up / 5, Quaternion.identity) as GameObject;
		currSurvivor.CurrZone.GetComponent<ZoneScript>().AddZombieToZone(zombie);
		zombie.GetComponent<Enemy>().currZone = currSurvivor.CurrZone;
		allZombies.Add (zombie.GetComponent<Enemy>());

		yield return new WaitForSeconds(0.5f);

		SurvivorToken.S.ShowSkills(GameController.S.currSurvivor);
		ActionWheel.S.MoveWheelDown ();
		if(!currSurvivor.skills.Contains("+1 free Search Action"))
			currSurvivor.numActions--;
		waitForAaahhSpawn = false;
		StartCoroutine(MoveSurvTurnImgOut());
	}

	public void SearchSetup(){
		if(currSurvivor == null) return;

		SurvivorToken.S.HideSkills();
		ActionWheel.S.MoveWheelUp ();
		picked = deck.draw ();
		
		currSurvivor.hasSearched = true;
		pickedImage.image.sprite = picked.but.image.sprite;
		pickedImage.transform.position = new Vector3 (Screen.width/2, Screen.height/2, 0);

		if(picked.cardName == "Aaahh!"){
			StartCoroutine(SpawnAaahhCard());
			return;
		}

		GameController.S.survTurnText.text = "Replace and/or Discard";

		pickedImage.image.sprite = picked.but.image.sprite;
		pickedImage.transform.position = new Vector3 (Screen.width/2, Screen.height/2, 0);
		deleteForSearch.transform.position = new Vector3 (Screen.width - 35, Screen.height / 2, 0);
		playerSearching = true;
	}

	public void tradeSetup() {
		if(currSurvivor == null) return;
		playerTrading = true;
		survTurnText.text = "Select another Survivor";
		stopAction.transform.position = new Vector3 (Screen.width / 2, Screen.height / 4, 0);

		SurvivorToken.S.MoveTokensOffscreen();
		//deactivate the buttons that cant be traded with
		foreach (Survivor guy in GameController.S.survivors) {
			if(guy.CurrZone == currSurvivor.CurrZone){
				SurvivorToken.S.MoveSpecificTokenOn(guy);
			}
		}
		ActionWheel.S.MoveWheelUp ();
	}

	IEnumerator SpawningZombiesInsideCo(GameObject zoneToStart){
		spawningIndoors = true;

		List<GameObject> buildingNeighbors = ZoneSelector.S.BuildingNeighbors(zoneToStart);
		zombTurnText.text = "Zombies' Spawn!";
		StartCoroutine(MoveZombTurnImgOut());

		Vector2 currPos = ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition;
		Vector2 desired = new Vector2(0, -35);
		ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "";
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition = Vector2.Lerp(currPos, desired, t);
			
			yield return 0;
		}
		//Spawn Zombies
		foreach(GameObject zone in buildingNeighbors){
			zone.GetComponent<ZoneScript>().hasSpawnedZombies = true;
			CameraController.S.MoveTo(zone.transform.position + new Vector3(0, 1, 0) + Vector3.back / 3, 0.2f);
			zone.GetComponent<ZoneScript>().Highlight();
			yield return new WaitForSeconds(0.2f);
			
			SpawnZombiesAt(zone);
			yield return new WaitForSeconds(1);
			zone.GetComponent<ZoneScript>().Unhighlight();
			
		}
		currPos = desired;
		desired = new Vector2(0, 35);
		
		ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "";
		t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition = Vector2.Lerp(currPos, desired, t);
			
			yield return 0;
		}
		CameraController.S.MoveTo(currSurvivor.CurrZone.transform.position + new Vector3(0, 1, -.5f) + Vector3.back / 3, 0.6f);
		
		StartCoroutine(MoveSurvTurnImgOut());
		currSurvivor.numActions--;
		spawningIndoors = false;
	}

	public void OpenDoorSetup(){
		if(currSurvivor == null) return;

		int pZone = currSurvivor.CurrZone.GetComponent<ZoneScript>().zoneNum;
		GameObject zoneToStartSpawn = null;
		for(int i = 0; i < BoardLayout.S.doorConnections.Count; ++i){
			BoardLayout.Door door = BoardLayout.S.doorConnections[i];
			if(door.zoneOne == pZone || door.zoneTwo == pZone){
				if(!door.isOpened){
					BoardLayout.S.doors[i].transform.Rotate(Vector3.right, 180);
					Vector3 newPos = BoardLayout.S.doors[i].transform.position + Vector3.up / 3.0f;
					BoardLayout.S.doors[i].transform.position = newPos;
					door.isOpened = true;

					int otherZone = door.zoneOne;
					if(door.zoneOne == pZone) otherZone = door.zoneTwo;
					BoardLayout.S.UpdateGraph(pZone, otherZone);

					if(!BoardLayout.S.isStreetZone[otherZone] && !BoardLayout.S.createdZones[otherZone].GetComponent<ZoneScript>().hasSpawnedZombies){
						zoneToStartSpawn = BoardLayout.S.createdZones[otherZone];
					}
				}
			}
		}

		if(currSurvivor.front1.openDoor){
			if(currSurvivor.front1.doorNoise){
				currSurvivor.CurrZone.GetComponent<ZoneScript>().AddNoiseToken();
			}
		}
		else if(currSurvivor.front2.openDoor){
			if(currSurvivor.front2.doorNoise){
				currSurvivor.CurrZone.GetComponent<ZoneScript>().AddNoiseToken();
			}
		}

		if(zoneToStartSpawn != null){
			StartCoroutine(SpawningZombiesInsideCo(zoneToStartSpawn));
		}
		else{
			currSurvivor.numActions--;
		}

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

	public void RangedAttackSetup(GameObject zone){
		ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
		ActionWheel.S.MoveWheelUp();
		survTurnText.text = "Shooting!";

		bool dualWield = false;
		if(currSurvivor.front1.cardName == currSurvivor.front2.cardName && currSurvivor.front1.dualWield){
			dualWield = true;
		}
		if(attackingWeapon.cardName == "Sawed Off Shotgun"){
			currSurvivor.hasUsedShotty = true;
		}
		AttackScript.S.CreateAttackWheels(zone, false, attackingWeapon, dualWield);
	}

	public void RangedZoneSetup(){
		needToChooseWeapon = false;
		GameObject survZone = currSurvivor.CurrZone;
		List<GameObject> rangeList = ZoneSelector.S.GetZonesCanSeeFromInRange(survZone, attackingWeapon.closeRange, attackingWeapon.farRange);
		survTurnText.text = "Choose a Zone to Attack!";
		foreach(GameObject zone in rangeList){
			if (zone.GetComponent<ZoneScript>().EnemiesInZone() > 0){
				zone.GetComponent<ZoneScript>().Highlight();
			}
		}
	}

	public void RangedWeaponSetup(){
		if(currSurvivor.front1.ranged && currSurvivor.front2.ranged && !((currSurvivor.front1.cardName == "Sawed Off Shotgun" || currSurvivor.front2.cardName == "Sawed Off Shotgun") && currSurvivor.hasUsedShotty)){
			SurvivorToken.S.front1.image.color = Color.yellow;
			SurvivorToken.S.front2.image.color = Color.yellow;
			if(currSurvivor.front1.cardName == currSurvivor.front2.cardName && currSurvivor.front1.dualWield){

				attackingWeapon = currSurvivor.front1;
				RangedZoneSetup();
			}
			else{
				needToChooseWeapon = true;
				clickedCard = null;
				attackingWeapon = null;
			}
		}
		else{
			if(currSurvivor.front1.ranged && !(currSurvivor.front1.cardName == "Sawed Off Shotgun" && currSurvivor.hasUsedShotty)){
				attackingWeapon = currSurvivor.front1;
				SurvivorToken.S.front1.image.color = Color.yellow;
			}
			else if(currSurvivor.front2.ranged && !(currSurvivor.front2.cardName == "Sawed Off Shotgun" && currSurvivor.hasUsedShotty)){
				attackingWeapon = currSurvivor.front2;
				SurvivorToken.S.front2.image.color = Color.yellow;
			}
			RangedZoneSetup();
		}
	}

	public void FinishAttackAction(){
		SurvivorToken.S.front1.image.color = Color.white;
		SurvivorToken.S.front2.image.color = Color.white;

		ActionWheel.S.MoveWheelDown();

		if(currSurvivor.skills.Contains("+1 free Combat Action")){
			if(!currSurvivor.hasDoneCombatAction){
				currSurvivor.hasDoneCombatAction = true;
			}
			else{
				currSurvivor.numActions--;
			}
		}
		else{
			currSurvivor.numActions--;
		}
	}

	public void MeleeSetup(){
		survTurnText.text = "Attacking!";
		ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
		ActionWheel.S.MoveWheelUp();
		bool dualWield = false;
		if(currSurvivor.front1.cardName == currSurvivor.front2.cardName && currSurvivor.front1.dualWield){
			dualWield = true;
		}
		AttackScript.S.CreateAttackWheels(currSurvivor.CurrZone, true, attackingWeapon, dualWield);
	}

	public void MeleeWeaponSetup(){
		if(currSurvivor.front1.melee && currSurvivor.front2.melee){
			SurvivorToken.S.front1.image.color = Color.yellow;
			SurvivorToken.S.front2.image.color = Color.yellow;
			if(currSurvivor.front1.cardName == currSurvivor.front2.cardName && currSurvivor.front1.dualWield){
				
				attackingWeapon = currSurvivor.front1;
				MeleeSetup();
			}
			else{
				needToChooseWeapon = true;
				clickedCard = null;
				attackingWeapon = null;
			}
		}
		else{
			if(currSurvivor.front1.melee){
				attackingWeapon = currSurvivor.front1;
				SurvivorToken.S.front1.image.color = Color.yellow;
			}
			else{
				attackingWeapon = currSurvivor.front2;
				SurvivorToken.S.front2.image.color = Color.yellow;
			}
			MeleeSetup();
		}
	}

	public void DoNothingSetup(){
		currSurvivor.DoNothing();
	}

	public void MoveSetup(){
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			BoardLayout.S.createdZones[i].GetComponent<ZoneScript>().Unhighlight();
		}
		
		if(!currSurvivor.CanMove()){
			ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
			return;
		}

		
		int distCanTravel = 1;
		if(currSurvivor.skills.Contains("2 Zones per Move Action")){
			distCanTravel++;
		}
		if(currSurvivor.skills.Contains("+1 Zone per Move")){
			distCanTravel++;
		}

		if(distCanTravel == 1){
			ZoneSelector.S.HighlightNeighborsOf(currSurvivor.CurrZone);
		}
		else{
			ZoneSelector.S.HighlightMoveableZonesUpTo(currSurvivor.CurrZone, distCanTravel);
		}
	}

	public void ClickedInvButton(Button clicked){
		if (ActionWheel.S.CurrAction == "Trade" || SurvivorToken.S.sacrificeThem)
			return;
		if(clicked.name == "Front1"){
			clickedCard = currSurvivor.front1;
		}
		else if(clicked.name == "Front2"){
			clickedCard = currSurvivor.front2;
		}
	}

	public void SurvTurnImgOut(){
		StartCoroutine(MoveSurvTurnImgOut());
	}
	public void ZombTurnImgOut(){
		StartCoroutine(MoveZombTurnImgOut());
	}

	IEnumerator MoveSurvTurnImgOut(){

		float t = 0;

		Vector2 currSurvPos = survTurnImg.rectTransform.anchoredPosition;
		Vector2 newSurvPos = new Vector2(222, -38);
		
		Vector2 currZombPos = zombTurnImg.rectTransform.anchoredPosition;
		Vector2 newZombPos = new Vector2(149, -38);

		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;

			survTurnImg.rectTransform.anchoredPosition = Vector2.Lerp(currSurvPos, newSurvPos, t);
			zombTurnImg.rectTransform.anchoredPosition = Vector2.Lerp(currZombPos, newZombPos, t);

			yield return 0;
		}

		zombTurnText.text = "";
	}

	IEnumerator MoveZombTurnImgOut(){
		float t = 0;
		
		Vector2 currSurvPos = survTurnImg.rectTransform.anchoredPosition;
		Vector2 newSurvPos = new Vector2(-149, -38);
		
		Vector2 currZombPos = zombTurnImg.rectTransform.anchoredPosition;
		Vector2 newZombPos = new Vector2(-222, -38);
		
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			
			survTurnImg.rectTransform.anchoredPosition = Vector2.Lerp(currSurvPos, newSurvPos, t);
			zombTurnImg.rectTransform.anchoredPosition = Vector2.Lerp(currZombPos, newZombPos, t);
			
			yield return 0;
		}

	}

	IEnumerator PlayerTurn(){
		StartCoroutine(MoveSurvTurnImgOut());
		playerGoing = true;
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			BoardLayout.S.createdZones[i].GetComponent<ZoneScript>().Unhighlight();
		}
		
		SurvivorToken.S.moveActionThingBack();
		while(true){
			if(waitForAaahhSpawn){
				yield return 0;
				continue;
			}
			if(spawningIndoors){
				yield return 0;
				continue;
			}

			if(currSurvivor == null){
				if(Input.GetKey(KeyCode.X)){
					foreach(Survivor s in survivors){
						s.DoNothing();
					}
				}
				survTurnText.text = "Choose a Survivor";
				//check to see if all survivors have finished - otherwise, keep looping
				bool allSurvivorsDone = true;
				for(int i = 0; i < survivors.Count; ++i){
					if(!survivors[i].HasGone) allSurvivorsDone = false;
				}
				if(allSurvivorsDone) break;


				yield return 0;
				continue;
			}
			currSurvivor.Highlight();

			if(currSurvivor.numActions == 0){
				if(currSurvivor.doingSkillStuff){
					yield return 0;
					continue;
				}
				SurvivorToken.S.moveActionThingOut(currSurvivor.name);
				ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
				currSurvivor.currTurn = false;
				currSurvivor.Unhighlight();
				currSurvivor.HasGone = true;
				SurvivorToken.S.resetTokens();
				currSurvivor = null;
				yield return 0;
				continue;
			}

			switch(ActionWheel.S.CurrAction){
			case "Move":
				if(Input.GetMouseButton(0) && !ActionWheel.S.mouseInWheel && !ActionWheel.S.mouseInWheelButton){
					GameObject clickedZone = ZoneSelector.S.CurrZone;
					ActionWheel.S.tradeBtn.interactable = false;
					if(clickedZone == null) break;
					GameObject survZone = currSurvivor.CurrZone;

					int distCanTravel = 1;
					if(currSurvivor.skills.Contains("2 Zones per Move Action")){
						distCanTravel++;
					}
					if(currSurvivor.skills.Contains("+1 Zone per Move")){
						distCanTravel++;
					}

					bool canTravelThere = false;
					if(distCanTravel == 1){
						canTravelThere = ZoneSelector.S.IsNeighborOf(clickedZone, currSurvivor.CurrZone);
					}
					else{
						canTravelThere = ZoneSelector.S.ZoneDistance(survZone, clickedZone) <= distCanTravel && ZoneSelector.S.ZoneDistance(survZone, clickedZone) > 0;
					}

					if(canTravelThere){
						ZoneScript zs = currSurvivor.CurrZone.GetComponent<ZoneScript>();

						int actionCount = 1;
						
						if(zs.EnemiesInZone() > 0) actionCount = 2;

						if(currSurvivor.skills.Contains("Slippery")) actionCount = 1;

						currSurvivor.MoveTo(clickedZone, actionCount);
					}
				}
				break;

			case "Ranged":
				if(needToChooseWeapon){
					survTurnText.text = "Choose A Weapon!";
					if(clickedCard == currSurvivor.front1 || clickedCard == currSurvivor.front2){
						needToChooseWeapon = false;
						attackingWeapon = clickedCard;
						if(clickedCard == currSurvivor.front1){
							SurvivorToken.S.front1.image.color = Color.yellow;
							SurvivorToken.S.front2.image.color = Color.white;
						}
						if(clickedCard == currSurvivor.front2){
							SurvivorToken.S.front2.image.color = Color.yellow;
							SurvivorToken.S.front1.image.color = Color.white;
						}

						RangedZoneSetup();
					}
				}
				else{
					if(Input.GetMouseButton(0) && !ActionWheel.S.mouseInWheel && !ActionWheel.S.mouseInWheelButton){
						GameObject clickedZone = ZoneSelector.S.CurrZone;
						GameObject survZone = currSurvivor.CurrZone;
						List<GameObject> rangeList = ZoneSelector.S.GetZonesCanSeeFromInRange(survZone, attackingWeapon.closeRange, attackingWeapon.farRange);
						
						if(rangeList.Contains(clickedZone) && clickedZone.GetComponent<ZoneScript>().EnemiesInZone() > 0){
							RangedAttackSetup(clickedZone);
						}
					}
				}
				break;
			case "Melee":
				if(needToChooseWeapon){
					survTurnText.text = "Choose A Weapon!";
					if(clickedCard == currSurvivor.front1 || clickedCard == currSurvivor.front2){
						needToChooseWeapon = false;
						attackingWeapon = clickedCard;
						if(clickedCard == currSurvivor.front1){
							SurvivorToken.S.front1.image.color = Color.yellow;
							SurvivorToken.S.front2.image.color = Color.white;
						}
						if(clickedCard == currSurvivor.front2){
							SurvivorToken.S.front2.image.color = Color.yellow;
							SurvivorToken.S.front1.image.color = Color.white;
						}
						MeleeSetup();
					}
				}
				break;

			case "Search":
				if(!playerSearching) {
					if(currSurvivor.HasCard("Flashlight") && shouldUseFlashlight){
						SearchSetup();
						shouldUseFlashlight = false;
					}
					else{
						shouldUseFlashlight = true;
						ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
						if(!currSurvivor.skills.Contains("+1 free Search Action"))
							currSurvivor.numActions--;
					}
				}
				break;
			case "ReorganizeInventory":
				if(!playerOrganizing) {
					ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
					currSurvivor.numActions--;
				}
				break;
			case "Trade":
				if(!playerTrading) {
					ActionWheel.S.ActionClick(ActionWheel.S.CurrAction);
					currSurvivor.numActions--;
				}
				break;
			}

			if(ActionWheel.S.CurrAction != "Trade" && ActionWheel.S.CurrAction != "Search" && ActionWheel.S.CurrAction != "Ranged")
				survTurnText.text = "Actions Remaining: " + currSurvivor.numActions;

			yield return 0;
		}
		SurvivorToken.S.MoveTokensOffscreen();
		ActionWheel.S.MoveWheelUp();

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
		foreach(Survivor surv in survivors){
			surv.hasUsedShotty = false;
		}
		zombTurnText.text = "Zombies' Move!";
		StartCoroutine(MoveZombTurnImgOut());
		zombieGoing = true;

		//Move Zombies on board
		CameraController.S.ZoomOut(0.5f);
		yield return new WaitForSeconds(0.5f);

		foreach(GameObject zone in BoardLayout.S.createdZones){
			if(allZombies.Count == 0) continue;
			while(zombiesAttacking){
				yield return 0;
			}
			if(zone.GetComponent<ZoneScript>().EnemiesInZone() == 0) {
				yield return 0;
				continue;
			}

			
			zone.GetComponent<ZoneScript>().DoZombieActions();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1);
		while(zombiesAttacking){
			yield return 0;
		}
		
		zombTurnText.text = "Zombies' Spawn!";
		Vector2 currPos = ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition;
		Vector2 desired = new Vector2(0, -35);
		ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "";
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition = Vector2.Lerp(currPos, desired, t);
			
			yield return 0;
		}
		//Spawn Zombies
		foreach(GameObject zone in zombieSpawnZones){
			CameraController.S.MoveTo(zone.transform.position + new Vector3(0, 1, 0) + Vector3.back / 3, 1);
			zone.GetComponent<ZoneScript>().Highlight();
			yield return new WaitForSeconds(1);

			SpawnZombiesAt(zone);
			yield return new WaitForSeconds(1);
			zone.GetComponent<ZoneScript>().Unhighlight();

		}
		currPos = desired;
		desired = new Vector2(0, 35);
		
		ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "";
		t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition = Vector2.Lerp(currPos, desired, t);
			
			yield return 0;
		}

		foreach(Survivor surv in survivors){
			surv.HasGone = false;
			surv.numActions = surv.maxActions;
			surv.hasSearched = false;
			surv.hasDoneMove = false;
			surv.hasDoneCombatAction = false;
		}
		foreach(Enemy zomb in allZombies){
			zomb.hasDoneAction = false;
		}
		foreach(GameObject zone in BoardLayout.S.createdZones){
			zone.GetComponent<ZoneScript>().RemoveNoiseTokens();
		}
		
		CameraController.S.ZoomOut(0.5f);
		yield return new WaitForSeconds(0.5f);
		ActionWheel.S.MoveWheelDown();
		SurvivorToken.S.MoveTokensOnScreen();
		zombieGoing = false;
		playerTurn = true;
	}

	void SpawnZombiesAt(GameObject zone){

		SpawnDeck.SpawnCard card = SpawnDeck.S.GetSpawnCard();
		int topPLevel = HighestPlayerLevel();

		Enemy.EnemyType type = Enemy.EnemyType.None;
		int zombieType = 0;
		int numToSpawn = 0;
		if(topPLevel == 0){
			type = card.blueLevelType;
			numToSpawn = card.blueLevelNum;
		}
		else if(topPLevel == 1){
			type = card.yellowLevelType;
			numToSpawn = card.yellowLevelNum;
		}
		else if(topPLevel == 2){
			type = card.orangeLevelType;
			numToSpawn = card.orangeLevelNum;
		}
		else if(topPLevel == 3){
			type = card.redLevelType;
			numToSpawn = card.redLevelNum;
		}

		if(numToSpawn == 0){
			ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "No Zombies Spawn!";
		}
		else if(type == Enemy.EnemyType.Walker){
			if(numToSpawn == 1){
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Walker Spawns!";
			}
			else{
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Walkers Spawn!";
			}

		}
		else if(type == Enemy.EnemyType.Runner) {
			if(numToSpawn == 1){
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Runner Spawns!";
			}
			else{
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Runners Spawn!";
			}
			zombieType = 1;
		}
		else if(type == Enemy.EnemyType.Fatty) {
			if(numToSpawn == 1){
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Fatty and " + numToSpawn * 2 + " Walkers Spawn!";
			}
			else{
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Fatties and " + numToSpawn * 2 + " Walkers  Spawn!";
			}
			zombieType = 2;
		}
		else if(type == Enemy.EnemyType.Abomination) {
			zombieType = 3;
			foreach(Enemy enemy in allZombies){
				if (enemy.type == Enemy.EnemyType.Abomination) {
					zombieType = 2;
					ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = numToSpawn + " Fatty and " + numToSpawn * 2 + " Walkers Spawn!";
				}
			}
			if(zombieType == 3){
				ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "An Abomination has Spawned!";
			}
		}

		//Random zombie spawn while we don't have cards fully implemented yet

		Vector3 topRightCorner = zone.GetComponent<BoxCollider>().bounds.max;
		Vector3 topLeftCorner = topRightCorner;
		topLeftCorner.x = zone.GetComponent<BoxCollider>().bounds.min.x;
		topRightCorner.z += -0.03f;
		topLeftCorner.z += -0.03f;
		
		Vector3 spawnPos = Vector3.Lerp (topLeftCorner, topRightCorner, (zombieType + 1) / 5.0f);

		if(zombieType == 3) spawnPos = zone.transform.position;


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

	public void SetZombieNumText(Vector3 pos, int num, string zombieType, int damageToKill, int speed){
		Vector2 viewportPoint = Camera.main.WorldToScreenPoint(pos + new Vector3(0, 0, 0.3f)); //convert game object position to VievportPoint
		zombieDescription.transform.position = viewportPoint;

		zombieDescription.transform.FindChild("Number").GetComponentInChildren<Text>().text = "Number in Zone: " + num;
		zombieDescription.transform.FindChild("Damage").GetComponentInChildren<Text>().text = "Damage To Kill: " + damageToKill;
		zombieDescription.transform.FindChild("Speed").GetComponentInChildren<Text>().text = "Zombie Speed: " + speed;
		zombieDescription.transform.FindChild("ZombieType").GetComponentInChildren<Text>().text = "Zombie Type: " + zombieType;
		zombieDescription.transform.FindChild("ZombieType2").GetComponentInChildren<Text>().text = "Zombie Type: " + zombieType;
	}

	public void MoveZombieNumOff(){
		zombieDescription.transform.position = new Vector3(-2000,-2000,0);
	}

	public int HighestPlayerLevel(){
		int highestLevel = 0;
		foreach(Survivor surv in survivors){
			if(surv.currLevel > highestLevel) highestLevel = surv.currLevel;
		}
		return highestLevel;
	}

	public void ShowReload(string slot){
		if(GameController.S.currSurvivor == null) return;

		switch(slot){
		case("front1"):
			if(currSurvivor.front1.cardName == "Sawed Off Shotgun" && currSurvivor.hasUsedShotty){
				reloadImg.rectTransform.anchoredPosition = new Vector2(180, 55);
				reloadImgOut = true;
				btnShowingReload = slot;
			}
			break;
		case("front2"):
			if(currSurvivor.front2.cardName == "Sawed Off Shotgun" && currSurvivor.hasUsedShotty){
				reloadImg.rectTransform.anchoredPosition = new Vector2(180, 55);
				reloadImgOut = true;
				btnShowingReload = slot;
			}
			break;
		}

	}

	public void HideReload(string slot){
		if(GameController.S.currSurvivor == null) return;
		if(btnShowingReload != slot) return;

		reloadImgOut = false;
		reloadImg.rectTransform.anchoredPosition = new Vector2(-300, 55);
	}

	IEnumerator ShowReload(){
		reloadImg.GetComponentInChildren<Text>().text = "Reloaded!";
		yield return new WaitForSeconds(1);
		reloadImg.rectTransform.anchoredPosition = new Vector2(-300, 55);
	}

	public void ClickReload(string slot){
		if(GameController.S.currSurvivor == null) return;
		switch(slot){
		case("front1"):
			if(currSurvivor.front1.cardName == "Sawed Off Shotgun" && currSurvivor.hasUsedShotty){
				currSurvivor.hasUsedShotty = false;
				btnShowingReload = "";
				currSurvivor.numActions--;
				StartCoroutine(ShowReload());
			}
			break;
		case("front2"):
			if(currSurvivor.front2.cardName == "Sawed Off Shotgun" && currSurvivor.hasUsedShotty){
				currSurvivor.hasUsedShotty = false;
				btnShowingReload = "";
				currSurvivor.numActions--;
				StartCoroutine(ShowReload());			
			}
			break;
		}

	}

}
