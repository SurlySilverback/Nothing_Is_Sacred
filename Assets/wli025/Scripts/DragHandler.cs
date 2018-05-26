using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Good good;
	[SerializeField]
	private Transform canvasTransform;

	private Vector3 startPosition;
	private Transform startParent;

    #region IBeginDragHandler implementation

    public void OnBeginDrag (PointerEventData eventData)
    {
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
	    GetComponent<CanvasGroup>().blocksRaycasts = true;
	    if (transform.parent == canvasTransform)
        {
			transform.SetParent(startParent);
		    transform.position = startPosition;
	    }
	}
	
	#endregion
}