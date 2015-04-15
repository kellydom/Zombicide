using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDescriptions : MonoBehaviour {
	public static SkillDescriptions S;

	[System.Serializable]
	public class SkillsAndDescriptions{
		public string skillName;
		public string description;
	}

	public List<SkillsAndDescriptions> skillDescriptions;


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
	
	// Update is called once per frame
	void Update () {
	
	}
}
