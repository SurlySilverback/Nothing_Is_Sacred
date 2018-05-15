using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    // [SerializeField, Range(100, 10000)]
    // private float minViewSize;
    // [SerializeField, Range(100, 10000)]
    // private float maxViewSize;
    // [SerializeField, Range(1, 1000)]
    // private float zoomSpeed;
    // [SerializeField, Range(1, 100000)]
    // private float panSpeed;

    [SerializeField] 
    Transform[] targets;

    [SerializeField] 
    private float minViewSize = 2000;

    [SerializeField] 
    private float maxViewSize = 6669;

    [SerializeField] 
    private float zoomSpeed = 20f;

    [SerializeField] 
    private float panSpeed = 4000;

    [SerializeField] 
    float boundingBoxPadding = 2f;
    private Camera myCamera;
    private float originalZoomSize;

    private void OnValidate()
    {
        if (minViewSize > maxViewSize)
        {
            minViewSize = maxViewSize;
        }
    }

    private void Awake()
    {
        this.myCamera = GetComponent<Camera>();
        originalZoomSize = this.myCamera.orthographicSize;
        myCamera.orthographic = true;
    }

    private void Update()
    {
        // Zooming
        float scrollMovement = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - scrollMovement, minViewSize, maxViewSize);
        // Panning
        Vector3 position = this.transform.localPosition;
        float panChange = Time.deltaTime * panSpeed * (this.myCamera.orthographicSize / originalZoomSize);

        const float minX = -16940;
        const float maxX = 16940;
        const float minY = -14014;
        const float maxY = 14014;

        Rect boundingBox = Rect.MinMaxRect(minX, minY, maxX, maxY);
        
        // if (Input.GetKey(KeyCode.W) && position.y < boundingBox.yMax)
        if (Input.GetKey(KeyCode.W) && position.y < boundingBox.yMax)
        {
            position.y = position.y + panChange;
        }
        // if (Input.GetKey(KeyCode.S) && position.y > boundingBox.yMin)
        if (Input.GetKey(KeyCode.S) && position.y > boundingBox.yMin)
        {
            position.y = position.y - panChange;
        }
        // if (Input.GetKey(KeyCode.A) && position.x > boundingBox.xMin)
        if (Input.GetKey(KeyCode.A) && position.x > boundingBox.xMin)
        {
            position.x = position.x - panChange;
        }
        // if (Input.GetKey(KeyCode.D) && position.x < boundingBox.xMax)
        if (Input.GetKey(KeyCode.D) && position.x < boundingBox.xMax)
        {
            position.x = position.x + panChange;
        }

        // Vector3 boundingBoxOffset = CalculateCameraPosition(boundingBox);
        // boundingBoxOffset.z = 0; 
        // myCamera.orthographicSize = CalculateOrthographicSize(boundingBox);
        
        this.transform.localPosition = position;
    }

    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector2 boundingBoxCenter = boundingBox.center;
        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, -10f);
    }

    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = myCamera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
        Vector3 topRightAsViewport = myCamera.WorldToViewportPoint(topRight);
       
        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / myCamera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(myCamera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minViewSize, Mathf.Infinity);
    }

}
