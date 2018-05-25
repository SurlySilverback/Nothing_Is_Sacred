using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


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
	[SerializeField] private float maxChaos = 1000;
	public float GetMaxChaos(){return maxChaos;}
	public float GetChaos(){return chaos;}
	public void SetChaos(float new_val)
	{
		chaos = new_val;
	}

	// HEAT
	[SerializeField] private float heat;
	[SerializeField] private float maxHeat = 1000;
	public float GetMaxHeat(){return maxHeat;}
	public float GetHeat(){return heat;}
	public void SetHeat(float new_val)
	{
		heat = new_val;
	}

	private Curve SupplyCurve;

	/********************************************************** STATE MANAGEMENNT **********************************************************/
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

	// STOREHOUSE SIZES -- represents three months worth of storage for food and water at max capacity.
	[SerializeField] private float MaxDrugsSupply = 2160f;
	[SerializeField] private float MaxExoticsSupply = 2160f;
	[SerializeField] private float MaxFoodSupply = 2160f;
	[SerializeField] private float MaxFuelSupply = 2160f;
	[SerializeField] private float MaxIdeasSupply = 1000f;
	[SerializeField] private float MaxMedicineSupply = 2160f;
	[SerializeField] private float MaxTextilesSupply = 2160f;
	[SerializeField] private float MaxWaterSupply = 2160f;
	[SerializeField] private float MaxWeaponsSupply = 2160f;

	// CONSUMPTION RATES
	[SerializeField] private float ConsumeDrugs = 0.25f;
	[SerializeField] private float ConsumeExotics = 0.05f;
	[SerializeField] private float ConsumeFood = 1.0f;
	[SerializeField] private float ConsumeFuel = 0.5f;
	[SerializeField] private float ConsumeMedicine = 0.25f;
	[SerializeField] private float ConsumeTextiles = 0.5f;
	[SerializeField] private float ConsumeWater = 1.0f;
	[SerializeField] private float ConsumeWeapons = 0.05f;

	// CHAOS ACCRURAL RATES
	[SerializeField] private float VeryLowChaosIncrease = 0.0625f;
	[SerializeField] private float LowChaosIncrease = 0.125f;
	[SerializeField] private float MedChaosIncrease = 0.25f;
	[SerializeField] private float HighChaosIncrease = 0.5f;
	[SerializeField] private float VeryHighChaosIncrease = 1.0f;
	[SerializeField] private float CritChaosIncrease = 2.0f;

	/********************************************************** INVENTORIES **********************************************************/
	private Inventory playerInventory;
	private Inventory peoplesInventory;
	private Inventory govtInventory;


	// Cardinal Goods
	[SerializeField] private List<Good> goods;
	
	// Contains the city's supply of each Cardinal Good.
	[SerializeField] private List<float> supplyOfGoods;

	// Contains the current prices of all of the Cardinal Goods in the city.
	[SerializeField] private List<float> pricesOfGoods;

	// Method for referencing the current supply of a Good.
	public Dictionary<Good, float> goodsToSupply;


	/************************************************** ECONOMIC CALCULATIONS **************************************************/
	public float GetSupplyMultipler(float value)
	{
		// Use city supply to determine multiplier.
		return SupplyCurve.Evaluate(value);
	}


	// Function for getting the price of a Good in the local (City) context. Good prices vary between cities based on their supply.
	// FIXME: The second parameter may not be necessary, depending on how the UI wants to grab the price.
	public float GetPrice(Good g)
	{
		float supply_multiplier = GetSupplyMultipler ( goodsToSupply[g] );
		float heat_multiplier = g.GetHeatMultiplier ();

		return (supply_multiplier + heat_multiplier) * g.GetBasePrice ();
	}


	// Call each Good type in the Supply dictionary and reduce its supply by one in-game hour's worth.
	private void ConsumeGoods()
	{
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Drugs)] -= ConsumeDrugs;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Exotics)] -= ConsumeExotics;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Food)] -= ConsumeFood;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Fuel)] -= ConsumeFuel;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Medicine)] -= ConsumeMedicine;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Textiles)] -= ConsumeTextiles;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Water)] -= ConsumeWater;
		goodsToSupply[goods.Single(g => g.type == Good.GoodType.Weapons)] -= ConsumeWeapons;
	}

	private void CalculateChaos()
	{
		float food_supply = goodsToSupply [goods.Single (g => g.type == Good.GoodType.Food)];
		float water_supply = goodsToSupply [goods.Single (g => g.type == Good.GoodType.Water)];
		float fuel_supply = goodsToSupply [goods.Single (g => g.type == Good.GoodType.Fuel)];
		float ideas_supply = goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)];

		// Checking Food Supply
		if (food_supply <= 0.1 * MaxFoodSupply)
			chaos += CritChaosIncrease;
		else if (food_supply <= 0.2 * MaxFoodSupply)
			chaos += VeryHighChaosIncrease;
		else if (food_supply <= 0.3 * MaxFoodSupply)
			chaos += HighChaosIncrease;
		else if (food_supply <= 0.4 * MaxFoodSupply)
			chaos += MedChaosIncrease;
		else if (food_supply <= 0.5 * MaxFoodSupply)
			chaos += LowChaosIncrease;

		// Checking Water Supply
		if (water_supply <= 0.1 * MaxWaterSupply)
			chaos += CritChaosIncrease;
		else if (water_supply <= 0.2 * MaxWaterSupply)
			chaos += VeryHighChaosIncrease;
		else if (water_supply <= 0.3 * MaxWaterSupply)
			chaos += HighChaosIncrease;
		else if (water_supply <= 0.4 * MaxWaterSupply)
			chaos += MedChaosIncrease;
		else if (water_supply <= 0.5 * MaxWaterSupply)
			chaos += LowChaosIncrease;
	
		// Checking Fuel Supply
		if (fuel_supply <= 0.1 * MaxFuelSupply)
			chaos += HighChaosIncrease;
		else if (fuel_supply <= 0.2 * MaxFuelSupply)
			chaos += MedChaosIncrease;
		else if (fuel_supply <= 0.3 * MaxFuelSupply)
			chaos += LowChaosIncrease;
		else if (fuel_supply <= 0.4 * MaxFuelSupply)
			chaos += VeryLowChaosIncrease;

		// Checking Ideas Supply
		if (ideas_supply >= 0.8 * MaxIdeasSupply)
			chaos += MedChaosIncrease;
		else if (ideas_supply >= 0.4 * MaxIdeasSupply)
			chaos += LowChaosIncrease;
		else if (ideas_supply >= 0.2 * MaxIdeasSupply)
			chaos += VeryLowChaosIncrease;
	}
		
	// This function is called once every in-game hour to update the state of the city.
	private void DetermineState()
	{
		ConsumeGoods ();		// Consume goods for the city.
		CalculateChaos ();		// Calculate Chaos accrual based on current supply status.

		timeCapture += Time.deltaTime;

		if (timeCapture >= 1) 		// Determine the amount of in-game time passed since last tick.
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

					// Reduce Chaos
					if (chaos < 0.3 * maxChaos)
						chaos = 0;
					else 
						chaos -= (0.3 * maxChaos);
					
					state = CityState.normal;
					OnEndPacifying.Invoke();
				}
			break;

			case CityState.infiltrating:

				// One week
				if (intervention_timer == 10800)
				{
					intervention_timer = 0;
					
					// Reduce Idea Supply
					if (goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)] < 0.3 * goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)])
						goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)] = 0;
					else
						goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)] -= 0.3 * goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)];

					state = CityState.normal;
					OnEndInfiltrating.Invoke();
				}
			break;

			case CityState.ending_demonstrations:

				// Two days
				if (intervention_timer == 2880)
				{
					intervention_timer = 0;

					// Reduce Chaos
					if (chaos < 0.3 * maxChaos)
						chaos = 0;
					else 
						chaos -= (0.3 * maxChaos);

					// Reduce Idea Supply
					if (goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)] < 0.3 * goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)])
						goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)] = 0;
					else
						goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)] -= 0.3 * goodsToSupply [goods.Single (g => g.type == Good.GoodType.Ideas)];

					state = CityState.normal;
					OnEndInfiltrating.Invoke();
				}
			break;

			case CityState.genocide:

				// One week
				if (intervention_timer == 10800)
				{
					intervention_timer = 0;

					// Reduce Chaos
					if (chaos < 0.3 * maxChaos)
						chaos = 0;
					else 
						chaos -= (0.3 * maxChaos);

					// Reduce Weapons Supply
					if (goodsToSupply [goods.Single (g => g.type == Good.GoodType.Weapons)] < 0.3 * goodsToSupply [goods.Single (g => g.type == Good.GoodType.Weapons)])
						goodsToSupply [goods.Single (g => g.type == Good.GoodType.Weapons)] = 0;
					else
						goodsToSupply [goods.Single (g => g.type == Good.GoodType.Weapons)] -= 0.3 * goodsToSupply [goods.Single (g => g.type == Good.GoodType.Weapons)];

					state = CityState.normal;
					OnEndGenocide.Invoke();
				}
			break;

			case CityState.raiding:

				// Three days
				if (intervention_timer == 4320)
				{
					intervention_timer = 0;

					// FIXME: EMPTY PLAYER STOREHOUSE in this city

					state = CityState.normal;
					OnEndRaiding.Invoke();
				}
			break;

			case CityState.independent:

				state = CityState.normal;

			break;

			default:

				// If values are below threshhold, status is normal.
				if (chaos <= (0.6 * maxChaos) && heat <= (0.4 * maxHeat))
					state = CityState.normal;

				// If chaos is high...
				else if (chaos > (0.6 * maxChaos)) 
				{
					// ...and if Idea supply is high...
					if (goodsToSupply [goods.Single(g => g.type == Good.GoodType.Ideas)] >= 300) {		// <-- Terrible coding practice, don't do this in final version.
						state = CityState.anti_govt_demonstrations;		// ...anti-goverment demonstrations.
					}
					// ...and if Weapon supply is high...
					if (goodsToSupply [goods.Single(g => g.type == Good.GoodType.Weapons)] >= 300) {
						state = CityState.civil_war;					// ...civil war.
					}

					state = CityState.unrest;
				} 

				// If heat is high...
				else if (heat > 0.4 * maxHeat) {
					state = CityState.high_blackmarketeering;	// ...high black marketeering.
				}

				// If Idea supply is high...
				else if (goodsToSupply [goods.Single(g => g.type == Good.GoodType.Ideas)] >= 300) 
				{
					state = CityState.anti_govt_sentiments;
				}

			break;
		}
	}

	private void Awake()
	{
		goodsToSupply = new Dictionary<Good, float>();
		for(int i = 0; i < goods.Count; i++) {
			goodsToSupply.Add(goods[i], supplyOfGoods[i]);
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
