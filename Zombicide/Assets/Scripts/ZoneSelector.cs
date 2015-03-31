using UnityEngine;
using System.Collections;

public class ZoneSelector : MonoBehaviour {

	public GameObject boardObj;
	BoardLayout bl;

	// Use this for initialization
	void Start () {
		bl = boardObj.GetComponent<BoardLayout>();
	}
	
	// Update is called once per frame
	void Update () {


	}
}
