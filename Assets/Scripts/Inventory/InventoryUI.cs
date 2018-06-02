using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private InventorySlot inventorySlot;
    [SerializeField]
    private Transform gridTransform;
    
    public UnityEvent OnMoveGood;
    private Inventory inventory;
    // Keeps track of whether UI should update(relative with Inventory) or not
    private bool shouldUpdate;

    private void Awake()
    {
        shouldUpdate = true;
        if (OnMoveGood == null)
        {
            OnMoveGood = new UnityEvent();
        }
    }

    private void InitInventory(IList<Good> inventory)
    {
        UnityUtility.DestroyChildren(gridTransform);
        for (int i = 0; i < inventory.Count; ++i)
        {
            GameObject slot = Instantiate(inventorySlot.gameObject, gridTransform);
            slot.GetComponent<InventorySlot>().UI = this;
            Good good = inventory[i];
            if (good != null)
            {
                Instantiate(good.visual, slot.transform);
            }
        }
    }

    public void SetInventory(Inventory inventory)
    {
        Assert.IsNotNull(inventory);
        if (this.inventory != null)
        {
            this.inventory.OnInventoryChange.RemoveListener(UpdateView);
        }
        this.inventory = inventory;
        this.inventory.OnInventoryChange.AddListener(UpdateView);
        InitInventory(inventory.GetEntireInventory());
    }

    // Conditional Update
    // If user did something to the items, don't update the view since it's already current
    // If inventory was changed through logic, update the view
    private void UpdateView()
    {
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
    
    public bool TryDropItem(InventoryUI source, int sourceIndex, int destinationIndex)
    {
        if (source == this)
        {
            shouldUpdate = false;
            inventory.SwapGood(sourceIndex, destinationIndex);
            return true;
        }
        Inventory seller = source.inventory;
        Good item = seller.GetGood(sourceIndex);
        Assert.IsNotNull(item);
        if (seller.CanPlayerTrade(inventory) && inventory.CanAddGood(item, destinationIndex))
        {
            Player player = ServiceLocator.Instance.GetPlayer();
            // player is the buyer
            if (inventory.IsPlayerOwned)
            {
                if (!player.CanBuyGood(item))
                {
                    return false;
                }
                else
                {
                    player.BuyGood(item);
                }
            }
            else
            {
                player.SellGood(item);
            }
            shouldUpdate = false;
            source.shouldUpdate = false;
            seller.TradeGoodTo(inventory, sourceIndex, destinationIndex);
            return true;
        }
        return false;
    }
}