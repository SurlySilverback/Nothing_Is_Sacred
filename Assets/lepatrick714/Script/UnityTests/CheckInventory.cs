﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CheckInventory : MonoBehaviour {
	public List<Good> Goods;
	private Inventory Testing; 
	public Unit u;

	bool Test1() 
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

		Debug.Log("Test1 Works!"); 
		return true; 
	}

	// Use this for initialization
	void Start () {
		Test1(); 

	}
	
	// Update is called once per frame
	void Update () {
	}
}
