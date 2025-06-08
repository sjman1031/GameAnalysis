using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/OpenWall")]
public class OpenWallAction : InteractionAction
{
    [Header("속도 설정")]
    public float openHeight = 5f;
    public float openDuration = 1f;

    public override void Execute(GameObject source, GameObject target)
    {
        CoroutineRunner.Instance.Run(OpenRoutine(target.transform));
    }

    private IEnumerator OpenRoutine(Transform wallTransform)
    {
        Vector3 start = wallTransform.position;
        Vector3 end = start + Vector3.up * openHeight;

        float t = 0;
        while (t < openDuration)
        {
            wallTransform.position = Vector3.Lerp(start, end, t / openDuration);
            t += Time.deltaTime;
            yield return null;
        }

        wallTransform.position = end;
    }
}