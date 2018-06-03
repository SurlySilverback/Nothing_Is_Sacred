using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CityUI : MonoBehaviour
{
    [SerializeField]
    private Toggle cityToggle;

    [Header("Inventories")]
    [SerializeField]
    private InventoryUI peopleInventory;
    [SerializeField]
    private InventoryUI govInventory;
    [SerializeField]
    private InventoryUI storeHouseInventory;
    [Space(10)]

    [Header("City Stats")]
    [SerializeField]
    private TextMeshProUGUI cityPopulation;
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
    private Slider textilesMeter;
    [SerializeField]
    private Slider waterMeter;
    [SerializeField]
    private Slider weaponsMeter;
    [Space(10)]

    public UnityEvent OnChangeCityView;
    private City city;

    private void Awake()
    {
        if (OnChangeCityView == null)
        {
            OnChangeCityView = new UnityEvent();
        }
    }

    // TODO: Change this to be event driven
    private void Update()
    {
        if (city != null)
        {
            drugsMeter.value = city.GetSupply(Good.GoodType.Drugs);
            exoticsMeter.value = city.GetSupply(Good.GoodType.Exotics);
            foodMeter.value = city.GetSupply(Good.GoodType.Food);
            ideasMeter.value = city.GetSupply(Good.GoodType.Ideas);
            medicineMeter.value = city.GetSupply(Good.GoodType.Medicine);
            textilesMeter.value = city.GetSupply(Good.GoodType.Textiles);
            waterMeter.value = city.GetSupply(Good.GoodType.Water);
            weaponsMeter.value = city.GetSupply(Good.GoodType.Weapons);
            cityPopulation.text = "Pop.: " + city.Population;
        }
    }

    public void SetCity(City current)
    {
        if (city == current)
        {
            return;
        }
        this.city = current;
        // Text
        cityName.text = current.CityName;
        cityPopulation.text = "Pop.: " + city.Population;
        // Meters
        drugsMeter.maxValue = city.MaxDrugsSupply;
        exoticsMeter.maxValue = city.MaxExoticsSupply;
        foodMeter.maxValue = city.MaxFoodSupply;
        ideasMeter.maxValue = city.MaxIdeasSupply;
        medicineMeter.maxValue = city.MaxMedicineSupply;
        textilesMeter.maxValue = city.MaxTextilesSupply;
        waterMeter.maxValue = city.MaxWaterSupply;
        weaponsMeter.maxValue = city.MaxWeaponsSupply;
        // Inventories
        peopleInventory.SetInventory(city.PeoplesInventory);
        govInventory.SetInventory(city.GovtInventory);
        storeHouseInventory.SetInventory(city.PlayerInventory);
        // Event
        cityToggle.isOn = true;
        OnChangeCityView.Invoke();
    }
}