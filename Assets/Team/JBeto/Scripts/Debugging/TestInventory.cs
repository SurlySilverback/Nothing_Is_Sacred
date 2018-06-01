using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private int size;
    [SerializeField]
    private float weight;
    public InventoryModel inventory;
    public InventoryModel inventory1;
    public InventoryModel inventory2;
    public List<Good> goods;

    private void Awake()
    {
        this.inventory = new InventoryModel(size, weight, false);
        this.inventory1 = new InventoryModel(size, weight, false);
        this.inventory2 = new InventoryModel(size, weight, true);

        foreach (Good g in goods)
        {
            inventory.AddGood(g);
            inventory1.AddGood(g);
            inventory2.AddGood(g);
        }
    }
}
