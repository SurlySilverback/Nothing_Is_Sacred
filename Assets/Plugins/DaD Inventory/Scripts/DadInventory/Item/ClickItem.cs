using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This item will operate double click on it.
/// </summary>
public class ClickItem : MonoBehaviour, IPointerClickHandler
{
	private static float clickTimeout = 0.5f;										// Max timeout between two clicks

	private Coroutine clickCoroutine = null;										// Waiting for second click

	/// <summary>
	/// Raises the pointer click event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick(PointerEventData eventData)
	{
		if (clickCoroutine == null)													// On first click
		{
			clickCoroutine = StartCoroutine(ClickCoroutine());						// Start to wait second click
		}
		else 																		// On second click
		{
			StopCoroutine(clickCoroutine);
			clickCoroutine = null;
			// Notify application about double click
			SendMessageUpwards("OnItemClick", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	/// <summary>
	/// Wait for second click.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator ClickCoroutine()
	{
		yield return new WaitForSeconds(clickTimeout);
		clickCoroutine = null;
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
