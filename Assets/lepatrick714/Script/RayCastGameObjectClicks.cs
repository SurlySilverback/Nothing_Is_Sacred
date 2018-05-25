using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGameObjectClicks : MonoBehaviour {
	[Header("City View")]
	[SerializeField] private CityUI cityUi; 
	[SerializeField] private InventoryUI peopleInventory; 
	[SerializeField] private InventoryUI govInventory; 
	[SerializeField] private InventoryUI storeHouseInventory; 
	[Space(10)]

	[Header("Unit View")]
	[SerializeField] private InventoryUI unitInventory; 
	[SerializeField] private InventoryUI unitInformation; 

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
						City c = i.transform.gameObject.GetComponent<City>();
						cityUi.SetCityView(c);
						// peopleInventory.SetInventory(c.pplInventory);
						// govInventory.SetInventory(c.govInventory);
					}
					if (i.transform.gameObject.layer == LayerMask.NameToLayer("Unit")) {
						Debug.Log("FOUND Unit");
						unitInventory.SetInventory(i.transform.gameObject.GetComponent<Unit>().Items); 
					}
				}
			}
        }
	}
}
