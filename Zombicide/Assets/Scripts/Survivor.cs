using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Survivor : MonoBehaviour {

	public int survNum;

	GameObject currZone;
	public GameObject CurrZone{
		get{return currZone;}
	}
	Color baseColor;

	bool hasGone;
	public bool HasGone{
		get{return hasGone;}
		set{hasGone = value;}
	}
	public string name;
	public int numActions;
	public int maxActions = 3;
	public bool slippery = false;
	public bool extraAction = false;
	public bool freeSearch = false;
	public bool doubleMove = false;
	public Card front1;
	public Card front2;
	public Card back1;
	public Card back2;
	public Card back3;
	public int numWounds = 0;

	public bool currTurn = false;

	public bool hasSearched = false;

	int currExp = 0;
	int expToYellow = 7;
	public List<string> skillsAtYellowLevel;
	int expToOrange = 19;
	public List<string> skillsAtOrangeLevel;
	int expToRed = 43;
	public List<string> skillsAtRedLevel;

	public List<string> skills;
	public int currLevel = 0;
	public Image levelImage;
	public bool doingSkillStuff = false;

	public bool hasDoneMove = false;
	public bool hasDoneCombatAction = false;
	bool waitingToSelectSkill = false;


	// Use this for initialization
	void Start () {
		numActions = 3;
		baseColor = gameObject.GetComponent<Renderer>().material.color;
	}

	public void SetZone(GameObject newZone){
		currZone = newZone;
		Vector3 basePos = currZone.transform.position;
		ZoneScript zone = currZone.GetComponent<ZoneScript>();
		int index = zone.zoneNum;
		Vector3 size = BoardLayout.S.zoneSizes[index];

		Vector3 bottomLeft = newZone.GetComponent<BoxCollider>().bounds.min;
		Vector3 bottomRight = bottomLeft;
		bottomRight.x = newZone.GetComponent<BoxCollider>().bounds.max.x;
		bottomLeft.z += .03f;
		bottomRight.z += .03f;

		if(survNum >= 3) {
			bottomLeft.z = (bottomLeft.z + newZone.GetComponent<BoxCollider>().bounds.max.z) / 2;
			bottomRight.z = bottomLeft.z;
		}

		int xValue = survNum;
		if(xValue >= 3) xValue -= 3;

		transform.position = Vector3.Lerp (bottomLeft, bottomRight, (xValue + 1) / 4.0f ) + Vector3.up / 5;
	}

	public void DoNothing(){
		numActions = 0;
	}

	public void MoveTo(GameObject newZone, int actionCost){
		if(skills.Contains("+1 free Move Action")){
			if(hasDoneMove){
				numActions -= actionCost;
			}
			else{
				hasDoneMove = true;
			}
		}
		else{
			numActions -= actionCost;
		}
		SetZone(newZone);
		GameController.S.MoveSetup();
	}
	public bool CanMove(){
		ZoneScript zs = currZone.GetComponent<ZoneScript>();
		
		if(numActions > 1) return true;
		if(numActions == 0) return false;

		if(skills.Contains("Slippery")) return true;
		
		if(zs.walkersInZone.Count > 0) return false;
		if(zs.runnersInZone.Count > 0) return false;
		if(zs.fattiesInZone.Count > 0) return false;
		if(zs.abombInZone.Count > 0) return false;
		
		return true;
	}

	public bool CanDoMelee(){
		if(front1 != null){
			if(front1.melee){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				
				if(zs.walkersInZone.Count > 0) return true;
				if(zs.runnersInZone.Count > 0) return true;
				if(zs.fattiesInZone.Count > 0) return true;
				if(zs.abombInZone.Count > 0) return true;

			}
		}
		if(front2 != null){
			if(front2.melee){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				
				if(zs.walkersInZone.Count > 0) return true;
				if(zs.runnersInZone.Count > 0) return true;
				if(zs.fattiesInZone.Count > 0) return true;
				if(zs.abombInZone.Count > 0) return true;
				
			}
		}

		return false;
	}
	public bool CanDoRanged(){
		if(front1 != null){
			if(front1.ranged){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				List<GameObject> frontRange1 = new List<GameObject>();
				frontRange1 = ZoneSelector.S.GetZonesInRange(currZone, front1.closeRange, front1.farRange);
				
				foreach(GameObject zone in frontRange1){
					
					if(zone.GetComponent<ZoneScript>().walkersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().runnersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().fattiesInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().abombInZone.Count > 0) return true;
				}
			}
		}
		if(front2 != null){
			if(front2.ranged){
				ZoneScript zs = currZone.GetComponent<ZoneScript>();
				List<GameObject> frontRange2 = new List<GameObject>();
				frontRange2 = ZoneSelector.S.GetZonesInRange(currZone, front2.closeRange, front2.farRange);
				
				foreach(GameObject zone in frontRange2){
					
					if(zone.GetComponent<ZoneScript>().walkersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().runnersInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().fattiesInZone.Count > 0) return true;
					if(zone.GetComponent<ZoneScript>().abombInZone.Count > 0) return true;
				}
			}
		}
		
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.playerTurn) {Unhighlight(); return;}

		/*if(GameController.S.currSurvivor != null) {
			if(GameController.S.currSurvivor != this) Unhighlight();

			return;
		}
		if(GameController.S.closestSurvivor == this) Highlight();
		else Unhighlight();*/
	}

	void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			//GameController.S.SelectSurvivor(this);
		}
	}

	public void Highlight(){
		gameObject.GetComponent<Renderer>().material.color = new Color(1,1,0,0.2f);

	}
	public void Unhighlight(){
		gameObject.GetComponent<Renderer>().material.color = baseColor;

	}

	void OnMouseEnter(){
		if(hasGone) return;
		if(currTurn) return;
		//Highlight();
	}
	
	void OnMouseExit(){
		if(currTurn) return;
		//Unhighlight();
	}

	IEnumerator ShowNewSkill(string newSkill){
		doingSkillStuff = true;
		skills.Add(newSkill);
		SurvivorToken.S.newSkillImage.rectTransform.anchoredPosition = new Vector3(0,0,0);
		SurvivorToken.S.newSkillImage.transform.FindChild("Skill1").GetComponentInChildren<Text>().text = newSkill;
		ActionWheel.S.MoveWheelUp();
		SurvivorToken.S.ShowSkills(GameController.S.currSurvivor);


		while(true){
			if(Input.GetMouseButton(0)) break;
			yield return 0;
		}

		
		ActionWheel.S.MoveWheelDown();
		SurvivorToken.S.newSkillImage.rectTransform.anchoredPosition = new Vector3(0,-1000,0);
		doingSkillStuff = false;
	}

	IEnumerator ChooseNewSkill(){
		doingSkillStuff = true;
		SurvivorToken.S.chooseSkillImage.rectTransform.anchoredPosition = new Vector3(0,0,0);
		if(currLevel == 2){
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill1").GetComponentInChildren<Text>().text = skillsAtOrangeLevel[0];
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill1").GetComponent<RectTransform>().anchoredPosition = new Vector3(0,11,0);

			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill2").GetComponentInChildren<Text>().text = skillsAtOrangeLevel[1];
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill2").GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-78,0);

			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill3").GetComponent<Image>().enabled = false;
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill3").GetComponentInChildren<Text>().enabled = false;

			SurvivorToken.S.chooseSkillImage.rectTransform.sizeDelta = new Vector2(410.36f, 278.9f);
		}
		else if(currLevel == 3){
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill1").GetComponentInChildren<Text>().text = skillsAtRedLevel[0];
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill1").GetComponent<RectTransform>().anchoredPosition = new Vector3(0,73.3f,0);

			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill2").GetComponentInChildren<Text>().text = skillsAtRedLevel[1];
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill2").GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-15,0);

			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill3").GetComponent<Image>().enabled = true;
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill3").GetComponentInChildren<Text>().enabled = true;
			SurvivorToken.S.chooseSkillImage.transform.FindChild("Skill3").GetComponentInChildren<Text>().text = skillsAtRedLevel[2];

			SurvivorToken.S.chooseSkillImage.rectTransform.sizeDelta = new Vector2(410.36f, 435.9f);
		}

		waitingToSelectSkill = true;

		while(waitingToSelectSkill){
			yield return 0;
		}

		StartCoroutine(ShowNewSkill(skills[currLevel]));
		SurvivorToken.S.chooseSkillImage.rectTransform.anchoredPosition = new Vector3(0, -1300, 0);
	}

	public void SelectedSkill(int num){

		if(currLevel == 2){
			if(num != 0 && num != 1) return;
			skills[2] = skillsAtOrangeLevel[num];
		}
		else if(currLevel == 3){
			
			if(num != 0 && num != 1 && num != 2) return;
			skills[3] = skillsAtRedLevel[num];
		}

		waitingToSelectSkill = false;
	}

	void LevelUp(){
		if(currLevel == 1){
			skills[1] = skillsAtYellowLevel[0];
			StartCoroutine(ShowNewSkill(skillsAtYellowLevel[0]));
			maxActions = 4;
		}
		else if(currLevel == 2){
			StartCoroutine(ChooseNewSkill());
		}
		else if(currLevel == 3){
			StartCoroutine(ChooseNewSkill());
		}
	}

	public void GiveEXP(int numXP){
		currExp += numXP;

		if(currExp >= expToRed){
			currLevel = 3;
			if(levelImage.sprite != SurvivorToken.S.levelSprites[3]){
				LevelUp();
			}
			levelImage.sprite = SurvivorToken.S.levelSprites[3];
		}
		else if(currExp >= expToOrange){
			currLevel = 2;
			if(levelImage.sprite != SurvivorToken.S.levelSprites[2]){
				levelImage.sprite = SurvivorToken.S.levelSprites[2];
				levelImage.transform.rotation = Quaternion.identity;
				LevelUp();
			}
			else{
				levelImage.transform.Rotate(0.0f,0.0f,(-360.0f / (expToRed - expToOrange)));
			}
		}
		else if(currExp >= expToYellow){
			currLevel = 1;
			if(levelImage.sprite != SurvivorToken.S.levelSprites[1]){
				levelImage.sprite = SurvivorToken.S.levelSprites[1];
				levelImage.transform.rotation = Quaternion.identity;
				LevelUp();
			}
			else{
				levelImage.transform.Rotate(0.0f,0.0f,(-360.0f / (expToOrange - expToYellow)));
			}
		}
		else{
			levelImage.transform.Rotate(0.0f,0.0f,(-360.0f / expToYellow));
		}
	}

}
