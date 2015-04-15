using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AttackScript : MonoBehaviour {
	public static AttackScript S;

	public GameObject attackWheelPrefab;

	List<GameObject> attackWheels = new List<GameObject>();
	GameObject attackingZone = null;
	bool isMelee = true;

	Enemy.EnemyType typeAttacking;
	public bool waitingToGetEnemyType = false;

	public Image chooseZombieText;

	bool needToMoveUp = true;
	public Card attWeapon;

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

	int AttacksLeft(){
		int attacks = 0;
		foreach(GameObject spinner in attackWheels){
			if(!spinner.GetComponent<SpinnerScript>().finishedAttacking) attacks++;
		}
		return attacks;
	}

	void Attack(){
		foreach(GameObject spinner in attackWheels){
			if(!spinner.GetComponent<SpinnerScript>().finishedAttacking){
				spinner.GetComponent<SpinnerScript>().Attack();
				return;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator PrepareForAttacks(){
		bool allFinished = false;
		while(!allFinished){
			allFinished = true;
			for(int i = attackWheels.Count - 1; i >= 0; --i){
				if(!attackWheels[i].GetComponent<SpinnerScript>().finishedSpinning){
					allFinished = false;
				}
			}

			yield return 0;
		}


		while(AttacksLeft() > 0 && attackingZone.GetComponent<ZoneScript>().EnemiesInZone() > 0){
			if(GameController.S.currSurvivor != null){
				if(GameController.S.currSurvivor.doingSkillStuff){
					yield return 0;
					continue;
				}
			}
			waitingToGetEnemyType = true;
			
			ZoneScript zone = attackingZone.GetComponent<ZoneScript>();
			if(isMelee){
				if(typeAttacking != Enemy.EnemyType.None){
					if(!needToMoveUp){
						StartCoroutine(ChooseZombieDown());
					}
					
					if(typeAttacking == Enemy.EnemyType.Walker){
						while(zone.walkersInZone.Count > 0 && AttacksLeft() > 0){
							if(GameController.S.currSurvivor != null){
								if(GameController.S.currSurvivor.doingSkillStuff){
									yield return 0;
									continue;
								}
							}
							Destroy(zone.walkersInZone[zone.walkersInZone.Count - 1]);
							zone.walkersInZone.RemoveAt(zone.walkersInZone.Count - 1);
							Attack ();
							GameController.S.currSurvivor.GiveEXP(1);
							yield return 0;
						}
						GameController.S.MoveZombieNumOff();
						typeAttacking = Enemy.EnemyType.None;
					}
					if(typeAttacking == Enemy.EnemyType.Runner){
						while(zone.runnersInZone.Count > 0 && AttacksLeft() > 0){
							if(GameController.S.currSurvivor != null){
								if(GameController.S.currSurvivor.doingSkillStuff){
									yield return 0;
									continue;
								}
							}
							Destroy(zone.runnersInZone[zone.runnersInZone.Count - 1]);
							zone.runnersInZone.RemoveAt(zone.runnersInZone.Count - 1);
							Attack ();
							GameController.S.currSurvivor.GiveEXP(1);
							yield return 0;
						}
						GameController.S.MoveZombieNumOff();
						typeAttacking = Enemy.EnemyType.None;
					}
					if(typeAttacking == Enemy.EnemyType.Fatty && attWeapon.damage > 1){
						if(GameController.S.currSurvivor != null){
							if(GameController.S.currSurvivor.doingSkillStuff){
								yield return 0;
								continue;
							}
						}
						while(zone.fattiesInZone.Count > 0 && AttacksLeft() > 0){
							Destroy(zone.fattiesInZone[zone.fattiesInZone.Count - 1]);
							zone.fattiesInZone.RemoveAt(zone.fattiesInZone.Count - 1);
							Attack ();
							GameController.S.currSurvivor.GiveEXP(1);
							yield return 0;
						}
						GameController.S.MoveZombieNumOff();
						typeAttacking = Enemy.EnemyType.None;
					}
					if(typeAttacking == Enemy.EnemyType.Abomination && attWeapon.damage > 2){
						while(zone.abombInZone.Count > 0 && AttacksLeft() > 0){
							if(GameController.S.currSurvivor != null){
								if(GameController.S.currSurvivor.doingSkillStuff){
									yield return 0;
									continue;
								}
							}
							Destroy(zone.abombInZone[zone.abombInZone.Count - 1]);
							zone.abombInZone.RemoveAt(zone.abombInZone.Count - 1);
							Attack ();
							GameController.S.currSurvivor.GiveEXP(5);
							yield return 0;
						}
						GameController.S.MoveZombieNumOff();
						typeAttacking = Enemy.EnemyType.None;
					}
				}
				else{
					if(needToMoveUp){
						StartCoroutine(ChooseZombieUp());
					}
				}
			}
			else{
				while(zone.EnemiesInZone() > 0 && AttacksLeft() > 0){
					if(GameController.S.currSurvivor != null){
						if(GameController.S.currSurvivor.doingSkillStuff){
							yield return 0;
							continue;
						}
					}
					foreach(Survivor surv in GameController.S.survivors){
						if(surv == GameController.S.currSurvivor) continue;

						if(surv.CurrZone == attackingZone){
							while(surv.numWounds < 2 && AttacksLeft() > 0){
								surv.numWounds++;
								Attack();
								yield return 0;
							}
						}
					}
					while(zone.walkersInZone.Count > 0 && AttacksLeft() > 0){
						if(GameController.S.currSurvivor != null){
							if(GameController.S.currSurvivor.doingSkillStuff){
								yield return 0;
								continue;
							}
						}
						Destroy(zone.walkersInZone[zone.walkersInZone.Count - 1]);
						zone.walkersInZone.RemoveAt(zone.walkersInZone.Count - 1);
						Attack ();
						GameController.S.currSurvivor.GiveEXP(1);
						yield return 0;
					}
					while(zone.fattiesInZone.Count > 0 && AttacksLeft() > 0){
						if(GameController.S.currSurvivor != null){
							if(GameController.S.currSurvivor.doingSkillStuff){
								yield return 0;
								continue;
							}
						}
						Attack ();
						if(attWeapon.damage < 2) continue;
						Destroy(zone.fattiesInZone[zone.fattiesInZone.Count - 1]);
						zone.fattiesInZone.RemoveAt(zone.fattiesInZone.Count - 1);
						GameController.S.currSurvivor.GiveEXP(1);
						yield return 0;
					}
					while(zone.abombInZone.Count > 0 && AttacksLeft() > 0){
						if(GameController.S.currSurvivor != null){
							if(GameController.S.currSurvivor.doingSkillStuff){
								yield return 0;
								continue;
							}
						}
						Attack ();
						if(attWeapon.damage < 3) continue;
						Destroy(zone.abombInZone[zone.abombInZone.Count - 1]);
						zone.abombInZone.RemoveAt(zone.abombInZone.Count - 1);
						GameController.S.currSurvivor.GiveEXP(5);
						yield return 0;
					}
					while(zone.runnersInZone.Count > 0 && AttacksLeft() > 0){
						if(GameController.S.currSurvivor != null){
							if(GameController.S.currSurvivor.doingSkillStuff){
								yield return 0;
								continue;
							}
						}
						Destroy(zone.runnersInZone[zone.runnersInZone.Count - 1]);
						zone.runnersInZone.RemoveAt(zone.runnersInZone.Count - 1);
						GameController.S.currSurvivor.GiveEXP(1);
						Attack ();
						yield return 0;
					}
				}
			}



			yield return 0;
		}
		for(int i = attackWheels.Count - 1; i >= 0; --i){
			Destroy(attackWheels[i]);
			attackWheels.RemoveAt(i);
		}
		
		if(!needToMoveUp){
			StartCoroutine(ChooseZombieDown());
		}

		GameController.S.FinishAttackAction();
	}

	IEnumerator ChooseZombieUp(){
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			
			chooseZombieText.rectTransform.anchoredPosition = Vector3.Lerp(new Vector3(0, -30, 0), new Vector3(0, 22, 0), t);
			
			yield return 0;
		}
		needToMoveUp = false;
	}

	IEnumerator ChooseZombieDown(){
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			
			chooseZombieText.rectTransform.anchoredPosition = Vector3.Lerp(new Vector3(0, 22, 0), new Vector3(0, -30, 0), t);
			
			yield return 0;
		}
		needToMoveUp = true;
	}

	public void SetEnemyType(Enemy.EnemyType newType, GameObject zoneClicked){
		if(typeAttacking != Enemy.EnemyType.None) return;
		if(zoneClicked != attackingZone) return;
		if(!waitingToGetEnemyType) return;
		typeAttacking = newType;
	}

	public void CreateAttackWheels(GameObject zone, bool melee, Card attackingWeapon, bool dualWield){
		attackingZone = zone;
		isMelee = melee;
		typeAttacking = Enemy.EnemyType.None;
		waitingToGetEnemyType = false;
		attWeapon = attackingWeapon;
		GameObject canvas = GameObject.Find("Canvas");
		
		if(attWeapon.noise){
			GameController.S.currSurvivor.CurrZone.GetComponent<ZoneScript>().AddNoiseToken();
		}

		int numAttacking = attackingWeapon.dice;
		if(dualWield) numAttacking += numAttacking;
		int ctr = 0;
		while(numAttacking > 0){
			numAttacking--;
			GameObject newAttackWheel = Instantiate(attackWheelPrefab) as GameObject;

			float y = Mathf.FloorToInt(numAttacking / 4);
			y = 1 - 1.0f/4 * (y + 1);

			float x = numAttacking % 4;
			x = 1.0f/5.0f * (x + 1);

			Vector2 viewportPoint = Camera.main.ViewportToScreenPoint(new Vector2(x, y)); //convert game object position to VievportPoint
			// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
			newAttackWheel.transform.SetParent(canvas.transform);
			newAttackWheel.transform.position = viewportPoint;

			
			int hitChance = attackingWeapon.minDiceNumber;
			newAttackWheel.GetComponent<SpinnerScript>().SetHitChance(hitChance);
			newAttackWheel.GetComponent<SpinnerScript>().num = ctr;
			attackWheels.Add (newAttackWheel);
			ctr++;
		}

		StartCoroutine(PrepareForAttacks());
	}
}
