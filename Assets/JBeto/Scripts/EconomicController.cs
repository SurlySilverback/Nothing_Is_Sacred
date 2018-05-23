using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomicController : MonoBehaviour {

	[SerializeField] private Curve supplyCurve;

	// Contains a list of all cities in the game
	[SerializeField] private List<City> listOfCities;

	[SerializeField] private List<string> goods = new List<string>(){ "Drugs", "Exotics", "Food", 
																		"Fuel", "Ideas", "Medicine", 
																		"People", "Textiles", "Water", 
																		"Weapons" };
	
	[SerializeField] private List<float> listOfBasePrices = new List<float>() { 3, 15, 2,
																				15, 1, 10,
																				5, 5, 1, 10};

	private Dictionary<string, float> goodsToBasePrice;



	// Updates all prices after purchases or sales
	public void updatePrices()
	{
		foreach (City city in listOfCities) 
		{
			// Now we need another foreach loop that traverses the Dictionary entries in each
			foreach (KeyValuePair<string, float> pair in city.goodsToPrices) 
			{
				// FIXME: Can the same key/value pair be used when calling city.goodToPrice AND this.listOfBasePrices? Will this call the correct Good's basePrice?
				city.goodsToPrices[pair.Key] = ( (calculateHeatMultiplier( ) + calculateSupplyMultipler( city.goodsToSupply[pair.Key] )) * this.goodsToBasePrice[pair.Key] );
			}
		}
	}

	// Use this for initialization
	void Start () {
	

	}

	void Awake() {
	
		goodsToBasePrice = new Dictionary<string, float>();
		for(int i = 0; i < goods.Count; i++) 
		{
			goodsToBasePrice.Add(goods[i], listOfBasePrices[i]);
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		updatePrices ();
	}
}
