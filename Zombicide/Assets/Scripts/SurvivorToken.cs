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
	int setCards = -1000;
	int frontCards = 148;
	int backCards = 48;
	int tempFrontCards = 1200;
	int tempBackCards = 1300;
	bool tempOut = false;
	bool cardsOut = false;

	string whichCard;
	bool cardSwitch = false;

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
		else if(currSurvivor.name != name){
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
		Vector3 newScale = new Vector3 (4, 4, 0);
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
				removeCards();
				expandCards();
				phil.image.transform.localScale = new Vector3(1, 1, 0);
				philClicked = false;
			}
			wandaClicked = true;
			for (int i = 0; i < GameController.S.survivors.Count; i++) {
				if("Wanda" == GameController.S.survivors[i].name) {
					GameController.S.currSurvivor = GameController.S.survivors[i];
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
				wanda.image.transform.localScale = new Vector3(1, 1, 0);
				wandaClicked = false;
				removeCards();
				expandCards();
			}
			philClicked = true;
			for (int i = 0; i < GameController.S.survivors.Count; i++) {
				if("Phil" == GameController.S.survivors[i].name) {
					GameController.S.currSurvivor = GameController.S.survivors[i];
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
		GameController.S.playerSearching = false;
	}

	void trade(Card surv1, Card surv2) {

	}
}
