using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public UnityEvent BeforeChangeItem;
    public Inventory Items { get; set; }

    private void Awake()
    {
        if (BeforeChangeItem == null)
        {
            BeforeChangeItem = new UnityEvent();
        }
    }

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
        if (TryDropItem(originalSlot, originalSlot.transform.GetSiblingIndex(), transform.GetSiblingIndex()))
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

    // Changes the local representation of inventory to correspond to visual representation
    public bool TryDropItem(InventorySlot originalSlot, int sourceIndex, int destinationIndex)
    {
        Inventory inventory = originalSlot.Items;
        if (inventory == Items)
        {
            BeforeChangeItem.Invoke();
            originalSlot.BeforeChangeItem.Invoke();
            Items.SwapGood(sourceIndex, destinationIndex);
            return true;
        }
        Good item = inventory.GetGood(sourceIndex);
        Assert.IsNotNull(item);
        if (inventory.CanPlayerTrade(Items) && Items.CanAddGood(item, destinationIndex))
        {
            Player player = ServiceLocator.Instance.GetPlayer();
            // player is the buyer
            if (Items.IsPlayerOwned)
            {
                if (!player.CanBuyGood(item))
                {
                    return false;
                }
                else if (!inventory.IsPlayerOwned)
                {
                    player.BuyGood(item);
                }
            }
            else
            {
                player.SellGood(item);
            }
            BeforeChangeItem.Invoke();
            originalSlot.BeforeChangeItem.Invoke();
            inventory.TradeGoodTo(Items, sourceIndex, destinationIndex);
            return true;
        }
        return false;
    }
}