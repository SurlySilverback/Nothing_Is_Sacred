using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Linq;
using System;

public class Inventory
{
    public bool IsPlayerOwned { get; private set; }
    public bool IsTradeEnabled { get; set; }
    public int Size { get; private set; }
    public float WeightCapacity { get; private set; }
    public float CurrentWeight { get; private set; }
    private Good[] inventory;
    public UnityEvent OnInventoryChange { get; private set; }

    public Inventory(int size, float weightCap, bool isPlayerOwned)
    {
        IsTradeEnabled = true;
        IsPlayerOwned = isPlayerOwned;
        Size = size;
        WeightCapacity = weightCap;
        CurrentWeight = 0;
        this.inventory = new Good[Size];
        OnInventoryChange = new UnityEvent();
    }

    public IList<Good> GetEntireInventory()
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

    // TODO, Needs error checking
    public void SetInventory(IList<Good> newInventory)
    {
        Assert.IsTrue(newInventory.Count == inventory.Length);
        for (int i = 0; i < newInventory.Count; i++)
        {
            this.inventory[i] = newInventory[i];
        }
        OnInventoryChange.Invoke();
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

    public bool CanPlayerTrade(Inventory items)
    {
        if (IsPlayerOwned)
        {
            return items.IsTradeEnabled;
        }
        else if (items.IsPlayerOwned)
        {
            return IsTradeEnabled;
        }
        else
        {
            return false;
        }
    }

    public void TradeGoodFrom(Inventory seller, int sellerIndex, int buyerIndex)
    {
        if (this == seller)
        {
            SwapGood(sellerIndex, buyerIndex);
        }
        else
        {
            Assert.IsTrue(CanPlayerTrade(seller));
            Good item = seller.inventory[sellerIndex];
            Assert.IsNotNull(item);
            Assert.IsTrue(CanAddGood(item, buyerIndex));
            inventory[buyerIndex] = item;
            CurrentWeight += item.Weight;
            seller.inventory[sellerIndex] = null;
            seller.CurrentWeight -= item.Weight;
            OnInventoryChange.Invoke();
            seller.OnInventoryChange.Invoke();
        }
    }

    public void TradeGoodTo(Inventory buyer, int sellerIndex, int buyerIndex)
    {
        if (this == buyer)
        {
            SwapGood(sellerIndex, buyerIndex);
        }
        else
        {
            Assert.IsTrue(CanPlayerTrade(buyer));
            Good item = this.inventory[sellerIndex];
            Assert.IsNotNull(item);
            Assert.IsTrue(buyer.CanAddGood(item, buyerIndex));
            buyer.inventory[buyerIndex] = item;
            buyer.CurrentWeight += item.Weight;
            this.inventory[sellerIndex] = null;
            CurrentWeight -= item.Weight;
            OnInventoryChange.Invoke();
            buyer.OnInventoryChange.Invoke();
        }
    }

    public void SwapGood(int position1, int position2)
    {
        UnityUtility.Swap(ref this.inventory[position1], ref this.inventory[position2]);
        OnInventoryChange.Invoke();
    }

    public void AddGood(Good item)
    {
        int nextEmpty = GetNextEmpty();
        Assert.IsTrue(nextEmpty != -1 && MeetsWeightConstraint(item));
        this.inventory[nextEmpty] = item;
        CurrentWeight += item.Weight;
        OnInventoryChange.Invoke();
    }

    public void AddGood(Good item, int position)
    {
        Assert.IsNotNull(item);
        Assert.IsTrue(CanAddGood(item, position));
        this.inventory[position] = item;
        CurrentWeight += item.Weight;
        OnInventoryChange.Invoke();
    }

    public void RemoveGood(int position)
    {
        Good toRemove = this.inventory[position];
        Assert.IsNotNull(toRemove);
        CurrentWeight -= toRemove.Weight;
        this.inventory[position] = null;
        OnInventoryChange.Invoke();
    }

    public void RemoveGoodIfAny(int position)
    {
        Good toRemove = this.inventory[position];
        if (toRemove != null)
        {
            CurrentWeight -= toRemove.Weight;
            this.inventory[position] = null;
            OnInventoryChange.Invoke();
        }
    }

	public void ClearInventory()
	{
		for (int i = 0; i < this.inventory.Length; ++i)
        {
		
			inventory[i] = null;
		}
		CurrentWeight = 0;
        OnInventoryChange.Invoke();
    }

	public List<int> GetGoodsOfType (Good.GoodType type) {
		List<int> result = new List<int> ();
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null && type == inventory[i].type) {
				result.Add (i);
			}
		}
		return result;
	}
}