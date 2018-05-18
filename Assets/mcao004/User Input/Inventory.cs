using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public int Size { get; private set; }
    public float WeightCapacity { get; private set; }
    public float CurrentWeight { get; private set; }

    // Indexing is position of good
    private Good[] inventory;

    public Inventory(int size, float weightCapacity)
    {
        Size = size;
        WeightCapacity = weightCapacity;
        CurrentWeight = 0;
        this.inventory = new Good[size];
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

    private bool MeetsWeightConstraint(Good item)
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
        bool isOccupied = this.inventory[position] == null;
        return (!isOccupied && MeetsWeightConstraint(item));
    }

    public void SwapGood(int position1, int position2)
    {
        UnityUtility.Swap(ref this.inventory[position1], ref this.inventory[position2]);
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

    public void AddGood(Good item)
    {
        int nextEmpty = GetNextEmpty();
        if (nextEmpty != -1 && MeetsWeightConstraint(item))
        {
            this.inventory[nextEmpty] = item;
            CurrentWeight += item.Weight;
        }
    }

    public void AddGood(Good item, int position)
    {
        if (CanAddGood(item, position))
        {
            this.inventory[position] = item;
            CurrentWeight += item.Weight;
        }
    }

    public void RemoveGood(int position)
    {
        Good toRemove = this.inventory[position];
        if (toRemove != null)
        {
            CurrentWeight -= toRemove.Weight;
            this.inventory[position] = null;
        }
    }
}