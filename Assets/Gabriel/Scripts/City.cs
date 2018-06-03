using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
using MonsterLove.StateMachine;

public class City : MonoBehaviour, IMarket
{
    #region Events
    // Unity Events for Observer Pattern: Calls MainGovernment script to restore spent Tyranny when strategy actions are complete.
  public UnityEvent OnChangeState;
	public UnityEvent OnEndPacifying;
	public UnityEvent OnEndInfiltrating;
	public UnityEvent OnEndEndingDemonstrations;
	public UnityEvent OnEndGenocide;
	public UnityEvent OnEndRaiding;
    #endregion

    #region Init
    [SerializeField]
    private Curve SupplyCurve;

    // NAME
    [Header("Name")]
    [SerializeField]
    private string cityName;
	public string CityName { get { return cityName; } private set { } }
    [Space(10)]

    // CHAOS
    [Header("Chaos")]
    [SerializeField]
    private float chaos;
    public float Chaos { get { return chaos; } set { } }
	[SerializeField]
    private float maxChaos = 1000;
	public float MaxChaos { get { return maxChaos; } private set { } }
    [Space(10)]

	[Header("Heat")]
	[SerializeField]
    private float maxHeat = 1000;
	public float MaxHeat { get { return maxHeat; } private set { } }
    [SerializeField]
    private float heat;
    public float Heat { get { return heat; } set { } }
    #endregion

    #region Goods
    [Space(10)]
    [Header("Goods")]
    public float Population;
    [SerializeField]
    private float ProduceFood; 
	[SerializeField]
    private float ProduceWater; 
    [SerializeField]
    private float ProduceDrugs;
    [SerializeField]
    private float ProduceExotics;
    [SerializeField]
    private float ProduceFuel;
    [SerializeField]
    private float ProduceIdeas;    
    [SerializeField]
    private float ProduceMedicine;
    [SerializeField]
    private float ProduceTextiles;
    [SerializeField]
    private float ProduceWeapons;


	// STOREHOUSE SIZES -- represents three months worth of storage for food and water at max capacity.
	public float MaxDrugsSupply = 2160f;
	public float MaxExoticsSupply = 2160f;
	public float MaxFoodSupply = 2160f;
	public float MaxFuelSupply = 2160f;
	public float MaxIdeasSupply = 1000f;
	public float MaxMedicineSupply = 2160f;
	public float MaxTextilesSupply = 2160f;
	public float MaxWaterSupply = 2160f;
	public float MaxWeaponsSupply = 2160f;

	// CONSUMPTION RATES
	[SerializeField]
    private float ConsumeDrugs = 0.25f;
	[SerializeField]
    private float ConsumeExotics = 0.05f;
	[SerializeField]
    private float ConsumeFood = 1.0f;
	[SerializeField]
    private float ConsumeFuel = 0.5f;
	[SerializeField]
    private float ConsumeMedicine = 0.25f;
	[SerializeField]
    private float ConsumeTextiles = 0.5f;
	[SerializeField]
    private float ConsumeWater = 1.0f;
	[SerializeField]
    private float ConsumeWeapons = 0.05f;
    [Space(10)]

	// CHAOS RATES
    [Header("Chaos")]
	[SerializeField]
    private float VeryLowChaosIncrease = 0.0625f;
	[SerializeField]
    private float LowChaosIncrease = 0.125f;
	[SerializeField]
    private float MedChaosIncrease = 0.25f;
	[SerializeField]
    private float HighChaosIncrease = 0.5f;
	[SerializeField]
    private float VeryHighChaosIncrease = 1.0f;
	[SerializeField]
    private float CritChaosIncrease = 2.0f;
    [Space(10)]
    #endregion
    
    #region Inventory
    [Header("Inventories")]

    [Space(5)]
    [SerializeField]
    private int storeHouseSize;
    [SerializeField]
    private int storeHouseWeight;

    [Space(5)]
    [SerializeField]
    private int peopleSize;
    [SerializeField]
    private int peopleWeight;

    [Space(5)]
    [SerializeField]
    private int govtSize;
    [SerializeField]
    private int govtWeight;

    [SerializeField]
    private List<Good> goods;

    public Inventory PlayerInventory { get; private set; }
    public Inventory PeoplesInventory { get; private set; }
	public Inventory GovtInventory { get; private set; }
    #endregion

    #region CityState
    public enum CityState
    {
        Normal, HighBlackmarketeering, Unrest, AntiGovtSentiments, AntiGovtDemonstrations, CivilWar,
        Pacifying, Infiltrating, EndingDemonstrations, Genocide, Raiding
    };

    private CityState state;
    public CityState State
    {
        get
        {
            return state;
        }
        set
        {
            if (state != value)
            {
                state = value;
                OnChangeState.Invoke();
            }
        }
    }
    #endregion

    // Method for referencing the current supply of a Good.
    private Dictionary<Good.GoodType, float> goodsToSupply;
    private int hoursInState; // In Hours
    private const int HoursInDays = 24;

