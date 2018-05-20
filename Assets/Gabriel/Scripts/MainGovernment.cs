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

	//private void Strategy ();					// Strategic logic for the Main Government.

	//private void Pacification ( City c );		// ACTION: When a city's Chaos rises above the Chaos threshhold, the government will deploy peacekeeping forces
												// to quell riots and arrest malcontents. This will cost Tyranny and reduce the Chaos level of the target City.

	//private void Infiltrate ( City c );			// ACTION: When a city's supply of Ideas grows beyond a certain threshhold, the government will deploy secret
												// police to find and execute the malcontents. This process will take time, but it will eventually lead to the
												// reduction of a City's supply of Ideas.

	//private void Genocide ( City c );			// #futurefeature: When a city's Chaos and Supply of Ideas are both high, then the government will implement a
												// program of mass genocide to end Idea-driven dissent.

	//private void DeployPatrols ( City c );		

	//private void OrderRaid ( City c );

	[SerializeField] private MapGraph graph;	// A structure which holds a MapGraph representation of the cities and roads. SerializeField allows us to
												// click and drag a Unity prefab from the scene list into the object so we can populate this with a MapGraph
												// automatically.


	// TaskList contains the list of cities, sorted by their state. The government enacts strategic interventions using this list.
	private PriorityQueue <City> TaskList;

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
					
					// Deploy multiple patrols
					if (CurrentTyranny >= 50) 
					{
						CurrentTyranny = CurrentTyranny - 50;
						c.SetState (City.CityState.raiding);
						// SYSTEM MESSAGE: Government is investigating black market activity in City.Name.
					}

				break;

				// If the city is in a state of unrest (high Chaos)...
				case City.CityState.unrest:

					if (CurrentTyranny >= 150) 
					{
						c.SetState (City.CityState.pacifying);	// ...pacify the populace (spend Tyranny to lower Chaos)
						CurrentTyranny = CurrentTyranny - 150;
						// SYSTEM MESSAGE: Government is pacifying the angry citizens of City.Name.
					}

				break;
			
				case City.CityState.anti_govt_sentiments:

					if (CurrentTyranny >= 10) 
					{
						c.SetState (City.CityState.infiltrating);
						CurrentTyranny = CurrentTyranny - 10;
						// SYSTEM MESSAGE: Government is investigating anti-government sentiments in City.Name.
					}

				break;

				case City.CityState.anti_govt_demonstrations:
				
					if (CurrentTyranny >= 250) 
					{
						c.SetState (City.CityState.ending_demonstrations);
						CurrentTyranny = CurrentTyranny - 250;
						// SYSTEM MESSAGE: Government is suppressing anti-government demmonstrations in City.Name.
					}

				break;

				case City.CityState.civil_war:

					if (CurrentTyranny >= 300) 
					{
						c.SetState (City.CityState.genocide);
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
		
			c.OnEndRaiding.AddListener ( delegate{ RestoreTyranny(50); });
			c.OnEndPacifying.AddListener ( delegate{ RestoreTyranny(150); });
			c.OnEndInfiltrating.AddListener ( delegate{ RestoreTyranny (10); });
			c.OnEndEndingDemonstrations.AddListener ( delegate{ RestoreTyranny (250); });
			c.OnEndGenocide.AddListener ( delegate{ RestoreTyranny (300); });
		}

	}
	
	// Update is called once per frame
	void Update () {

		// SurveyChaos();
		// Execute Chaos-reducing actions.

		// SurveyHeat();
		// Execute anti-player actios.
	}
}
