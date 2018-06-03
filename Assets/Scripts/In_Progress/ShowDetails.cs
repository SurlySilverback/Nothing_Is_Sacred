using UnityEngine;
using UnityEngine.EventSystems;

public class PlainMarket : IMarket
{
    public float GetPrice(Good g)
    {
        return 10f;
    }
}

public class ShowDetails : MonoBehaviour
{
    public IMarket SelectedMarket { get; private set; }

	[Header("City View")]
	[SerializeField] private InventoryUI peopleInventory; 
	[SerializeField] private InventoryUI govInventory; 
	[SerializeField] private InventoryUI storeHouseInventory; 
	[Space(10)]

	[Header("Unit View")]
	[SerializeField] private InventoryUI unitInventory;

    private void Awake()
    {
        SelectedMarket = new PlainMarket();
    }

    // Update is called once per frame
    void Update () 
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
					}
				}
			}
        }
	}
}