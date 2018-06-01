using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CityUI : MonoBehaviour
{
    [Header("Inventories")]
    [SerializeField]
    private InventoryViewModel peopleInventory;
    [SerializeField]
    private InventoryViewModel govInventory;
    [SerializeField]
    private InventoryViewModel storeHouseInventory;
    [Space(10)]

    [Header("City Stats")]
    [SerializeField]
    private TextMeshProUGUI cityName;
    [SerializeField]
    private Slider drugsMeter;
    [SerializeField]
    private Slider exoticsMeter;
    [SerializeField]
    private Slider foodMeter;
    [SerializeField]
    private Slider ideasMeter;
    [SerializeField]
    private Slider medicineMeter;
    [SerializeField]
    private Slider peopleMeter;
    [SerializeField]
    private Slider textilesMeter;
    [SerializeField]
    private Slider waterMeter;
    [SerializeField]
    private Slider weaponsMeter;
    [Space(10)]

    public UnityEvent OnChangeCityView;

    private void Awake()
    {
        if (OnChangeCityView == null)
        {
            OnChangeCityView = new UnityEvent();
        }
    }

    public void SetCity(City current)
    {
        OnChangeCityView.Invoke();
    }
}
