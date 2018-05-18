using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
[RequireComponent(typeof(CanvasGroup))]
public class InventoryUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public UnityEvent OnChangeInventory;
    private Inventory currentInventory;
    [SerializeField]
    private GameObject slot; 
    [SerializeField]
    private GridLayoutGroup grid;
    [SerializeField]
    private List<GameObject> sprites;
    [SerializeField]
    private List<Good> goods;
    private Dictionary<Good, GameObject> goodToSprite;

    private void Awake()
    {
        if (OnChangeInventory == null)
        {
            OnChangeInventory = new UnityEvent();
        }
        this.canvasGroup = GetComponent<CanvasGroup>();
        goodToSprite = new Dictionary<Good, GameObject>();
        for(int i = 0; i < sprites.Count; ++i){
            goodToSprite.Add(goods[i],sprites[i]);
        }
    }

    public void ToggleView(bool isVisible)
    {
        if (isVisible)
        {
            this.canvasGroup.alpha = 0;
            this.canvasGroup.blocksRaycasts = false;
        }
        else
        {
            this.canvasGroup.alpha = 1;
            this.canvasGroup.blocksRaycasts = true;
        }
    }

    // Based on the current inventory that should be displayed, SetInventory will update the view and set currentInventory

    public void SetInventory(Inventory next)
    {
        //Set the private variable to the current inventory
        //update the view
        int currentNumElements = currentInventory.Size;

        for(int i = 0; i < currentInventory.Size; ++i){
            Destroy(grid.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < next.Size; ++i){
            Instantiate(slot, grid.transform);
        }

        Good[] allGoods = next.GetEntireInventory();
        for (int i = 0; i < next.Size; ++i){
            var currentSlot = grid.transform.GetChild(i);
            Good currentGood = allGoods[i];
            if(currentGood != null) {
                GameObject visual = goodToSprite[currentGood];
                Instantiate(visual, currentSlot); 
            }
        }
    }   
}
