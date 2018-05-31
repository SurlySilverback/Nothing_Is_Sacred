using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

// Inventory groups
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Inventory Store { get; set; }
    [SerializeField]
    private Player player;
    
    private bool IsEmpty()
    {
        return (transform.childCount == 0);
    }
    
	#region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        DragHandler item = UnityUtility.GetSafeComponent<DragHandler>(eventData.pointerDrag);
        InventorySlot originalSlot = item.CurrentSlot.GetComponent<InventorySlot>();
        Inventory sourceInv = originalSlot.Store;
        int itemPosition = originalSlot.transform.GetSiblingIndex();

        if (IsEmpty())
        {
            // Trade item
            if (!Store.CanPlayerTrade(sourceInv))
            {
                return;
            }
            if (sourceInv.IsPlayerOwned)
            {
                player.SellGood(item.good);
                Store.AddGood(item.good, transform.GetSiblingIndex());
                sourceInv.RemoveGood(itemPosition);
                item.transform.SetParent(transform);
            }
            else if (player.TryToBuyGood(item.good))
            {
                Store.AddGood(item.good, transform.GetSiblingIndex());
                sourceInv.RemoveGood(itemPosition);
                item.transform.SetParent(transform);
            }
        }
        else if (sourceInv == Store)
        {
            Store.SwapGood(itemPosition, transform.GetSiblingIndex());
            Transform currentItem = transform.GetChild(0);
            item.transform.SetParent(transform);
            currentItem.SetParent(originalSlot.transform);
        }
    }

    public Good GetGood()
    {
        return Store.GetGood(transform.GetSiblingIndex());
    }
	#endregion
}