using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class ZoneScript : MonoBehaviour {

	public int zoneNum;

	VectorLine border;
	List<Vector3> linePoints = new List<Vector3>();

	// Use this for initialization
	void Start () {
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
	}

	public void addLinePoints(Vector3 size){
		Vector3 p1 = transform.position + new Vector3(size.x, size.y, size.z);
		Vector3 p2 = transform.position + new Vector3(size.x, size.y, -size.z);
		Vector3 p3 = transform.position + new Vector3(-size.x, size.y, -size.z);
		Vector3 p4 = transform.position + new Vector3(-size.x, size.y, size.z);
		Vector3 p5 = transform.position + new Vector3(size.x, size.y, size.z);

		linePoints.Add(p1);
		linePoints.Add(p2);
		linePoints.Add(p3);
		linePoints.Add(p4);
		linePoints.Add(p5);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Highlight(){
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(1,1,0,0.2f);
		if(border == null){
			
			border = new VectorLine("Border", linePoints, null, 10, LineType.Continuous, Joins.Fill);
			border.color = Color.yellow;
			border.Draw3D();

		}
	}

	public void Unhighlight(){
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
		VectorLine.Destroy(ref border);
	}

	void OnMouseEnter(){
	}

	void OnMouseExit(){
	}
}
