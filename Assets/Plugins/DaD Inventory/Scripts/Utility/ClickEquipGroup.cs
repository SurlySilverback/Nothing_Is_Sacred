using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place item into specified stack group on double click.
/// </summary>
public class ClickEquipGroup : MonoBehaviour
{
	[Tooltip("Target stack group in which target will be placed")]
	public StackGroup targetStackGroup;											// Target stack group in which target will be placed

	private StackGroup myStackGroup;											// My stack group

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		myStackGroup = GetComponentInParent<StackGroup>();
		Debug.Assert(targetStackGroup && myStackGroup, "Wrong settings");
	}

	/// <summary>
	/// Raises the item click event.
	/// </summary>
	/// <param name="item">Item.</param>
	public void OnItemClick(GameObject item)
	{
		if (item != null)
		{
			StackItem stackItem = item.GetComponent<StackItem>();
			if (stackItem != null)
			{
				// Try to lace item into free space of specified stack group
				if (targetStackGroup.AddItem(stackItem, stackItem.GetStack()) <= 0)
				{
					// If group have no free space for item
					// Get similar items in that group
					List<StackItem> similarItems = targetStackGroup.GetSimilarStackItems(stackItem);
					if (similarItems.Count > 0)
					{
						// Try to replace with first similar item
						targetStackGroup.ReplaceItems(similarItems[0], stackItem, myStackGroup);
					}
				}
			}
		}
	}
}
