using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryUI UI { get; set; }

    private bool IsEmpty()
    {
        return (transform.childCount == 0);
    }
    
	#region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        DragHandler item = eventData.pointerDrag.GetComponent<DragHandler>();
        // If we clicked on empty slot
        if (item == null)
        {
            return;
        }
        InventorySlot originalSlot = item.CurrentSlot.GetComponent<InventorySlot>();
        // If item can be dropped to current slot
        if (UI.TryDropItem(originalSlot.UI, originalSlot.transform.GetSiblingIndex(), transform.GetSiblingIndex()))
        {
            if (!IsEmpty())
            {
                // Then swap item
                transform.GetChild(0).SetParent(originalSlot.transform);
            }
            item.transform.SetParent(transform);
        }
    }
	#endregion
}