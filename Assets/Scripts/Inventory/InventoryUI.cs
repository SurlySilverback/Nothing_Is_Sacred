﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private InventorySlot inventorySlot;
    [SerializeField]
    private Transform gridTransform;
    [SerializeField]
    private TextMeshProUGUI currentWeight;
    [SerializeField]
    private TextMeshProUGUI maxWeight;
    public UnityEvent OnMoveGood;
    #endregion Inspector

    private Inventory inventory;
    private bool shouldUpdate;

    private void Awake()
    {
        shouldUpdate = true;
        if (OnMoveGood == null)
        {
            OnMoveGood = new UnityEvent();
        }
    }
    
    // Sets the inventory UI to the specified inventory
    public void SetInventory(Inventory inventory)
    {
        if (this.inventory == inventory) return;

        // Setup inventory(logic component) and add listener to detect changes
        Assert.IsNotNull(inventory);
        if (this.inventory != null)
        {
            this.inventory.OnInventoryChange.RemoveListener(UpdateView);
        }
        this.inventory = inventory;
        this.inventory.OnInventoryChange.AddListener(UpdateView);

        if (currentWeight != null)
        {
            currentWeight.text = "WEIGHT: " + inventory.CurrentWeight.ToString();
        }
        if (maxWeight != null)
        {
            maxWeight.text = "CAPACITY: " + inventory.WeightCapacity.ToString();
        }

        // Setup UI elements and add listener to detect changes
        IList<Good> goods = inventory.GetEntireInventory();
        UnityUtility.DestroyChildren(gridTransform);
        for (int i = 0; i < goods.Count; ++i)
        {
            GameObject slotObject = Instantiate(inventorySlot.gameObject, gridTransform);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slot.GetComponent<InventorySlot>().Items = inventory;
            slot.BeforeChangeItem.AddListener(delegate { this.shouldUpdate = false; });
            Good good = goods[i];
            if (good != null)
            {
                Instantiate(good.visual, slot.transform);
            }
        }
    }

    // Conditional Update
    // If user did something to the items, don't update the view since it's already current
    // If inventory was changed through logic, update the view
    private void UpdateView()
    {
        if (currentWeight != null)
        {
            currentWeight.text = "Weight: " +  inventory.CurrentWeight.ToString();
        }
        if (maxWeight != null)
        {
            maxWeight.text = "Cap.: " + inventory.WeightCapacity.ToString();
        }
        if (!shouldUpdate)
        {
            shouldUpdate = true;
            return;
        }
        SetInventory(inventory.GetEntireInventory());
    }

    private void SetInventory(IList<Good> inventory)
    {
        Assert.IsTrue(gridTransform.childCount == inventory.Count);
        for(int i = 0; i < inventory.Count; i++)
        {
            Transform slot = gridTransform.GetChild(i);
            UnityUtility.DestroyChildren(slot.transform);
            Good good = inventory[i];
            if (good != null)
            {
                Instantiate(good.visual, slot);
            }
        }
    }
}