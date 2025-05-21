using System.Collections;
using UnityEngine;

public abstract class InteractionAction : ScriptableObject
{
    public abstract void Execute(GameObject source, GameObject target);
}

[CreateAssetMenu(menuName = "Actions/OpenWall")]
public class OpenWallAction : InteractionAction
{
    [Header("속도 설정")]
    public float openHeigt = 5f;
    public float openDuration = 1f;

    public override void Execute(GameObject source, GameObject target)
    {
        CouroutineRunner.Instance.StartCoroutine(OpenRoutine(target.transform));
    }

    private IEnumerator OpenRoutine(Transform wallTransform)
    {
        Vector3 start   = wallTransform.position; 
        Vector3 end     = start + Vector3.up * openHeigt;

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

[CreateAssetMenu(menuName = "Actions/ActivatePlatformAction")]
public class ActivatePlatformAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        if(target == null)
        {
            Debug.LogWarning("ActivatePlatformAction: target이 설정되지 않았습니다.");
            return;
        }

        target.SetActive(true); 
    }
}