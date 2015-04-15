using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SurvivorToken : MonoBehaviour {
	public static SurvivorToken S;

	public Button phil;
	public Button wanda;
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
	
	public List<Sprite>	levelSprites;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void tokenOnHover(string name) {
		if (ActionWheel.S.CurrAction == "Trade") 
			return;
		Vector3 newScale = new Vector3 (4, 4, 0);
		switch (name) {
		case "Wanda":
			wanda.image.transform.localScale = newScale;
			break;
		case "Phil":
			phil.image.transform.localScale = newScale;
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
		} 
		else if(currSurvivor.name != name && !inTrade){
			expandTempCards();
			tempOut = true;
		}
	}

	public void tokenOnLeave(string name) {
		Vector3 newScale = new Vector3 (1, 1, 0);
		if (!clicked) {
			removeCards ();
		} 
		if (tempOut) {
			removeTempCards ();
			tempOut = false;
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
		tempf1.transform.position = new Vector3 (tempFrontCards, front1.transform.position.y);
		tempf2.transform.position = new Vector3 (tempFrontCards, front2.transform.position.y);
		tempb1.transform.position = new Vector3 (tempBackCards, back1.transform.position.y);
		tempb2.transform.position = new Vector3 (tempBackCards, back2.transform.position.y);
		tempb3.transform.position = new Vector3 (tempBackCards, back3.transform.position.y);
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
		front1.transform.position = new Vector3 (frontCards, front1.transform.position.y);
		front2.transform.position = new Vector3 (frontCards, front2.transform.position.y);
		back1.transform.position = new Vector3 (backCards, back1.transform.position.y);
		back2.transform.position = new Vector3 (backCards, back2.transform.position.y);
		back3.transform.position = new Vector3 (backCards, back3.transform.position.y);
	}

	void removeCards() {
		cardsOut = false;
		front1.transform.position = new Vector3 (setCards, front1.transform.position.y);
		front2.transform.position = new Vector3 (setCards, front2.transform.position.y);
		back1.transform.position = new Vector3 (setCards, back1.transform.position.y);
		back2.transform.position = new Vector3 (setCards, back2.transform.position.y);
		back3.transform.position = new Vector3 (setCards, back3.transform.position.y);
	}

	public void tokenOnClicked(string name) {
		print ("Clicked!: " + wandaClicked + " " + philClicked);
		bool reload = false;
		Vector3 newScale = new Vector3 (4, 4, 0);
		if (ActionWheel.S.CurrAction == "Trade")
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
			if(philClicked) {
				if (GameController.S.currSurvivor != null)
					if (GameController.S.currSurvivor.numActions < 3)
						return;
				removeCards();
				//expandCards();
				reload = true;
				GameController.S.currSurvivor.Unhighlight();
				phil.image.transform.localScale = new Vector3(1, 1, 0);
				philClicked = false;
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
			if(wandaClicked) {
				if (GameController.S.currSurvivor != null)
					if (GameController.S.currSurvivor.numActions < 3)
						return;
				wanda.image.transform.localScale = new Vector3(1, 1, 0);
				wandaClicked = false;
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
		GameController.S.deck.returnToDeck (GameController.S.picked.cardName);
		GameController.S.pickedImage.transform.position = new Vector3 (-3000, 0, 0);
		GameController.S.deleteForSearch.transform.position = new Vector3 (-1000,0,0);
		GameController.S.playerSearching = false;
		ActionWheel.S.MoveWheelDown ();
	}

	void expandTrade() {
		tempf1.image.sprite = tradingSurvivor.front1.but.image.sprite;
		tempf2.image.sprite = tradingSurvivor.front2.but.image.sprite;
		tempb1.image.sprite = tradingSurvivor.back1.but.image.sprite;
		tempb2.image.sprite = tradingSurvivor.back2.but.image.sprite;
		tempb3.image.sprite = tradingSurvivor.back3.but.image.sprite;
		tempf1.transform.position = new Vector3 (tempFrontCards, front1.transform.position.y);
		tempf2.transform.position = new Vector3 (tempFrontCards, front2.transform.position.y);
		tempb1.transform.position = new Vector3 (tempBackCards, back1.transform.position.y);
		tempb2.transform.position = new Vector3 (tempBackCards, back2.transform.position.y);
		tempb3.transform.position = new Vector3 (tempBackCards, back3.transform.position.y);
	}

	public void finishTrade() {
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
			}
			expandTrade();
		}
	}
}
