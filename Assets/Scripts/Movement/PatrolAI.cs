using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UberAudio;

[RequireComponent(typeof(Deploy),typeof(LineRenderer))]
public class PatrolAI : MonoBehaviour {
	enum Mode{ Init, Patrol, Chase, CaptureUnit, LostInCity, LostUnit, GoHome };
	private Deploy deploy;
	private LineRenderer linerenderer;
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

	public UnityEvent OnCapture;

	void Awake() 
	{
		if (OnCapture == null) {
			OnCapture = new UnityEvent ();
		}
	}

	// Use this for initialization
	void Start ()
    {
		this.deploy = GetComponent<Deploy>();
		this.linerenderer = GetComponent<LineRenderer> ();
		OnCapture.AddListener (delegate {
			AudioManager.Instance.Play ("Scream");	
		});

		// get the home by raycast
		home = PickCity();
		transform.position = home.transform.position;

		this.mode = Mode.Init;
		this.patrolTime = timeInDay;
		this.target = null;

	}

	// When a patrol is made, can be called to set starting point of patrol
	public void SetHome(GameObject home)
    {
		this.home = home;
		transform.position = home.transform.position;
		this.patrolTime = timeInDay;
		this.mode = Mode.Patrol;
		target = null;
	}

	// picks a random city to move towards
	private GameObject PickCity()
    {
		List<City> cities = new List<City> (ServiceLocator.Instance.GetCities());
		return cities[Random.Range (0, cities.Count)].gameObject;
	}

	private bool TargetAtCity()
    {
		// assume in chase mode, so seeUnit() should be in target
		target = SeeUnit();
		if (target == null)
			return false;
		RaycastHit2D hit = Physics2D.Raycast(target.transform.position, -Vector2.up, 0.1f, LayerMask.GetMask("City"));
		return (hit.collider != null);
	}

