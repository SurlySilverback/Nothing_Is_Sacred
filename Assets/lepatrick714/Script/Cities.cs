using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cities : MonoBehaviour { 
	[SerializeField] private string Name;  
	[SerializeField] private float Heat; 

	// Cardinal Goods
	[SerializeField] private List<string> goods = new List<string>(){ "Drugs", "Exotics", "Food", 
																	  "Fuel", "Ideas", "Medicine", 
																	  "People", "Textiles", "Water", 
																	  "Weapons" };
	[SerializeField] private List<int> values = new List<int>(){ 0, 0, 0, 
																 0, 0, 0, 
																 0, 0, 0, 
																 0 };
	private Dictionary<string, int> goodsToValues;


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
		goodsToValues = new Dictionary<string, int>();
		for(int i = 0; i < goods.Count; i++) {
			goodsToValues.Add(goods[i], values[i]);
		}

		peopleGoodsToValues = new Dictionary<string, int>();
		for(int i = 0; i < peopleGoods.Count; i++) {
			peopleGoodsToValues.Add(peopleGoods[i], peopleValues[i]);
		}

		peopleGoodsToValues = new Dictionary<string, int>();
		for(int i = 0; i < govGoods.Count; i++) {
			govGoodsToValues.Add(govGoods[i], govValues[i]);
		}
	}

	private void OnValidate() {
		if (goods.Count != values.Count) {
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
