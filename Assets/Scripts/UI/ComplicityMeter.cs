using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ComplicityMeter : MonoBehaviour
{
    [SerializeField]
    private Slider complicitySlider;

    private void Awake()
    {
        complicitySlider = GetComponent<Slider>();
    }

    public void UpdateComplicity()
    {
        complicitySlider.value = ServiceLocator.Instance.GetPlayer().Complicity;
    }

    private void Update()
    {
        UpdateComplicity();
    }
}
