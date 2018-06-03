using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class InGameTime : MonoBehaviour 
{
    public const float MinToIRL = 1f;
    public const float HourToIRL = 60f;
    public const float DayToIRL = 1440f;
    public const float WeekToIRL = 10080f;

	private System.DateTime moment;
	private float timePassed;

    [SerializeField]
    private TextMeshProUGUI displayDate;
    public UnityEvent OnHour;
    public UnityEvent OnDay;   // 6AM 
	public UnityEvent OnNight; // 6PM 
	public UnityEvent OnDaily; // Day begins midnight 

	void Awake()
	{
        Assert.IsNotNull(displayDate);
        if (OnHour == null)
        {
            OnHour = new UnityEvent();
        }
		if(OnDay == null)
        { 
			OnDay = new UnityEvent(); 
		}
		if(OnNight == null)
        { 
			OnNight = new UnityEvent(); 
		}
		if(OnDaily == null)
        { 
			OnDaily = new UnityEvent(); 
		}
        moment = new System.DateTime(1957, 10, 12, 5, 0, 0);
    }

    private void Start()
    {
        SetClockOnScreen();
    }

    void SetClockOnScreen() 
	{ 
		List<string> monthNames = new List<string>(new string[]{"JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"});
		string hourStr = moment.Hour.ToString();
		if(hourStr.Length == 1){
			hourStr = "0" + hourStr;
		}
		string minuteStr = moment.Minute.ToString();
		if(minuteStr.Length == 1){
			minuteStr = "0" + minuteStr;
		}
		int temp;
		Int32.TryParse(moment.Month.ToString(), out temp);
		string monthStr = monthNames[temp];

		string timeStr = moment.Year.ToString() + ", " + monthStr + " " + moment.Day.ToString() + ", " + hourStr + ":" + minuteStr;
		displayDate.text = timeStr;
	}
    
    private void Update() 
	{	
		timePassed += Time.deltaTime; 
		if (timePassed >= 1) 
		{ 
			timePassed -= 1;
            moment = moment.AddMinutes(1);
            CheckCallBacks(); 
			SetClockOnScreen(); 
		}
	}

    void CheckCallBacks()
    {
        if (moment.Minute == 0)
        {
            OnHour.Invoke();
        }
        if (moment.Hour == 5 && moment.Minute == 0)
        {
            OnDay.Invoke();
        }
        else if (moment.Hour == 17 && moment.Minute == 0)
        {
            OnNight.Invoke();
        }
        else if (moment.Hour == 0 && moment.Minute == 0)
        {
            OnDaily.Invoke();
        }
    }
}