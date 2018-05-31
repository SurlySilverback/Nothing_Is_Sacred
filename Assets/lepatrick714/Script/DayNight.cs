using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DayNight : MonoBehaviour
{
    [Inject]
    private DateTime dateTime;
    [SerializeField]
    private Image mapOverlay;
    [SerializeField]
    private Color night;
    [SerializeField]
    private AnimationCurve dayNightTransition;

    // Use this for initialization
    private void Start ()
    {
        Color day = night;
        day.a = 0;
        
        dateTime.OnDay.AddListener(delegate { StartCoroutine(StartAnimation(night, day)); });
        dateTime.OnNight.AddListener(delegate { StartCoroutine(StartAnimation(day, night)); });
        mapOverlay.color = day;
    }

    private IEnumerator StartAnimation(Color start, Color end)
    {
        float timeStep = .01f;
        float timeFrame = dayNightTransition[dayNightTransition.length - 1].time;
        for (float animationTime = 0; animationTime < timeFrame; animationTime += timeStep)
        {
            mapOverlay.color = Color.Lerp(start, end, dayNightTransition.Evaluate(animationTime / timeFrame));
            yield return new WaitForSeconds(timeStep);
        }
    }
}
