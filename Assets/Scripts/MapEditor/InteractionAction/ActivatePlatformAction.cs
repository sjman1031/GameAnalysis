using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ActivatePlatformAction")]
public class ActivatePlatformAction : InteractionAction
{
    public enum eObjectType { Lucy, Paul, Common, NULL };

    public eObjectType type = eObjectType.NULL;

    public void UsePlatform(GameObject target)
    {
        if (target.activeSelf == false)
        {
            target.SetActive(true);
            return;
        }
        else
        {
            target.SetActive(false);
            return;
        }
    }


    public override void Execute(GameObject source, GameObject target)
    {
        Debug.Log($"[ActivatePlatformAction] asset={name}, type={type}, source={source.name}");
        if (target == null)
        {
            Debug.LogWarning("ActivatePlatformAction: target이 설정되지 않았습니다.");
            return;
        }

        if (type == eObjectType.Lucy && source.name == "Lucy")
            UsePlatform(target);
        else if (type == eObjectType.Paul && source.name == "Paul")
            UsePlatform(target);
        else if (type == eObjectType.Common)
            UsePlatform(target);

    }
}
