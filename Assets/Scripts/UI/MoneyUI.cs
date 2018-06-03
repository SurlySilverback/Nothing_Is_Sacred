using UnityEngine;
using TMPro;
using System;

public class MoneyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyUI;
    private Player player;
    
	private void Start()
    {
        player = ServiceLocator.Instance.GetPlayer();
        player.OnMoneyChanged.AddListener(UpdateMoneyUI);
        moneyUI.text = "MONEY: $" + Math.Round(player.Money, 2);
	}
	
    public void UpdateMoneyUI()
    {
        moneyUI.text = "MONEY: $" + Math.Round(player.Money, 2);
    }
}