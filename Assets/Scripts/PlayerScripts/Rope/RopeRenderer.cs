using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    // ����

    public Transform startPos;  // ������(Red)
    public Transform endPos;    // ����(Blue)

    public int curveResolution = 20; // ��� �ε巯�� ����(������ � �� ����)
    public float curveHeight = 2.0f; // ��� �־����� ����

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
        Vector3 p0 = startPos.position;                          // ������
        Vector3 p2 = endPos.position;                            // ����
        Vector3 p1 = (p0 + p2) / 2 + Vector3.down * curveHeight; // �߰��� (�Ʒ��� �־����� ����)

        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            Vector3 curvePoint = BezierCurve(p0, p1, p2, t);

            lineRenderer.SetPosition(i, curvePoint);
        }
    }

    // 2�� ������ � ��� �Լ�
    public Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
    }
}
