using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ActivateDashAction")]
public class ActivateDashAction : InteractionAction
{
    public eObjectType type = eObjectType.NULL;

    public override bool Execute(GameObject source, GameObject target)
    {
        if (source.GetComponent<PlayerController>() == null)
        {
            Debug.LogWarning("ActivateDashAction: PlayerController가 없습니다.");
            return false;
        }

        if (type == eObjectType.Lucy)
        {
            if (source.name == "Lucy")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash 활성화");
                return true;
            }
        }
        else if (type == eObjectType.Paul)
        {
            if (source.name == "Paul")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash 활성화");
                return true;
            }
        }
        else if (type == eObjectType.Common) 
        {
            source.GetComponent<PlayerController>().canDash = true;
            Debug.Log($"{source.name} Dash 활성화");
            return true;
        }

        return false;
    }
}
