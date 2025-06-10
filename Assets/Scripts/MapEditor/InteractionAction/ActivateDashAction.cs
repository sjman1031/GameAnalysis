using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ActivateDashAction")]
public class ActivateDashAction : InteractionAction
{
    public eObjectType type = eObjectType.NULL;

    public override void Execute(GameObject source, GameObject target)
    {
        if (source.GetComponent<PlayerController>() == null)
        {
            Debug.LogWarning("ActivateDashAction: PlayerController가 없습니다.");
            return;
        }

        if (type == eObjectType.Lucy)
        {
            if (source.name == "Lucy")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash 활성화");
                Destroy(this);
                return;
            }
        }
        else if (type == eObjectType.Paul)
        {
            if (source.name == "Paul")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash 활성화");
                Destroy(this);
                return;
            }
        }
        else
        {
            source.GetComponent<PlayerController>().canDash = true;
            Debug.Log($"{source.name} Dash 활성화");
            Destroy(this);
            return;
        }
    }
}