    #region Unity Callbacks
    private void Awake()
    {
        // Inventory setup
        this.PlayerInventory = new Inventory(storeHouseSize, storeHouseWeight, true);
        this.GovtInventory = new Inventory(govtSize, govtWeight, false);
        this.PeoplesInventory = new Inventory(peopleSize, peopleWeight, false);

        this.goodsToSupply = new Dictionary<Good.GoodType, float>
        {
            { Good.GoodType.Drugs, 10 },
            { Good.GoodType.Exotics, 10 },
            { Good.GoodType.Food, 10 },
            { Good.GoodType.Fuel, 10 },
            { Good.GoodType.Ideas, 10 },
            { Good.GoodType.Medicine, 10 },
            { Good.GoodType.People, 10 },
            { Good.GoodType.Textiles, 10 },
            { Good.GoodType.Water, 10 },
            { Good.GoodType.Weapons, 10 }
        };
        this.hoursInState = 0;
        State = CityState.Normal;

        PlayerInventory = new Inventory(storeHouseSize, storeHouseWeight, true);
        PeoplesInventory = new Inventory(peopleSize, peopleWeight, false);
        GovtInventory = new Inventory(govtSize, govtWeight, false);

        foreach(Good g in goods)
        {
            PlayerInventory.AddGood(g);
            PeoplesInventory.AddGood(g);
            GovtInventory.AddGood(g);
        }
    }

    private void Start()
    {
        ServiceLocator.Instance.GetClock().OnHour.AddListener(DetermineState);
    }

    #endregion

    #region FSM
    // This function is called once every in-game hour to update the state of the city.
    private void DetermineState()
    {
        ConsumeGoods();
        CalculateChaos();
        switch (State)
        {
            case CityState.Pacifying:
                if (hoursInState == HoursInDays * 2)
                {             
                    chaos = chaos < 0.3f * maxChaos ? 0 : chaos - (0.3f * maxChaos);
                    State = CityState.Normal;
                    OnEndPacifying.Invoke();
                    hoursInState = 0;
                }
                else
                {
                    hoursInState++;
                }
                break;
            case CityState.Infiltrating:
                if (hoursInState == HoursInDays * 7)
                {
                    float ideaSupply = goodsToSupply[Good.GoodType.Ideas];
                    goodsToSupply[Good.GoodType.Ideas] = ideaSupply < 0.3f ? 0 : ideaSupply - 0.3f;
                    State = CityState.Normal;
                    OnEndInfiltrating.Invoke();
                    hoursInState = 0;
                }
                else
                {
                    hoursInState++;
                }
                break;
            case CityState.EndingDemonstrations:
                if (hoursInState == HoursInDays * 2)
                {
                    chaos = chaos < 0.3f * maxChaos ? 0 : chaos - (0.3f * maxChaos);
                    float ideaSupply = goodsToSupply[Good.GoodType.Ideas];
                    goodsToSupply[Good.GoodType.Ideas] =  ideaSupply < 0.3f ? 0 : ideaSupply - 0.3f;
                    State = CityState.Normal;
                    OnEndInfiltrating.Invoke();
                    hoursInState = 0;
                }
                else
                {
                    hoursInState++;
                }
                break;
            case CityState.Genocide:
                if (hoursInState == HoursInDays * 7)
                {
                    chaos = chaos < 0.3f * maxChaos ? 0 : chaos - (0.3f * maxChaos);
                    float weaponSupply = goodsToSupply[Good.GoodType.Weapons];
                    goodsToSupply[Good.GoodType.Weapons] = weaponSupply < 0.3f ? 0 : weaponSupply - 0.3f;
                    State = CityState.Normal;
                    OnEndGenocide.Invoke();
                    hoursInState = 0;
                }
                else
                {
                    hoursInState++;
                }
                break;
            case CityState.Raiding:
                if (hoursInState == HoursInDays * 3)
                {
                    hoursInState = 0;
                    PlayerInventory.ClearInventory();
                    State = CityState.Normal;
                    OnEndRaiding.Invoke();
                }
                else
                {
                    hoursInState++;
                }
                break;
            default:
                hoursInState = 0;
                // If values are below threshhold, status is normal.
                if (chaos <= (0.6f * maxChaos) && heat <= (0.4f * maxHeat))
                {
                    State = CityState.Normal;
                }
                // If chaos is high...
                else if (chaos > (0.6f * maxChaos))
                {
                    // ...and if Idea supply is high...
                    if (goodsToSupply[Good.GoodType.Ideas] >= 300)
                    {
                        State = CityState.AntiGovtDemonstrations;
                    }
                    // ...and if Weapon supply is high...
                    else if (goodsToSupply[Good.GoodType.Weapons] >= 300)
                    {
                        State = CityState.CivilWar;
                    }
                    else
                    {
                        State = CityState.Unrest;
                    }
                }
                // If heat is high...
                else if (heat > 0.4f * maxHeat)
                {
                    State = CityState.HighBlackmarketeering;
                }
                // If Idea supply is high...
                else if (goodsToSupply[Good.GoodType.Ideas] >= 300)
                {
                    State = CityState.AntiGovtSentiments;
                }
                break;
        }
    }
    #endregion

