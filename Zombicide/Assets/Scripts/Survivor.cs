using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {

	GameObject currZone;
	Color baseColor;

	// Use this for initialization
	void Start () {
		baseColor = gameObject.GetComponent<Renderer>().material.color;
	}

	public void setZone(GameObject newZone){
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
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			GameController.S.SelectSurvivor(this);
		}
	}

	public void IncreaseSize(float sizeInc){
		Vector3 size = transform.localScale;
		size *= sizeInc;
		transform.localScale = size;

	}

	void OnMouseEnter(){
		gameObject.GetComponent<Renderer>().material.color = new Color(1,1,0,0.2f);
	}
	
	void OnMouseExit(){
		gameObject.GetComponent<Renderer>().material.color = baseColor;
	}
}
