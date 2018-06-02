using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DayNight : MonoBehaviour
{
    private Image mapOverlay;
    [SerializeField]
    private Color night;
    [SerializeField]
    private float timeFrame;

    private void Awake()
    {
        this.mapOverlay = GetComponent<Image>();
    }

    // Use this for initialization
    private void Start ()
    {
        Color day = night;
        day.a = 0;

        InGameTime clock = ServiceLocator.Instance.GetClock();
        clock.OnDay.AddListener(delegate { StartCoroutine(StartAnimation(night, day)); });
        clock.OnNight.AddListener(delegate { StartCoroutine(StartAnimation(day, night)); });
        mapOverlay.color = day;
    }

    private IEnumerator StartAnimation(Color start, Color end)
    {
        float timeStep = .01f;
        for (float animationTime = 0; animationTime < timeFrame; animationTime += timeStep)
        {
            mapOverlay.color = Color.Lerp(start, end, animationTime / timeFrame);
            yield return new WaitForSeconds(timeStep);
        }
    }
}
