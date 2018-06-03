using UnityEngine;

// IMPORTANT NOTE*: Assumes UI element is NOT moving elsewhere
// If UI element IS moving, alter 'ToggleView' method by updating 
// 'maximizedPosition' and 'minimizedPosition'
[RequireComponent(typeof(RectTransform))]
public class MinimizeUI : MonoBehaviour
{
    [SerializeField]
    private Transform minimizeButton;
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private Transform maximizedTransform;
    [SerializeField]
    private Transform minimizedTransform;

    // Total time of animation
    private float timeFrame;
    // Total time passed in animation
    private float timePassed;

    private enum UIState { Stationary, MovingUp, MovingDown };
    private UIState state;

	private Vector3 maximizedPosition;
	private Vector3 minimizedPosition;

    private void Awake()
    {
        this.maximizedPosition = maximizedTransform.position;
        this.minimizedPosition = minimizedTransform.position;
        this.timeFrame = animationCurve[animationCurve.length - 1].time;
        this.state = UIState.Stationary;
        this.timePassed = 0;
    }

    // Toggles which direction to move the UI - Minimize or Maximize
    public void ToggleView(bool isVisible)
    {
        int direction = isVisible ? 1 : -1;
        minimizeButton.localScale = new Vector3(1, direction, 1);
        this.timePassed = (state == UIState.Stationary) ? 0 : this.timeFrame - this.timePassed;
        this.state = isVisible ? UIState.MovingUp : UIState.MovingDown;
    }
    
    private void UpdateAnimation(float t)
    {
        this.timePassed += Time.unscaledDeltaTime;
        transform.position = Vector3.Lerp(maximizedPosition, minimizedPosition, animationCurve.Evaluate(t));
        if (this.timePassed >= this.timeFrame)
        {
            this.state = UIState.Stationary;
            this.timePassed = 0;
        }
    }

    private void Update()
    {
        switch (this.state)
        {
            case UIState.MovingDown:
                UpdateAnimation(this.timePassed / this.timeFrame);
                break;
            case UIState.MovingUp:
                UpdateAnimation(1 - (this.timePassed / this.timeFrame));
                break;
            case UIState.Stationary:
                break;
        }
	}
}