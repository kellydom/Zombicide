using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public enum EnemyType{
		Walker,
		Runner,
		Fatty,
		Abomination,
		None
	}

	public EnemyType type;
	public bool hasDoneAction = false;
	public GameObject currZone;
	public int damageToKill;

	// Use this for initialization
	void Start () {
	
	}

	public void MoveTo(GameObject zone, GameObject oldZone, float vertOffset){
		currZone = zone;
		Vector3 topRightCorner = zone.GetComponent<BoxCollider>().bounds.max;
		Vector3 topLeftCorner = topRightCorner;
		topLeftCorner.x = zone.GetComponent<BoxCollider>().bounds.min.x;
		topRightCorner.z += -0.03f;
		topLeftCorner.z += -0.03f;
		
		int zombieType = 0;
		if(type == EnemyType.Runner) zombieType = 1;
		else if(type == EnemyType.Fatty) zombieType = 2;

		Vector3 movePos = Vector3.Lerp (topLeftCorner, topRightCorner, (zombieType + 1) / 5.0f);
		if(type == EnemyType.Abomination){
			movePos = zone.transform.position;
		}

		transform.position = movePos + Vector3.up / 5.0f * vertOffset;

		if(type == EnemyType.Walker){
			zone.GetComponent<ZoneScript>().walkersInZone.Add (this.gameObject);
		}
		if(type == EnemyType.Runner){
			zone.GetComponent<ZoneScript>().runnersInZone.Add (this.gameObject);
		}
		if(type == EnemyType.Fatty){
			zone.GetComponent<ZoneScript>().fattiesInZone.Add (this.gameObject);
		}
		if(type == EnemyType.Abomination){
			zone.GetComponent<ZoneScript>().abombInZone.Add (this.gameObject);
		}

		oldZone.GetComponent<ZoneScript>().RemoveEnemy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseEnter(){
		List<GameObject> zoneZombies = new List<GameObject>();
		if(type == EnemyType.Walker){
			zoneZombies = currZone.GetComponent<ZoneScript>().walkersInZone;
		}
		if(type == EnemyType.Runner){
			zoneZombies = currZone.GetComponent<ZoneScript>().runnersInZone;
		}
		if(type == EnemyType.Fatty){
			zoneZombies = currZone.GetComponent<ZoneScript>().fattiesInZone;
		}
		if(type == EnemyType.Abomination){
			zoneZombies = currZone.GetComponent<ZoneScript>().abombInZone;
		}

		Vector3 avgPos = Vector3.zero;

		foreach(GameObject zombie in zoneZombies){
			avgPos += zombie.transform.position;
		}

		avgPos /= zoneZombies.Count;
		GameController.S.SetZombieNumText(transform.position, zoneZombies.Count);
	}

	void OnMouseExit(){
		GameController.S.MoveZombieNumOff();
	}

	void OnMouseDown(){
		AttackScript.S.SetEnemyType(type, currZone);
	}

}
