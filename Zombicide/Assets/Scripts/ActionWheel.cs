using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionWheel : MonoBehaviour {
	public static ActionWheel S;


	public bool mouseInWheel;
	public bool mouseInWheelButton;

	bool wheelIsMinimized;
	bool wheelIsChanging;

	public Button middleButton;
	public Image wheelImage;

	public Sprite buttonWhite;
	public Sprite buttonBlack;

	public Text actionText;

	float scale;

	string currAction = "";
	public string CurrAction{
		get{return currAction;}
	}

	// Use this for initialization
	void Start () {
		if(S == null){
			S = this;
		}
		else
		{
			if(this != S)
				Destroy(this.gameObject);
		}

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
	public void MouseEnterWheel(){
		mouseInWheel = true;
	}
	
	public void MouseExitWheel(){
		mouseInWheel = false;
		
	}
	
	public void MouseEnterButton(){
		mouseInWheelButton = true;
	}
	
	public void MouseExitButton(){
		mouseInWheelButton = false;
		
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
		MouseEnterButton();
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
		MouseExitButton();
		if(currAction == "") actionText.text = "";	
		else actionText.text = currAction;
	}

	public void ActionClick(string action){
		if(currAction == action){
			currAction = "";
			actionText.text = "";
			return;
		}
		currAction = action;

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
}
