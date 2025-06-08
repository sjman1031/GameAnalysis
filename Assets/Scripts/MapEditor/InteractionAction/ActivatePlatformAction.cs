using System.Collections;
using UnityEngine;

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