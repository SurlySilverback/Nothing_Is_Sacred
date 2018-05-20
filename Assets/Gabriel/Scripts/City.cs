using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class City : MonoBehaviour { 

	// STATES: The states the cities can be in.
	public enum CityState{normal, high_blackmarketeering, unrest, anti_govt_sentiments, anti_govt_demonstrations, civil_war,
						   pacifying, infiltrating, ending_demonstrations, genocide, raiding, independent};

	// Unity Events for Observer Pattern: calls MainGovernment script to restore spent Tyranny when strategy actions are complete.
	public UnityEvent OnEndPacifying;
	public UnityEvent OnEndInfiltrating;
	public UnityEvent OnEndEndingDemonstrations;
	public UnityEvent OnEndGenocide;
	public UnityEvent OnEndRaiding;

	// NAME
	[SerializeField] private string name;
	public string GetName(){return name;}

	// CHAOS
	[SerializeField] private float chaos; 
	public float GetChaos(){return chaos;}
	public void SetChaos(float new_val)
	{
		chaos = new_val;
	}

	// HEAT
	[SerializeField] private float heat; 
	public float GetHeat(){return heat;}
	public void SetHeat(float new_val)
	{
		heat = new_val;
	}

	// STATE
	private CityState state;
	public CityState GetState(){return state;}
	public void SetState(CityState new_state)
	{
		state = new_state;
	}

	[SerializeField] private float foodProductionRate; 
	[SerializeField] private float waterProductionRate; 
	[SerializeField] private float population; 

	private float timeCapture;		// Used along with intervention_timer to measure time passed since intervention events were initiated.

	// TODO: Make this a Timer Class which increments its own value using time.deltaTime()
	[SerializeField] float intervention_timer;
	public float GetTimer(){return intervention_timer;}
	public void SetTimer(float new_time)
	{
		intervention_timer = new_time;
	}


	// Cardinal Goods
	[SerializeField] private List<string> goods = new List<string>(){ "Drugs", "Exotics", "Food", 
																	  "Fuel", "Ideas", "Medicine", 
																	  "People", "Textiles", "Water", 
																	  "Weapons" };
	
	// Contains the city's supply of each Cardinal Good.
	[SerializeField] private List<int> supplyOfGoods = new List<int>(){ 0, 0, 0, 
																		 0, 0, 0, 
																		 0, 0, 0, 
																 		0 };

	// Contains the current prices of all of the Cardinal Goods in the city.
	[SerializeField] private List<float> pricesOfGoods = new List<float>(){ 0, 0, 0, 
																			0, 0, 0, 
																			0, 0, 0, 
																			0 };


	// Method for referencing the current supply of a Good.
	public Dictionary<string, int> goodsToSupply;

	// Method for referencing the current price of a Good.
	public Dictionary<string, float> goodsToPrices;


	// Inventory of the People
	[SerializeField] private List<string> peopleGoods = new List<string>(){ "Drugs", "Exotics", "Food", 
																	  "Fuel", "Ideas", "Medicine", 
																	  "People", "Textiles", "Water", 
																	  "Weapons" };
	[SerializeField] private List<int> peopleValues = new List<int>(){ 0, 0, 0, 
																	   0, 0, 0, 
																	   0, 0, 0, 
																	   0 };
	public Dictionary<string, int> peopleGoodsToValues;


	// Inventory of the Gov't
	[SerializeField] private List<string> govGoods = new List<string>(){ "Drugs", "Exotics", "Food", 
																	  "Fuel", "Ideas", "Medicine", 
																	  "People", "Textiles", "Water", 
																	  "Weapons" };
	[SerializeField] private List<int> govValues = new List<int>(){ 0, 0, 0, 
																	0, 0, 0, 
																	0, 0, 0, 
																	0 };
	private Dictionary<string, int> govGoodsToValues;


	private void DetermineState()
	{
		// Update storehouses here using ( 1/1440th of consumption rate * time.deltaTime);
		// FIXME: Hardcoding consumption rates for project demo. #fututefeature: Implement a script to handle consumption rate based on population size.


		timeCapture += Time.deltaTime;

		if (timeCapture >= 1) 
		{
			timeCapture--;
			intervention_timer++;
		}

		switch(state)
		{
			case CityState.pacifying:

				// Two days
				if (intervention_timer == 2880)
				{
					intervention_timer = 0;
					state = CityState.normal;
					OnEndPacifying.Invoke();
				}
			break;

			case CityState.infiltrating:

				// One week
				if (intervention_timer == 10800)
				{
					intervention_timer = 0;
					state = CityState.normal;
					OnEndInfiltrating.Invoke();
				}
			break;

			case CityState.ending_demonstrations:

			// Two days
			if (intervention_timer == 2880)
			{
				intervention_timer = 0;
				state = CityState.normal;
				OnEndInfiltrating.Invoke();
			}
			break;

			case CityState.genocide:

				// One week
				if (intervention_timer == 10800)
				{
					intervention_timer = 0;
					state = CityState.normal;
					OnEndGenocide.Invoke();
				}
			break;

			case CityState.raiding:

				// Three days
				if (intervention_timer == 4320)
				{
					intervention_timer = 0;
					state = CityState.normal;
					OnEndRaiding.Invoke();
				}
			break;

			case CityState.independent:

				state = CityState.normal;

			break;

			default:

				// If values are below threshhold, status is normal.
				if (chaos <= 599 && heat <= 399)
					state = CityState.normal;

				// If chaos is high...
				else if (chaos > 599) 
				{
					// ...and if Idea supply is high...
					if (goodsToSupply ["Ideas"] >= 299) {
						state = CityState.anti_govt_demonstrations;		// ...anti-goverment demonstrations.
					}
					// ...and if Weapon supply is high...
					if (goodsToSupply ["Weapons"] >= 299) {
						state = CityState.civil_war;					// ...civil war.
					}

					state = CityState.unrest;
				} 

				// If heat is high...
				else if (heat > 399) {
					state = CityState.high_blackmarketeering;	// ...high black marketeering.
				}

				// If Idea supply is high...
				else if (goodsToSupply ["Ideas"] >= 299) 
				{
					state = CityState.anti_govt_sentiments;
				}

			break;
		}
	}

	private void Awake()
	{
		goodsToSupply = new Dictionary<string, int>();
		for(int i = 0; i < goods.Count; i++) {
			goodsToSupply.Add(goods[i], supplyOfGoods[i]);
		}

		peopleGoodsToValues = new Dictionary<string, int>();
		for(int i = 0; i < peopleGoods.Count; i++) {
			peopleGoodsToValues.Add(peopleGoods[i], peopleValues[i]);
		}

		govGoodsToValues = new Dictionary<string, int>();
		for(int i = 0; i < govGoods.Count; i++) {
			govGoodsToValues.Add(govGoods[i], govValues[i]);
		}
	}

	private void OnValidate() {
		if (goods.Count != supplyOfGoods.Count) {
			Debug.LogError("Number of keys do not match number of values");
		}
	}
		
	// Use this for initialization
	void Start () {

		timeCapture = 0;
		intervention_timer = 0;
							
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
