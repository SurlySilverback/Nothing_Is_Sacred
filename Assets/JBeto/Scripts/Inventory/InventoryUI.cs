using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;
using TMPro;

[RequireComponent(typeof(GridLayoutGroup))]
public class InventoryUI : MonoBehaviour, IInventoryView
{
    public UnityEvent OnMoveGood;
    [SerializeField]
    private InventorySlot inventorySlot;

    private void Awake()
    {
        if (OnMoveGood == null)
        {
            OnMoveGood = new UnityEvent();
        }
    }

    public void SetInventory(Inventory items)
    {
        Good[] inventory = items.GetEntireInventory();
        // Destroy all the slots and any items, occurs 'End of frame'
        UnityUtility.DestroyChildren(transform);
        // Create and fill each slot individually
        for (int i = 0; i < inventory.Length; ++i)
        {
            GameObject slot = Instantiate(inventorySlot.gameObject, transform);
            slot.GetComponent<InventorySlot>().Store = items;
            Good good = inventory[i];
            if (good != null)
            {
                Instantiate(good.visual, slot.transform);
            }
        }
    }

    public void SetGood(Good good, int index)
    {
        Transform slot = transform.GetChild(index);
        // Clear that slot
        if (good == null)
        {
            UnityUtility.DestroyChildren(slot);
        }
        // Add a good
        else
        {
            Instantiate(good.visual, slot);
        }
        OnMoveGood.Invoke();
    }

    public void ClearInventory()
    {
        foreach(Transform slot in transform)
        {
            UnityUtility.DestroyChildren(slot);
        }
    }
}