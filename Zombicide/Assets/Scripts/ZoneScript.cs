using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class ZoneScript : MonoBehaviour {

	public int zoneNum;

	VectorLine border;
	List<Vector3> linePoints = new List<Vector3>();

	public List<GameObject> walkersInZone = new List<GameObject>();
	public List<GameObject> runnersInZone = new List<GameObject>();
	public List<GameObject> fattiesInZone = new List<GameObject>();
	public List<GameObject> abombInZone = new List<GameObject>();

	public List<GameObject> noiseTokensInZone = new List<GameObject>();


	// Use this for initialization
	void Start () {
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
	}

	public void addLinePoints(Vector3 size){
		Vector3 p1 = transform.position + new Vector3(size.x, size.y, size.z);
		Vector3 p2 = transform.position + new Vector3(size.x, size.y, -size.z);
		Vector3 p3 = transform.position + new Vector3(-size.x, size.y, -size.z);
		Vector3 p4 = transform.position + new Vector3(-size.x, size.y, size.z);
		Vector3 p5 = transform.position + new Vector3(size.x, size.y, size.z);

		linePoints.Add(p1);
		linePoints.Add(p2);
		linePoints.Add(p3);
		linePoints.Add(p4);
		linePoints.Add(p5);

	}

	public void AddZombieToZone(GameObject zombie){
		Enemy e = zombie.GetComponent<Enemy>();
		if(e.type == Enemy.EnemyType.Walker) walkersInZone.Add (zombie);
		if(e.type == Enemy.EnemyType.Runner) runnersInZone.Add (zombie);
		if(e.type == Enemy.EnemyType.Fatty) fattiesInZone.Add (zombie);
		if(e.type == Enemy.EnemyType.Abomination) abombInZone.Add (zombie);
	}

	public void DoZombieActions(){
		StartCoroutine(ActionCoroutine());
	}

	IEnumerator ActionCoroutine(){
		//These are the zombies that actually have to do actions
		//It may be exactly the lists above, but it is possible
		//There are zombies in this zone who have already gone, so
		//they shouldn't go again
		List<GameObject> walkersToGo = new List<GameObject>();
		List<GameObject> runnersToGo = new List<GameObject>();
		List<GameObject> fattiesToGo = new List<GameObject>();
		List<GameObject> abombToGo = new List<GameObject>();
		
		foreach(GameObject zombie in walkersInZone){
			if(zombie.GetComponent<Enemy>().hasDoneAction) continue;
			walkersToGo.Add (zombie);
		}
		foreach(GameObject zombie in runnersInZone){
			if(zombie.GetComponent<Enemy>().hasDoneAction) continue;
			runnersToGo.Add (zombie);
		}
		foreach(GameObject zombie in fattiesInZone){
			if(zombie.GetComponent<Enemy>().hasDoneAction) continue;
			fattiesToGo.Add (zombie);
		}
		foreach(GameObject zombie in abombInZone){
			if(zombie.GetComponent<Enemy>().hasDoneAction) continue;
			abombToGo.Add (zombie);
		}
		
		
		//Now actually do actions
		
		List<GameObject> allZombies = new List<GameObject>();
		allZombies.AddRange(walkersToGo);
		allZombies.AddRange(runnersToGo);
		allZombies.AddRange(fattiesToGo);
		allZombies.AddRange(abombToGo);
		foreach(GameObject zombie in allZombies){
			zombie.GetComponent<Enemy>().hasDoneAction = true;
		}

		//The reason I split it up like this is because
		//if zombies split to go down multiple paths,
		//each group must have the same number of each type of enemy,
		//so the current list groupings are important
		DoAnAction(walkersToGo);
		DoAnAction(runnersToGo);
		DoAnAction(fattiesToGo);
		DoAnAction(abombToGo);

		yield return new WaitForSeconds(0.5f);
		//Runners get to go again!
		DoAnAction(runnersToGo);

	}

	public void DoAnAction(List<GameObject> zombies){
		if(zombies.Count == 0) return;
		
		GameObject enemyZone = zombies[0].GetComponent<Enemy>().currZone;
		List<GameObject> closestZones = ZoneSelector.S.NoisiestSeeableZonesWithSurvivor(enemyZone);

		//if can't see any survivors, get one with highest noise
		if(closestZones.Count == 0){
			List<GameObject> noisiestZones = ZoneSelector.S.GetNoisiestZones();
			closestZones = ZoneSelector.S.ClosestZonesFromList(noisiestZones, enemyZone);
		}

		//if closest zones is still count 0, then the survivors are behind locked doors
		//which is something I don't think we need to worry about now (or ever?)
		if(closestZones.Count == 0) return;
		
		if(closestZones[0] == enemyZone){
			//The zombies are at a zone with a survivor, so attack
		}
		else{
			List<GameObject> nextSteps = new List<GameObject>();
			foreach(GameObject go in closestZones){
				int dist = ZoneSelector.S.ZoneDistance(enemyZone, go);
				List<GameObject> stepsToGO = ZoneSelector.S.StepTowardsZone(enemyZone, go, dist);
				nextSteps.AddRange(stepsToGO);
			}

			GameObject nextZone = nextSteps[0];

			float offsetCtr = 1;
			foreach(GameObject zombie in zombies){
				zombie.GetComponent<Enemy>().MoveTo(nextZone, offsetCtr);
				offsetCtr+= 0.1f;
			}
		}
	}

	public void AddNoiseToken(){
		GameObject temp = new GameObject();
		noiseTokensInZone.Add (temp);
	}

	public int ZoneNoise(){
		int survNoise = 0;
		foreach(Survivor surv in GameController.S.survivors){
			if(surv.CurrZone == this.gameObject) survNoise++;
		}

		return survNoise + noiseTokensInZone.Count;
	}

	public void RemoveNoiseTokens(){
		noiseTokensInZone.Clear();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Highlight(){
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(1,1,0,0.2f);
		if(border == null){
			
			border = new VectorLine("Border", linePoints, null, 10, LineType.Continuous, Joins.Fill);
			border.color = Color.yellow;
			border.Draw3D();

		}
	}

	public void Unhighlight(){
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
		VectorLine.Destroy(ref border);
	}

	void OnMouseEnter(){
	}

	void OnMouseExit(){
	}
}
