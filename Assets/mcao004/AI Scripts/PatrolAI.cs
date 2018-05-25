using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Deploy))]
public class PatrolAI : MonoBehaviour {
	enum Mode{patrol, chase, captureUnit, lostInCity, lostUnit, goHome};
	private Deploy deploy;
	private GameObject home;
	[SerializeField]
	private float baseSpeed;
	[SerializeField]
	private float fieldOfView;
	[SerializeField]
	private Mode mode;
	private float tyrannyCost;
	[SerializeField]
	private float patrolTime;
	[SerializeField]
	private GameObject target;

	private const float timeInDay = 1440.0f;

	// Use this for initialization
	void Start () {
		this.deploy = GetComponent<Deploy>();

		// get the home by raycast
		home = PickCity();
		transform.position = home.transform.position;

		this.mode = Mode.patrol;
		this.baseSpeed = 8.0f;
		this.fieldOfView = 10.0f;
		this.tyrannyCost = 10.0f;
		this.patrolTime = timeInDay;
		this.target = null;
	}

	public void SetHome(GameObject home) {
		this.home = home;
		transform.position = home.transform.position;
	}

	// picks a random city to move towards
	private GameObject PickCity() {
		List<GameObject> cities = new List<GameObject> ();
		foreach (PolygonCollider2D city in FindObjectsOfType(typeof(PolygonCollider2D))) {
			if (city.gameObject.name == "Collision_City") {
				cities.Add (city.gameObject);
			}
		}
		return cities[Random.Range (0, cities.Count)];
	}

	private bool targetAtCity() {
		// assume in chase mode, so seeUnit() should be in target
		target = seeUnit();
		if (target == null)
			return false;
		RaycastHit2D hit = Physics2D.Raycast(target.transform.position, -Vector2.up, 0.1f, LayerMask.GetMask("City"));
		return (hit.collider != null);
	}

	// if this patrol unit sees a player unit
	private GameObject seeUnit() {
		GameObject result = null;
		int unitmask = LayerMask.GetMask ("Unit");

		// calc the field of view in whatever conditions are present
		float currFieldOfView = this.fieldOfView;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
		if (LayerMask.LayerToName (hit.transform.gameObject.layer) == "Deep Forest") {
			currFieldOfView *= 0.4f;
		} else if (LayerMask.LayerToName (hit.transform.gameObject.layer) == "Forest") {
			currFieldOfView *= 0.7f;
		}
		// actual collider using that fieldOfView as radius of visibility circle
		Collider2D collider = Physics2D.OverlapCircle (transform.position, fieldOfView, unitmask);
		if (collider != null) {
			result = collider.gameObject;
		}
		return result;
	}

	void ChangeMode() {
		switch (mode) {
		case Mode.patrol:
			if ((target = seeUnit ()) != null) {
				moveToTarget ();
				mode = Mode.chase;
				this.patrolTime = timeInDay * 3.0f;
			} else if (patrolTime < 0) {
				target = home;
				moveToTarget ();
				mode = Mode.goHome;
			}
			break;
		case Mode.chase:
			// capture, lostunit, or lostincity
			if ((target = seeUnit()) != null && Vector2.Distance(transform.position, target.transform.position) < 0.5f) {
				// capture target
				target.GetComponent<Deploy> ().StopMove ();
				this.patrolTime = 15.0f;
				mode = Mode.captureUnit;
				Destroy (target.gameObject);
				deploy.StopMove ();
				target = home;
			} else if ((target = seeUnit ()) == null) { // lost unit
				mode = Mode.lostUnit;
				patrolTime = 20.0f;
			} else if (targetAtCity ()) { // lost in city
				mode = Mode.lostInCity;
				this.patrolTime = 15.0f;
				target = PickCity ();
				moveToTarget ();
			}
			break;
		case Mode.captureUnit:
			if (patrolTime < 0.0f) {
				target = home;
				moveToTarget();
				mode = Mode.goHome;
			}
			break;
		case Mode.lostInCity:
			if (patrolTime < 0.0f) {
				mode = Mode.patrol;
				patrolTime = timeInDay;
			}
			break;
		case Mode.lostUnit:
			if (patrolTime < 0.0f) {
				mode = Mode.patrol;
				patrolTime = timeInDay;
				target = PickCity ();
				moveToTarget ();
			} else if ((target = seeUnit ()) != null) {
				mode = Mode.chase;
				patrolTime = timeInDay;
				moveToTarget ();
			}
			break;
		case Mode.goHome:
			if ((target = seeUnit ()) != null) {
				moveToTarget ();
				mode = Mode.chase;
				this.patrolTime = timeInDay * 3.0f;
			} else if (Vector2.Distance(transform.position,home.transform.position) < 0.5f) {
				// got home, so despawn
				/////////////////////FIXFORSETTINGTYRANNY();
				//gameObject.SetActive(false);
				Destroy (gameObject);
			}
			break;
		}
	}

	// reassign the destination to the target
	private void moveToTarget() {
		if (mode == Mode.chase && (target = seeUnit()) == null) {
			target = home;
		}
		List<Vector3> moveList = new List<Vector3> ();
		moveList.Add (transform.position);
		moveList.Add (target.transform.position);
		deploy.StartMove (moveList, baseSpeed);
	}

	void ProcessMode() {
		switch (mode) {
		case Mode.patrol:
			if (!deploy.isDeployed ()) {
				// choose new city to go to
				target = PickCity ();
				moveToTarget ();
			}
			break;
		case Mode.chase:
			// may be moving so reassign as we go
			moveToTarget();
			break;
		case Mode.captureUnit:
			// Do nothing
			break;
		case Mode.lostInCity:
			// Not implemented
			break;
		case Mode.lostUnit:
			// Not implemented
			break;
		case Mode.goHome:
			// Do nothing
			break;
		}
		patrolTime -= Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		ChangeMode ();
		ProcessMode ();
	}
}
