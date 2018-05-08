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
	// Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	// RaycastHit hit;

	// Use this for initialization
	void Start () {
		myImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		//  if(Input.GetMouseButtonDown(0)){
		// 	print("got mouse button click!");
		// 	myImage.sprite = highlightImage;
		// }
		// if (Input.GetMouseButtonDown(0)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     // Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
        //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);
        //     if (hit != null) {
		// 		Debug.Log(hit.collider.gameObject.name);
        //         hit.collider.attachedRigidbody.AddForce(Vector2.up);
        //     }
        // }
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
		}
		else if (highlighted){
			myImage.sprite = image;
			highlighted = false;
		}

	}
}
