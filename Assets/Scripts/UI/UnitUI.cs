using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField]
    private Toggle unitToggle;
    private Unit unit;
    [SerializeField]
    private InventoryUI unitInventory;
    public UnityEvent OnChangeUnit;

    private void Awake()
    {
        if (OnChangeUnit == null)
        {
            OnChangeUnit = new UnityEvent();
        }
    }

    private void Start()
    {
        SetUnit(ServiceLocator.Instance.GetPlayer().GetComponent<Unit>());
    }

    public void SetUnit(Unit current)
    {
        if (this.unit != current) OnChangeUnit.Invoke();
        this.unit = current;
        unitToggle.isOn = true;
        unitInventory.SetInventory(this.unit.Items);
    }
}