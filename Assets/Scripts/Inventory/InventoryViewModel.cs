using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(InventoryView))]
public class InventoryViewModel : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private InventoryView view;
    private InventoryModel inventory;
    private bool shouldUpdate;

    private void Awake()
    {
        this.view = GetComponent<InventoryView>();
        shouldUpdate = true;
    }

    public void SetInventory(InventoryModel inventory)
    {
        if (this.inventory != null)
        {
            this.inventory.OnInventoryChange.RemoveListener(UpdateView);
        }
        this.inventory = inventory;
        this.inventory.OnInventoryChange.AddListener(UpdateView);
        this.view.DestroyInventory();
        this.view.InitInventory(this, inventory.GetEntireInventory());
    }

    private void UpdateView()
    {
        if (!shouldUpdate)
        {
            // Debug.Log("NOT Updating view");
            shouldUpdate = true;
            return;
        }
        // Debug.Log("Updating view");
        view.AssignInventory(inventory.GetEntireInventory());
    }

    public bool TryDropItem(InventoryViewModel source, int sourceIndex, int destinationIndex)
    {
        if (source == this)
        {
            shouldUpdate = false;
            inventory.SwapGood(sourceIndex, destinationIndex);
            return true;
        }
        InventoryModel seller = source.inventory;
        Good item = seller.GetGood(sourceIndex);
        Assert.IsNotNull(item);
        if (seller.CanPlayerTrade(inventory) && inventory.CanAddGood(item, destinationIndex))
        {
            // player is the buyer
            if (inventory.IsPlayerOwned)
            {
                if (!player.CanBuyGood(item))
                {
                    return false;
                }
                else
                {
                    player.BuyGood(item);
                }
            }
            else
            {
                player.SellGood(item);
            }
            shouldUpdate = false;
            source.shouldUpdate = false;
            seller.TradeGoodTo(inventory, sourceIndex, destinationIndex);
            return true;
        }
        return false;
    }
}