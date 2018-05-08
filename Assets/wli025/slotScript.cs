using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class slotScript : MonoBehaviour, IDropHandler {
	public GameObject item {
		get {
			if(transform.childCount > 0){ //if it has a child
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}

	#region IDropHandler implementation

	 public void OnDrop (PointerEventData eventData){
		 if(!item){ //if there already exists an item
			dragHandler.itemBeingDragged.transform.SetParent(transform);
		 }
	 }
	
	#endregion
}
