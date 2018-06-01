using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameTime : MonoBehaviour 
{
	private System.DateTime moment; 
	private float timePassed;
    
    [SerializeField]
    private Text displayDate;

    public UnityEvent OnDay;   // 6AM 
	public UnityEvent OnNight; // 6PM 
	public UnityEvent OnDaily; // Day begins midnight 

	// One second of Real time = One Minute of In Game Time 
	void UpdateMin() 
	{ 
		moment = moment.AddMinutes(1);
	}

	void CheckCallBacks()
	{
        // Checking for 6AM 
        if (moment.Hour == 5 && moment.Minute == 0)
		{
			OnDay.Invoke(); 
		}
        if (moment.Hour == 17 && moment.Minute == 0) 
		{ 
			OnNight.Invoke(); 
		}
		if(moment.Hour == 0 && moment.Minute == 0) 
		{
			OnDaily.Invoke(); 
		}
	}

	void SetClockOnScreen() 
	{ 
		displayDate.text = "Time: " + moment.Year.ToString() + " " + moment.Month.ToString() + " " + moment.Day.ToString() + " " + moment.Hour.ToString() + " : " + moment.Minute.ToString();
	}

	void Awake()
	{
		if(OnDay == null) { 
			OnDay = new UnityEvent(); 
		}
		if(OnNight == null) { 
			OnNight = new UnityEvent(); 
		}
		if(OnDaily == null) { 
			OnDaily = new UnityEvent(); 
		}
        moment = new System.DateTime(1957, 10, 12, 5, 0, 0);
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
		timePassed += Time.deltaTime; 
		if (timePassed >= 1) 
		{ 
			timePassed -= 1; 
			UpdateMin(); 
			CheckCallBacks(); 
			SetClockOnScreen(); 
		}
	}
}
