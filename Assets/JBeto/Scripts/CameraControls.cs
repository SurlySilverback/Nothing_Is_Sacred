using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    [SerializeField, Range(1, 10000)]
    private float minViewSize;
    [SerializeField, Range(1, 10000)]
    private float maxViewSize;
    [SerializeField, Range(1, 1000)]
    private float zoomSpeed;
    [SerializeField, Range(1, 100000)]
    private float panSpeed;
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
    }

    private void Update()
    {
        // Zooming
        float scrollMovement = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - scrollMovement, minViewSize, maxViewSize);
        // Panning
        Vector3 position = this.transform.localPosition;
        float zoomChange = Time.deltaTime * panSpeed * (this.myCamera.orthographicSize / originalZoomSize);

        if (Input.GetKey(KeyCode.W))
        {
            position.y = position.y + zoomChange;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.y = position.y - zoomChange;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x = position.x - zoomChange;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x = position.x + zoomChange;
        }
        this.transform.localPosition = position;
    }
}
