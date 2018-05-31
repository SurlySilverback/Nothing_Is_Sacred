using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullInventoryView : IInventoryView
{
    public void SetInventory(Inventory items) { }
    public void ClearInventory() { }
    public void SetGood(Good good, int position) { }
}
