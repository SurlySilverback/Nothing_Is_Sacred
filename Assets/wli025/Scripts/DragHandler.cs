using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	Transform canvasTransform;

	Vector3 startPosition;
	Transform startParent;
    public static GameObject itemBeingDragged;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
    {
	 	itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		transform.SetParent(canvasTransform);
	    GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
	
	#endregion

	#region IDragHandler implementation

    public void OnDrag (PointerEventData eventData)
    {
        transform.position = eventData.position;
	}
	
	#endregion
	
	#region IEndnDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
    {
	    itemBeingDragged = null;
	    GetComponent<CanvasGroup>().blocksRaycasts = true;
	    if (transform.parent == canvasTransform)
        {
			transform.SetParent(startParent);
		    transform.position = startPosition;

	    }
	}
	
	#endregion
}