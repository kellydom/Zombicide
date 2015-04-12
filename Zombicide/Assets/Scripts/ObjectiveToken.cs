using UnityEngine;
using System.Collections;

public class ObjectiveToken : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(FindZone());
	}

	IEnumerator FindZone(){
		yield return new WaitForEndOfFrame();

		GameObject zone = ZoneSelector.S.GetZoneAt(transform.position);
		zone.GetComponent<ZoneScript>().objectiveInRoom = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
