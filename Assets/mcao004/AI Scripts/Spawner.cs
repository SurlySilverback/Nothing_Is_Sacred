using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField]
	private GameObject Patrol;

	// Use this for initialization
	void Start () {
		
	}

	private GameObject PickCity() {
		List<GameObject> cities = new List<GameObject> ();
		foreach (PolygonCollider2D city in FindObjectsOfType(typeof(PolygonCollider2D))) {
			if (city.gameObject.name == "Collision_City") {
				cities.Add (city.gameObject);
			}
		}
		return cities[Random.Range (0, cities.Count)];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (2)) {
			GameObject newpatrol = (GameObject)Instantiate (Patrol);
			newpatrol.GetComponent<PatrolAI> ().SetHome (PickCity());
		}
	}
}
