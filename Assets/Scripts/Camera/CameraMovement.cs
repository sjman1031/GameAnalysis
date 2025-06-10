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

    private void Awake()
    {
        player1 = GameObject.Find("Lucy").transform;
        player2 = GameObject.Find("Paul").transform;
    }

    private void LateUpdate()
    {
        if (player1 == null) player1 = GameObject.Find("Lucy").transform;
        if (player2 == null) player2 = GameObject.Find("Paul").transform;

        if (player1 == null || player2 == null) return;

        Vector3 centerPoint = (player1.position + player2.position) * 0.5f;
        Vector3 desiredPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
