using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class City : MonoBehaviour { 
	[SerializeField] private string name;
	public string GetName(){return name;}

	[SerializeField] private float heat; 
	public float GetHeat(){return heat;}

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
	private Dictionary<string, int> goodsToSupply;

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
	private Dictionary<string, int> peopleGoodsToValues;


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


	[SerializeField] private float foodProductionRate; 
	[SerializeField] private float waterProductionRate; 
	[SerializeField] private float population; 


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


							
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
