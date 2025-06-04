using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    [Header("카메라 부드러운 이동")]
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [Header("카메라 확대/축소")]
    public float minZoom = 6f;
    public float maxZoom = 15f;
    public float zoomLimiter = 6f;
    public float zoomSmoothTime = 0.25f;
    private Camera cam;
    private float currentZoom;


    private void Awake()
    {
        player1 = GameObject.Find("Lucy").transform;
        player2 = GameObject.Find("Paul").transform;

        cam = GetComponent<Camera>();
        currentZoom = cam.orthographicSize;
    }

    private void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        Vector3 centerPoint = (player1.position + player2.position) * 0.5f;
        Vector3 desiredPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        float distance = Vector3.Distance(player1.position, player2.position);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime / zoomSmoothTime);
    }
}
