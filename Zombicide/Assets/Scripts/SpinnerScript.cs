using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpinnerScript : MonoBehaviour {

	public int num;
	public Text hitText;

	public List<Image> circles;
	bool beenClicked = false;
	public bool finishedSpinning = false;
	public bool finishedAttacking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetHitChance(int hitChance){
		for(int i = hitChance - 1; i < 6; ++i){
			circles[i].color = Color.red;
		}
	}

	IEnumerator SpinWheel(){
		beenClicked = true;
		hitText.text = "";

		float t = 0;
		float ctr = 0;

		int index = 0;
		Color currColor = circles[index].color;
		circles[index].color = currColor / 2.0f;		

		float timing = Random.Range (2.0f, 4.0f);

		while(t < 1){
			t += Time.deltaTime * Time.timeScale / timing;
			ctr += Time.deltaTime * Time.timeScale / 0.1f;

			if(ctr > 1){
				ctr = 0;

				circles[index].color = currColor;
				index++;
				if(index > 5) index = 0;

				currColor = circles[index].color;
				circles[index].color = currColor / 2.0f;	
			}

			yield return 0;
		}

		if(currColor == Color.red) hitText.text = "HIT!";
		if(currColor == Color.white) {
			hitText.text = "Miss";
			hitText.color = Color.black;
			finishedAttacking = true;
			t = 0;
			
			while(t < 1){
				t += Time.deltaTime * Time.timeScale / 0.5f;
				yield return 0;
			}

			Vector2 startPos = Camera.main.ScreenToViewportPoint(transform.position);
			t = 0;
			while(t < 1){
				t += Time.deltaTime * Time.timeScale / 0.5f;
				transform.position = Camera.main.ViewportToScreenPoint(Vector2.Lerp(startPos, startPos + Vector2.up, t));
				yield return 0;
			}
		}
		finishedSpinning = true;

	}

	public void Click(){
		if(beenClicked) return;

		if(AttackScript.S.attWeapon.noise){
			GameController.S.currSurvivor.CurrZone.GetComponent<ZoneScript>().AddNoiseToken();
		}
		StartCoroutine(SpinWheel ());
	}

	IEnumerator MoveOut(){
		Vector2 startPos = Camera.main.ScreenToViewportPoint(transform.position);
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			transform.position = Camera.main.ViewportToScreenPoint(Vector2.Lerp(startPos, startPos + Vector2.up, t));
			yield return 0;
		}
	}

	public void Attack(){
		finishedAttacking = true;
		StartCoroutine(MoveOut());
	}
}
