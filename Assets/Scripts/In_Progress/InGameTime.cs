using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class InGameTime : MonoBehaviour 
{
    public const float MinToIRL = 1f;
    public const float HourToIRL = 60f;
    public const float DayToIRL = 1440f;
    public const float WeekToIRL = 10080f;

	private System.DateTime moment;
	private float timePassed;

    [SerializeField]
    private Text displayDate;
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

    private void SetClockOnScreen()
    {
        displayDate.text = "Time: " + moment.Year.ToString() + " " + moment.Month.ToString() + " " + moment.Day.ToString() + " " + moment.Hour.ToString() + " : " + moment.Minute.ToString();
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