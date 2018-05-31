using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGovernment : MonoBehaviour {

	// FIXME: MAY NOT BE NEEDED NOW
	//private Dictionary <City, CityState> listOfCityStates;		// A list of the states of all the cities in the game. This is how the government determines
																	// if a city has already been instructed to raid, infiltrate, pacify, etc, so that redundant
																	// orders are not issued.

	public float MaxTyranny{ get; set; }		// A measure of the main governnment's Maximum Tyranny. It is a measure of the government's control over the
												// capitol city plus all of the other cities in the country. Half of this value comes from the Tyranny of the
												// capitol city, and half comes from summing the Tyranny values of all the other cities on the map.

	public float CurrentTyranny{ get; set; }	// A measure of the main government's Current Tyranny. Whenever the goverment deploys units or takes actions,
												// it spends part of its maximum Tyranny. While its maximum Tyranny is reduced in this way, Current Tyranny is
												// used to make Tyranny-based calculations.

	[SerializeField] private MapGraph graph;	// A structure which holds a MapGraph representation of the cities and roads. SerializeField allows us to
												// click and drag a Unity prefab from the scene list into the object so we can populate this with a MapGraph
												// automatically.

	[SerializeField] GameObject Patrol;


    // TaskList contains the list of cities, sorted by their state. The government enacts strategic interventions using this list.
    private PriorityQueue<City> TaskList = new PriorityQueue<City>();

	private void Survey()
	{
		foreach (City c in graph) 
		{
			TaskList.Enqueue (c, (int)c.GetState ());
		}
	}

	private void Strategy()
	{
		// Employ new strategy system using single TaskList. Foreach city in TaskList, switch statement based on city's state
		foreach (City c in TaskList) 
		{
			// high_blackmarketeering, unrest, anti_govt_sentiments, anti_govt_demonstrations, civil_war
			switch (c.GetState ()) 
			{
				case City.CityState.high_blackmarketeering:

					// FIXME: #futurefeature: Once black marketeering is detected, launch a Timer object: if timer reaches thresshold,
					// 		  then begin preparing for a raid. Give player time to cool down Heat meter.

					// System message: Government has detected black market activity, deploying patrols...
					
					// FIXME: Include a timer parameter in this conditional so that a raid is only launched if the timer has elapsed.
					if (CurrentTyranny >= 80) 
					{
						CurrentTyranny = CurrentTyranny - 80;
						c.SetState (City.CityState.raiding);
						c.SetFlag (new Color (0,176,9));	// dark green
						// SYSTEM MESSAGE: Government is investigating black market activity in City.Name.

						GameObject new_patrol_1 = (GameObject)Instantiate(Patrol);
						GameObject new_patrol_2 = (GameObject)Instantiate(Patrol);
						GameObject new_patrol_3 = (GameObject)Instantiate(Patrol);
						new_patrol_1.GetComponent<PatrolAI> ().SetHome(c.gameObject);
						new_patrol_2.GetComponent<PatrolAI> ().SetHome(c.gameObject);
						new_patrol_3.GetComponent<PatrolAI> ().SetHome(c.gameObject);
					}

				break;

				case City.CityState.raiding:

					// Add a timer that keeps track of time elapsed since the City was assigned the raiding state.
					// Once this timer has elapsed, the player's inventory in that city should be cleared out.

				break;

				// If the city is in a state of unrest (high Chaos)...
				case City.CityState.unrest:

					if (CurrentTyranny >= 150) 
					{
						c.SetState (City.CityState.pacifying);	// ...pacify the populace (spend Tyranny to lower Chaos)
						c.SetFlag(new Color(14,0,189)); // dark blue
						CurrentTyranny = CurrentTyranny - 150;
						// SYSTEM MESSAGE: Government is pacifying the angry citizens of City.Name.
					}

				break;
			
				case City.CityState.anti_govt_sentiments:

					if (CurrentTyranny >= 10) 
					{
						c.SetState (City.CityState.infiltrating);
						c.SetFlag (new Color (178,0,229)); // dark purple
						CurrentTyranny = CurrentTyranny - 10;
						// SYSTEM MESSAGE: Government is investigating anti-government sentiments in City.Name.
					}

				break;

				case City.CityState.anti_govt_demonstrations:
				
					if (CurrentTyranny >= 250) 
					{
						c.SetState (City.CityState.ending_demonstrations);
						c.SetFlag (new Color (229, 99, 0));	// dark orange
						CurrentTyranny = CurrentTyranny - 250;
						// SYSTEM MESSAGE: Government is suppressing anti-government demmonstrations in City.Name.
					}

				break;

				case City.CityState.civil_war:

					if (CurrentTyranny >= 300) 
					{
						c.SetState (City.CityState.genocide);
						c.SetFlag (new Color(200,0,0));
						CurrentTyranny = CurrentTyranny - 300;
						// SYSTEM MESSAGE: Government is fighting a violent civil uprising in City.Name.
					}

				break;

				default:

				//pacifying, infiltrating, ending_demonstrations, genocide, raiding, deploying, independent

				break;
			}
		}

		ClearQueue ();
	}

	private void ClearQueue()
	{
		while (TaskList.Count > 0) 
		{
			TaskList.Dequeue ();
		}
	}

	private void RestoreTyranny(float amount){
	
		CurrentTyranny += amount;
	}

	// Use this for initialization
	void Start () {

		foreach (City c in graph) {
		
			c.OnEndRaiding.AddListener ( delegate{ RestoreTyranny(80); });
			c.OnEndPacifying.AddListener ( delegate{ RestoreTyranny(150); });
			c.OnEndInfiltrating.AddListener ( delegate{ RestoreTyranny (10); });
			c.OnEndEndingDemonstrations.AddListener ( delegate{ RestoreTyranny (250); });
			c.OnEndGenocide.AddListener ( delegate{ RestoreTyranny (300); });
		}
		DateTime dt = FindObjectOfType<DateTime> ();
		dt.onDaily.AddListener (delegate {Survey(); Strategy();});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
