using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionWheel : MonoBehaviour {

	public enum Actions{
		Melee,
		Ranged,
		Move,
		Search,
		ReorganizeInventory,
		OpenDoor,
		MakeNoise,
		DoNothing,
		GetIntoCar,
		SwitchSeats,
		DriveCar,
		TakeObjective
	}

	bool wheelIsMinimized;
	bool wheelIsChanging;

	public Button middleButton;
	public Image wheelImage;

	public Sprite buttonWhite;
	public Sprite buttonBlack;

	public Text actionText;

	float scale;

	// Use this for initialization
	void Start () {
		wheelIsChanging = false;
		wheelIsMinimized = true;

		wheelImage.color = new Color(1,1,1,0);

		scale = wheelImage.transform.localScale.x;
		wheelImage.transform.localScale = Vector3.zero;
		wheelImage.transform.eulerAngles = new Vector3(0,0,90);

		actionText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ExpandWheel(){
		wheelIsChanging = true;

		middleButton.image.sprite = buttonWhite;
		wheelImage.color = new Color(1,1,1,1);

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.1f;

			Vector3 newScale = Vector3.Lerp(Vector3.zero, new Vector3(scale, scale), t);
			wheelImage.transform.localScale = newScale;

			wheelImage.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(90, 0, t));

			yield return 0;

		}

		wheelIsChanging = false;
		wheelIsMinimized = false;
	}

	IEnumerator RetractWheel(){
		wheelIsChanging = true;

		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.1f;
			
			Vector3 newScale = Vector3.Lerp(new Vector3(scale, scale), Vector3.zero, t);
			wheelImage.transform.localScale = newScale;
			
			wheelImage.transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 90, t));

			yield return 0;
			
		}

		middleButton.image.sprite = buttonBlack;
		wheelImage.color = new Color(1,1,1,0);
		
		wheelIsChanging = false;
		wheelIsMinimized = true;

	}

	public void ActionWheelButtonClick(){
		if(wheelIsChanging) return;

		if(wheelIsMinimized) StartCoroutine(ExpandWheel());
		else StartCoroutine(RetractWheel());
	}

	public void ActionHover(string action){
		GameController.S.MouseEnterButton();
		switch (action){
		case "Melee":
			actionText.text = "Melee";
			break;
		case "Ranged":
			actionText.text = "Ranged";
			break;
		case "Move":
			actionText.text = "Move";
			break;
		case "OpenDoor":
			actionText.text = "Open Door";
			break;
		case "Search":
			actionText.text = "Search";
			break;
		case "MakeNoise":
			actionText.text = "Make Noise";
			break;
		case "ReorganizeInventory":
			actionText.text = "Reorganize Inventory";
			break;
		case "DoNothing":
			actionText.text = "Do Nothing";
			break;
		case "GetIntoCar":
			actionText.text = "Get Into Car";
			break;
		case "SwitchSeats":
			actionText.text = "Switch Seats";
			break;
		case "DriveCar":
			actionText.text = "Drive Car";
			break;
		case "TakeObjective":
			actionText.text = "Take Objective";
			break;
		}

	}

	public void ActionMouseLeave(){
		GameController.S.MouseExitButton();
		actionText.text = "";
	}

	public void ActionClick(string action ){

		switch (action){
		case "Melee":
			print ("Melee");
			break;
		case "Ranged":
			print ("Ranged");
			break;
		case "Move":
			print ("Move");
			break;
		case "OpenDoor":
			print ("OpenDoor");
			break;
		case "Search":
			print ("Search");
			break;
		case "MakeNoise":
			print ("MakeNoise");
			break;
		case "ReorganizeInventory":
			print ("ReorganizeInventory");
			break;
		case "DoNothing":
			print ("DoNothing");
			break;
		case "GetIntoCar":
			print ("GetIntoCar");
			break;
		case "SwitchSeats":
			print ("SwitchSeats");
			break;
		case "DriveCar":
			print ("DriveCar");
			break;
		case "TakeObjective":
			print ("TakeObjective");
			break;
		}

	}
}