	// if this patrol unit sees a player unit
	private GameObject SeeUnit()
    {
		GameObject result = null;
		int unitmask = LayerMask.GetMask ("Unit");

		// calc the field of view in whatever conditions are present
		float currFieldOfView = this.fieldOfView;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f);
		if (hit.collider != null) {
			if (LayerMask.LayerToName (hit.transform.gameObject.layer) == "Deep Forest") {
				currFieldOfView *= 0.4f;
			} else if (LayerMask.LayerToName (hit.transform.gameObject.layer) == "Forest") {
				currFieldOfView *= 0.7f;
			}
		}
		
		
		// actual collider using that fieldOfView as radius of visibility circle
		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, fieldOfView + 2.0f, unitmask);
		Vector2 sight = deploy.direction ();
		if (sight == Vector2.zero) {
			return result;
		}

		foreach (Collider2D collider in colliders) {
			if (collider != null) {
				Vector2 collideDirection = (collider.gameObject.transform.position - transform.position);
				//Debug.DrawRay (transform.position, sight.normalized, Color.cyan, 1.0f, false);
				//Debug.Log (Vector2.Angle (sight, collideDirection));
				if (Vector2.Angle(sight, collideDirection) < 20.0f)
				{
					//Debug.Log (collider.gameObject.name);
					return collider.gameObject;
				}
			}
		}
		return result;
	}

	private void ChangeMode()
    {
		switch (mode) {
		case Mode.Init:
			AudioManager.Instance.Play ("IScream", this.gameObject);
			//AudioManager.Instance.Play ("blackflagblues");
			if (home == null) {
				SetHome (PickCity ());
			}
			mode = Mode.Patrol;
			target = PickCity ();
			patrolTime = timeInDay;
			break;
		case Mode.Patrol:
			if ((target = SeeUnit ()) != null && !TargetAtCity()) {
				MoveToTarget ();
				mode = Mode.Chase;
				this.patrolTime = timeInDay * 3.0f;
				AudioManager.Instance.Play ("OhShit");
			} else if (patrolTime < 0) {
				target = home;
				MoveToTarget ();
				mode = Mode.GoHome;
			}
			break;
		case Mode.Chase:
			// capture, lostunit, or lostincity
			if ((target = SeeUnit()) != null && Vector2.Distance(transform.position, target.transform.position) < 0.5f) {
				// capture target
				target.GetComponent<Deploy> ().StopMove ();
				this.patrolTime = 15.0f;
				mode = Mode.CaptureUnit;
				Destroy (target.gameObject);
				//Camera.main.GetComponent<AudioListener> ().enabled = true;
				OnCapture.Invoke ();
				deploy.StopMove ();
				target = home;
			} else if ((target = SeeUnit ()) == null) { // lost unit
				//Debug.Log("Lost");
				mode = Mode.LostUnit;
				patrolTime = 20.0f;
			} else if (TargetAtCity ()) { // lost in city
				mode = Mode.LostInCity;
				this.patrolTime = 15.0f;
				MoveToTarget ();
				target = PickCity ();
			}
			break;
		case Mode.CaptureUnit:
			if (patrolTime < 0.0f) {
				target = home;
				MoveToTarget();
				mode = Mode.GoHome;
			}
			break;
		case Mode.LostInCity:
			if (patrolTime < 0.0f) {
				mode = Mode.Patrol;
				patrolTime = timeInDay;
			} else if ((target = SeeUnit ()) != null && !TargetAtCity ()) {
				MoveToTarget ();
				mode = Mode.Chase;
				this.patrolTime = timeInDay * 3.0f;
				AudioManager.Instance.Play ("OhShit");
			}
			break;
		case Mode.LostUnit:
			if (patrolTime < 0.0f) {
				mode = Mode.Patrol;
				patrolTime = timeInDay;
				target = PickCity ();
				MoveToTarget ();
			} else if ((target = SeeUnit ()) != null) {
				mode = Mode.Chase;
				patrolTime = timeInDay;
				MoveToTarget ();
				AudioManager.Instance.Play ("OhShit");
			}
			break;
		case Mode.GoHome:
			if ((target = SeeUnit ()) != null) {
				MoveToTarget ();
				mode = Mode.Chase;
				this.patrolTime = timeInDay * 3.0f;
				AudioManager.Instance.Play ("OhShit");
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
	private void MoveToTarget()
    {
		if (mode == Mode.Chase && (target = SeeUnit()) == null) {
			return;
		}
		Vector3 temp;
		List<Vector3> moveList = new List<Vector3> ();
		temp = transform.position;
		temp.z = -5.0f;
		moveList.Add (temp);
		temp = target.transform.position;
		temp.z = -5.0f;
		moveList.Add (temp);
		this.deploy.StartMove (moveList,baseSpeed);
	}

	void ProcessMode()
    {
		switch (mode) {
		case Mode.Init:
			break;
		case Mode.Patrol:
			if (!deploy.isDeployed ()) {
				// choose new city to go to
				target = PickCity ();
				MoveToTarget ();
			}
			break;
		case Mode.Chase:
			// may be moving so reassign as we go
			MoveToTarget();
			break;
		case Mode.CaptureUnit:
			// Do nothing
			break;
		case Mode.LostInCity:
			// Not implemented
			break;
		case Mode.LostUnit:
			// Not implemented
			break;
		case Mode.GoHome:
			// Do nothing
			break;
		}
		patrolTime -= Time.deltaTime;
	}

	private void DrawLineOfSight ()
    {
		if (!deploy.isDeployed ()) {
			linerenderer.enabled = false;
		}
		linerenderer.enabled = true;
		Vector3 position = transform.position;
		position.z = -1.0f;
		Vector2 direction = deploy.direction ().normalized * fieldOfView;
		if (target != null) {
			direction = target.transform.position - transform.position;
			direction = direction.normalized * fieldOfView;
		}
		linerenderer.positionCount = 2;
		linerenderer.SetPosition (0, position);
		linerenderer.SetPosition (1, position + (Vector3)direction);
		linerenderer.startWidth = 0;
		linerenderer.endWidth = 1 * fieldOfView;
		linerenderer.sortingOrder = SortingLayer.GetLayerValueFromName ("Unit");
	}
	
	// Update is called once per frame
	void Update () {
		ChangeMode ();
		ProcessMode ();
		DrawLineOfSight ();
	}
}
