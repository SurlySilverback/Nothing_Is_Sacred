using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UberAudio;

public class Deploy : MonoBehaviour {
	[SerializeField]
	private int nextlrPoint;
	[SerializeField]
	private bool deployed;
	[SerializeField]
	private IList<Vector3> list;
	private float unitSpeed;
	private float usableSpeed;

	// at position, find the terrain coef of the tile under that position
	float GetTerrainCoef(Vector3 position)
	{
		float terrainCoef = 1.0f;
		int layermask = LayerMask.GetMask ("Land", "Grass", "Ocean", "River", "Forest", "Water", "Deep Forest", "Hill", "Tundra", "Snow", "City", "Road");
		RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up, Mathf.Infinity, layermask);
		if (hit)
		{
			//Debug.Log (LayerMask.LayerToName (hit.transform.gameObject.layer));
			switch (LayerMask.LayerToName(hit.transform.gameObject.layer))
			{
			case "Land":
				terrainCoef = 1.0f;
				break;
			case "Grass":
				terrainCoef = 1.0f;
				break;
			case "Ocean":
				terrainCoef = 6.0f;
				break;
			case "River":
				terrainCoef = 6.0f;
				break;
			case "Forest":
				terrainCoef = 1.5f;
				break;
			case "Water":
				terrainCoef = 6.0f;
				break;
			case "Deep Forest":
				terrainCoef = 3.0f;
				break;
			case "Hill":
				terrainCoef = 1.2f;
				break;
			case "Tundra":
				terrainCoef = 1.2f;
				break;
			case "Snow":
				terrainCoef = 1.4f;
				break;
			case "City":
				//Debug.Log ("HELOOOOOO");
				terrainCoef = 0.5f;
				break;
			case "Road":
				terrainCoef = 0.5f;
				break;
			}
		}
		return terrainCoef;
	}

	// if called, have the selected unit transform along the linerenderer
	public void MoveUnit()
	{
		float dist = 0.0f;
		float terrainCoef = 1.0f;
		usableSpeed = unitSpeed * Time.deltaTime;
		Vector3 currPos = transform.position;
		Vector3 finalmov = Vector3.zero;
		if (nextlrPoint >= list.Count)
		{
			deployed = false;
			return;
		}

		// Get terrain speed
		terrainCoef = GetTerrainCoef(currPos);
		dist = Vector3.Distance(currPos, list[nextlrPoint]) * terrainCoef;

		while (dist < usableSpeed)
		{
			// not finished moving
			currPos = list[nextlrPoint];
			++nextlrPoint;
			if (nextlrPoint >= list.Count)
			{
				deployed = false;
				transform.position = currPos;
				return;
			}
			usableSpeed -= dist;
			terrainCoef = GetTerrainCoef (currPos);
			dist = Vector3.Distance (currPos, list[nextlrPoint]) * terrainCoef;
		}
		if (nextlrPoint >= list.Count)
		{
			deployed = false;
			transform.position = currPos;
			return;
		}

		// moved to some point between the currPos
		// and the next point on the line renderer
		finalmov = (list[nextlrPoint] - currPos) * (usableSpeed / dist);
		//Debug.Log (finalmov);
		transform.position = currPos + finalmov;
	}

	public void StartMove(IList<Vector3> list, float unitSpeed) {
		nextlrPoint = 0;
		this.list = list;
		/*foreach (Vector3 item in list) {
			Debug.Log (item);
		}*/
		this.unitSpeed = unitSpeed;
		deployed = true;
	}

	public void StopMove() {
		deployed = false;
		nextlrPoint = 0;
	}

	public bool isDeployed() {
		return deployed;
	}

	public Vector2 direction() {
		if (!deployed) {
			return Vector2.one;
		} else {
			return list [nextlrPoint] - transform.position;
		}
	}

	// Use this for initialization
	void Start () {
		nextlrPoint = 0;
		deployed = false;
		list = new List<Vector3> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (deployed) {
			MoveUnit ();
		}
		/*if (direction ().x >= 0) {
			Vector3 temp = transform.localScale;
			temp.x = 1;
			transform.localScale = temp;
		} else {
			Vector3 temp = transform.localScale;
			temp.x = -1;
			transform.localScale = temp;
		}*/
	}
}
