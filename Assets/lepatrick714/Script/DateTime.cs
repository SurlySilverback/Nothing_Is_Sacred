using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTime : MonoBehaviour 
{
	private static System.DateTime moment; 
	private static float timePassed; 

	// Getters
	public static System.DateTime getDate() { 
		return DateTime.moment; 
	}

	// One second of Real time = One Minute of In Game Time 
	void UpdateMin() 
	{ 
		DateTime.moment.AddMinutes(1);
	}

	void UpdateClockOnScreen() 
	{ 

	}

	
	void Awake()
	{
		DateTime.moment = new System.DateTime(2018, 10, 12, 0, 0, 0);
	}

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{	
		DateTime.timePassed += Time.deltaTime; 
		if (DateTime.timePassed >= 1) 
		{ 
			UpdateMin(); 
			timePassed -= 1; 
			UpdateClockOnScreen(); 
		}
	}
}
