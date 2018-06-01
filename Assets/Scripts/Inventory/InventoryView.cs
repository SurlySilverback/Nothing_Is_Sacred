using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private InventorySlot inventorySlot;
    [SerializeField]
    private Transform gridTransform;
    public UnityEvent OnMoveGood;

    private void Awake()
    {
        if (OnMoveGood == null)
        {
            OnMoveGood = new UnityEvent();
        }
    }

    public IEnumerable<InventorySlot> GetAllSlots()
    {
        foreach(Transform transform in gridTransform)
        {
            yield return transform.GetComponent<InventorySlot>();
        }
    }
    
    public void InitInventory(InventoryViewModel controller, Good[] inventory)
    {
        for (int i = 0; i < inventory.Length; ++i)
        {
            GameObject slot = Instantiate(inventorySlot.gameObject, gridTransform);
            slot.GetComponent<InventorySlot>().Controller = controller;
            Good good = inventory[i];
            if (good != null)
            {
                Instantiate(good.visual, slot.transform);
            }
        }
    }

    public void AssignInventory(Good[] inventory)
    {
        Assert.IsTrue(gridTransform.childCount == inventory.Length);
        for(int i = 0; i < inventory.Length; i++)
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

    public void DestroyInventory()
    {
        UnityUtility.DestroyChildren(gridTransform);
    }

    public void ClearInventory()
    {
        foreach(Transform slot in gridTransform)
        {
            UnityUtility.DestroyChildren(slot);
        }
    }
}