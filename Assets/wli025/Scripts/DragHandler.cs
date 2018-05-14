using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	Vector3 startPosition;
	Transform startParent;
    // GameObject itemBeingDragged;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
    {
	 	// itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
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
	    // itemBeingDragged = null;
	    GetComponent<CanvasGroup>().blocksRaycasts = true;
        
	    if (transform.parent == startParent)
        {
		    transform.position = startPosition;
	    }
	}
	
	#endregion
}