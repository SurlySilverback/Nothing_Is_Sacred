using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float money;
    [SerializeField]
    private float complicity;

    public UnityEvent OnGameOver;
    public UnityEvent OnMoneyChanged;
    public UnityEvent OnComplicityChanged;

    [Space(10)]
    [Header("Unit")]
    [SerializeField]
    private GameObject unit;
    [SerializeField]
    private float unitPrice;

    [SerializeField]
    Transform capitalCity;

    // Stores the running total of the Player's money.
    public float Money
    {
        get
        {
            return this.money;
        }
        set
        {
            if (!Mathf.Approximately(this.money, value))
            {
                if (value >=0)
                {
                    this.money = value;
                    OnMoneyChanged.Invoke();
                }
            }
        }
    }

    public float Complicity
    {
        get
        {
            return this.complicity;
        }
        set
        {
            if (!Mathf.Approximately(this.money, value))
            {
                OnComplicityChanged.Invoke();
            }
            this.complicity = value;
        }
    }

    private void Awake()
    {
        if (OnGameOver == null)
        {
            OnGameOver = new UnityEvent();
        }
        if (OnMoneyChanged == null)
        {
            OnMoneyChanged = new UnityEvent();
        }
        if (OnComplicityChanged == null)
        {
            OnComplicityChanged = new UnityEvent();
        }
    }

    private void OnDestroy()
    {
        OnGameOver.Invoke();
    }

    public bool BuyGood(Good g)
    {
        float price = ServiceLocator.Instance.GetViewInfo().SelectedMarket.GetPrice(g);
        if (Money - price >= 0)
        {
            Money -= price;
            return true;
        }
        return false;
    }

    public bool CanBuyGood(Good g)
    {
        float price = ServiceLocator.Instance.GetViewInfo().SelectedMarket.GetPrice(g);
        return (Money - price >= 0);
    }

    public void SellGood(Good g)
    {
        float price = ServiceLocator.Instance.GetViewInfo().SelectedMarket.GetPrice(g);
        Money += price;
    }

    public void PurchaseUnit()
    {
        if (Money >= unitPrice)
        {
            Money -= unitPrice;
            Instantiate(unit, new Vector3(capitalCity.position.x, capitalCity.position.y, -5), Quaternion.identity);
        }
    }
}