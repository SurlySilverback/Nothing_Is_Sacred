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
                sprite.color = new Color32(255, 205, 98, 255);
                break;
            case City.CityState.AntiGovtSentiments:
                sprite.color = new Color32(248, 128, 255, 255); // Light Orange
                break;
            case City.CityState.CivilWar:
				sprite.color = new Color32(255, 128, 128, 255); // Salmon
                break;
            case City.CityState.EndingDemonstrations:
				sprite.color = new Color32(229, 99, 0, 255);
                break;
            case City.CityState.Genocide:
				sprite.color = new Color32(200, 0, 0, 255);
                break;
            case City.CityState.HighBlackmarketeering:
				sprite.color = new Color32(128, 255, 149, 255); // Light purple
                break;
            case City.CityState.Infiltrating:
				sprite.color = new Color32(178, 0, 229, 255);
                break;
            case City.CityState.Normal:
                sprite.color = Color.white;
                break;
            case City.CityState.Pacifying:
                sprite.color = new Color32(14, 0, 189, 255);
                break;
            case City.CityState.Raiding:
                sprite.color = new Color32(0, 176, 9, 255);
                break;
            case City.CityState.Unrest:
                sprite.color = new Color32(137, 128, 255, 255); // Light Blue
                break;
        }

        Debug.Log(state);
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