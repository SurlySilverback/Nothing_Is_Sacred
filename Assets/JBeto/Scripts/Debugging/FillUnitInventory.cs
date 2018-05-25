using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillUnitInventory : MonoBehaviour
{
    [SerializeField]
    private Unit unit;
    [SerializeField]
    private List<Good> goods;
    
	// Use this for initialization
	void Start ()
    {
        Inventory inv = unit.Items;
        foreach(Good g in goods)
        {
            inv.AddGood(g);
        }
	}
}
