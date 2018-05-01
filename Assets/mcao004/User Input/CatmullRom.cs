using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class CatmullRom : MonoBehaviour {
	enum State {start, drawMode, makePoint, stillHeld, lastNode};

	[SerializeField]
	private const float tension = 1.0f;
	[SerializeField]
	private const int ptsInBetween = 20;
	[SerializeField]
	private const float const_z = 0.0f;
	[SerializeField]
	private State state = State.start;
	[SerializeField]
	private List<Vector3> controlPoints;
	[SerializeField]
	private LineRenderer lr;
	[SerializeField]
	private Matrix4x4 m;

	// Use this for initialization
	void Start () {
		state = State.start;
		controlPoints = new List<Vector3>();
		lr = gameObject.AddComponent<LineRenderer> ();
		lr.startColor = Color.red;
		lr.endColor = Color.red;
		lr.useWorldSpace = true;
		lr.widthMultiplier = 10.0f;

		m = new Matrix4x4 ();
		m[0,0] = 0;
		m [0, 1] = 2;
		m [0, 2] = 0;
		m [0, 3] = 0;
		m [1, 0] = -tension;
		m [1, 1] = 0;
		m [1, 2] = tension;
		m [1, 3] = 0;
		m [2, 0] = 2 * tension;
		m [2, 1] = tension - 6;
		m [2, 2] = -2 * (tension - 3);
		m [2, 3] = -tension;
		m [3, 0] = -tension;
		m [3, 1] = 4 - tension;
		m [3, 2] = tension - 4;
		m [3, 3] = tension;
	}

	void splitByFour() {
		List<Vector3> fourpts = new List<Vector3>();
		// add ghost points to beginning and end of controlPoints
		controlPoints.Insert(0, 2*controlPoints[0]-controlPoints[1]);
		controlPoints.Add (2 * controlPoints [controlPoints.Count - 1] - controlPoints [controlPoints.Count - 2]);

		Vector3[] result = new Vector3[(controlPoints.Count-3) * ptsInBetween];
		lr.positionCount = (controlPoints.Count-3)*ptsInBetween;
		int pos = 0;
		foreach (Vector3 pt in controlPoints) {
			// shifting window of four points in fourpts list
			fourpts.Add (pt);
			if (fourpts.Count < 4) {
				continue;
			} else if (fourpts.Count > 4) {
				fourpts.RemoveAt (0);
			}

			// pass into spline
			spline (fourpts, result, pos);
			pos += ptsInBetween;
		}
		lr.SetPositions (result);

		// remove ghost points
		controlPoints.RemoveAt(0);
		controlPoints.RemoveAt (controlPoints.Count - 1);
	}

	// take four nodes and add some number(ptsInBetween) of nodes between the middle two
	void spline(List<Vector3> fourpts, Vector3[] result, int startingPos) {
		// error check
		if (fourpts.Count != 4) {
			Debug.Log ("Wrong number of points passed to spline()");
			return;
		}

		int i = 0;
		// precalculate two right matrices
		Matrix4x4 temp = new Matrix4x4();
		for (i = 0; i < 4; i++) {
			temp.SetRow (i, new Vector4 (fourpts[i].x, fourpts[i].y, fourpts[i].z));
		}

		temp = m * temp;
		float t = 0.0f, deltat = 1.0f/ptsInBetween;
		for (i = 0; i < ptsInBetween; i++) {
			t = deltat * i;
			Vector4 vect = new Vector4 (1.0f, t, t*t, t*t*t);
			result [startingPos + i] =  temp.transpose * (0.5f *vect);
		}
	}

	// takes location of mouse and stores as point
	void recordPoint() {
		Vector3 v = (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		v.z = const_z;
		controlPoints.Add (v);
	}

	void DrawCurve() {
		// if below two points only displays the previous setup of controlPoints

		if (controlPoints.Count == 2) {
			//Debug.DrawLine (controlPoints [0], controlPoints [1], Color.red);
			int i = 0;
			lr.positionCount = controlPoints.Count;
			foreach (var pt in controlPoints) {
				lr.SetPosition (i, pt);
				i++;
			}
		} else if (controlPoints.Count >= 3) {
			splitByFour ();
		}
	}

	// State machine that has takes left click to change to drawMode
	// While holding left click, right click creates a Node
	// releasing left click denotes last Node
	void UpdateState() {
		switch (state){
		case State.start:
			if (Input.GetMouseButtonDown (0)) {
				state = State.drawMode;
			}
			break;
		case State.drawMode:
			if (Input.GetMouseButtonUp (0)) {
				state = State.lastNode;
			} else if (Input.GetMouseButtonDown (1)) {
				state = State.makePoint;
			}
			break;
		case State.makePoint:
			if (Input.GetMouseButtonUp (0)) {
				state = State.lastNode;
			} else if (Input.GetMouseButtonUp(1)) {
				state = State.drawMode;
			} else {
				state = State.stillHeld;
			}
			break;
		case State.stillHeld:
			if (Input.GetMouseButtonUp (0)) {
				state = State.lastNode;
			} else if (Input.GetMouseButtonUp (1)) {
				state = State.drawMode;
			}
			break;
		case State.lastNode:
			state = State.start;
			break;
		}
	}

	// actions taken at each state
	void ProcessState() {
		switch (state) {
		case State.start:
			// TEMPORARY
			controlPoints.Clear ();
			break;
		case State.drawMode:
			// maybe shade the map
			// otherwise does nothing
			break;
		case State.makePoint:
			recordPoint ();
			break;
		case State.stillHeld:
			controlPoints.RemoveAt (controlPoints.Count-1);
			recordPoint ();
			break;
		case State.lastNode:
			recordPoint ();

			break;
		}
	}

	// Update is called once per frame
	// updates state machine
	// invokes DrawCurve()
	void Update () {
		UpdateState ();
		ProcessState ();
		DrawCurve ();
	}
}
