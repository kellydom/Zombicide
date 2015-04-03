using UnityEngine;
using System.Collections;

public class ZoneSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Plane boardPlane = new Plane(Vector3.up, new Vector3(0,0,0));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hit;

		if(boardPlane.Raycast(ray, out hit)){
			Vector3 hitPos = ray.GetPoint(hit);
			GameController.S.HighlightZone(hitPos);
		}

		if(Input.GetMouseButtonDown(0)){
			GameObject currZone = GameController.S.GetCurrZone();
			if(currZone != null){
				GameController.S.ClickedCurrZone();
			}
		}
	
	}
}
