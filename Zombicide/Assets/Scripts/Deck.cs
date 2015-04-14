using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	public Card empty = new Card ();
	public Card wounded = new Card ();
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
	public Button sawedoffButton;
	public Button rifleButton;
	public Button emptyButton;
	public Button masButton;
	public Button aaahButton;
	public Button ammoPButton;
	public Button ammoSButton;
	public Button axeButton;
	public Button batButton;
	public Button bottleButton;
	public Button foodButton;
	public Button chainButton;
	public Button crowbarButton;
	public Button twinButton;
	public Button flashButton;
	public Button gasButton;
	public Button katanaButton;
	public Button macheteButton;
	public Button maskButton;
	public Button molotovButton;
	public Button panButton;
	public Button pistolButton;
	public Button riceButton;
	public Button shotgunButton;
	public Button scopeButton;
	public Button smgButton;
	public Button waterButton;
	public Button woundedButton;
	bool twinTaken = false;
	bool maShotTaken = false;

	public List<string> equipment = new List<string>();
	public List<string> initial = new List<string>();

	// Use this for initialization
	void Start () {
		empty.but = emptyButton;
		empty.cardName = "Empty";
		
		pan.cardName = "Pan";
		pan.combinable = false;
		pan.dualWield = false;
		pan.closeRange = 0;
		pan.farRange = 0;
		pan.ranged = false;
		pan.dice = 1;
		pan.minDiceNumber = 6;
		pan.damage = 1;
		pan.openDoor = false;
		pan.noise = false;
		pan.melee = true;
		pan.but = panButton;
		initial.Add(pan.cardName);
		for (int i = 0; i < 2; i++) {
			equipment.Add(pan.cardName);
		}

		molotov.cardName = "Molotov";
		molotov.ranged = true;
		molotov.farRange = 1;
		molotov.closeRange = 0;
		molotov.dualWield = false;
		molotov.melee = false;
		molotov.dice = 0;
		molotov.minDiceNumber = 0;
		molotov.damage = 10;
		molotov.but = molotovButton;
		molotov.openDoor = false;
		molotov.noise = true;
		molotov.doorNoise = false;

		crowbar.cardName = "Crowbar";
		crowbar.openDoor = true;
		crowbar.noise = false;
		crowbar.doorNoise = false;
		crowbar.combinable = false;
		crowbar.melee = true;
		crowbar.dice = 1;
		crowbar.but = crowbarButton;
		crowbar.minDiceNumber = 4;
		crowbar.damage = 1;
		crowbar.closeRange = 0;
		crowbar.farRange = 0;
		crowbar.ranged = false;
		initial.Add(crowbar.cardName);
		for (int i = 0; i < 1; i++) {
			equipment.Add(crowbar.cardName);
		}

		bat.cardName = "Baseball Bat";
		bat.combinable = false;
		bat.noise = false;
		bat.closeRange = 0;
		bat.farRange = 0;
		bat.minDiceNumber = 3;
		bat.damage = 1;
		bat.dice = 1;
		bat.dualWield = false;
		bat.openDoor = false;
		bat.melee = true;
		bat.ranged = false;
		bat.but = batButton;
		for (int i = 0; i < 2; i++) {
			equipment.Add(bat.cardName);
		}

		axe.cardName = "Fire Axe";
		axe.but = axeButton;
		axe.openDoor = true;
		axe.noise = false;
		axe.doorNoise = true;
		axe.combinable = false;
		axe.melee = true;
		axe.dice = 1;
		axe.minDiceNumber = 4;
		axe.damage = 2;
		axe.farRange = 0;
		axe.closeRange = 0;
		axe.ranged = false;
		initial.Add(axe.cardName);
		for (int i = 0; i < 1; i++) {
			equipment.Add(axe.cardName);
		}

		machete.cardName = "Machete";
		machete.but = macheteButton;
		machete.dualWield = true;
		machete.openDoor = false;
		machete.dice = 1;
		machete.minDiceNumber = 4;
		machete.damage = 2;
		machete.farRange = 0;
		machete.closeRange = 0;
		machete.noise = false;
		machete.melee = true;
		machete.ranged = false;
		for (int i = 0; i < 4; i++) {
			equipment.Add(machete.cardName);
		}

		chainsaw.cardName = "Chainsaw";
		chainsaw.but = chainButton;
		chainsaw.melee = true;
		chainsaw.ranged = false;
		chainsaw.dualWield = false;
		chainsaw.openDoor = true;
		chainsaw.noise = true;
		chainsaw.doorNoise = true;
		chainsaw.combinable = false;
		chainsaw.farRange = 0;
		chainsaw.closeRange = 0;
		chainsaw.dice = 5;
		chainsaw.damage = 2;
		chainsaw.minDiceNumber = 5;
		for (int i = 0; i < 2; i++) {
			equipment.Add(chainsaw.cardName);
		}

		katana.cardName = "Katana";
		katana.but = katanaButton;
		katana.melee = true;
		katana.ranged = false;
		katana.dualWield = false;
		katana.openDoor = false;
		katana.combinable = false;
		katana.noise = false;
		katana.farRange = 0;
		katana.closeRange = 0;
		katana.dice = 2;
		katana.minDiceNumber = 4;
		katana.damage = 1;
		for (int i = 0; i < 2; i++) {
			equipment.Add(katana.cardName);
		}

		sawedOff.cardName = "Sawed Off Shotgun";
		sawedOff.but = sawedoffButton;
		sawedOff.ranged = true;
		sawedOff.melee = false;
		sawedOff.noise = true;
		sawedOff.openDoor = false;
		sawedOff.dualWield = true;
		sawedOff.combinable = false;
		sawedOff.farRange = 1;
		sawedOff.closeRange = 0;
		sawedOff.dice = 2;
		sawedOff.minDiceNumber = 3;
		sawedOff.damage = 1;
		for (int i = 0; i < 4; i++) {
			equipment.Add(sawedOff.cardName);
		}

		subMG.cardName = "Submachine Gun";
		subMG.but = smgButton;
		subMG.ranged = true;
		subMG.melee = false;
		subMG.dualWield = true;
		subMG.combinable = false;
		subMG.noise = true;
		subMG.openDoor = false;
		subMG.farRange = 1;
		subMG.closeRange = 0;
		subMG.dice = 3;
		subMG.minDiceNumber = 5;
		subMG.damage = 1;
		for (int i = 0; i < 2; i++) {
			equipment.Add(subMG.cardName);
		}

		pistol.cardName = "Pistol";
		pistol.but = pistolButton;
		pistol.ranged = true;
		pistol.melee = false;
		pistol.openDoor = false;
		pistol.noise = false;
		pistol.dualWield = true;
		pistol.combinable = false;
		pistol.farRange = 1;
		pistol.closeRange = 0;
		pistol.dice = 1;
		pistol.minDiceNumber = 4;
		pistol.damage = 1;
		initial.Add(pistol.cardName);
		for (int i = 0; i < 2; i++) {
			equipment.Add(pistol.cardName);
		}
	
		shotgun.cardName = "Shotgun";
		shotgun.but = shotgunButton;
		shotgun.ranged = true;
		shotgun.melee = false;
		shotgun.noise = true;
		shotgun.dualWield = false;
		shotgun.combinable = false;
		shotgun.openDoor = false;
		shotgun.farRange = 1;
		shotgun.closeRange = 0;
		shotgun.dice = 2;
		shotgun.minDiceNumber = 4;
		shotgun.damage = 2;
		for (int i = 0; i < 2; i++) {
			equipment.Add(shotgun.cardName);
		}

		rifle.cardName = "Rifle";
		rifle.but = rifleButton;
		rifle.ranged = true;
		rifle.melee = false;
		rifle.noise = true;
		rifle.openDoor = false;
		rifle.combinable = true;
		rifle.dualWield = false;
		rifle.combineWith = "Scope";
		rifle.farRange = 3;
		rifle.closeRange = 0;
		rifle.dice = 1;
		rifle.minDiceNumber = 3;
		rifle.damage = 1;
		for (int i = 0; i < 2; i++) {
			equipment.Add(rifle.cardName);
		}

		maShotgun.cardName = "Ma's Shotgun";
		maShotgun.but = masButton;
		maShotgun.melee = true;
		maShotgun.ranged = true;
		maShotgun.dualWield = false;
		maShotgun.combinable = false;
		maShotgun.noise = true;
		maShotgun.openDoor = false;
		maShotgun.dualWield = false;
		maShotgun.farRange = 1;
		maShotgun.closeRange = 0;
		maShotgun.dice = 2;
		maShotgun.minDiceNumber = 3;
		maShotgun.damage = 2;
		//for (int i = 0; i < 1; i++) {
		//	equipment.Add(maShotgun.cardName);
		//}

		evilTwin.cardName = "Evil Twins";
		evilTwin.but = twinButton;
		evilTwin.ranged = true;
		evilTwin.melee = false;
		evilTwin.noise = true;
		evilTwin.dualWield = false;
		evilTwin.combinable = false;
		evilTwin.openDoor = false;
		evilTwin.dice = 2;
		evilTwin.farRange = 1;
		evilTwin.closeRange = 0;
		evilTwin.minDiceNumber = 4;
		evilTwin.damage = 1;
		//for (int i = 0; i < 1; i++) {
		//	equipment.Add(evilTwin.cardName);
		//}

		cannedFood.cardName = "Canned Food";
		cannedFood.but = foodButton;
		cannedFood.melee = false;
		cannedFood.ranged = false;
		cannedFood.combinable = false;
		cannedFood.noise = false;
		cannedFood.openDoor = false;
		cannedFood.dualWield = false;
		for (int i = 0; i < 3; i++) {
			equipment.Add(cannedFood.cardName);
		}

		flashlight.cardName = "Flashlight";
		flashlight.but = flashButton;
		flashlight.ranged = false;
		flashlight.melee = false;
		flashlight.combinable = false;
		flashlight.openDoor = false;
		flashlight.noise = false;
		flashlight.dualWield = false;
		for (int i = 0; i < 2; i++) {
			equipment.Add(flashlight.cardName);
		}

		gasoline.cardName = "Gasoline";
		gasoline.combinable = true;
		gasoline.but = gasButton;
		gasoline.combineWith = "Glass Bottle";
		gasoline.ranged = false;
		gasoline.melee = false;
		gasoline.dualWield = false;
		gasoline.noise = false;
		for (int i = 0; i < 2; i++) {
			equipment.Add(gasoline.cardName);
		}

		bottle.cardName = "Glass Bottle";
		bottle.but = bottleButton;
		bottle.combinable = true;
		bottle.combineWith = "Gasoline";
		bottle.ranged = false;
		bottle.melee = false;
		bottle.noise = false;
		bottle.dualWield = false;
		bottle.openDoor = false;
		for (int i = 0; i < 2; i++) {
			equipment.Add(bottle.cardName);
		}

		mask.cardName = "Goalie Mask";
		mask.but = maskButton;
		mask.combinable = false;
		mask.ranged = false;
		mask.melee = false;
		mask.noise = false;
		mask.dualWield = false;
		mask.openDoor = false;
		for (int i = 0; i < 1; i++) {
			equipment.Add(mask.cardName);
		}


		rice.cardName = "Bag of Rice";
		rice.but = riceButton;
		rice.ranged = false;
		rice.melee = false;
		rice.openDoor = false;
		rice.noise = false;
		rice.combinable = false;
		rice.dualWield = false;
		for (int i = 0; i < 3; i++) {
			equipment.Add(rice.cardName);
		}

		scope.cardName = "Scope";
		scope.but = scopeButton;
		scope.combinable = true;
		scope.combineWith = "Rifle";
		scope.ranged = false;
		scope.melee = false;
		scope.noise = false;
		scope.dualWield = false;
		for (int i = 0; i < 2; i++) {
			equipment.Add(scope.cardName);
		}

		water.cardName = "Water";
		water.but = waterButton;
		water.ranged = false;
		water.melee = false;
		water.combinable = false;
		water.dualWield = false;
		water.noise = false;
		water.openDoor = false;
		for (int i = 0; i < 3; i++) {
			equipment.Add(water.cardName);
		}

		zombie.cardName = "Aaahh!";
		zombie.but = aaahButton;
		zombie.combinable = false;
		zombie.ranged = false;
		zombie.melee = false;
		zombie.combinable = false;
		zombie.dualWield = false;
		zombie.openDoor = false;
		zombie.noise = false;
		for (int i = 0; i < 4; i++) {
			equipment.Add(zombie.cardName);
		}

		ammoH.cardName = "Plenty of Ammo (Heavy)";
		ammoH.but = ammoSButton;
		ammoH.combinable = false;
		ammoH.noise = false;
		ammoH.ranged = false;
		ammoH.melee = false;
		ammoH.noise = false;
		ammoH.dualWield = false;
		for (int i = 0; i < 3; i++) {
			equipment.Add(ammoH.cardName);
		}

		ammoL.cardName = "Plenty of Ammo (Light)";
		ammoL.but = ammoPButton;
		ammoL.ranged = false;
		ammoL.melee = false;
		ammoL.combinable = false;
		ammoL.dualWield = false;
		ammoL.noise = false;
		ammoL.openDoor = false;
		for (int i = 0; i < 3; i++) {
			equipment.Add(ammoL.cardName);
		}

		initialize ();
	}

	//set up the survivors
	public void initialize() {
		int what = -1;
		Card drawn = empty;
		string cardName;
		for(int i = 0; i < GameController.S.survivors.Count; i++) {
			what = Random.Range(0, initial.Count);
			if(GameController.S.survivors[i].name == "Phil") {
				GameController.S.survivors[i].front2 = pistol;
			}
			cardName = initial[what];
			switch(cardName){
			case "Fire Axe":
				drawn = axe;
				break;
			case "Pan":
				drawn = pan;
				break;
			case "Crowbar":
				drawn = crowbar;
				break;
			case "Pistol":
				drawn = pistol;
				break;
			}
			GameController.S.survivors[i].front1 = drawn;
			initial.Remove(cardName);
		}
	}

	public void returnToDeck(string cardName) {
		if (cardName != "Empty") {
			print ("returned " + cardName);
			equipment.Add (cardName);
		}
	}

	public Card searchCar() {
		int num = -1;
		if (maShotTaken == true) {
			return evilTwin;
		}
		else if (twinTaken == true) {
			return maShotgun;
		}
		//not sure what we want to do here
		else if (twinTaken && maShotTaken) {
			return empty;
		}
		num = Random.Range (0, 1);
		switch (num) {
		case 0:
			maShotTaken = true;
			return maShotgun;
			break;
		case 1:
			twinTaken = true;
			return evilTwin;
			break;
		}
		return empty;
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
	public bool dualWield;
	public bool combinable;
	public string combineWith;
	public int closeRange;
	public int farRange;
	public int dice;
	public int minDiceNumber;
	public int damage;
	public Button but;
	public bool noise;
	public bool openDoor;
	public bool doorNoise;
	public bool ranged;
	public bool melee;
}
