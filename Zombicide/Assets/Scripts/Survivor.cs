using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Survivor : MonoBehaviour {

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

	public int numActions;
	public bool slippery = false;
	public bool extraAction = false;
	public bool freeSearch = false;
	public bool doubleMove = false;

	public bool currTurn = false;


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

		float randomX = Random.Range(-size.x, size.x);
		float randomZ = Random.Range(-size.z, size.z);
		float randomHeight = Random.Range(0.1f, 0.6f);

		transform.position = basePos + new Vector3(randomX, randomHeight, randomZ);
	}

	public void DoNothing(){
		numActions = 0;
	}

	public void MoveTo(GameObject newZone, int actionCost){
		numActions -= actionCost;
		SetZone(newZone);
		GameController.S.MoveSetup();
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.playerTurn) {Unhighlight(); return;}

		if(GameController.S.currSurvivor != null) {
			if(GameController.S.currSurvivor != this) Unhighlight();

			return;
		}
		if(GameController.S.closestSurvivor == this) Highlight();
		else Unhighlight();
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
}
