using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public UnityEvent OnChangeItem;
    public Inventory Items { get; set; }

    private void Awake()
    {
        if (OnChangeItem == null)
        {
            OnChangeItem = new UnityEvent();
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
        if (TryDropItem(originalSlot.Items, originalSlot.transform.GetSiblingIndex(), transform.GetSiblingIndex()))
        {
            if (!IsEmpty())
            {
                // Then swap item
                transform.GetChild(0).SetParent(originalSlot.transform);
            }
            OnChangeItem.Invoke();
            originalSlot.OnChangeItem.Invoke();
            item.transform.SetParent(transform);
        }
    }
    #endregion

    // Changes the local representation of inventory to correspond to visual representation
    public bool TryDropItem(Inventory source, int sourceIndex, int destinationIndex)
    {
        if (source == Items)
        {
            Items.SwapGood(sourceIndex, destinationIndex);
            return true;
        }
        Good item = source.GetGood(sourceIndex);
        Assert.IsNotNull(item);
        if (source.CanPlayerTrade(Items) && Items.CanAddGood(item, destinationIndex))
        {
            Player player = ServiceLocator.Instance.GetPlayer();
            // player is the buyer
            if (Items.IsPlayerOwned)
            {
                if (!player.CanBuyGood(item))
                {
                    return false;
                }
                else if (!source.IsPlayerOwned)
                {
                    player.BuyGood(item);
                }
            }
            else
            {
                player.SellGood(item);
            }
            source.TradeGoodTo(Items, sourceIndex, destinationIndex);
            return true;
        }
        return false;
    }
}