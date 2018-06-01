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
	[SerializeField] private InventoryViewModel peopleInventory; 
	[SerializeField] private InventoryViewModel govInventory; 
	[SerializeField] private InventoryViewModel storeHouseInventory; 
	[Space(10)]

	[Header("Unit View")]
	[SerializeField] private InventoryView unitInventory;

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
                        TestInventory c = UnityUtility.GetSafeComponent<TestInventory>(i.transform.parent.parent.gameObject);
                        peopleInventory.SetInventory(c.inventory);
                        govInventory.SetInventory(c.inventory1);
                        storeHouseInventory.SetInventory(c.inventory2);
					}
					if (i.transform.gameObject.layer == LayerMask.NameToLayer("Unit")) {
                        unitInventory.gameObject.SetActive(true);
						Debug.Log("FOUND Unit");
					}
				}
			}
        }
	}
}