using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryLog : MonoBehaviour {

	private List<Image> inventoryList = new List<Image> ();
	public TextMeshProUGUI moneystext;
	public TextMeshProUGUI projectedTotalText;
	private float moneys = 1000;
	protected float lastPrice;

	// Use this for initialization
	void Start () {
		lastPrice = InventoryValue ();
	}


	float InventoryValue() {
		inventoryList.Clear ();
		for (int i = 0; i < transform.childCount; ++i) {
			if (transform.GetChild (i).childCount > 0) {
				inventoryList.Add(transform.GetChild (i).GetChild(0).GetComponent<Image>());
			}
		}

		float tempValue = 0.0f;
		//Debug.Log(inventoryList [0].GetComponent<info> ().Price);

		foreach (Image good in inventoryList) {
			tempValue += good.GetComponent<info>().Price;
		}
		return tempValue;
	}

	public void Confirm() {
		moneys += lastPrice - InventoryValue ();
		lastPrice = InventoryValue ();
		moneystext.text =  ("MONEY\n$" + moneys + "\n");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(lastPrice - InventoryValue ());
		projectedTotalText.text = "PROJECTED TOTAL\t$" + (lastPrice - InventoryValue());
	}
}
