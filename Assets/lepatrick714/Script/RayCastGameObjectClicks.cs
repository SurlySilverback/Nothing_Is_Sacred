using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGameObjectClicks : MonoBehaviour {
	// Update is called once per frame
	void Update () 
	{
		//If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
			if (hit.Length > 0)
			{ 
				foreach (RaycastHit2D i in hit) 
				{ 
					if (i.transform.gameObject.layer == LayerMask.NameToLayer("City")) {
						Debug.Log("FOUND CITY");
					}
					if (i.transform.gameObject.layer == LayerMask.NameToLayer("Unit")) {
						Debug.Log("FOUND Unit");
					}
					// if (i.transform.gameObject.layer == LayerMask.NameToLayer(""))
				}
			}
        }
	}
}
