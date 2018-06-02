using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 
using TMPro;
using System;
using System.Linq;

public class DateTime : MonoBehaviour 
{
	private static System.DateTime moment; 
	private static float timePassed;
    
    [SerializeField]
    private TextMeshProUGUI displayDate;
    [SerializeField]
    private Image mapOverlay;
    [SerializeField]
    private Color night;
    [SerializeField]
    private AnimationCurve dayNightTransition;

    public UnityEvent onDay;   // 6AM 
	public UnityEvent onNight; // 6PM 
	public UnityEvent onDaily; // Day begins midnight 

	// Getters
	public static System.DateTime getDate() { 
		return DateTime.moment; 
	}

	// One second of Real time = One Minute of In Game Time 
	void UpdateMin() 
	{ 
		DateTime.moment = DateTime.moment.AddMinutes(1);
	}

    private IEnumerator StartAnimation(Color start, Color end)
    {
        float timeStep = .01f;
        float timeFrame = dayNightTransition[dayNightTransition.length - 1].time;
        for (float animationTime = 0; animationTime < timeFrame; animationTime += timeStep)
        {
            mapOverlay.color = Color.Lerp(start, end, dayNightTransition.Evaluate(animationTime / timeFrame));
            yield return new WaitForSeconds(timeStep);
        }
    }

	void CheckCallBacks()
	{
        // Checking for 6AM 
        if (DateTime.moment.Hour == 5 && DateTime.moment.Minute == 0)
		{
			onDay.Invoke(); 
		}
        if (DateTime.moment.Hour == 17 && DateTime.moment.Minute == 0) 
		{ 
			onNight.Invoke(); 
		}
		if(DateTime.moment.Hour == 0 && DateTime.moment.Minute == 0) 
		{
			onDaily.Invoke(); 
		}
	}

	void SetClockOnScreen() 
	{ 
		List<string> monthNames = new List<string>(new string[]{"JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"});
		string hourStr = DateTime.moment.Hour.ToString();
		if(hourStr.Length == 1){
			hourStr = "0" + hourStr;
		}
		string minuteStr = DateTime.moment.Minute.ToString();
		if(minuteStr.Length == 1){
			minuteStr = "0" + minuteStr;
		}
		int temp;
		Int32.TryParse(DateTime.moment.Month.ToString(), out temp);
		string monthStr = monthNames[temp];

		string timeStr = DateTime.moment.Year.ToString() + ", " + monthStr + " " + DateTime.moment.Day.ToString() + ", " + hourStr + ":" + minuteStr;
		displayDate.text = timeStr;
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

        Color day = night;
        day.a = 0;


        onDay.AddListener(delegate { StartCoroutine(StartAnimation(night, day)); });
        onNight.AddListener(delegate { StartCoroutine(StartAnimation(day, night)); });
        DateTime.moment = new System.DateTime(1957, 10, 12, 5, 0, 0);
        mapOverlay.color = day;
	}

	// Use this for initialization
	void Start () 
	{
		SetClockOnScreen(); 
	}

    // mapOverlay.color = Color.Lerp(day, night, dayNightTransition.Evaluate(animationTime / timeFrame));
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
