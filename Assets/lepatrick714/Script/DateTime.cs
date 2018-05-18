using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DateTime : MonoBehaviour 
{
	private static System.DateTime moment; 
	private static float timePassed; 

	public Text displayDate; 


	// Getters
	public static System.DateTime getDate() { 
		return DateTime.moment; 
	}

	// One second of Real time = One Minute of In Game Time 
	void UpdateMin() 
	{ 
		DateTime.moment = DateTime.moment.AddMinutes(1);
	}

	void SetClockOnScreen() 
	{ 
		displayDate.text = "Time: " + DateTime.moment.Year.ToString() + " " + DateTime.moment.Month.ToString() + " " + DateTime.moment.Day.ToString() + " " + DateTime.moment.Hour.ToString() + " : " + DateTime.moment.Minute.ToString();
		Debug.Log("CALLING: " + DateTime.moment.Date.ToString()); 
	}

	
	void Awake()
	{
		DateTime.moment = new System.DateTime(2018, 10, 12, 0, 0, 0);
	}

	// Use this for initialization
	void Start () 
	{
		SetClockOnScreen(); 
	}

	// Update is called once per frame
	void Update () 
	{	
		DateTime.timePassed += Time.deltaTime; 
		if (DateTime.timePassed >= 1) 
		{ 
			timePassed -= 1; 
			UpdateMin(); 
			SetClockOnScreen(); 
		}
	}
}
