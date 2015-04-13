using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AttackScript : MonoBehaviour {
	public static AttackScript S;

	public GameObject attackWheelPrefab;

	List<GameObject> attackWheels = new List<GameObject>();

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateAttackWheels(){
		GameObject canvas = GameObject.Find("Canvas");

		int ran = Random.Range(1,7);

		while(ran > 0){
			ran--;
			GameObject newAttackWheel = Instantiate(attackWheelPrefab) as GameObject;

			float y = Mathf.FloorToInt(ran / 4);
			y = 1 - 1.0f/4 * (y + 1);

			float x = ran % 4;
			x = 1.0f/5.0f * (x + 1);

			Vector2 viewportPoint = Camera.main.ViewportToScreenPoint(new Vector2(x, y)); //convert game object position to VievportPoint
			// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
			newAttackWheel.transform.SetParent(canvas.transform);
			newAttackWheel.transform.position = viewportPoint;

			int hitChance = Random.Range(1, 6);
			while(hitChance > 0){
				hitChance--;

				newAttackWheel.transform.GetChild(hitChance).GetComponent<Image>().color = Color.red;
			}

			attackWheels.Add (newAttackWheel);
		}

	}
}
