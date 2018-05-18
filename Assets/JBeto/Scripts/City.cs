using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class City : MonoBehaviour
{
    [SerializeField]
    private string cityName;
    [SerializeField]
    private float heat; 
	
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
    
    private Inventory peopleInventory;
    private Inventory govInventory;
    
	[SerializeField]
    private float foodProductionRate;
	[SerializeField]
    private float population;

    public float GetHeat()
    {
        return this.heat;
    }

    public string GetName()
    {
        return this.cityName;
    }

    private void Awake()
	{
		
	}
}
