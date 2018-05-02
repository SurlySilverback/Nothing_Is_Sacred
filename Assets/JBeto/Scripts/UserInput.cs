using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    private int layerMask;
    [SerializeField]
    private List<string> layersToHit;
    [SerializeField]
    private Text layerText;

    private void Awake()
    {
        this.layerMask = LayerMask.GetMask(this.layersToHit.ToArray());
        Debug.Log("Layer Mask: " + layerMask);
    }

    private void Update()
    {
		if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, 0, layerMask);
            if (hit)
            {
                // Outputs which layer got hit
                layerText.text = "Object: " + LayerMask.LayerToName(hit.transform.gameObject.layer);
            }
        }
    }
}