using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Dummy inventory control for demo scene.
/// </summary>
public class DummyInventoryControl : MonoBehaviour
{
	[Tooltip("Equipments cells sheet")]
	public GameObject equipment;											// Equipments cells sheet
	[Tooltip("Inventory cells sheet")]
	public GameObject inventory;											// Inventory cells sheet
	[Tooltip("Vendor cells sheet")]
	public GameObject vendor;												// Vendor cells sheet
	[Tooltip("Inventory stack group")]
	public StackGroup inventoryStackGroup;									// Inventory stack group

	private PriceGroup priceGroup;											// Player's price group

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		priceGroup = GetComponentInParent<PriceGroup>();
		Debug.Assert(equipment && inventory && vendor && inventoryStackGroup && priceGroup, "Wrong settings");
		priceGroup.ShowPrices(vendor.activeSelf);
	}

	/// <summary>
	/// Show/Hide the equipments.
	/// </summary>
	public void ToggleEquipment()
	{
		if (equipment.activeSelf == false)
		{
			equipment.SetActive(true);
			priceGroup.ShowPrices(vendor.activeSelf);
		}
		else
		{
			equipment.SetActive(false);
		}
	}

	/// <summary>
	/// Show/Hide the inventory.
	/// </summary>
	public void ToggleInventory()
	{
		if (inventory.activeSelf == false)
		{
			inventory.SetActive(true);
			priceGroup.ShowPrices(vendor.activeSelf);
		}
		else
		{
			inventory.SetActive(false);
		}
	}

	/// <summary>
	/// Show/Hide the vendor.
	/// </summary>
	public void ToggleVendor()
	{
		if (vendor.activeSelf == false)
		{
			vendor.SetActive(true);
			priceGroup.ShowPrices(true);
		}
		else
		{
			vendor.SetActive(false);
			priceGroup.ShowPrices(false);
		}
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// On click
		if (Input.GetMouseButtonDown(0) == true)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, results);
			// If clicked not on UI
			if (results.Count <= 0)
			{
				// Raycast to colliders2d
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
				if (hit.collider != null)
				{
					DummyItemPickUp dummyItemPickUp = hit.collider.gameObject.GetComponent<DummyItemPickUp>();
					if (dummyItemPickUp != null)
					{
						// Hitted on DummyItemPickUp item
						// Get stack item from DummyItemPickUp item
						StackItem stackItem = dummyItemPickUp.PickUp(inventoryStackGroup.GetAllowedSpace(dummyItemPickUp.itemPrefab));
						if (stackItem != null)
						{
							// Try to place item into inventory
							dummyItemPickUp.stack -= inventoryStackGroup.AddItem(stackItem, stackItem.GetStack());
							// Show item price if vendor is active
							priceGroup.ShowPrices(vendor.activeSelf);
						}
					}
				}
			}
		}
	}
}
