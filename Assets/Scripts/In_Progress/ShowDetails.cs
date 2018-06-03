using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDetails : MonoBehaviour
{
    public IMarket SelectedMarket { get; private set; }

    [Header("City View")]
    [SerializeField]
    private CityUI cityUI;
    [Space(10)]

    [Header("Unit View")]
    [SerializeField]
    private UnitUI unitUI;
    
    private void Update () 
	{
		//If the left mouse button is clicked.
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
			if (hit.Length > 0)
			{ 
				foreach (RaycastHit2D i in hit) 
				{
<<<<<<< HEAD
                    if (i.transform.gameObject.layer == LayerMask.NameToLayer("City")) {
                        // TODO
                        unitInventory.gameObject.SetActive(false);
                        City c = UnityUtility.GetSafeComponent<City>(i.transform.parent.parent.gameObject);
                        peopleInventory.SetInventory(c.PeoplesInventory);
                        govInventory.SetInventory(c.GovtInventory);
                        storeHouseInventory.SetInventory(c.PlayerInventory);
                        SelectedMarket = c;
					}
					if (i.transform.gameObject.layer == LayerMask.NameToLayer("Unit")) {
                        unitInventory.gameObject.SetActive(true);
                        Unit u = UnityUtility.GetSafeComponent<Unit>(i.transform.gameObject);
                        unitInventory.SetInventory(u.Items);
						Debug.Log("FOUND Unit");
=======
                    if (i.transform.gameObject.layer == LayerMask.NameToLayer("City"))
                    {
                        City city = UnityUtility.GetSafeComponent<City>(i.transform.parent.parent.gameObject);
                        cityUI.SetCity(city);
                        SelectedMarket = city;
					}
					if (i.transform.gameObject.layer == LayerMask.NameToLayer("Unit"))
                    {
                        Unit unit = UnityUtility.GetSafeComponent<Unit>(i.transform.gameObject);
                        unitUI.SetUnit(unit);
>>>>>>> master
					}
				}
			}
        }
	}
}