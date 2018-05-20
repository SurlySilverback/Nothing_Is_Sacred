using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGovernment : MonoBehaviour {

	// FIXME: MAY NOT BE NEEDED NOW
	//private Dictionary <City, CityState> listOfCityStates;		// A list of the states of all the cities in the game. This is how the government determines
																	// if a city has already been instructed to raid, infiltrate, pacify, etc, so that redundant
																	// orders are not issued.
	// STATES: The states the cities can be in.
	enum CityState{normal, high_blackmarketeering, unrest, anti_govt_sentiments, anti_govt_demonstrations, civil_war,
				   pacifying, infiltrating, genocide, raiding, deploying, independent};


	public float MaxTyranny{ get; set; }		// A measure of the main governnment's Maximum Tyranny. It is a measure of the government's control over the
												// capitol city plus all of the other cities in the country. Half of this value comes from the Tyranny of the
												// capitol city, and half comes from summing the Tyranny values of all the other cities on the map.

	public float CurrentTyranny{ get; set; }	// A measure of the main government's Current Tyranny. Whenever the goverment deploys units or takes actions,
												// it spends part of its maximum Tyranny. While its maximum Tyranny is reduced in this way, Current Tyranny is
												// used to make Tyranny-based calculations.

	private void Strategy ();					// Strategic logic for the Main Government.

	private void Pacification ( City c );		// ACTION: When a city's Chaos rises above the Chaos threshhold, the government will deploy peacekeeping forces
												// to quell riots and arrest malcontents. This will cost Tyranny and reduce the Chaos level of the target City.

	private void Infiltrate ( City c );			// ACTION: When a city's supply of Ideas grows beyond a certain threshhold, the government will deploy secret
												// police to find and execute the malcontents. This process will take time, but it will eventually lead to the
												// reduction of a City's supply of Ideas.

	private void Genocide ( City c );			// #futurefeature: When a city's Chaos and Supply of Ideas are both high, then the government will implement a
												// program of mass genocide to end Idea-driven dissent.

	private void DeployPatrols ( City c );		

	private void OrderRaid ( City c );

	[SerializeField] private MapGraph graph;	// A structure which holds a MapGraph representation of the cities and roads. SerializeField allows us to
												// click and drag a Unity prefab from the scene list into the object so we can populate this with a MapGraph
												// automatically.


	// TaskList contains the list of cities, sorted by their state. The government enacts strategic interventions using this list.
	private PriorityQueue <City> TaskList;

	private void Survey()
	{
		foreach (City c in graph) 
		{
			TaskList.Enqueue (c, c.GetState ());
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
				case CityState.high_blackmarketeering:

					// FIXME: #futurefeature: Once black marketeering is detected, launch a Timer object: if timer reaches thresshold,
					// 		  then begin preparing for a raid. Give player time to cool down Heat meter.

					// System message: Government has detected black market activity, deploying patrols...
					
					// Deploy multiple patrols

					c.SetState (CityState.raiding);

				break;

				// If the city is in a state of unrest (high Chaos)...
				case CityState.unrest:

					c.SetState (CityState.pacifying);	// ...pacify the populace (spend Tyranny to lower Chaos)

				break;
			
				case CityState.anti_govt_sentiments:

					c.SetState (CityState.infiltrating);

				break;

				case CityState.anti_govt_demonstrations:
				
					c.SetState (CityState.pacifying);

				break;

				case CityState.civil_war:

					c.SetState (CityState.genocide);

				break;

				default:

				//pacifying, infiltrating, genocide, raiding, deploying, independent

				break;
			}
		}

		ClearQueue ();
	}

	/* OLD CODE
	private void Strategy()
	{
		// Chaos Interventions
		foreach( City c in ChaosList ) 
		{
			City current_city = ChaosList.Peek ();

			// PACIFICATION: If Chaos > 399 and the city is not currently being pacified...
			if (current_city.GetChaos () > 399 && listOfCityStates [current_city] == CityState.normal) 
			{
				listOfCityStates [current_city] = CityState.pacification;	// Change the city's state to reflect the pacification order.
				Pacification (current_city);								// Issue order to deploy peacekeeping forces and pacify the population.
				ChaosList.Dequeue();
				continue;
			}

			// INFILTRATE: If Ideas supply > 29 and city is not currently infiltrated...
			if (current_city.goodsToSupply["Ideas"] > 29 && listOfCityStates [current_city] == CityState.normal) 
			{
				listOfCityStates [current_city] = CityState.infiltrate;		// Change the city's state to reflect the infiltration order.
				Infiltrate (current_city);									// Issue order to infiltrate the city and weed out dissident thinkers.
				ChaosList.Dequeue();
				continue;
			}
		}

		// Heat Interventions
		foreach (City c in HeatList) 
		{
			City current_city = HeatList.Peek ();

			// ORDER RAID: If Heat > 599 and the city is not currently being staged for a raid...
			if (current_city.GetHeat () > 599 && listOfCityStates [current_city] == CityState.normal) 
			{
				listOfCityStates [current_city] = CityState.orderraid;	// Change the city's state to reflect the raid order.
				OrderRaid (current_city);								// Begin staging a raid of the city.
				ChaosList.Dequeue();
				continue;
			}

			// DEPLOY PATROLS: If Heat > 199 and the city is not currently under a special state...
			if (current_city.GetHeat () > 199 && listOfCityStates [current_city] == CityState.normal) 
			{
				DeployPatrols (current_city);							// Deploy patrols in the surrounding area.
				ChaosList.Dequeue();
				continue;
			}
		}

		ClearQueues ();
	}*/


	private void ClearQueue()
	{
		while (TaskList.Count > 0) 
		{
			TaskList.Dequeue ();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// SurveyChaos();
		// Execute Chaos-reducing actions.

		// SurveyHeat();
		// Execute anti-player actios.
	}
}
