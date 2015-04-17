using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuSCript : MonoBehaviour {

	public Button Play;
	public Button Quit;

	// Use this for initialization
	void Start () {
		//Play.transform.position = new Vector3 (Screen.width/2, Screen.width/2, 0);
		//Quit.transform.position = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playGame() {
		Application.LoadLevel (1);
	}

	public void quitGame() {
		Application.Quit ();
	}
}
