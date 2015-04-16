using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnDeck : MonoBehaviour {
	public static SpawnDeck S;

	[System.Serializable]
	public class SpawnCard{
		public Enemy.EnemyType redLevelType;
		public int redLevelNum;
		
		public Enemy.EnemyType orangeLevelType;
		public int orangeLevelNum;
		
		public Enemy.EnemyType yellowLevelType;
		public int yellowLevelNum;
		
		public Enemy.EnemyType blueLevelType;
		public int blueLevelNum;
	}
	public List<SpawnCard> spawnCards;
	List<SpawnCard> discardedSpawns = new List<SpawnCard>();


	// Use this for initialization
	void Start () {
		//Singleton initialization
		if(S == null){
			S = this;
		}
		else
		{
			if(this != S)
				Destroy(this.gameObject);
		}
	
	}

	void Reshuffle(){
		spawnCards.AddRange(discardedSpawns);
		discardedSpawns.Clear();
	}

	public SpawnCard GetSpawnCard(){
		int ran = Random.Range(0, spawnCards.Count);

		SpawnCard returnVal = spawnCards[ran];
		spawnCards.RemoveAt(ran);
		discardedSpawns.Add (returnVal);

		if(spawnCards.Count == 0){
			Reshuffle();
		}

		return returnVal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
