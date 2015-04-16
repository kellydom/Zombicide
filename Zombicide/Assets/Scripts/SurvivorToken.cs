using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SurvivorToken : MonoBehaviour {
	public static SurvivorToken S;

	public Button phil;
	public Button wanda;
	public Button josh;
	public Button ned;
	public Button front1;
	public Button front2;
	public Button back1;
	public Button back2;
	public Button back3;
	public Button tempf1;
	public Button tempf2;
	public Button tempb1;
	public Button tempb2;
	public Button tempb3;
	bool philClicked = false;
	bool wandaClicked = false;
	bool nedClicked = false;
	bool joshClicked = false;
	bool clicked = false;
	Survivor currSurvivor;
	Survivor tempSurvivor;
	Survivor tradingSurvivor;
	bool inTrade = false;
	int setCards = -1000;
	int frontCards = 148;
	int backCards = 48;
	int tempFrontCards = 1200;
	int tempBackCards = 1300;
	bool tempOut = false;
	bool cardsOut = false;

	public bool sacrificeThem = false;
	public Button discardComplete;
	public Button woundComplete;

	string whichCard;
	bool cardSwitch = false;

	bool leftTradeSelected = false;
	bool rightTradeSelected = false;
	Button tempLeft;
	Button tempRight;
	string tradeLeft;
	string tradeRight;
	Card left;
	Card right;

	public Button tradeCards;

	public Image skillList;
	public Image chooseSkillImage;
	public Image newSkillImage;

	bool discardForWound = false;
	bool woundTime = false;
	Survivor woundedSurvivor;
	bool chosenWoundedSurvivor = false;
	
	public List<Sprite>	levelSprites;

	public Image skillDescription;
	Image skillShowingDescriptionOf;
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
		float high = Screen.width/8.0f;
		float wide = high * 2063/3186;

		front1.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		front2.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		back1.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		back2.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		back3.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		tempf1.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		tempf2.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		tempb1.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		tempb2.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		tempb3.image.rectTransform.sizeDelta = new Vector3(wide, high, 0);
		//Canvas canvas = GameObject.FindObjectOfType(Canvas) as Canvas;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void tokenOnHover(string name) {
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

		if (chosenWoundedSurvivor || inTrade) 
			return;
		Vector3 newScale = new Vector3 (4, 4, 0);
		switch (name) {
		case "Wanda":
			wanda.image.transform.localScale = newScale;
			break;
		case "Phil":
			phil.image.transform.localScale = newScale;
			break;
		case "Ned":
			ned.image.transform.localScale = newScale;
			break;
		case "Josh":
			josh.image.transform.localScale = newScale;
			break;
		}
		for (int i = 0; i < GameController.S.survivors.Count; i++) {
			if(name == GameController.S.survivors[i].name) {
				GameController.S.survivors[i].Highlight();
			}
		}
		for (int i = 0; i < GameController.S.survivors.Count; i++) {
			if(name == GameController.S.survivors[i].name) {
				if(!clicked)
					currSurvivor = GameController.S.survivors[i];
				else 
					tempSurvivor = GameController.S.survivors[i];
			}
		}
		if (!clicked) {
			expandCards ();
			Survivor surv = null;
			foreach(Survivor tempSurv in GameController.S.survivors){
				if(name == tempSurv.name) surv = tempSurv;
			}
			ShowSkills(surv);
		} 
		else if(currSurvivor.name != name && !inTrade && !sacrificeThem){
			expandTempCards();
			tempOut = true;
			HideSkills();
		}
	}

	public void tokenOnLeave(string name) {
		if(GameController.S.currSurvivor != null){
			if(GameController.S.currSurvivor.doingSkillStuff){
				return;
			}
		}
		if(GameController.S.waitForAaahhSpawn){
			return;
		}
		if(GameController.S.spawningIndoors || chosenWoundedSurvivor){
			return;
		}
		if(inTrade) return;

		Vector3 newScale = new Vector3 (1, 1, 0);
		if (!clicked) {
			removeCards ();
			HideSkills();
		} 
		if (tempOut) {
			removeTempCards ();
			tempOut = false;
			ShowSkills(GameController.S.currSurvivor);
		}
		switch (name) {
		case "Wanda":
			if(!wandaClicked) {
				wanda.image.transform.localScale = newScale;
			}
			break;
		case "Phil":
			if(!philClicked) {
				phil.image.transform.localScale = newScale;
			}
			break;
		case "Ned":
			if(!nedClicked) {
				ned.image.transform.localScale = newScale;
			}
			break;
		case "Josh":
			if(!joshClicked) {
				josh.image.transform.localScale = newScale;
			}
			break;
		}
		
		for (int i = 0; i < GameController.S.survivors.Count; i++) {
			if(name == GameController.S.survivors[i].name) {
				GameController.S.survivors[i].Unhighlight();
			}
		}
	}

	void expandTempCards() {
		tempf1.image.sprite = tempSurvivor.front1.but.image.sprite;
		tempf2.image.sprite = tempSurvivor.front2.but.image.sprite;
		tempb1.image.sprite = tempSurvivor.back1.but.image.sprite;
		tempb2.image.sprite = tempSurvivor.back2.but.image.sprite;
		tempb3.image.sprite = tempSurvivor.back3.but.image.sprite;

		float frontPos = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x - tempf1.GetComponent<RectTransform>().sizeDelta.x * 3.0f/2.0f;
		float backPos = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x - tempf1.GetComponent<RectTransform>().sizeDelta.x * 1.0f/2.0f;

		tempf1.transform.position = new Vector3 (frontPos, front1.transform.position.y);
		tempf2.transform.position = new Vector3 (frontPos, front2.transform.position.y);
		tempb1.transform.position = new Vector3 (backPos, back1.transform.position.y);
		tempb2.transform.position = new Vector3 (backPos, back2.transform.position.y);
		tempb3.transform.position = new Vector3 (backPos, back3.transform.position.y);

	}

	void removeTempCards() {
		tempf1.transform.position = new Vector3 (setCards, front1.transform.position.y);
		tempf2.transform.position = new Vector3 (setCards, front2.transform.position.y);
		tempb1.transform.position = new Vector3 (setCards, back1.transform.position.y);
		tempb2.transform.position = new Vector3 (setCards, back2.transform.position.y);
		tempb3.transform.position = new Vector3 (setCards, back3.transform.position.y);
	}

	void expandCards() {
		cardsOut = true;
		front1.image.sprite = currSurvivor.front1.but.image.sprite;
		front2.image.sprite = currSurvivor.front2.but.image.sprite;
		back1.image.sprite = currSurvivor.back1.but.image.sprite;
		back2.image.sprite = currSurvivor.back2.but.image.sprite;
		back3.image.sprite = currSurvivor.back3.but.image.sprite;
		
		float frontPos = front1.GetComponent<RectTransform>().sizeDelta.x * 3.0f/2.0f;
		float backPos = front1.GetComponent<RectTransform>().sizeDelta.x * 1.0f/2.0f;

		front1.transform.position = new Vector3 (frontPos, back2.transform.position.y + back2.GetComponent<RectTransform>().sizeDelta.y/2.0f);
		front2.transform.position = new Vector3 (frontPos, back2.transform.position.y - back2.GetComponent<RectTransform>().sizeDelta.y/2.0f);
		back1.transform.position = new Vector3 (backPos, back2.transform.position.y + back2.GetComponent<RectTransform>().sizeDelta.y);
		back2.transform.position = new Vector3 (backPos, back2.transform.position.y);
		back3.transform.position = new Vector3 (backPos, back2.transform.position.y - back2.GetComponent<RectTransform>().sizeDelta.y);
	}

	void removeCards() {
		cardsOut = false;
		front1.transform.position = new Vector3 (setCards, front1.transform.position.y);
		front2.transform.position = new Vector3 (setCards, front2.transform.position.y);
		back1.transform.position = new Vector3 (setCards, back1.transform.position.y);
		back2.transform.position = new Vector3 (setCards, back2.transform.position.y);
		back3.transform.position = new Vector3 (setCards, back3.transform.position.y);
	}

	IEnumerator MoveSkillsOnscreen(){
		Vector3 currPos = skillList.rectTransform.anchoredPosition;

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			skillList.rectTransform.anchoredPosition = Vector3.Lerp(currPos, new Vector3(-90, 103, 0),t);
			yield return 0;
		}
	}
	
	IEnumerator MoveSkillsOffscreen(){
		Vector3 currPos = skillList.rectTransform.anchoredPosition;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			skillList.rectTransform.anchoredPosition = Vector3.Lerp(currPos, new Vector3(180, 103, 0),t);
			yield return 0;
		}
	}

	public void ShowSkills(Survivor surv){
		Text skill1 = skillList.transform.FindChild("Skill1").GetComponentInChildren<Text>();
		skill1.text = surv.skills[0];
		
		skillList.transform.FindChild("Skill2").GetComponent<RectTransform>().anchoredPosition = new Vector3(-13, -82, 0);
		Text skill2 = skillList.transform.FindChild("Skill2").GetComponentInChildren<Text>();
		skill2.text = surv.skills[1];
		
		skillList.transform.FindChild("Skill3").GetComponent<RectTransform>().anchoredPosition = new Vector3(-13, -129, 0);
		Text skill3 = skillList.transform.FindChild("Skill3").GetComponentInChildren<Text>();
		skill3.text = surv.skills[2];
		
		skillList.transform.FindChild("Skill4").GetComponent<RectTransform>().anchoredPosition = new Vector3(-13, -176, 0);
		Text skill4 = skillList.transform.FindChild("Skill4").GetComponentInChildren<Text>();
		skill4.text = surv.skills[3];

		if(skill4.text == ""){
			skillList.transform.FindChild("Skill4").GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 45);
		}
		if(skill3.text == ""){
			skillList.transform.FindChild("Skill4").GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 45);
			skillList.transform.FindChild("Skill3").GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 45);

		}
		if(skill2.text == ""){
			skillList.transform.FindChild("Skill4").GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 45);
			skillList.transform.FindChild("Skill3").GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 45);
			skillList.transform.FindChild("Skill2").GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 45);

		}

		StartCoroutine(MoveSkillsOnscreen());
	}

	public void HideSkills(){
		StartCoroutine(MoveSkillsOffscreen());
	}

	public void tokenOnClicked(string name) {
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
		bool reload = false;
		Vector3 newScale = new Vector3 (4, 4, 0);
		if (ActionWheel.S.CurrAction == "Trade" || sacrificeThem)
			return;
		clicked = true;
		switch (name) {
		case "Wanda":
			if(wandaClicked) {
				clicked = false;
				removeCards();
				wandaClicked = false;
				wanda.image.transform.localScale = new Vector3(1, 1, 0);
				GameController.S.currSurvivor = null;
				break;
			}
			if(!cardsOut)
				expandCards();
			wanda.image.transform.localScale = newScale;
			if(philClicked || nedClicked || joshClicked) {
				if (GameController.S.currSurvivor != null)
					if (GameController.S.currSurvivor.numActions < 3)
						return;
				removeCards();
				//expandCards();
				reload = true;
				GameController.S.currSurvivor.Unhighlight();
				phil.image.transform.localScale = new Vector3(1, 1, 0);
				philClicked = false;
				ned.image.transform.localScale = new Vector3(1, 1, 0);
				nedClicked = false;
				josh.image.transform.localScale = new Vector3(1, 1, 0);
				joshClicked = false;
			}
			wandaClicked = true;
			for (int i = 0; i < GameController.S.survivors.Count; i++) {
				if("Wanda" == GameController.S.survivors[i].name) {
					GameController.S.currSurvivor = GameController.S.survivors[i];
					currSurvivor = GameController.S.survivors[i];
					if(reload)
						removeTempCards();
						expandCards();
				}
			}

			break;
		case "Phil":
			if(philClicked) {
				philClicked = false;
				removeCards();
				clicked = false;
				phil.image.transform.localScale = new Vector3(1, 1, 0);
				GameController.S.currSurvivor = null;
				break;
			}
			phil.image.transform.localScale = newScale;
			if(!cardsOut)
				expandCards();
			if(wandaClicked || nedClicked || joshClicked) {
				if (GameController.S.currSurvivor != null)
					if (GameController.S.currSurvivor.numActions < 3)
						return;
				wanda.image.transform.localScale = new Vector3(1, 1, 0);
				wandaClicked = false;
				ned.image.transform.localScale = new Vector3(1, 1, 0);
				nedClicked = false;
				josh.image.transform.localScale = new Vector3(1, 1, 0);
				joshClicked = false;
				removeCards();
				//expandCards();
				reload = true;
				GameController.S.currSurvivor.Unhighlight();
			}
			philClicked = true;
			for (int i = 0; i < GameController.S.survivors.Count; i++) {
				if("Phil" == GameController.S.survivors[i].name) {
					GameController.S.currSurvivor = GameController.S.survivors[i];
					currSurvivor = GameController.S.survivors[i];
					if(reload)
						removeTempCards();
						expandCards();
				}
			}
			break;
		case "Ned":
			if(nedClicked) {
				nedClicked = false;
				removeCards();
				clicked = false;
				ned.image.transform.localScale = new Vector3(1, 1, 0);
				GameController.S.currSurvivor = null;
				break;
			}
			ned.image.transform.localScale = newScale;
			if(!cardsOut)
				expandCards();
			if(wandaClicked || philClicked || joshClicked) {
				if (GameController.S.currSurvivor != null)
					if (GameController.S.currSurvivor.numActions < 3)
						return;
				wanda.image.transform.localScale = new Vector3(1, 1, 0);
				wandaClicked = false;
				phil.image.transform.localScale = new Vector3(1, 1, 0);
				philClicked = false;
				josh.image.transform.localScale = new Vector3(1, 1, 0);
				joshClicked = false;
				removeCards();
				//expandCards();
				reload = true;
				GameController.S.currSurvivor.Unhighlight();
			}
			nedClicked = true;
			for (int i = 0; i < GameController.S.survivors.Count; i++) {
				if("Ned" == GameController.S.survivors[i].name) {
					GameController.S.currSurvivor = GameController.S.survivors[i];
					currSurvivor = GameController.S.survivors[i];
					if(reload)
						removeTempCards();
					expandCards();
				}
			}
			break;
		case "Josh":
			if(joshClicked) {
				joshClicked = false;
				removeCards();
				clicked = false;
				josh.image.transform.localScale = new Vector3(1, 1, 0);
				GameController.S.currSurvivor = null;
				break;
			}
			josh.image.transform.localScale = newScale;
			if(!cardsOut)
				expandCards();
			if(wandaClicked || nedClicked || philClicked) {
				if (GameController.S.currSurvivor != null)
					if (GameController.S.currSurvivor.numActions < 3)
						return;
				wanda.image.transform.localScale = new Vector3(1, 1, 0);
				wandaClicked = false;
				ned.image.transform.localScale = new Vector3(1, 1, 0);
				nedClicked = false;
				phil.image.transform.localScale = new Vector3(1, 1, 0);
				philClicked = false;
				removeCards();
				//expandCards();
				reload = true;
				GameController.S.currSurvivor.Unhighlight();
			}
			joshClicked = true;
			for (int i = 0; i < GameController.S.survivors.Count; i++) {
				if("Josh" == GameController.S.survivors[i].name) {
					GameController.S.currSurvivor = GameController.S.survivors[i];
					currSurvivor = GameController.S.survivors[i];
					if(reload)
						removeTempCards();
					expandCards();
				}
			}
			break;
		}

		if(GameController.S.currSurvivor == null){
			HideSkills();
		}
		else{
			ShowSkills(GameController.S.currSurvivor);
		}
	}

	public void sendForSearch(string cardName) {
		if (!GameController.S.playerSearching)
			return;
		Survivor surv = GameController.S.currSurvivor;
		Card picked = new Card();
		switch (cardName) {
		case "front1":
			picked = surv.front1;
			surv.front1 = GameController.S.picked;
			front1.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "front2":
			picked = surv.front2;
			surv.front2 = GameController.S.picked;
			front2.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "back1":
			picked = surv.back1;
			surv.back1 = GameController.S.picked;
			back1.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "back2":
			picked = surv.back2;
			surv.back2 = GameController.S.picked;
			back2.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "back3":
			picked = surv.back3;
			surv.back3 = GameController.S.picked;
			back3.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		}
		cardSwitch = true;
		whichCard = cardName;
	}

	public void deleteSearchCard() {
		if (GameController.S.picked.cardName == "Wound") {
			GameController.S.survTurnText.text = "Can't discard Wound";
			return;
		}
		GameController.S.deck.returnToDeck (GameController.S.picked.cardName);
		GameController.S.pickedImage.transform.position = new Vector3 (-3000, 0, 0);
		GameController.S.deleteForSearch.transform.position = new Vector3 (-1000,0,0);
		GameController.S.playerSearching = false;
		ActionWheel.S.MoveWheelDown ();
		ShowSkills(GameController.S.currSurvivor);
	}

	void expandTrade() {
		HideSkills();
		tempf1.image.sprite = tradingSurvivor.front1.but.image.sprite;
		tempf2.image.sprite = tradingSurvivor.front2.but.image.sprite;
		tempb1.image.sprite = tradingSurvivor.back1.but.image.sprite;
		tempb2.image.sprite = tradingSurvivor.back2.but.image.sprite;
		tempb3.image.sprite = tradingSurvivor.back3.but.image.sprite;
		
		float frontPos = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x - tempf1.GetComponent<RectTransform>().sizeDelta.x * 3.0f/2.0f;
		float backPos = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x - tempf1.GetComponent<RectTransform>().sizeDelta.x * 1.0f/2.0f;
		
		tempf1.transform.position = new Vector3 (frontPos, front1.transform.position.y);
		tempf2.transform.position = new Vector3 (frontPos, front2.transform.position.y);
		tempb1.transform.position = new Vector3 (backPos, back1.transform.position.y);
		tempb2.transform.position = new Vector3 (backPos, back2.transform.position.y);
		tempb3.transform.position = new Vector3 (backPos, back3.transform.position.y);
	}

	public void finishTrade() {
		ShowSkills(GameController.S.currSurvivor);
		Card tempCard;
		//there's gotta be a very straightforward, easy way to do this but I'm dead
		//so.. here goes!
		switch (tradeLeft) {
		case "f1":
			switch(tradeRight) {
				case "tf1":
					tempCard = GameController.S.currSurvivor.front1;
					GameController.S.currSurvivor.front1 = tradingSurvivor.front1;
					tradingSurvivor.front1 = tempCard;
					front1.image.sprite = currSurvivor.front1.but.image.sprite;
					break;
				case "tf2":
					tempCard = GameController.S.currSurvivor.front1;
					GameController.S.currSurvivor.front1 = tradingSurvivor.front2;
					tradingSurvivor.front2 = tempCard;
					front1.image.sprite = currSurvivor.front1.but.image.sprite;
					break;
				case "tb1":
					tempCard = GameController.S.currSurvivor.front1;
					GameController.S.currSurvivor.front1 = tradingSurvivor.back1;
					tradingSurvivor.back1 = tempCard;
					front1.image.sprite = currSurvivor.front1.but.image.sprite;
					break;
				case "tb2":
					tempCard = GameController.S.currSurvivor.front1;
					GameController.S.currSurvivor.front1 = tradingSurvivor.back2;
					tradingSurvivor.back2 = tempCard;
					front1.image.sprite = currSurvivor.front1.but.image.sprite;
					break;
				case "tb3":
					tempCard = GameController.S.currSurvivor.front1;
					GameController.S.currSurvivor.front1 = tradingSurvivor.back3;
					tradingSurvivor.back3 = tempCard;
					front1.image.sprite = currSurvivor.front1.but.image.sprite;
					break;
			}
			break;
		case "f2":
			switch(tradeRight) {
				case "tf1":
					tempCard = GameController.S.currSurvivor.front2;
					GameController.S.currSurvivor.front2 = tradingSurvivor.front1;
					tradingSurvivor.front1 = tempCard;
					front2.image.sprite = currSurvivor.front2.but.image.sprite;
					break;
				case "tf2":
					tempCard = GameController.S.currSurvivor.front2;
					GameController.S.currSurvivor.front2 = tradingSurvivor.front2;
					tradingSurvivor.front2 = tempCard;
					front2.image.sprite = currSurvivor.front2.but.image.sprite;
					break;
				case "tb1":
					tempCard = GameController.S.currSurvivor.front2;
					GameController.S.currSurvivor.front2 = tradingSurvivor.back1;
					tradingSurvivor.back1 = tempCard;
					front2.image.sprite = currSurvivor.front2.but.image.sprite;
					break;
				case "tb2":
					tempCard = GameController.S.currSurvivor.front2;
					GameController.S.currSurvivor.front2 = tradingSurvivor.back2;
					tradingSurvivor.back2 = tempCard;
					front2.image.sprite = currSurvivor.front2.but.image.sprite;
					break;
				case "tb3":
					tempCard = GameController.S.currSurvivor.front2;
					GameController.S.currSurvivor.front2 = tradingSurvivor.back3;
					tradingSurvivor.back3 = tempCard;
					front2.image.sprite = currSurvivor.front2.but.image.sprite;
					break;
			}
			break;
		case "b1":
			switch(tradeRight) {
			case "tf1":
				tempCard = GameController.S.currSurvivor.back1;
				GameController.S.currSurvivor.back1 = tradingSurvivor.front1;
				tradingSurvivor.front1 = tempCard;
				back1.image.sprite = currSurvivor.back1.but.image.sprite;
				break;
			case "tf2":
				tempCard = GameController.S.currSurvivor.back1;
				GameController.S.currSurvivor.back1 = tradingSurvivor.front2;
				tradingSurvivor.front2 = tempCard;
				back1.image.sprite = currSurvivor.back1.but.image.sprite;
				break;
			case "tb1":
				tempCard = GameController.S.currSurvivor.back1;
				GameController.S.currSurvivor.back1 = tradingSurvivor.back1;
				tradingSurvivor.back1 = tempCard;
				back1.image.sprite = currSurvivor.back1.but.image.sprite;
				break;
			case "tb2":
				tempCard = GameController.S.currSurvivor.back1;
				GameController.S.currSurvivor.back1 = tradingSurvivor.back2;
				tradingSurvivor.back2 = tempCard;
				back1.image.sprite = currSurvivor.back1.but.image.sprite;
				break;
			case "tb3":
				tempCard = GameController.S.currSurvivor.back1;
				GameController.S.currSurvivor.back1 = tradingSurvivor.back3;
				tradingSurvivor.back3 = tempCard;
				back1.image.sprite = currSurvivor.back1.but.image.sprite;
				break;
			}
			break;
		case "b2":
			switch(tradeRight) {
			case "tf1":
				tempCard = GameController.S.currSurvivor.back2;
				GameController.S.currSurvivor.back2 = tradingSurvivor.front1;
				tradingSurvivor.front1 = tempCard;
				back2.image.sprite = currSurvivor.back2.but.image.sprite;
				break;
			case "tf2":
				tempCard = GameController.S.currSurvivor.back2;
				GameController.S.currSurvivor.back2 = tradingSurvivor.front2;
				tradingSurvivor.front2 = tempCard;
				back2.image.sprite = currSurvivor.back2.but.image.sprite;
				break;
			case "tb1":
				tempCard = GameController.S.currSurvivor.back2;
				GameController.S.currSurvivor.back2 = tradingSurvivor.back1;
				tradingSurvivor.back1 = tempCard;
				back2.image.sprite = currSurvivor.back2.but.image.sprite;
				break;
			case "tb2":
				tempCard = GameController.S.currSurvivor.back2;
				GameController.S.currSurvivor.back2 = tradingSurvivor.back2;
				tradingSurvivor.back2 = tempCard;
				back2.image.sprite = currSurvivor.back2.but.image.sprite;
				break;
			case "tb3":
				tempCard = GameController.S.currSurvivor.back2;
				GameController.S.currSurvivor.back2 = tradingSurvivor.back3;
				tradingSurvivor.back3 = tempCard;
				back2.image.sprite = currSurvivor.back2.but.image.sprite;
				break;
			}
			break;
		case "b3":
			switch(tradeRight) {
			case "tf1":
				tempCard = GameController.S.currSurvivor.back3;
				GameController.S.currSurvivor.back3 = tradingSurvivor.front1;
				tradingSurvivor.front1 = tempCard;
				back3.image.sprite = currSurvivor.back3.but.image.sprite;
				break;
			case "tf2":
				tempCard = GameController.S.currSurvivor.back3;
				GameController.S.currSurvivor.back3 = tradingSurvivor.front2;
				tradingSurvivor.front2 = tempCard;
				back3.image.sprite = currSurvivor.back3.but.image.sprite;
				break;
			case "tb1":
				tempCard = GameController.S.currSurvivor.back3;
				GameController.S.currSurvivor.back3 = tradingSurvivor.back1;
				tradingSurvivor.back1 = tempCard;
				back3.image.sprite = currSurvivor.back3.but.image.sprite;
				break;
			case "tb2":
				tempCard = GameController.S.currSurvivor.back3;
				GameController.S.currSurvivor.back3 = tradingSurvivor.back2;
				tradingSurvivor.back2 = tempCard;
				back3.image.sprite = currSurvivor.back3.but.image.sprite;
				break;
			case "tb3":
				tempCard = GameController.S.currSurvivor.back3;
				GameController.S.currSurvivor.back3 = tradingSurvivor.back3;
				tradingSurvivor.back3 = tempCard;
				back3.image.sprite = currSurvivor.back3.but.image.sprite;
				break;
			}
			break;
		}
		tradeCards.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
		tempLeft.image.color = Color.white;
		tempRight.image.color = Color.white;
		tempf1.transform.position = new Vector3 (setCards, front1.transform.position.y);
		tempf2.transform.position = new Vector3 (setCards, front2.transform.position.y);
		tempb1.transform.position = new Vector3 (setCards, back1.transform.position.y);
		tempb2.transform.position = new Vector3 (setCards, back2.transform.position.y);
		tempb3.transform.position = new Vector3 (setCards, back3.transform.position.y);
		inTrade = false;
		rightTradeSelected = false;
		leftTradeSelected = false;
		GameController.S.playerTrading = false;
		Vector3 newScale = new Vector3 (1, 1, 0);
		foreach(Survivor surv in GameController.S.survivors){
			if(surv == GameController.S.currSurvivor) continue;
			surv.Unhighlight();
			switch(surv.name){
			case "Wanda":
				wanda.image.transform.localScale = newScale;
				break;
			case "Phil":
				phil.image.transform.localScale = newScale;
				break;
			case "Ned":
				ned.image.transform.localScale = newScale;
				break;
			case "Josh": 
				josh.image.transform.localScale = newScale;
				break;
			}
		}
		ActionWheel.S.MoveWheelDown ();
		tradeCards.transform.position = new Vector3 (-1000, 0, 0);
	}

	void highlightCard(Button cards, string side) {
		if (side == "left" && !leftTradeSelected) {
			cards.image.color = Color.yellow;
			leftTradeSelected = true;
			tempLeft = cards;
			if(leftTradeSelected && rightTradeSelected) {
				tradeCards.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
				GameController.S.survTurnText.text = "Confirm trade";
			}
			return;
		} 
		else if (side == "Right" && !rightTradeSelected) {
			cards.image.color = Color.yellow;
			rightTradeSelected = true;
			tempRight = cards;
			if(leftTradeSelected && rightTradeSelected) {
				tradeCards.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
				GameController.S.survTurnText.text = "Confirm trade";
			}
			return;
		} 
		else if (side == "Right") {
			if(cards == tempRight)
				return;
			tempRight.image.color = Color.white;
			cards.image.color = Color.yellow;
			tempRight = cards;
			if(leftTradeSelected && rightTradeSelected) {
				tradeCards.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
				GameController.S.survTurnText.text = "Confirm trade";
			}
			return;
		} 
		else if (side == "left") {
			if(cards == tempLeft)
				return;
			tempLeft.image.color = Color.white;
			cards.image.color = Color.yellow;
			tempLeft = cards;
			if(leftTradeSelected && rightTradeSelected) {
				tradeCards.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
				GameController.S.survTurnText.text = "Confirm trade";
			}
			return;
		}

	}

	public void tradingCards(string whichPos) {
		if (ActionWheel.S.CurrAction != "Trade") {
			return;
		}
		switch (whichPos) {
		case "front1":
			//tempLeft = front1;
			highlightCard(front1, "left");
			tradeLeft = "f1";
			break;
		case "front2":
			//tempL = front2;
			highlightCard(front2, "left");
			tradeLeft = "f2";
			break;
		case "back1":
			//temp = back1;
			highlightCard(back1, "left");
			tradeLeft = "b1";
			break;
		case "back2":
			//temp = back2;
			highlightCard(back2, "left");
			tradeLeft = "b2";
			break;
		case "back3":
			//temp = back3;
			highlightCard(back3, "left");
			tradeLeft = "b3";
			break;
		case "tempf1":
			//temp = tempf1;
			right = tradingSurvivor.front1;
			highlightCard(tempf1, "Right");
			tradeRight = "tf1";
			break;
		case "tempf2":
			//temp = tempf2;
			right = tradingSurvivor.front2;
			highlightCard(tempf2, "Right");
			tradeRight = "tf2";
			break;
		case "tempb1":
			//temp = tempb1;
			highlightCard(tempb1, "Right");
			tradeRight = "tb1";
			break;
		case "tempb2":
			//temp = tempb2;
			highlightCard(tempb2, "Right");
			tradeRight = "tb2";
			break;
		case "tempb3":
			//temp = tempb3;
			highlightCard(tempb3, "Right");
			tradeRight = "tb3";
			break;
		}
	}

	public void trade(string tradeWith) {
		if (ActionWheel.S.CurrAction == "Trade") {
			GameController.S.survTurnText.text = "Select cards";
			inTrade = true;
			switch(tradeWith) {
			case "Wanda":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Wanda" == GameController.S.survivors[i].name) {
						tradingSurvivor = GameController.S.survivors[i];
					}
				}
				break;
			case "Phil":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Phil" == GameController.S.survivors[i].name) {
						tradingSurvivor = GameController.S.survivors[i];
					}
				}
				break;
			case "Ned":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Ned" == GameController.S.survivors[i].name) {
						tradingSurvivor = GameController.S.survivors[i];
					}
				}
				break;
			case "Josh":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Josh" == GameController.S.survivors[i].name) {
						tradingSurvivor = GameController.S.survivors[i];
					}
				}
				break;
			}
			expandTrade();
		}
	}

	public void ChooseSkill(int skillNum){
		GameController.S.currSurvivor.SelectedSkill(skillNum);
	}

	public void ShowSkillText(Image skillToShow){
		if(skillToShow.GetComponentInChildren<Text>().text == "") return;

		skillShowingDescriptionOf = skillToShow;
		skillDescription.rectTransform.anchoredPosition = new Vector3(0,0,0);

		string textToShow = "";

		foreach(SkillDescriptions.SkillsAndDescriptions sad in SkillDescriptions.S.skillDescriptions){
			if(sad.skillName == skillToShow.GetComponentInChildren<Text>().text){
				textToShow = sad.description;
				break;
			}
		}

		skillDescription.GetComponentInChildren<Text>().text = textToShow;

		skillToShow.color = new Color(0.6f, 0.6f, 1);
	}

	public void HideSkillText(Image skillToHide){
		if(skillToHide.GetComponentInChildren<Text>().text == "") return;

		if(skillShowingDescriptionOf != skillToHide) return;
		skillShowingDescriptionOf = null;
		skillDescription.rectTransform.anchoredPosition = new Vector3(-2000,-2000,0);
		skillToHide.color = Color.white;
	}

	public void selectPlayerForWound(GameObject currZone) {
		front1.image.color = Color.white;
		front2.image.color = Color.white;
		GameController.S.zombTurnText.text = "Wound a player";
		currZone.GetComponent<ZoneScript>().Highlight();
		foreach(Survivor surv in GameController.S.survivors){
			if(GameController.S.currSurvivor != null){
				if(GameController.S.currSurvivor == surv) continue;
			}

			if(surv.CurrZone == currZone){
				MoveSpecificTokenOn(surv);
			}
		}
		//find the players in the same zone and move their buttons up		
	}
	
	public void sacrifice(string playerName) {
		if (!GameController.S.zombiesAttacking && !AttackScript.S.attackingSurvivor) {
			return;
		}
		if (sacrificeThem) {
			Vector3 newScale = new Vector3 (4, 4, 0);
			switch(playerName) {
			case "Wanda":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Wanda" == GameController.S.survivors[i].name) {
						woundedSurvivor = GameController.S.survivors[i];
						wanda.image.transform.localScale = newScale;
					}
				}
				break;
			case "Phil":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Phil" == GameController.S.survivors[i].name) {
						woundedSurvivor = GameController.S.survivors[i];
						phil.image.transform.localScale = newScale;
					}
				}
				break;
			case "Ned":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Ned" == GameController.S.survivors[i].name) {
						woundedSurvivor = GameController.S.survivors[i];
						ned.image.transform.localScale = newScale;
					}
				}
				break;
			case "Josh":
				for (int i = 0; i < GameController.S.survivors.Count; i++) {
					if("Josh" == GameController.S.survivors[i].name) {
						woundedSurvivor = GameController.S.survivors[i];
						josh.image.transform.localScale = newScale;
					}
				}
				break;
			}
			chosenWoundedSurvivor = true;
			woundedSurvivor.CurrZone.GetComponent<ZoneScript>().Unhighlight();
			MoveTokensOffscreen();
			MoveSpecificTokenOn(woundedSurvivor);
			expandWound();
			if((woundedSurvivor.front1.cardName != "Empty" && woundedSurvivor.front1.cardName != "Wound") || (woundedSurvivor.front2.cardName != "Empty" && woundedSurvivor.front2.cardName != "Wound") || (woundedSurvivor.back1.cardName != "Empty" && woundedSurvivor.back1.cardName != "Wound") || (woundedSurvivor.back2.cardName != "Empty" && woundedSurvivor.back2.cardName != "Wound") || (woundedSurvivor.back3.cardName != "Empty" && woundedSurvivor.back3.cardName != "Wound")) {
				discardForWound = true;
				GameController.S.zombTurnText.text = "Discard one item";
				GameController.S.picked = GameController.S.deck.empty;
				GameController.S.pickedImage.image.sprite = GameController.S.picked.but.image.sprite;
				GameController.S.pickedImage.transform.position = new Vector3 (Screen.width/2, Screen.height/2, 0);
				discardComplete.transform.position = new Vector3 (Screen.width - 35, Screen.height / 2, 0);
			}
			else {
				woundSetup();
			}
		}
	}

	public void discard(string cardName) {
		if (!discardForWound && !woundTime)
			return;
		Card picked = new Card ();
		switch (cardName) {
		case "front1":
			picked = woundedSurvivor.front1;
			woundedSurvivor.front1 = GameController.S.picked;
			front1.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "front2":
			picked = woundedSurvivor.front2;
			woundedSurvivor.front2 = GameController.S.picked;
			front2.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "back1":
			picked = woundedSurvivor.back1;
			woundedSurvivor.back1 = GameController.S.picked;
			back1.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "back2":
			picked = woundedSurvivor.back2;
			woundedSurvivor.back2 = GameController.S.picked;
			back2.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		case "back3":
			picked = woundedSurvivor.back3;
			woundedSurvivor.back3 = GameController.S.picked;
			back3.image.sprite = GameController.S.picked.but.image.sprite;
			GameController.S.picked = picked;
			GameController.S.pickedImage.image.sprite = picked.but.image.sprite;
			break;
		}
	}

	public void finishDiscard() {
		if (GameController.S.picked.cardName == "Wound" || GameController.S.picked.cardName == "Empty") {
			GameController.S.zombTurnText.text = "Must select item";
			return;
		}
		discardForWound = false;
		if(GameController.S.picked.cardName != "Pan")
			GameController.S.deck.returnToDeck (GameController.S.picked.cardName);
		if (GameController.S.picked.cardName == "Mask") {
			GameController.S.zombTurnText.text = "Saved with Mask";
			discardComplete.transform.position = new Vector3 (-1000, 0, 0);
			finishWound ();
		} 
		else {
			GameController.S.pickedImage.transform.position = new Vector3 (-3000, 0, 0);
			discardComplete.transform.position = new Vector3 (-1000, 0, 0);
			woundSetup ();
		}
	}

	public void finishWound() {
		if (GameController.S.picked.cardName == "Wound") {
			GameController.S.zombTurnText.text = "Can't discard a Wound";
			return;
		}
		woundTime = false;
		if(GameController.S.picked.cardName != "Empty" && GameController.S.picked.cardName != "Pan")
			GameController.S.deck.returnToDeck (GameController.S.picked.cardName);
		GameController.S.pickedImage.transform.position = new Vector3 (-3000, 0, 0);
		woundComplete.transform.position = new Vector3 (-1000,0,0);
		MoveTokensOffscreen();
		Vector3 newScale = new Vector3 (1, 1, 0);
		foreach(Survivor surv in GameController.S.survivors){
			if(surv == GameController.S.currSurvivor) continue;
			surv.Unhighlight();
			switch(surv.name){
			case "Wanda":
				wanda.image.transform.localScale = newScale;
				break;
			case "Phil":
				phil.image.transform.localScale = newScale;
				break;
			case "Ned":
				ned.image.transform.localScale = newScale;
				break;
			case "Josh": 
				josh.image.transform.localScale = newScale;
				break;
			}
		}
		if(!GameController.S.picked.cardName == "Mask"){
			woundedSurvivor.TakeWound();
		}


		sacrificeThem = false;
		chosenWoundedSurvivor = false;
		removeCards ();
		if(GameController.S.currSurvivor != null){
			expandCards();
		}
	}

	void woundSetup() {
		woundTime = true;
		GameController.S.zombTurnText.text = "Add the wound";
		GameController.S.picked = GameController.S.deck.wounded;
		GameController.S.pickedImage.image.sprite = GameController.S.picked.but.image.sprite;
		GameController.S.pickedImage.transform.position = new Vector3 (Screen.width/2, Screen.height/2, 0);
		woundComplete.transform.position = new Vector3 (Screen.width - 35, Screen.height / 2, 0);
	}

	void expandWound() {
		HideSkills();
		front1.image.sprite = woundedSurvivor.front1.but.image.sprite;
		front2.image.sprite = woundedSurvivor.front2.but.image.sprite;
		back1.image.sprite = woundedSurvivor.back1.but.image.sprite;
		back2.image.sprite = woundedSurvivor.back2.but.image.sprite;
		back3.image.sprite = woundedSurvivor.back3.but.image.sprite;
		
		float frontPos = front1.GetComponent<RectTransform>().sizeDelta.x * 3.0f/2.0f;
		float backPos = front1.GetComponent<RectTransform>().sizeDelta.x * 1.0f/2.0f;

		front1.transform.position = new Vector3 (frontPos, front1.transform.position.y);
		front2.transform.position = new Vector3 (frontPos, front2.transform.position.y);
		back1.transform.position = new Vector3 (backPos, back1.transform.position.y);
		back2.transform.position = new Vector3 (backPos, back2.transform.position.y);
		back3.transform.position = new Vector3 (backPos, back3.transform.position.y);
	}
	
	public void takeWound() {
		
	}
	
	public void MoveTokensOffscreen(){
		foreach(Survivor surv in GameController.S.survivors){
			switch(surv.name){
			case "Wanda":
				wanda.GetComponent<RectTransform>().anchoredPosition = new Vector2(wanda.GetComponent<RectTransform>().anchoredPosition.x, -100);
				break;
			case "Phil":
				phil.GetComponent<RectTransform>().anchoredPosition = new Vector2(phil.GetComponent<RectTransform>().anchoredPosition.x, -100);
				break;
			case "Ned":
				ned.GetComponent<RectTransform>().anchoredPosition = new Vector2(ned.GetComponent<RectTransform>().anchoredPosition.x, -100);
				break;
			case "Josh": 
				josh.GetComponent<RectTransform>().anchoredPosition = new Vector2(josh.GetComponent<RectTransform>().anchoredPosition.x, -100);
				break;
			}
		}
	}

	public void MoveTokensOnScreen(){
		
		foreach(Survivor surv in GameController.S.survivors){
			switch(surv.name){
			case "Wanda":
				wanda.GetComponent<RectTransform>().anchoredPosition = new Vector2(wanda.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
				break;
			case "Phil":
				phil.GetComponent<RectTransform>().anchoredPosition = new Vector2(phil.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
				break;
			case "Ned":
				ned.GetComponent<RectTransform>().anchoredPosition = new Vector2(ned.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
				break;
			case "Josh": 
				josh.GetComponent<RectTransform>().anchoredPosition = new Vector2(josh.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
				break;
			}
		}
	}

	public void MoveSpecificTokenOn(Survivor surv){
		switch(surv.name){
		case "Wanda":
			wanda.GetComponent<RectTransform>().anchoredPosition = new Vector2(wanda.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
			break;
		case "Phil":
			phil.GetComponent<RectTransform>().anchoredPosition = new Vector2(phil.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
			break;
		case "Ned":
			ned.GetComponent<RectTransform>().anchoredPosition = new Vector2(ned.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
			break;
		case "Josh": 
			josh.GetComponent<RectTransform>().anchoredPosition = new Vector2(josh.GetComponent<RectTransform>().anchoredPosition.x, 45.4f);
			break;
		}
	}
}

