using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyUI;
    private Player player;
    
	private void Start()
    {
        player = ServiceLocator.Instance.GetPlayer();
        player.OnMoneyChanged.AddListener(UpdateMoneyUI);
	}
	
    public void UpdateMoneyUI()
    {
        moneyUI.text = player.Money.ToString();
    }
}