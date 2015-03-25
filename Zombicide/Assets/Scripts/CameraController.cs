using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float closeDist;
	public float farDist;

	public float zoomSpeed;
	public float panSpeed;

	bool dragging = false;
	Vector3 mouseDragStart;

	public GameObject board;
	float rightX, leftX, topZ, bottomZ;

	// Use this for initialization
	void Start () {
		transform.position = board.transform.position + Vector3.up * (farDist - closeDist) / 2;
		rightX = topZ = Mathf.NegativeInfinity;
		leftX = bottomZ = Mathf.Infinity;

		foreach(Transform child in board.transform){
			if(child.position.x > rightX) rightX = child.position.x;
			if(child.position.x < leftX) leftX = child.position.x;
			if(child.position.z > topZ) topZ = child.position.z;
			if(child.position.z < bottomZ) bottomZ = child.position.z;
		}
		topZ -= 1;
		bottomZ -= 1;
	}
	
	// Update is called once per frame
	void Update () {
		// Mouse wheel moving forward
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > closeDist)
		{
			transform.Translate(Vector3.forward * zoomSpeed);
		}
		
		// Mouse wheel moving backward
		if(Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < farDist)
		{
			transform.Translate(Vector3.back * zoomSpeed);
		}

		if(Input.GetMouseButtonDown(2)) {dragging = true; mouseDragStart = Input.mousePosition;}
		if(Input.GetMouseButtonUp(2)) dragging = false;

		if(dragging){
			float xDiff = mouseDragStart.x - Input.mousePosition.x;
			float zDiff = mouseDragStart.y - Input.mousePosition.y;
			Vector3 diffVec = new Vector3(xDiff, 0, zDiff);
			Vector3 newPos = transform.position + diffVec / 100 / panSpeed;

			if(newPos.x > rightX) newPos.x = rightX;
			if(newPos.x < leftX) newPos.x = leftX;
			if(newPos.z > topZ) newPos.z = topZ;
			if(newPos.z < bottomZ) newPos.z = bottomZ;
			transform.position = newPos;

			mouseDragStart = Input.mousePosition;
		}
	}
}
