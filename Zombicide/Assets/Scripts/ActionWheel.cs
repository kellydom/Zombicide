﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionWheel : MonoBehaviour {
	public static ActionWheel S;


	public bool mouseInWheel;
	public bool mouseInWheelButton;

	bool wheelIsMinimized;
	bool wheelIsChanging;

	public Button middleButton;
	public Image wheelImage;

	public Sprite buttonWhite;
	public Sprite buttonBlack;

	public Text actionText;

	float scale;

	string currAction = "";
	public string CurrAction{
		get{return currAction;}
	}

	public Button objBtn;
	public Button nothingBtn;
	public Button moveBtn;
	public Button rangedBtn;
	public Button meleeBtn;
	public Button openDoorBtn;
	public Button driveBtn;
	public Button tradeBtn;
	public Button switchSeatsBtn;
	public Button makeNoiseBtn;
	public Button searchBtn;
	public Button invBtn;

	Vector3 wheelStartingPos;

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

		wheelIsChanging = false;
		wheelIsMinimized = true;

		wheelImage.color = new Color(1,1,1,0);

		scale = wheelImage.transform.localScale.x;
		wheelImage.transform.localScale = Vector3.zero;
		wheelImage.transform.eulerAngles = new Vector3(0,0,90);

		actionText.text = "";

		wheelStartingPos = Camera.main.ScreenToViewportPoint(wheelImage.transform.position);
	}

	void CheckDoor(){
		if(!GameController.S.currSurvivor.front1.openDoor && !GameController.S.currSurvivor.front2.openDoor){
			openDoorBtn.interactable = false;
			return;
		}

		int pZone = GameController.S.currSurvivor.CurrZone.GetComponent<ZoneScript>().zoneNum;

		bool interactable = false;
		foreach(BoardLayout.Door door in BoardLayout.S.doorConnections){
			if(door.zoneOne == pZone || door.zoneTwo == pZone){
				if(!door.isOpened){
					interactable = true;
				}
			}
		}

		openDoorBtn.interactable = interactable;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameController.S.endGame){
			StartCoroutine(RetractWheel());
			return;
		}
		if(!wheelIsMinimized && !wheelIsChanging){
			if(!mouseInWheel && !mouseInWheelButton){
				StartCoroutine(RetractWheel());
			}
		}

		if(GameController.S.currSurvivor == null){
			objBtn.interactable = false;
			nothingBtn.interactable = false;
			moveBtn.interactable = false;
			rangedBtn.interactable = false;
			meleeBtn.interactable = false;
			openDoorBtn.interactable = false;
			//driveBtn.interactable = false;
			tradeBtn.interactable = false;
			//switchSeatsBtn.interactable = false;
			makeNoiseBtn.interactable = false;
			searchBtn.interactable = false;
			invBtn.interactable = false;

			return;
		}
		else{
			nothingBtn.interactable = true;
			makeNoiseBtn.interactable = true;
			invBtn.interactable = true;
		}
		CheckDoor();
		
		int zoneNum = GameController.S.currSurvivor.CurrZone.GetComponent<ZoneScript>().zoneNum;
		if(GameController.S.currSurvivor.hasSearched || BoardLayout.S.isStreetZone[zoneNum] || GameController.S.currSurvivor.CurrZone.GetComponent<ZoneScript>().EnemiesInZone() > 0) searchBtn.interactable = false;
		else searchBtn.interactable = true;


		if(!GameController.S.currSurvivor.CanMove()) moveBtn.interactable = false;
		else moveBtn.interactable = true;

		if(GameController.S.currSurvivor.CurrZone.GetComponent<ZoneScript>().objectiveInRoom == null) objBtn.interactable = false;
		else objBtn.interactable = true;

		bool canTrade = false;
		foreach (Survivor guy in GameController.S.survivors) {
			if(guy == GameController.S.currSurvivor) continue;
			if(guy.CurrZone == GameController.S.currSurvivor.CurrZone) {
				canTrade = true;
			}
		}
		if(canTrade) tradeBtn.interactable = true;
		else tradeBtn.interactable = false;

		//meleeBtn.interactable = true;
		if(GameController.S.currSurvivor.CanDoMelee()) meleeBtn.interactable = true;
		else  meleeBtn.interactable = false;

		//rangedBtn.interactable = true;
		if(GameController.S.currSurvivor.CanDoRanged()) rangedBtn.interactable = true;
		else  rangedBtn.interactable = false;
	
	}
	public void MouseEnterWheel(){
		if(GameController.S.endGame){
			return;
		}
		if(GameController.S.currSurvivor != null){
			if(GameController.S.currSurvivor.doingSkillStuff){
				return;
			}
		}
		if(GameController.S.waitForAaahhSpawn){
			return;
		}
		if(GameController.S.spawningIndoors){
			return;
		}
		mouseInWheel = true;
	}
	
	public void MouseExitWheel(){
		mouseInWheel = false;
		
	}
	
	public void MouseEnterButton(){
		if(GameController.S.currSurvivor != null){
			if(GameController.S.currSurvivor.doingSkillStuff){
				return;
			}
		}
		if(GameController.S.waitForAaahhSpawn){
			return;
		}
		if(GameController.S.spawningIndoors){
			return;
		}

		mouseInWheelButton = true;

		if(wheelIsMinimized && !wheelIsChanging){
			StartCoroutine(ExpandWheel());
		}
	}
	
	public void MouseExitButton(){
		mouseInWheelButton = false;
		
	}

	IEnumerator ExpandWheel(){
		wheelIsChanging = true;

		middleButton.image.sprite = buttonWhite;
		wheelImage.color = new Color(1,1,1,1);

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.1f;

			Vector3 newScale = Vector3.Lerp(Vector3.zero, new Vector3(scale, scale), t);
			wheelImage.transform.localScale = newScale;

			wheelImage.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(90, 0, t));

			yield return 0;

		}

		wheelIsChanging = false;
		wheelIsMinimized = false;
	}

	IEnumerator RetractWheel(){
		wheelIsChanging = true;

		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.1f;
			
			Vector3 newScale = Vector3.Lerp(new Vector3(scale, scale), Vector3.zero, t);
			wheelImage.transform.localScale = newScale;
			
			wheelImage.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 90, t));

			yield return 0;
			
		}

		middleButton.image.sprite = buttonBlack;
		wheelImage.color = new Color(1,1,1,0);
		
		wheelIsChanging = false;
		wheelIsMinimized = true;

	}

	IEnumerator WheelUpCo(){
		float t = 0;
		Vector3 currPos = wheelImage.transform.position;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			
			wheelImage.transform.position = Camera.main.ViewportToScreenPoint(Vector3.Lerp(currPos, wheelStartingPos + Vector3.up, t));

			yield return 0;
		}
	}

	IEnumerator WheelDownCo(){
		
		float t = 0;
		Vector3 currPos = wheelImage.transform.position;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			
			wheelImage.transform.position = Camera.main.ViewportToScreenPoint(Vector3.Lerp(currPos, wheelStartingPos, t));
			
			yield return 0;
		}
	}

	public void MoveWheelUp(){
		middleButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3000);
		StartCoroutine(WheelUpCo());
	}

	public void MoveWheelDown(){
		middleButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -124);
		StartCoroutine(WheelDownCo());
	}

	public void ActionWheelButtonClick(){
		if(GameController.S.endGame){
			return;
		}
		if(wheelIsChanging) return;

		//if(wheelIsMinimized) StartCoroutine(ExpandWheel());
		//else StartCoroutine(RetractWheel());
	}

	public void ActionHover(string action){
		if(GameController.S.endGame){
			return;
		}
		if(GameController.S.currSurvivor != null){
			if(GameController.S.currSurvivor.doingSkillStuff){
				return;
			}
		}
		if(GameController.S.waitForAaahhSpawn){
			return;
		}
		if(GameController.S.spawningIndoors){
			return;
		}
		MouseEnterButton();
		switch (action){
		case "Melee":
			actionText.text = "Melee";
			break;
		case "Ranged":
			actionText.text = "Ranged";
			break;
		case "Move":
			actionText.text = "Move";
			break;
		case "OpenDoor":
			actionText.text = "Open Door";
			break;
		case "Search":
			actionText.text = "Search";
			break;
		case "MakeNoise":
			actionText.text = "Make Noise";
			break;
		case "ReorganizeInventory":
			actionText.text = "Reorganize Inventory";
			break;
		case "DoNothing":
			actionText.text = "Do Nothing";
			break;
		case "Trade":
			actionText.text = "Trade";
			break;
		case "SwitchSeats":
			actionText.text = "Switch Seats";
			break;
		case "DriveCar":
			actionText.text = "Drive Car";
			break;
		case "TakeObjective":
			actionText.text = "Take Objective";
			break;
		}
		actionText.color = Color.white;

	}

	public void ActionMouseLeave(){
		if(GameController.S.endGame){
			return;
		}
		MouseExitButton();
		if(currAction == "") actionText.text = "";	
		else actionText.text = currAction;
		
		actionText.text = "<b>" + actionText.text + "</b>";
		actionText.color = Color.green;
	}

	public void ActionClick(string action){
		if(GameController.S.endGame){
			return;
		}
		for(int i = 0; i < BoardLayout.S.createdZones.Count; ++i){
			BoardLayout.S.createdZones[i].GetComponent<ZoneScript>().Unhighlight();
		}

		if(GameController.S.currSurvivor == null) return;
		if(currAction == action){
			switch(action){
			case "Move":
				break;
			}


			currAction = "";
			actionText.text = "";
			return;
		}
		currAction = action;

		switch (action){
		case "Melee":
			GameController.S.MeleeWeaponSetup();
			actionText.text = "Melee";
			break;
		case "Ranged":
			GameController.S.RangedWeaponSetup();
			actionText.text = "Ranged";
			break;
		case "Move":
			GameController.S.MoveSetup();
			actionText.text = "Move";
			break;
		case "OpenDoor":
			GameController.S.OpenDoorSetup();
			actionText.text = "Open Door";
			break;
		case "Search":
			GameController.S.SearchSetup();
			actionText.text = "Search";
			break;
		case "MakeNoise":
			GameController.S.MakeNoiseSetup();
			actionText.text = "Make Noise";
			break;
		case "ReorganizeInventory":
			GameController.S.ReorganizeInvSetup();
			actionText.text = "Reorganize Inventory";
			break;
		case "DoNothing":
			GameController.S.DoNothingSetup();
			actionText.text = "Do Nothing";
			break;
		case "Trade":
			GameController.S.tradeSetup();
			actionText.text = "Trade";
			break;
		case "SwitchSeats":
			GameController.S.ChangeSeatsSetup();
			actionText.text = "Switch Seats";
			break;
		case "DriveCar":
			GameController.S.DriveCarSetup();
			actionText.text = "Drive Car";
			break;
		case "TakeObjective":
			GameController.S.TakeObjSetup();
			actionText.text = "Take Objective";
			break;
		}
		actionText.text = "<b>" + actionText.text + "</b>";
		actionText.color = Color.green;

	}
}
