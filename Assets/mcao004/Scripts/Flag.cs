using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flag : MonoBehaviour
{
	private SpriteRenderer sprite;
	[SerializeField]
	private City city;
    
	private void ChangeFlagColor()
    {
		City.CityState state = city.State;
        switch (state)
        {
            case City.CityState.AntiGovtDemonstrations:
                sprite.color = new Color32(255, 255, 205, 98);
                break;
            case City.CityState.AntiGovtSentiments:
                sprite.color = new Color32(255, 248, 128, 255); // Light Orange
                break;
            case City.CityState.CivilWar:
				sprite.color = new Color32(255, 255, 128, 128); // Salmon
                break;
            case City.CityState.EndingDemonstrations:
				sprite.color = new Color32(255, 229, 99, 0);
                break;
            case City.CityState.Genocide:
				sprite.color = new Color32(255, 200, 0, 0);
                break;
            case City.CityState.HighBlackmarketeering:
				sprite.color = new Color32(255, 128, 255, 149); // Light green
                break;
            case City.CityState.Infiltrating:
				sprite.color = new Color32(255, 178, 0, 229);
                break;
            case City.CityState.Normal:
                sprite.color = Color.white;
                break;
            case City.CityState.Pacifying:
                sprite.color = new Color32(255, 14, 0, 189);
                break;
            case City.CityState.Raiding:
                sprite.color = new Color32(255, 0, 176, 9);
                break;
            case City.CityState.Unrest:
                sprite.color = new Color32(255, 137, 128, 255); // Light Blue
                break;
        }
	}
    
	private void Awake()
    {
		sprite = GetComponent<SpriteRenderer>();
	}

    private void Start()
    {
        city.OnChangeState.AddListener(ChangeFlagColor);
    }
}