using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public static CameraController S;	

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
		//Singleton initialization
		if(S == null){
			S = this;
		}
		else
		{
			if(this != S)
				Destroy(this.gameObject);
		}

		transform.position = board.transform.position + Vector3.up * (farDist - closeDist) / 2;
		rightX = topZ = Mathf.NegativeInfinity;
		leftX = bottomZ = Mathf.Infinity;

		foreach(Transform child in board.transform){
			if(child.position.x > rightX) rightX = child.position.x + 0.5f;
			if(child.position.x < leftX) leftX = child.position.x - 0.5f;
			if(child.position.z > topZ) topZ = child.position.z;
			if(child.position.z < bottomZ) bottomZ = child.position.z - 0.01f;
		}
		topZ -= 0.1f;
		bottomZ -= 1;
	}
	
	// Update is called once per frame
	void Update () {
		// Mouse wheel moving forward
		if((Input.GetKeyDown(KeyCode.PageUp) || Input.GetAxis("Mouse ScrollWheel") > 0) && transform.position.y > closeDist)
		{
			transform.Translate(Vector3.forward * zoomSpeed);
			//SurvivorToken.S.phil.enabled = false;
			//SurvivorToken.S.wanda.enabled = false;
		}
		
		// Mouse wheel moving backward
		if((Input.GetKeyDown(KeyCode.PageDown) || Input.GetAxis("Mouse ScrollWheel") < 0) && transform.position.y < farDist)
		{
			transform.Translate(Vector3.back * zoomSpeed);
			//SurvivorToken.S.phil.enabled = false;
			//SurvivorToken.S.wanda.enabled = false;
		}

		if(Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.forward / 25 / panSpeed, Space.World);
			//SurvivorToken.S.phil.enabled = false;
			//SurvivorToken.S.wanda.enabled = false;
		}
		if(Input.GetKey(KeyCode.S)){
			transform.Translate(-Vector3.forward / 25 / panSpeed, Space.World);
			//SurvivorToken.S.phil.enabled = false;
			//SurvivorToken.S.wanda.enabled = false;
		}
		if(Input.GetKey(KeyCode.A)){
			transform.Translate(-Vector3.right / 25 / panSpeed, Space.World);
			//SurvivorToken.S.phil.enabled = false;
			//SurvivorToken.S.wanda.enabled = false;
		}
		if(Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right / 25 / panSpeed, Space.World);
			//SurvivorToken.S.phil.enabled = false;
			//SurvivorToken.S.wanda.enabled = false;
		}
		Vector3 pos = transform.position;

		if(pos.x > rightX) pos.x = rightX;
		if(pos.x < leftX) pos.x = leftX;
		if(pos.z > topZ) pos.z = topZ;
		if(pos.z < bottomZ) pos.z = bottomZ;

		transform.position = pos;

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

	IEnumerator MoveOverTime(object[] parms){
		Vector3 pos = (Vector3)parms[0];
		float time = (float)parms[1];

		Vector3 startPos = transform.position;

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / time;
			transform.position = Vector3.Lerp (startPos, pos, t);
			yield return 0;
		}

	}

	public void MoveTo(Vector3 pos, float time){
		StopCoroutine("MoveOverTime");

		object[] parms = new object[2]{pos, time};
		StartCoroutine("MoveOverTime", parms);
	}

	public void ZoomOut(float time){
		StopCoroutine("MoveOverTime");
		
		object[] parms = new object[2]{new Vector3(0, 2.3f, -1.41f), time};
		StartCoroutine("MoveOverTime", parms);
	}
}
