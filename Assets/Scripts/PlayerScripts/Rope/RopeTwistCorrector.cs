using UnityEngine;

public class RopeTwistCorrector : MonoBehaviour
{
    public Rigidbody2D[] ropeSegments;
    public float minSegmentDistance = 0.33f;

    private void FixedUpdate()
    {
        for(int i = 1; i < ropeSegments.Length; i++)
        {
            Vector2 prevPos = ropeSegments[i - 1].position;
            Vector2 currPos = ropeSegments[i].position;
            Vector2 dir = (currPos - prevPos).normalized;
            float distance = Vector2.Distance(currPos, prevPos);


            if (distance < minSegmentDistance * 0.5f)
            {
                Vector2 correction = dir * (minSegmentDistance - distance) * 0.5f;
                ropeSegments[i - 1].MovePosition(prevPos - correction);
                ropeSegments[i].MovePosition(currPos + correction);
            }

            if (currPos.y > prevPos.y)
            {
                float swapY = (prevPos.y + currPos.y) * 0.5f;
                ropeSegments[i - 1].MovePosition(new Vector2(prevPos.x, swapY - minSegmentDistance / 2f));
                ropeSegments[i].MovePosition(new Vector2(currPos.x, swapY + minSegmentDistance / 2f));
            }
        }
    }

}