using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ActivateDashAction")]
public class ActivateDashAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        if (source.GetComponent<PlayerController>() == null)
        {
            Debug.LogWarning("ActivateDashAction: PlayerController�� �����ϴ�.");
            return;
        }

        if (this.name == "Lucy_Dash")
        {
            if (source.name == "Lucy")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash Ȱ��ȭ");
                Destroy(this);
                return;
            }
        }
        else if (this.name == "Paul_Dash")
        {
            if (source.name == "Paul")
            {
                source.GetComponent<PlayerController>().canDash = true;
                Debug.Log($"{source.name} Dash Ȱ��ȭ");
                Destroy(this);
                return;
            }
        }
        else if (this.name == "Common_Dash")
        {
            source.GetComponent<PlayerController>().canDash = true;
            Debug.Log($"{source.name} Dash Ȱ��ȭ");
            Destroy(this);
            return;
        }
    }
}