    #region Helper
    
    // Call each Good type in the Supply dictionary and increase its supply by one in-game hour's worth.
    private void ProduceGoods()
    {
        goodsToSupply[Good.GoodType.Drugs] += ProduceDrugs;
        goodsToSupply[Good.GoodType.Exotics] += ProduceExotics;
        goodsToSupply[Good.GoodType.Food] += ProduceFood;
        goodsToSupply[Good.GoodType.Fuel] += ProduceFuel;
        goodsToSupply[Good.GoodType.Medicine] += ProduceMedicine;
        goodsToSupply[Good.GoodType.Textiles] += ProduceTextiles;
        goodsToSupply[Good.GoodType.Water] += ProduceWater;
        goodsToSupply[Good.GoodType.Weapons] += ProduceWeapons;
    }
    
    // Call each Good type in the Supply dictionary and reduce its supply by one in-game hour's worth.
    private void ConsumeGoods()
    {
        goodsToSupply[Good.GoodType.Drugs] -= ConsumeDrugs;
        goodsToSupply[Good.GoodType.Exotics] -= ConsumeExotics;
        goodsToSupply[Good.GoodType.Food] -= ConsumeFood;
        goodsToSupply[Good.GoodType.Fuel] -= ConsumeFuel;
        goodsToSupply[Good.GoodType.Medicine] -= ConsumeMedicine;
        goodsToSupply[Good.GoodType.Textiles] -= ConsumeTextiles;
        goodsToSupply[Good.GoodType.Water] -= ConsumeWater;
        goodsToSupply[Good.GoodType.Weapons] -= ConsumeWeapons;
    }

    private void CalculateChaos()
    {
        float foodSupply = goodsToSupply[Good.GoodType.Food];
        float waterSupply = goodsToSupply[Good.GoodType.Water];
        float fuelSupply = goodsToSupply[Good.GoodType.Fuel];
        float ideasSupply = goodsToSupply[Good.GoodType.Ideas];

        // Checking Food Supply
        if (foodSupply <= 0.1 * MaxFoodSupply)
            chaos += CritChaosIncrease;
        else if (foodSupply <= 0.2 * MaxFoodSupply)
            chaos += VeryHighChaosIncrease;
        else if (foodSupply <= 0.3 * MaxFoodSupply)
            chaos += HighChaosIncrease;
        else if (foodSupply <= 0.4 * MaxFoodSupply)
            chaos += MedChaosIncrease;
        else if (foodSupply <= 0.5 * MaxFoodSupply)
            chaos += LowChaosIncrease;

        // Checking Water Supply
        if (waterSupply <= 0.1 * MaxWaterSupply)
            chaos += CritChaosIncrease;
        else if (waterSupply <= 0.2 * MaxWaterSupply)
            chaos += VeryHighChaosIncrease;
        else if (waterSupply <= 0.3 * MaxWaterSupply)
            chaos += HighChaosIncrease;
        else if (waterSupply <= 0.4 * MaxWaterSupply)
            chaos += MedChaosIncrease;
        else if (waterSupply <= 0.5 * MaxWaterSupply)
            chaos += LowChaosIncrease;

        // Checking Fuel Supply
        if (fuelSupply <= 0.1 * MaxFuelSupply)
            chaos += HighChaosIncrease;
        else if (fuelSupply <= 0.2 * MaxFuelSupply)
            chaos += MedChaosIncrease;
        else if (fuelSupply <= 0.3 * MaxFuelSupply)
            chaos += LowChaosIncrease;
        else if (fuelSupply <= 0.4 * MaxFuelSupply)
            chaos += VeryLowChaosIncrease;

        // Checking Ideas Supply
        if (ideasSupply >= 0.8 * MaxIdeasSupply)
            chaos += MedChaosIncrease;
        else if (ideasSupply >= 0.4 * MaxIdeasSupply)
            chaos += LowChaosIncrease;
        else if (ideasSupply >= 0.2 * MaxIdeasSupply)
            chaos += VeryLowChaosIncrease;
    }
    #endregion 

    #region Economics
    public float GetSupplyMultipler(float value)
	{
		return SupplyCurve.Evaluate(value);
	}

	public float GetPrice(Good good)
	{
		float supplyMultiplier = GetSupplyMultipler(goodsToSupply[good.type]);
		float heatMultiplier = good.GetHeatMultiplier();
		return (supplyMultiplier + heatMultiplier) * good.GetBasePrice();
	}

    public float GetSupply(Good.GoodType good)
    {
        return this.goodsToSupply[good];
    }
    #endregion
}