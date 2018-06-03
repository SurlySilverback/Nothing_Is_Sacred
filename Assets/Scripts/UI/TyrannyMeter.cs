using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TyrannyMeter : MonoBehaviour
{
    private Slider complicitySlider;

    private void Start()
    {
        complicitySlider = GetComponent<Slider>();
        ServiceLocator.Instance.GetMainGovernment().OnChangeTyranny.AddListener(UpdateTyranny);
    }

    public void UpdateTyranny()
    {
        complicitySlider.value = ServiceLocator.Instance.GetMainGovernment().CurrentTyranny;
    }
}