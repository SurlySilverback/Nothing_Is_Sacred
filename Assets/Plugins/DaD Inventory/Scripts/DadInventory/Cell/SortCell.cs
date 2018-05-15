using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This cell will drop only allowed items.
/// </summary>
public class SortCell : MonoBehaviour
{
	[Tooltip("List with allowed items sorts")]
	public List<string> allowedItemTypes = new List<string>();							// List with allowed items sorts
	[Tooltip("Highlight this cell when allowed item is dragging (only if sorts specified)")]
	public bool highlight = false;														// Highlight this cell when allowed item is dragging (only if sorts specified)
	[Tooltip("Part of cell that will be highlighted")]
	public Image highlightImage;														// Part of cell that will be highlighted
	[Tooltip("Highlight color")]
	public Color highlightColor = Color.white;											// Highlight color
	[Tooltip("Blink period while highlight")]
	public float highlightBlinkPeriod = 1f;												// Blink period while highlight

	private Color originColor = Color.white;											// Origin color of highlighted image

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		DadItem.OnItemDragStartEvent += OnAnyItemDragStart;         					// Handle any item drag start
		DadItem.OnItemDragEndEvent += OnAnyItemDragEnd;             					// Handle any item drag end

		if (highlightImage == null)														// If no highlight image specified - use Image of this GO
		{
			highlightImage = GetComponent<Image>();
		}
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		DadItem.OnItemDragStartEvent -= OnAnyItemDragStart;
		DadItem.OnItemDragEndEvent -= OnAnyItemDragEnd;

		StopAllCoroutines();                                       						// Stop all coroutines if there is any
	}

	/// <summary>
	/// Raises the any item drag start event.
	/// </summary>
	/// <param name="item">Item.</param>
	private void OnAnyItemDragStart(GameObject item)
	{
		if (highlight == true && highlightImage != null && item != null)
		{
			if (allowedItemTypes.Count > 0 && IsSortAllowed(item) == true)				// Allowed item is dragged
			{
				StartCoroutine(HighlightCoroutine());									// Highlight cell
			}
		}
	}

	/// <summary>
	/// Raises the any item drag end event.
	/// </summary>
	/// <param name="item">Item.</param>
	private void OnAnyItemDragEnd(GameObject item)
	{
		if (item != null)
		{
			StopAllCoroutines();
		}
		if (highlight == true && highlightImage != null)
		{
			highlightImage.color = originColor;
		}
	}

	/// <summary>
	/// Highlights this cell.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator HighlightCoroutine()
	{
		if (highlightImage != null)
		{
			originColor = highlightImage.color;
			float counter = 0f;
			while (true)
			{
				// Set color to highlightColor an back to originColor with highlightBlinkPeriod
				highlightImage.color = Color.Lerp(originColor, highlightColor, Mathf.PingPong(counter, highlightBlinkPeriod / 2f));
				yield return new WaitForFixedUpdate();
				counter += Time.fixedDeltaTime;
			}
		}
	}

	/// <summary>
	/// Gets the sort item from this cell.
	/// </summary>
	/// <returns>The sort item.</returns>
	public SortItem GetSortItem()
	{
		SortItem res = null;
		GameObject item = GetComponent<DadCell>().GetItem();
		if (item != null)
		{
			res = item.GetComponent<SortItem>();
		}
		return res;
	}

	/// <summary>
	/// Check if item may be placed into this cell.
	/// </summary>
	/// <returns><c>true</c> if this instance is sort allowed the specified item; otherwise, <c>false</c>.</returns>
	/// <param name="item">Item.</param>
	public bool IsSortAllowed(GameObject item)
	{
		bool res = false;
		if (item != null)
		{
			SortItem sortItem = item.GetComponent<SortItem>();
			if (allowedItemTypes.Count <= 0)
			{
				res = true;
			}
			else
			{
				if (sortItem != null)
				{
					foreach (string itemType in allowedItemTypes)
					{
						// If item has allowed sort
						if (itemType == sortItem.itemType)
						{
							res = true;
							break;
						}
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Determines if is item allowed the specified sort.
	/// </summary>
	/// <returns><c>true</c> if is sort allowed the specified cell item; otherwise, <c>false</c>.</returns>
	/// <param name="cell">Cell.</param>
	/// <param name="item">Item.</param>
	public static bool IsSortAllowed(GameObject cell, GameObject item)
	{
		bool res = false;
		if (cell != null && item != null)
		{
			res = true;
			SortCell sortCell = cell.GetComponent<SortCell>();
			if (sortCell != null)
			{
				res = sortCell.IsSortAllowed(item);
			}
		}
		return res;
	}

	/// <summary>
	/// Raises the DaD cell event event.
	/// </summary>
	/// <param name="desc">Desc.</param>
	public void OnDadCellEvent(DadCell.DadEventDescriptor desc)
	{
		switch (desc.triggerType)
		{
		case DadCell.TriggerType.DragCellRequest:
			GameObject swapItem = desc.destinationCell.GetItem();
			if (swapItem != null)
			{
				// Check for sort in destination cell
				if (IsSortAllowed(swapItem) == false)
				{
					desc.cellPermission = false;
				}
			}
			break;
		case DadCell.TriggerType.DropCellRequest:
			// Check for sort in source cell
			if (IsSortAllowed(desc.sourceCell.GetItem()) == false)
			{
				desc.cellPermission = false;
			}
			break;
		}
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
