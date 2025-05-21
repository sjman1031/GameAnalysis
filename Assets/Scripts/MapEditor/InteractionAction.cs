using System.Collections;
using UnityEngine;

public abstract class InteractionAction : ScriptableObject
{
    public abstract void Execute(GameObject source, GameObject target);
}

[CreateAssetMenu(menuName = "Actions/OpenWall")]
public class OpenWallAction : InteractionAction
{
    [Header("�ӵ� ����")]
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
            Debug.LogWarning("ActivatePlatformAction: target�� �������� �ʾҽ��ϴ�.");
            return;
        }

        target.SetActive(true); 
    }
}