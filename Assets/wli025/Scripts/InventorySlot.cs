using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Swap behaviour?
// Inventory groups
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public GameObject GetItem()
    {
        return (transform.childCount > 0) ? transform.GetChild(0).gameObject : null;
    }


	#region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (GetItem() != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
        }
    }
	#endregion
}