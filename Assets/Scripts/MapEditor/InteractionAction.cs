using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractionAction : ScriptableObject
{
    public abstract void Execute(GameObject source, GameObject target);
}

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

[CreateAssetMenu(menuName = "Actions/ActivatePlatformAction")]
public class ActivatePlatformAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("ActivatePlatformAction: target이 설정되지 않았습니다.");
            return;
        }

        target.SetActive(true);
    }
}

[CreateAssetMenu(menuName = "Actions/ActivateDashAction")]
public class ActivateDashAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        if (source.GetComponent<PlayerController>() == null)
        {
            Debug.LogWarning("ActivateDashAction: PlayerController가 없습니다.");
            return;
        }

        if (this.name == "Lucy_Dash")
        {
            if (source.name == "Lucy")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash 활성화");
                Destroy(this);
                return;
            }
        }
        else if (this.name == "Paul_Dash")
        {
            if (source.name == "Paul")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash 활성화");
                Destroy(this);
                return;
            }
        }
        else if (this.name == "Common_Dash")
        {
            source.GetComponent<PlayerController>().canDash = true;
            Debug.Log($"{source.name} Dash 활성화");
            Destroy(this);
            return;
        }
    }
}

[CreateAssetMenu(menuName = "Actions/ExtendRopeLengthAction")]
public class ExtendRopeLengthAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        GameObject lucyGO = GameObject.Find("Lucy");

        if (lucyGO == null)
        {
            Debug.LogWarning("ExtendLengthAction: Lucy GameObject가 없습니다.");
            return;
        }

        lucyGO.GetComponent<DistanceJoint2D>().distance += 2f;
    }
}