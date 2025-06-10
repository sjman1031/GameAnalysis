using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ActivatePlatformAction")]
public class ActivatePlatformAction : InteractionAction
{ 
    public enum eObject { Button, Lever, NULL }

    public eObjectType type = eObjectType.NULL;
    public eObject thisObject = eObject.NULL;
    public int cnt = 0;

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

        if (cnt >= 1 && thisObject == eObject.Button)
            return;

        if (type == eObjectType.Lucy && source.name == "Lucy")
            UsePlatform(target);
        else if (type == eObjectType.Paul && source.name == "Paul")
            UsePlatform(target);
        else if (type == eObjectType.Common)
            UsePlatform(target);

        cnt++;
    }
}
