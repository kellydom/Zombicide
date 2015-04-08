using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public enum EnemyType{
		Walker,
		Runner,
		Fatty,
		Abomination
	}

	public EnemyType type;
	public bool hasDoneAction;
	public GameObject currZone;

	// Use this for initialization
	void Start () {
		hasDoneAction = false;
	
	}

	public void MoveTo(GameObject zone, float vertOffset){
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
