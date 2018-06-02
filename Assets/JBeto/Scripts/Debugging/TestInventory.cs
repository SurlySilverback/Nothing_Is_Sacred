using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private int size;
    [SerializeField]
    private float weight;
    public Inventory inventory;
    public Inventory inventory1;
    public Inventory inventory2;
    public List<Good> goods;

    private void Awake()
    {
        this.inventory = new Inventory(size, weight, false);
        this.inventory1 = new Inventory(size, weight, false);
        this.inventory2 = new Inventory(size, weight, true);

        foreach (Good g in goods)
        {
            inventory.AddGood(g);
            inventory1.AddGood(g);
            inventory2.AddGood(g);
        }
    }
}
