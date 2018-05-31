using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

/* 
 * Directions to use:
 * 1. Drag to sprite to attach script
 * 2. Drag sprite into "selectedUnit" field inside the catmull-rom script's inspector
 * 3. Drag sprite into "lr" field inside the catmull-rom script's inspector
*/
[RequireComponent (typeof(LineRenderer))]
public class UnitController : MonoBehaviour
{
	enum State { Start, DrawMode, MakePoint, StillHeld, LastNode };
    private State state = State.Start;

    [SerializeField]
	private float tension = 1.0f;
    [SerializeField]
    private float clickRadius = 5.0f;

	private int ptsInBetween = 40;
	private const float CONST_Z = -5.0f;
	
	private List<Vector3> controlPoints;
	private LineRenderer lr;
	private Matrix4x4 m;
	private int nextlrPoint;

	// Use this for initialization of linerenderer and tension
	void Start()
    {
		state = State.Start;
		controlPoints = new List<Vector3>();
		lr = GetComponent<LineRenderer>();
		lr.useWorldSpace = true;
		lr.widthMultiplier = 1.0f;
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, transform.position);

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

	// takes location of mouse and stores as point
	void RecordPoint()
    {
		Vector3 v = (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		v.z = CONST_Z;
		controlPoints.Add (v);
	}

	// draws curve only when controlPoints exist(every state but start)
	// linear or catmull-rom spline
	void DrawCurve()
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
			    // Temporary for mult characters
			    if (Input.GetMouseButtonDown(0))
                {
				    Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				    mouse.z = CONST_Z;
				    if (Vector3.Distance (mouse, transform.position) > this.clickRadius)
                    {
					    // if mouse too far away from character
					    break;
				    }
				    // Take selected unit as starting point
				    controlPoints.Clear();
				    nextlrPoint = 0;
				    controlPoints.Add (transform.position);
				    state = State.DrawMode;
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
			    break;
		}
	}

	// at position, find the terrain coef of the tile under that position
	float GetTerrainCoef(Vector3 position)
    {
		float terrainCoef = 1.0f;
		RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up);
		if (hit)
        {
			switch (LayerMask.LayerToName(hit.transform.gameObject.layer))
            {
			    case "Land":
				    terrainCoef = 1.0f;
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
			    case "City":
				    terrainCoef = 0.5f;
				    break;
			    case "Path":
				    terrainCoef = 0.5f;
				    break;
			}
		}
		return terrainCoef;
	}

    public void MoveUnit(float speed)
    {

    }

    public void ResetPath()
    {

    }

	// if called, have the selected unit transform along the linerenderer
	public void MoveUnit()
    {
		float unitSpeed = 10.0f;
		float dist = 0.0f;
		float terrainCoef = 1.0f;

		unitSpeed *= Time.deltaTime;
		Vector3 currPos = transform.position;
		Vector3 finalmov = Vector3.zero;

		if (nextlrPoint >= lr.positionCount)
        {
			return;
		}

		// Get terrain speed
		terrainCoef = GetTerrainCoef(currPos);

		dist = Vector3.Distance(currPos, lr.GetPosition(nextlrPoint)) * terrainCoef;
		while (dist < unitSpeed)
        {
			// not finished moving
			currPos = lr.GetPosition(nextlrPoint);
			++nextlrPoint;
			if (nextlrPoint >= lr.positionCount)
            {
				return;
			}
			unitSpeed -= dist;
			terrainCoef = GetTerrainCoef (lr.GetPosition (nextlrPoint));
			dist = Vector3.Distance (currPos, lr.GetPosition (nextlrPoint)) * terrainCoef;
		}
		if (nextlrPoint >= lr.positionCount)
        {
			return;
		}

		// moved to some point between the currPos
		// and the next point on the line renderer
		finalmov = (lr.GetPosition(nextlrPoint) - currPos) * (unitSpeed / dist);
		transform.position = currPos + finalmov;
		
	}

	// actions taken at each state
	void ProcessState()
    {
		switch (state)
        {
		    case State.Start:
			    // TEMPORARY
			    MoveUnit ();
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
	// updates state machine
	// invokes DrawCurve()
	private void Update()
    {
		UpdateState();
		ProcessState();
		DrawCurve();
	}
}
