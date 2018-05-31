using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryView
{
    void SetInventory(Inventory items);
    void ClearInventory();
    void SetGood(Good good, int position);
}