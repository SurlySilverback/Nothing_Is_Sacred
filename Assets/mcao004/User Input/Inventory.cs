using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public Vector2Int Size { get; private set; }
    public float WeightCapacity { get; private set; }
    public float CurrentWeight { get; private set; }

    // Indexing is position of good
    private Good[,] inventory;

    public Inventory(Vector2Int size, float weightCapacity)
    {
        Size = size;
        WeightCapacity = weightCapacity;
        CurrentWeight = 0;
        this.inventory = new Good[size.x, size.y];
    }

    public Good[,] GetEntireInventory()
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

    public Good GetGood(Vector2Int position)
    {
        return this.inventory[position.x, position.y];
    }

    private bool MeetsWeightConstraint(Good item)
    {
        return (CurrentWeight + item.Weight) < WeightCapacity;
    }

    public bool CanAddGood(Good item)
    {
        bool isFull = GetNextEmpty() == new Vector2Int(-1, -1);
        return (!isFull && MeetsWeightConstraint(item));
    }

    public bool CanAddGood(Good item, Vector2Int position)
    {
        bool isOccupied = this.inventory[position.x, position.y] == null;
        return (!isOccupied && MeetsWeightConstraint(item));
    }

    public void SwapGood(Vector2Int position1, Vector2Int position2)
    {
        UnityUtility.Swap(ref this.inventory[position1.x, position1.y], ref this.inventory[position2.x, position2.y]);
    }

    private Vector2Int GetNextEmpty()
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                if (this.inventory[i, j] == null)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public void AddGood(Good item)
    {
        Vector2Int nextEmpty = GetNextEmpty();
        if (nextEmpty != new Vector2Int(-1, -1) && MeetsWeightConstraint(item))
        {
            this.inventory[nextEmpty.x, nextEmpty.y] = item;
            CurrentWeight += item.Weight;
        }
    }

    public void AddGood(Good item, Vector2Int position)
    {
        if (CanAddGood(item, position))
        {
            this.inventory[position.x, position.y] = item;
            CurrentWeight += item.Weight;
        }
    }

    public void RemoveGood(Vector2Int position)
    {
        Good toRemove = this.inventory[position.x, position.y];
        if (toRemove != null)
        {
            CurrentWeight -= toRemove.Weight;
            this.inventory[position.x, position.y] = null;
        }
    }
}