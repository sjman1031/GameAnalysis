using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    // 변경

    public Transform startPos;  // 시작점(Red)
    public Transform endPos;    // 끝점(Blue)

    public int curveResolution = 20; // 곡선의 부드러운 정도(베지어 곡선 점 개수)
    public float curveHeight = 2.0f; // 곡선이 휘어지는 정도

    public LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = curveResolution;
    }

    private void Update()
    {
        DrawBezierCurve();
    }

    public void DrawBezierCurve()
    {
        Vector3 p0 = startPos.position;                          // 시작점
        Vector3 p2 = endPos.position;                            // 끝점
        Vector3 p1 = (p0 + p2) / 2 + Vector3.down * curveHeight; // 중간점 (아래로 휘어지게 설정)

        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            Vector3 curvePoint = BezierCurve(p0, p1, p2, t);

            lineRenderer.SetPosition(i, curvePoint);
        }
    }

    // 2차 베지어 곡선 계산 함수
    public Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
    }
}
