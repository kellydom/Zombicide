using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	public Card empty = new Card ();
	public Card pan = new Card();
	public Card molotov = new Card();
	public Card crowbar = new Card();
	public Card bat = new Card();
	public Card axe = new Card();
	public Card machete = new Card();
	public Card chainsaw = new Card();
	public Card katana = new Card();
	public Card sawedOff = new Card();
	public Card subMG = new Card();
	public Card pistol = new Card();
	public Card shotgun = new Card();
	public Card rifle = new Card();
	public Card maShotgun = new Card();
	public Card evilTwin = new Card();
	public Card cannedFood = new Card();
	public Card flashlight = new Card();
	public Card gasoline = new Card();
	public Card bottle = new Card();
	public Card mask = new Card();
	public Card rice = new Card();
	public Card scope = new Card();
	public Card water = new Card();
	public Card zombie = new Card();
	public Card ammoH = new Card();
	public Card ammoL = new Card();
	public Button shotgunButton;
	public Button rifleButton;
	public Button emptyButton;

	public List<string> equipment = new List<string>();

	// Use this for initialization
	void Start () {
		empty.but = emptyButton;
		empty.cardName = "Empty";
		pan.cardName = "Pan";
		pan.weapon = true;
		pan.combinable = false;
		pan.dualWield = false;
		pan.range = 0;
		pan.dice = 1;
		pan.openDoor = false;
		pan.noise = false;
		pan.but = shotgunButton;
		for (int i = 0; i < 3; i++) {
			equipment.Add(pan.cardName);
		}

		molotov.cardName = "Molotov";
		molotov.weapon = true;
		molotov.dualWield = false;

		crowbar.cardName = "Crowbar";
		crowbar.weapon = true;
		crowbar.openDoor = true;
		crowbar.noise = false;
		crowbar.doorNoise = false;
		crowbar.combinable = false;
		crowbar.dice = 1;
		for (int i = 0; i < 2; i++) {
			equipment.Add(crowbar.cardName);
		}

		bat.cardName = "Baseball Bat";
		bat.weapon = true;
		bat.combinable = false;
		bat.noise = false;
		bat.range = 0;
		bat.dice = 1;
		bat.dualWield = false;
		bat.openDoor = false;
		for (int i = 0; i < 2; i++) {
			equipment.Add(bat.cardName);
		}

		axe.cardName = "Fire Axe";
		axe.weapon = true;
		axe.openDoor = true;
		axe.noise = false;
		axe.doorNoise = true;
		axe.combinable = false;
		axe.dice = 1;
		axe.range = 0;
		for (int i = 0; i < 2; i++) {
			equipment.Add(axe.cardName);
		}

		machete.cardName = "Machete";
		machete.weapon = true;
		machete.dualWield = true;
		machete.openDoor = false;
		machete.dice = 1;
		machete.range = 0;
		machete.noise = false;
		for (int i = 0; i < 4; i++) {
			equipment.Add(machete.cardName);
		}

		chainsaw.cardName = "Chainsaw";
		for (int i = 0; i < 2; i++) {
			equipment.Add(chainsaw.cardName);
		}
		katana.cardName = "Katana";
		for (int i = 0; i < 2; i++) {
			equipment.Add(katana.cardName);
		}

		sawedOff.cardName = "Sawed Off Shotgun";
		for (int i = 0; i < 4; i++) {
			equipment.Add(sawedOff.cardName);
		}

		subMG.cardName = "Submachine Gun";
		for (int i = 0; i < 2; i++) {
			equipment.Add(subMG.cardName);
		}

		pistol.cardName = "Pistol";
		for (int i = 0; i < 3; i++) {
			equipment.Add(pistol.cardName);
		}
	
		shotgun.cardName = "Shotgun";
		for (int i = 0; i < 2; i++) {
			equipment.Add(shotgun.cardName);
		}

		rifle.cardName = "Rifle";
		for (int i = 0; i < 2; i++) {
			equipment.Add(rifle.cardName);
		}

		maShotgun.cardName = "Ma's Shotgun";
		for (int i = 0; i < 1; i++) {
			equipment.Add(maShotgun.cardName);
		}

		evilTwin.cardName = "Evil Twins";
		for (int i = 0; i < 1; i++) {
			equipment.Add(evilTwin.cardName);
		}

		cannedFood.cardName = "Canned Food";
		for (int i = 0; i < 3; i++) {
			equipment.Add(cannedFood.cardName);
		}

		flashlight.cardName = "Flashlight";
		for (int i = 0; i < 2; i++) {
			equipment.Add(flashlight.cardName);
		}

		gasoline.cardName = "Gasoline";
		for (int i = 0; i < 2; i++) {
			equipment.Add(gasoline.cardName);
		}

		bottle.cardName = "Glass Bottle";
		for (int i = 0; i < 2; i++) {
			equipment.Add(bottle.cardName);
		}

		for (int i = 0; i < 1; i++) {
			equipment.Add(mask.cardName);
		}

		rice.cardName = "Bag of Rice";
		for (int i = 0; i < 3; i++) {
			equipment.Add(rice.cardName);
		}

		scope.cardName = "Scope";
		for (int i = 0; i < 2; i++) {
			equipment.Add(scope.cardName);
		}

		water.cardName = "Water";
		for (int i = 0; i < 3; i++) {
			equipment.Add(water.cardName);
		}

		zombie.cardName = "Aaahh!";
		for (int i = 0; i < 4; i++) {
			equipment.Add(zombie.cardName);
		}

		ammoH.cardName = "Plenty of Ammo (Heavy)";
		for (int i = 0; i < 3; i++) {
			equipment.Add(ammoH.cardName);
		}

		ammoL.cardName = "Plenty of Ammo (Light)";
		for (int i = 0; i < 3; i++) {
			equipment.Add(ammoL.cardName);
		}
	}

	public Card draw() {
		Card picked = new Card(); 
		if (equipment.Count == 0) {
			print ("We've run out of cards :(");
			picked.cardName = "Null";
			return picked;
		}
		int at = Random.Range (0, equipment.Count);
		string cardName	= equipment [at];
		equipment.RemoveAt (at);
		switch (cardName) {
		case "Pan":
			picked = pan;
			break;
		case "Crowbar":
			picked = crowbar;
			break;
		case "Baseball Bat":
			picked = bat;
			break;
		case "Fire Axe":
			picked = axe;
			break;
		case "Machete":
			picked = machete;
			break;
		case "Chainsaw":
			picked = chainsaw;
			break;
		case "Katana":
			picked = katana;
			break;
		case "Sawed Off Shotgun":
			picked = sawedOff;
			break;
		case "Submachine Gun":
			picked = subMG;
			break;
		case "Pistol":
			picked = pistol;
			break;
		case "Shotgun":
			picked = shotgun;
			break;
		case "Rifle":
			picked = rifle;
			break;
		case "Ma's Shotgun":
			picked = maShotgun;
			break;
		case "Evil Twins":
			picked = evilTwin;
			break;
		case "Canned Food":
			picked = cannedFood;
			break;
		case "Flashlight":
			picked = flashlight;
			break;
		case "Gasoline":
			picked = gasoline;
			break;
		case "Bottle":
			picked = bottle;
			break;
		case "Mask":
			picked = mask;
			break;
		case "Rice":
			picked = rice;
			break;
		case "Scope":
			picked = scope;
			break;
		case "Water":
			picked = water;
			break;
		case "Aaahh!":
			picked = zombie;
			break;
		case "Plenty of Ammo (Heavy)":
			picked = ammoH;
			break;
		case "Plenty of Ammo (Light)":
			picked = ammoL;
			break;
		}
		//print (picked.cardName);
		return picked;
	}

}

public class Card {
	public string cardName;
	public bool weapon;
	public bool dualWield;
	public bool combinable;
	public string combineWith;
	public int range;
	public int dice;
	public Button but;
	public bool noise;
	public bool openDoor;
	public bool doorNoise;
}
