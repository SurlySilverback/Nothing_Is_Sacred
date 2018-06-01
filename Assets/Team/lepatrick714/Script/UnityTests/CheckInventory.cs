using UnityEngine;

public class CheckInventory : MonoBehaviour {
    /*
	public List<Good> Goods;
	private Inventory Testing; 
	public Unit u;

	bool DeletingAllGoodsTest() 
	{
		// Ensure that the size in the inspector is the same
		Testing = new Inventory(12, 9000); 
		
		// Testing 1 : Adding Goods	
		foreach (Good i in Goods) { 
			Testing.AddGood(i); 
			u.Items.AddGood(i);
		}

		// Grabs all Items from Inventory Object
		Good[] Check = Testing.GetEntireInventory(); 
		
		// Checking if both containers are of the same size
		Assert.AreEqual(Check.Length, Goods.Count); 
		
		// Ensure all elements 
		for(int i=0; i<Check.Length; i++) 
		{
			Assert.AreEqual(Check[i].Weight, Goods[i].Weight); 
		}

		for(int i=0; i<Check.Length; i++) 
		{ 
			Testing.RemoveGood(i); 
		}

		Check = Testing.GetEntireInventory(); 

		foreach (Good i in Check)
		{ 
			Assert.IsNull(i); 
		}

		Debug.Log("PASSED DeletingAllGoodsTest"); 
		return true; 
	}
	
	bool AddingGoodsTest() 
	{
		// Ensure that the size in the inspector is the same
		Testing = new Inventory(12, 9000); 
		
		// Testing 1 : Adding Goods	
		foreach (Good i in Goods) { 
			Testing.AddGood(i); 
			u.Items.AddGood(i);
		}

		// Grabs all Items from Inventory Object
		Good[] Check = Testing.GetEntireInventory(); 
		
		// Checking if both containers are of the same size
		Assert.AreEqual(Check.Length, Goods.Count); 
		
		// Ensure all elements 
		for(int i=0; i<Check.Length; i++) 
		{
			Assert.AreEqual(Check[i].Weight, Goods[i].Weight); 
		}

		Debug.Log("PASSED AddingGoodsTest"); 
		return true; 
	}

	bool DeleteSingleGoodsTest() 
	{
		// Ensure that the size in the inspector is the same
		Testing = new Inventory(12, 9000); 
		
		// Testing 1 : Adding Goods	
		foreach (Good i in Goods) { 
			Testing.AddGood(i); 
			u.Items.AddGood(i);
		}

		// Grabs all Items from Inventory Object
		Good[] Check = Testing.GetEntireInventory(); 
		
		// Checking if both containers are of the same size
		Assert.AreEqual(Check.Length, Goods.Count); 
		
		// Ensure all elements 
		for(int i=0; i<Check.Length; i++) 
		{
			Assert.AreEqual(Check[i].Weight, Goods[i].Weight); 
		}

		// Removing a single element at index 0
		Testing.RemoveGood(0); 
		Check = Testing.GetEntireInventory(); 
		Assert.IsNull(Check[0]);

		Debug.Log("PASSED DeleteSingleGoodsTest"); 
		return true; 
	}



	// Use this for initialization
	void Start () {
		AddingGoodsTest(); 
		DeletingAllGoodsTest(); 
		DeleteSingleGoodsTest(); 
	}
	*/
	// Update is called once per frame
	void Update () {
	}
}