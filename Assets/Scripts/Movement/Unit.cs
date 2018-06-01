using UnityEngine;

[RequireComponent(typeof(Deploy), typeof(DrawCurve), typeof(LineRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Unit : MonoBehaviour
{
    public string UnitName;
    [SerializeField]
    private float baseSpeed = 10.0f;					    // Determines the base speed of the unit on roads.

    [Space(10f)]
    [Header("Inventory")]
    [SerializeField]
    private int inventorySize;
    [SerializeField]
    private float weightCap;

    public InventoryModel Items { get; private set; }
    public int Heat { get; set; }                   // Determines how aggressively the government will chase after this unit.
    public int Subtlety { get; set; }               // Determines how effective the unit is at avoiding detection.
    private Deploy deploy;
	private DrawCurve drawCurve;
	private LineRenderer linerenderer;

    private void Awake()
    {
        Items = new InventoryModel(inventorySize, weightCap, true);
        this.deploy = GetComponent<Deploy>();
		this.drawCurve = GetComponent<DrawCurve>();
		this.linerenderer = GetComponent<LineRenderer>();
    }
    
	private void CallStartMove()
    {
		Vector3[] positions = new Vector3[linerenderer.positionCount];
		linerenderer.GetPositions (positions);
		deploy.StartMove (positions, baseSpeed);
	}

    // Use this for initialization
    private void Start()
	{
		drawCurve.OnStartDrawing.AddListener (delegate { deploy.StopMove (); });
		drawCurve.OnEndDrawing.AddListener(delegate{CallStartMove();});
	}

	private void Update()
    {
	}
}
