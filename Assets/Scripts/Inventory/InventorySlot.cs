using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryViewModel Controller { get; set; }

    public bool IsEmpty()
    {
        return (transform.childCount == 0);
    }
    
	#region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        DragHandler item = eventData.pointerDrag.GetComponent<DragHandler>();
        if (item == null)
        {
            return;
        }
        InventorySlot originalSlot = item.CurrentSlot.GetComponent<InventorySlot>();
        if (Controller.TryDropItem(originalSlot.Controller, originalSlot.transform.GetSiblingIndex(), transform.GetSiblingIndex()))
        {
            if (!IsEmpty())
            {
                transform.GetChild(0).SetParent(originalSlot.transform);
            }
            item.transform.SetParent(transform);
        }
    }
	#endregion
}