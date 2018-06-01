using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CityUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField]
    private GameObject cityUI;
    [SerializeField]
    private GameObject cityView;
    public UnityEvent OnChangeCityView;

    private void Awake()
    {
        if (OnChangeCityView == null)
        {
            OnChangeCityView = new UnityEvent();
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

    public void SetCityView(City current)
    {

    }
}
