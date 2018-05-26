using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Swap behaviour?
// Inventory groups
public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private InventoryUI inventoryUI;
    
    private bool IsEmpty()
    {
        return (transform.childCount == 0);
    }

	#region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (IsEmpty())
        {
            eventData.pointerDrag.transform.SetParent(transform);
            Inventory inventory = inventoryUI.GetInventory();
            inventory.AddGood(eventData.pointerDrag.GetComponent<DragHandler>().good);
        }
    }

    public Good GetGood()
    {
        Inventory inventory = inventoryUI.GetInventory();
        return inventory.GetGood(transform.GetSiblingIndex());
    }
	#endregion
}