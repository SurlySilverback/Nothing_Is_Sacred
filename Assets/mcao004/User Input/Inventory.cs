using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private InventoryUI view;

    // Stores good and position in inventory
    private Dictionary<Goods, Vector2Int> items;

    public IEnumerable<Goods> GetInventory()
    {
        return items.Keys;
    }

    private void Awake()
    {
        
    }
}
