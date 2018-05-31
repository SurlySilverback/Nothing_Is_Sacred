using System.Collections;
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

    private void Awake()
    {
        this.inventory = new Inventory(size, weight, true);
    }
}
