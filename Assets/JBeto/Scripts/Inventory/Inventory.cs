using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using UnityEngine.Assertions;

public class Inventory
{
    public bool IsPlayerOwned { get; private set; }
    public bool IsTradeEnabled { get; set; }
    public int Size { get; private set; }
    public float WeightCapacity { get; private set; }
    public float CurrentWeight { get; private set; }
    private Good[] inventory;
    private IInventoryView uiDisplay;
    
    public bool CanPlayerTrade(Inventory items)
    {
        return (IsPlayerOwned || items.IsPlayerOwned) && (IsTradeEnabled && items.IsTradeEnabled);
    }

    public Inventory(int size, float weightCap, bool isPlayerOwned)
    {
        uiDisplay = new NullInventoryView();
        IsPlayerOwned = isPlayerOwned;
        Size = size;
        WeightCapacity = weightCap;
        CurrentWeight = 0;
        this.inventory = new Good[Size];
    }

    public void SetUIDisplay(IInventoryView display)
    {
        Assert.IsNotNull(display);
        this.uiDisplay = display;
        uiDisplay.SetInventory(this);
    }

    public Good[] GetEntireInventory()
    {
        return this.inventory;
    }

    public IEnumerable<Good> GetInventory()
    {
        foreach (Good g in this.inventory)
        {
            if (g != null)
            {
                yield return g;
            }
        }
    }

    public Good GetGood(int position)
    {
        return this.inventory[position];
    }

    public bool MeetsWeightConstraint(Good item)
    {
        return (CurrentWeight + item.Weight) < WeightCapacity;
    }

    public bool CanAddGood(Good item)
    {
        bool isFull = GetNextEmpty() == -1;
        return (!isFull && MeetsWeightConstraint(item));
    }

    public bool CanAddGood(Good item, int position)
    {
        bool isOccupied = this.inventory[position] != null;
        return (!isOccupied && MeetsWeightConstraint(item));
    }

    private int GetNextEmpty()
    {
        for (int i = 0; i < Size; i++)
        {
            if (this.inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    
    public void TradeGood(Inventory destination, int startItemPosition, int endItemPosition)
    {
        destination.AddGood(inventory[startItemPosition], endItemPosition);
        RemoveGood(startItemPosition);
    }

    // TODO
    public void SwapGood(int position1, int position2)
    {
        Good source = this.inventory[position1];
        Good dest = this.inventory[position2];
        UnityUtility.Swap(ref source, ref dest);
        uiDisplay.SetGood(source, position2);
        uiDisplay.SetGood(dest, position1);
    }

    public void AddGood(Good item)
    {
        int nextEmpty = GetNextEmpty();
        Assert.IsTrue(nextEmpty != -1 && MeetsWeightConstraint(item));
        this.inventory[nextEmpty] = item;
        CurrentWeight += item.Weight;
        uiDisplay.SetGood(item, nextEmpty);
    }

    public void AddGood(Good item, int position)
    {
        Assert.IsNotNull(item);
        Assert.IsTrue(CanAddGood(item, position));
        this.inventory[position] = item;
        CurrentWeight += item.Weight;
        uiDisplay.SetGood(item, position);
    }

    public void RemoveGood(int position)
    {
        Good toRemove = this.inventory[position];
        Assert.IsNotNull(toRemove);
        CurrentWeight -= toRemove.Weight;
        this.inventory[position] = null;
        uiDisplay.SetGood(null, position);
    }

    public void RemoveGoodIfAny(int position)
    {
        Good toRemove = this.inventory[position];
        if (toRemove != null)
        {
            CurrentWeight -= toRemove.Weight;
            this.inventory[position] = null;
            uiDisplay.SetGood(null, position);
        }
    }

	public void ClearInventory()
	{
		for (int i = 0; i < this.inventory.Length; ++i)
        {
		
			inventory[i] = null;
		}
		CurrentWeight = 0;
        uiDisplay.ClearInventory();
    }
}