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
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private GameObject inventoryGrid;
    public UnityEvent OnChangeInventory;


    private void Awake()
    {
        if (OnChangeInventory == null)
        {
            OnChangeInventory = new UnityEvent();
        }
        this.canvasGroup = GetComponent<CanvasGroup>();
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

    public void SetInventory(Inventory current)
    {

    }
}
