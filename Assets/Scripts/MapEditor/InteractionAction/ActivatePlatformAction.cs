using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ActivatePlatformAction")]
public class ActivatePlatformAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("ActivatePlatformAction: target�� �������� �ʾҽ��ϴ�.");
            return;
        }

        target.SetActive(true);
    }
}