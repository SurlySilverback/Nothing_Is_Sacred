using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clickedHighlight : MonoBehaviour, IPointerClickHandler {
	public Sprite highlightImage;
	public Sprite image;
	Image myImage;
	bool highlighted = false;
	private static Image lastImage;

	public currentlySelected selected;

	// Use this for initialization
	void Start () {
		myImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnPointerClick(PointerEventData eventData){
		print("got mouse button click!");
		if (lastImage != null) {
			lastImage.sprite = image;
		}
		
		lastImage = myImage;
		if(!highlighted && (transform.childCount > 0)){
			myImage.sprite = highlightImage;
			highlighted = true;
			selected.CurrentItem = this.transform.GetChild(0).GetComponent<Image>();
		}
		else if (highlighted){
			myImage.sprite = image;
			highlighted = false;
			// selected.Cur = null;
		}

		
	}
}
