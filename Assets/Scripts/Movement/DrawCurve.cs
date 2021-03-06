﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Events;

[RequireComponent (typeof(LineRenderer), typeof(Deploy), typeof(CircleCollider2D))]
public class DrawCurve : MonoBehaviour {

	enum State { Start, DrawMode, MakePoint, StillHeld, LastNode };
	private State state = State.Start;

	[SerializeField]
	private float tension = 1.0f;
	// [SerializeField]
	// private float clickRadius = 1.0f;

	private int ptsInBetween = 40;
	private const float CONST_Z = -5.0f;

	private List<Vector3> controlPoints;
	private LineRenderer lr;
	private Deploy deploy;
	private CircleCollider2D circleCollider;
	private Matrix4x4 m;

	public UnityEvent OnEndDrawing;
	public UnityEvent OnStartDrawing;

	private void Awake() {
		if (OnEndDrawing == null) {
			OnEndDrawing = new UnityEvent ();
		}
		if (OnStartDrawing == null) {
			OnStartDrawing = new UnityEvent ();
		}
	}

	// Use this for initialization
	void Start () {
		state = State.Start;
		controlPoints = new List<Vector3>();
		lr = GetComponent<LineRenderer>();
		lr.useWorldSpace = true;
		lr.widthMultiplier = 0.75f;
		lr.material = new Material (Shader.Find ("Diffuse"));
		lr.startColor = Color.black;
		lr.endColor = Color.black;
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, transform.position);

		deploy = GetComponent<Deploy> ();
		circleCollider = GetComponent<CircleCollider2D> ();

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

	// takes location of mouse and stores as point
	void RecordPoint()
	{
		Vector3 v = (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		v.z = CONST_Z;
		controlPoints.Add (v);
	}

	// Takes list of controlPoints, adds ghost points, and sets the linerenderer's Points based on them
	void SplitByFour()
	{
		List<Vector3> fourpts = new List<Vector3>();
		// add ghost points to beginning and end of controlPoints
		controlPoints.Insert(0, 2*controlPoints[0]-controlPoints[1]);
		controlPoints.Add (2 * controlPoints [controlPoints.Count - 1] - controlPoints [controlPoints.Count - 2]);

		Vector3[] result = new Vector3[(controlPoints.Count-3) * ptsInBetween];
		lr.positionCount = (controlPoints.Count-3)*ptsInBetween;
		int pos = 0;
		foreach (Vector3 pt in controlPoints)
		{
			// shifting window of four points in fourpts list
			fourpts.Add (pt);
			if (fourpts.Count < 4)
			{
				continue;
			}
			else if (fourpts.Count > 4)
			{
				fourpts.RemoveAt (0);
			}

			// pass into spline
			Spline (fourpts, result, pos);
			pos += ptsInBetween;
		}
		lr.SetPositions (result);

		// remove ghost points
		controlPoints.RemoveAt(0);
		controlPoints.RemoveAt (controlPoints.Count - 1);
	}

	// take four nodes and add some number(ptsInBetween) of nodes between the middle two
	private void Spline(List<Vector3> fourpts, Vector3[] result, int startingPos)
	{
		// error check
		if (fourpts.Count != 4)
		{
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
			t += deltat;
			Vector4 vect = new Vector4 (1.0f, t, t*t, t*t*t);
			result [startingPos + i] =  temp.transpose * (0.5f *vect);
			result [startingPos + i].z = CONST_Z;
		}
	}

	// draws curve only when controlPoints exist(every state but start)
	// linear or catmull-rom spline
	void DisplayCurve()
	{
		// if below two points only displays the previous setup of controlPoints
		if (controlPoints.Count == 2)
		{
			int i = 0;
			lr.positionCount = controlPoints.Count;
			foreach (var pt in controlPoints)
			{
				lr.SetPosition (i, pt);
				i++;
			}
		}
		else if (controlPoints.Count >= 3)
		{
			SplitByFour ();
		}
	}

	// State machine that has takes left click to change to drawMode
	// While holding left click, right click creates a Node
	// releasing left click denotes last Node
	void UpdateState()
	{
		switch (state)
		{
		case State.Start:
			if (!deploy.isDeployed ()) {
				lr.enabled = false;
				lr.positionCount = 2;
				lr.SetPosition (0, transform.position);
				lr.SetPosition (1, transform.position);
			}
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mouse.z = CONST_Z;
				RaycastHit2D hit = Physics2D.Raycast (mouse, -Vector2.up, 0.1f, LayerMask.GetMask ("Unit"));
				if ( hit.collider != circleCollider)
				{
					// if mouse too far away from character
					break;
				}
				// Take selected unit as starting point
				controlPoints.Clear();
				controlPoints.Add (transform.position);
				state = State.DrawMode;
				lr.enabled = true;
				OnStartDrawing.Invoke ();
			}
			break;
		case State.DrawMode:
			if (Input.GetMouseButtonUp (0))
			{
				state = State.LastNode;
			}
			else if (Input.GetMouseButtonDown (1))
			{
				state = State.MakePoint;
			}
			break;
		case State.MakePoint:
			if (Input.GetMouseButtonUp (0))
			{
				state = State.LastNode;
			}
			else if (Input.GetMouseButtonUp(1))
			{
				state = State.DrawMode;
			}
			else 
			{
				state = State.StillHeld;
			}
			break;
		case State.StillHeld:
			if (Input.GetMouseButtonUp (0))
			{
				state = State.LastNode;
			}
			else if (Input.GetMouseButtonUp (1))
			{
				state = State.DrawMode;
			}
			break;
		case State.LastNode:
			state = State.Start;
			OnEndDrawing.Invoke ();
			break;
		}
	}

	// actions taken at each state
	void ProcessState()
	{
		switch (state)
		{
		case State.Start:
			break;
		case State.DrawMode:
			// maybe shade the map
			// otherwise does nothing
			break;
		case State.MakePoint:
			RecordPoint ();
			break;
		case State.StillHeld:
			controlPoints.RemoveAt (controlPoints.Count-1);
			RecordPoint ();
			break;
		case State.LastNode:
			RecordPoint ();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Profiler.BeginSample ("unit");
		UpdateState();
		ProcessState();
		DisplayCurve();
		Profiler.EndSample ();
	}
}
