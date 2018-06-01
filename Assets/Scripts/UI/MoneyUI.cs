using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyUI;
    [SerializeField]
    private Player player;
    
	private void Start()
    {
        player.OnMoneyChanged.AddListener(UpdateMoneyUI);
	}
	
    public void UpdateMoneyUI()
    {
        moneyUI.text = player.Money.ToString();
    }
}