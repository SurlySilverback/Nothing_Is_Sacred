using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
	private Transform canvasTransform;
	private Vector3 startPosition;
    public Transform CurrentSlot { get; private set; }

    #region IBeginDragHandler implementation
    private void Awake()
    {
        // I know, this is insanity
        canvasTransform = transform.parent.parent.parent.parent.parent.parent.parent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemUI.SetGood(transform.parent.GetComponent<InventorySlot>().Items.GetGood(transform.parent.GetSiblingIndex()));
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
		startPosition = transform.position;
		CurrentSlot = transform.parent;
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
			transform.SetParent(CurrentSlot);
		    transform.position = startPosition;
	    }
	}

    #endregion
}