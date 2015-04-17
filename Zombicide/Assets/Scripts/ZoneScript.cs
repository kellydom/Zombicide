using UnityEngine;
using UnityEngine.UI;
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
	public GameObject noiseTokenPrefab;

	public GameObject objectiveInRoom;

	public bool hasSpawnedZombies = false;


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

	public int EnemiesInZone(){
		int enemies = 0;

		enemies += walkersInZone.Count + runnersInZone.Count + fattiesInZone.Count + abombInZone.Count;

		return enemies;
	}

	public void DoZombieActions(){
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

		object[] parms = new object[4]{walkersToGo, runnersToGo, fattiesToGo, abombToGo};
		
		List<GameObject> allZombies = new List<GameObject>();
		allZombies.AddRange(walkersToGo);
		allZombies.AddRange(runnersToGo);
		allZombies.AddRange(fattiesToGo);
		allZombies.AddRange(abombToGo);
		foreach(GameObject zombie in allZombies){
			zombie.GetComponent<Enemy>().hasDoneAction = true;
		}
		StartCoroutine("ActionCoroutine", parms);
	}

	IEnumerator ActionCoroutine(object[] parms){
		
		List<GameObject> walkersToGo = (List<GameObject>)parms[0];
		List<GameObject> runnersToGo = (List<GameObject>)parms[1];
		List<GameObject> fattiesToGo = (List<GameObject>)parms[2];
		List<GameObject> abombToGo = (List<GameObject>)parms[3];
		
		//Now actually do actions

		//The reason I split it up like this is because
		//if zombies split to go down multiple paths,
		//each group must have the same number of each type of enemy,
		//so the current list groupings are important
		while(GameController.S.zombiesAttacking){
			yield return 0;
			continue;
		}

		DoAnAction(walkersToGo);
		while(GameController.S.zombiesAttacking){
			yield return 0;
			continue;
		}
		DoAnAction(runnersToGo);
		while(GameController.S.zombiesAttacking){
			yield return 0;
			continue;
		}
		DoAnAction(fattiesToGo);
		while(GameController.S.zombiesAttacking){
			yield return 0;
			continue;
		}
		DoAnAction(abombToGo);
		while(GameController.S.zombiesAttacking){
			yield return 0;
			continue;
		}

		yield return new WaitForSeconds(0.5f);
		while(GameController.S.zombiesAttacking){
			yield return 0;
			continue;
		}
		//Runners get to go again!
		DoAnAction(runnersToGo);

	}

	IEnumerator ZombieAttack(int zombiesAttacking, GameObject zone){
		GameController.S.zombiesAttacking = true;
		GameController.S.zombTurnText.text = "Zombies Attack!";
		//do zombie attacking stuff here
		SurvivorToken.S.sacrificeThem = true;

		bool survivorsInZone = true;

		Vector2 currPos = ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition;
		Vector2 desired = new Vector2(0, -35);
		
		ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "";
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition = Vector2.Lerp(currPos, desired, t);

			yield return 0;
		}

		while(zombiesAttacking > 0 && survivorsInZone){
			SurvivorToken.S.sacrificeThem = true;
			SurvivorToken.S.selectPlayerForWound (zone);
			ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "Zombie Attacks Remaining: " + zombiesAttacking;
			while(SurvivorToken.S.sacrificeThem){
				yield return 0;
			}
			zombiesAttacking--;
			survivorsInZone = false;
			foreach(Survivor surv in GameController.S.survivors){
				if(surv.CurrZone == zone){
					survivorsInZone = true;
				}
			}

			yield return 0;
		}
		SurvivorToken.S.MoveTokensOffscreen();

		currPos = desired;
		desired = new Vector2(0, 35);
		
		ZoneSelector.S.zombAttRem.GetComponentInChildren<Text>().text = "";
		t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			ZoneSelector.S.zombAttRem.rectTransform.anchoredPosition = Vector2.Lerp(currPos, desired, t);
			
			yield return 0;
		}
		GameController.S.zombiesAttacking = false;

	}

	public void DoAnAction(List<GameObject> zombies){
		if(zombies.Count == 0) return;

		GameObject enemyZone = zombies[0].GetComponent<Enemy>().currZone;

		for(int i = zombies.Count - 1; i >= 0; --i){
			GameObject zom = zombies[i];
			GameObject nextZone = zom.GetComponent<Enemy>().currZone;
			if(nextZone != enemyZone){
				zombies.Remove(zom);
				List<GameObject> newZombies = new List<GameObject>();
				newZombies.Add (zom);
				DoAnAction(newZombies);
			}
		}


		if(ZoneSelector.S.IsPlayerZone(enemyZone)){
			//Attack the player
			StartCoroutine(ZombieAttack(zombies.Count, zombies[0].GetComponent<Enemy>().currZone));

			return;
		}

		List<GameObject> closestZones = ZoneSelector.S.NoisiestSeeableZonesWithSurvivor(enemyZone);

		//if can't see any survivors, get one with highest noise
		if(closestZones.Count == 0){
			List<GameObject> noisiestZones = ZoneSelector.S.GetNoisiestZones();
			closestZones = ZoneSelector.S.ClosestZonesFromList(noisiestZones, enemyZone);
		}

		//if closest zones is still count 0, then the survivors are behind locked doors
		//which is something I don't think we need to worry about now (or ever?)
		if(closestZones.Count == 0) return;

		List<GameObject> nextSteps = new List<GameObject>();
		//nextSteps.Add (closestZones[0]);
		foreach(GameObject go in closestZones){
			int dist = ZoneSelector.S.ZoneDistance(enemyZone, go);
			List<GameObject> stepsToGO = ZoneSelector.S.StepTowardsZone(enemyZone, go, dist);
			nextSteps.AddRange(stepsToGO);
		}

		if(zombies[0].GetComponent<Enemy>().type == Enemy.EnemyType.Abomination){
			zombies[0].GetComponent<Enemy>().MoveTo(nextSteps[0], enemyZone, 0);
			zombies[0].GetComponent<Enemy>().hasDoneAction = true;
			return;
		}

		int numDiffSteps = nextSteps.Count;
		if(numDiffSteps == 0) return;
		int extraZombies = 0;
		if(zombies.Count % numDiffSteps != 0){
			extraZombies = numDiffSteps - (zombies.Count - numDiffSteps);
		}
		while(extraZombies > 0){
			GameObject newEnemy = Instantiate(zombies[0], zombies[0].transform.position + Vector3.up / extraZombies, Quaternion.identity) as GameObject;
			newEnemy.GetComponent<Enemy>().currZone = enemyZone;
			extraZombies--;
			zombies.Add(newEnemy);
			GameController.S.allZombies.Add (newEnemy.GetComponent<Enemy>());
		}

		int zombiesPerZone = zombies.Count / numDiffSteps;

		int currZombie = 1;
		int currZoneCount = 0;
		float offsetCtr = 1;
		foreach(GameObject zombie in zombies){
			zombie.GetComponent<Enemy>().MoveTo(nextSteps[currZoneCount], enemyZone, offsetCtr);
			currZombie++;
			if(currZombie > zombiesPerZone){
				currZombie = 1;
				currZoneCount++;
			}
			offsetCtr+= 0.1f;
			zombie.GetComponent<Enemy>().hasDoneAction = true;
		}

	}
	public void RemoveEnemy(GameObject zombie){
		Enemy.EnemyType type = zombie.GetComponent<Enemy>().type;
		if(type == Enemy.EnemyType.Walker) walkersInZone.Remove(zombie);
		if(type == Enemy.EnemyType.Runner) runnersInZone.Remove(zombie);
		if(type == Enemy.EnemyType.Fatty) fattiesInZone.Remove(zombie);
		if(type == Enemy.EnemyType.Abomination) abombInZone.Remove(zombie);
	}

	public void AddNoiseToken(){
		Vector3 pos = transform.position;
		pos += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(0.2f, 0.5f), Random.Range(-0.05f, 0.05f));

		GameObject temp = Instantiate(noiseTokenPrefab, pos, Quaternion.identity) as GameObject;
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
		foreach(GameObject noiseToken in noiseTokensInZone){
			Destroy(noiseToken);
		}

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
