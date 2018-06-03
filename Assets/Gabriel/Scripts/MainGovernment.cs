using UnityEngine;
using UnityEngine.Events;

public class MainGovernment : MonoBehaviour
{
    // A measure of the main governnment's Maximum Tyranny. It is a measure of the government's control over the
    // capitol city plus all of the other cities in the country. Half of this value comes from the Tyranny of the
    // capitol city, and half comes from summing the Tyranny values of all the other cities on the map.
    public float TotalTyranny;   

    // A measure of the main government's Current Tyranny. Whenever the goverment deploys units or takes actions,
    // it spends part of its maximum Tyranny. While its maximum Tyranny is reduced in this way, Current Tyranny is
    // used to make Tyranny-based calculations.
    private float currentTyranny;
    public float CurrentTyranny
    {
        get
        {
            return currentTyranny;
        }
        set
        {
            if (value > TotalTyranny)
            {
                value = TotalTyranny;
            }
            if (!Mathf.Approximately(currentTyranny, value))
            {
                OnChangeTyranny.Invoke();
            }
            currentTyranny = value;
        }
    }                      

	[SerializeField]
    private GameObject Patrol;
    public UnityEvent OnChangeTyranny;
    private AlertSystem alertSystem;
    // TaskList contains the list of cities, sorted by their state. The government enacts strategic interventions using this list.
    private PriorityQueue<City> prioritizedCities;
    
    private void Awake()
    {
        if (OnChangeTyranny == null)
        {
            OnChangeTyranny = new UnityEvent();
        }
        prioritizedCities = new PriorityQueue<City>();
    }

    private void Start()
    {
        this.alertSystem = ServiceLocator.Instance.GetAlertSystem();
        foreach (City c in ServiceLocator.Instance.GetCities())
        {
            c.OnEndRaiding.AddListener(delegate { RestoreTyranny(80); });
            c.OnEndPacifying.AddListener(delegate { RestoreTyranny(150); });
            c.OnEndInfiltrating.AddListener(delegate { RestoreTyranny(10); });
            c.OnEndEndingDemonstrations.AddListener(delegate { RestoreTyranny(250); });
            c.OnEndGenocide.AddListener(delegate { RestoreTyranny(300); });
        }
        InGameTime clock = ServiceLocator.Instance.GetClock();
        clock.OnDaily.AddListener( delegate 
        {
            Survey();
            Strategy();
            EndSurvey();
        });
        currentTyranny = TotalTyranny;
    }
    
    private void RestoreTyranny(float amount)
    {
        CurrentTyranny += amount;
    }

	private void Survey()
	{
		foreach (City city in ServiceLocator.Instance.GetCities()) 
		{
			prioritizedCities.Enqueue(city, (int)city.State);
		}
	}

    private void EndSurvey()
    {
        prioritizedCities.Clear();
    }
    
    // If a city is in an undesirable state, perform a action
    private void Strategy()
	{
		Debug.Log ("Daily strategy call");
		// Employ new strategy system using single TaskList. Foreach city in TaskList, switch statement based on city's state
		foreach (City city in prioritizedCities) 
		{
			// high_blackmarketeering, unrest, anti_govt_sentiments, anti_govt_demonstrations, civil_war
			switch (city.State) 
			{
				case City.CityState.HighBlackmarketeering:
                    // FIXME: #futurefeature: Once black marketeering is detected, launch a Timer object: if timer reaches thresshold,
                    // 		  then begin preparing for a raid. Give player time to cool down Heat meter.

                    // FIXME: Include a timer parameter in this conditional so that a raid is only launched if the timer has elapsed.
                    Debug.Log("This city is troublesome");
                    this.alertSystem.AlertPlayer("Government has detected black market activity, deploying patrols");
                    if (CurrentTyranny >= 80)
                    {
                        CurrentTyranny = CurrentTyranny - 80;
                        city.State = City.CityState.Raiding;
                        GameObject new_patrol_1 = (GameObject)Instantiate(this.Patrol);
                        GameObject new_patrol_2 = (GameObject)Instantiate(this.Patrol);
                        GameObject new_patrol_3 = (GameObject)Instantiate(this.Patrol);
                        new_patrol_1.GetComponent<PatrolAI>().SetHome(city.gameObject);
                        new_patrol_2.GetComponent<PatrolAI>().SetHome(city.gameObject);
                        new_patrol_3.GetComponent<PatrolAI>().SetHome(city.gameObject);
                    }
                    break;
				case City.CityState.Unrest:
                    if (CurrentTyranny >= 150)
                    {
                        // ...pacify the populace (spend Tyranny to lower Chaos)
                        city.State = City.CityState.Pacifying;
                        CurrentTyranny -= 150;
                        this.alertSystem.AlertPlayer("Government is pacifying angry citizens in " + city.CityName);
                    }
				    break;
				case City.CityState.AntiGovtSentiments:
					if (CurrentTyranny >= 10) 
					{
						city.State = City.CityState.Infiltrating;
                        CurrentTyranny -= 10;
                        this.alertSystem.AlertPlayer("Investigating anti-government sentiments in " + city.CityName);
                    }
				    break;
				case City.CityState.AntiGovtDemonstrations:
					if (CurrentTyranny >= 250) 
					{
						city.State = City.CityState.EndingDemonstrations;
                        CurrentTyranny -= 250;
                        this.alertSystem.AlertPlayer("Supressing anti-government demonstrations in " + city.CityName);
					}
				    break;
				case City.CityState.CivilWar:
					if (CurrentTyranny >= 300) 
					{
						city.State =  (City.CityState.Genocide);
                        CurrentTyranny -= 300;
                        this.alertSystem.AlertPlayer("Fighting violent civil uprising in " + city.CityName);
                    }
				    break;
				default:
                    //pacifying, infiltrating, ending_demonstrations, genocide, raiding, deploying, independent
                    break;
			}
		}
	}
}