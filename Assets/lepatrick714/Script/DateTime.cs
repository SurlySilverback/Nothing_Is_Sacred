using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public class DateTime : MonoBehaviour 
{
	private static System.DateTime moment; 
	private static float timePassed; 

	public UnityEvent onDay;   // 6AM 
	public UnityEvent onNight; // 6PM 
	public UnityEvent onDaily; // Day begins midnight 

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

	void CheckCallBacks()
	{ 
		// Checking for 6AM 
		if(DateTime.moment.Hour == 5 || DateTime.moment.Minute == 0)
		{
			onDay.Invoke(); 
		}
		if(DateTime.moment.Hour == 17 || DateTime.moment.Minute == 0) 
		{ 
			onNight.Invoke(); 
		}
		if(DateTime.moment.Hour == 0 || DateTime.moment.Minute == 0) 
		{
			onDaily.Invoke(); 
		}
	}

	void SetClockOnScreen() 
	{ 
		displayDate.text = "Time: " + DateTime.moment.Year.ToString() + " " + DateTime.moment.Month.ToString() + " " + DateTime.moment.Day.ToString() + " " + DateTime.moment.Hour.ToString() + " : " + DateTime.moment.Minute.ToString();
		Debug.Log("CALLING: " + DateTime.moment.Date.ToString()); 
	}

	
	void Awake()
	{
		if(onDay == null) { 
			onDay = new UnityEvent(); 
		}
		if(onNight == null) { 
			onNight = new UnityEvent(); 
		}
		if(onDaily == null) { 
			onDaily = new UnityEvent(); 
		}


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
			CheckCallBacks(); 
			SetClockOnScreen(); 
		}
	}
}
