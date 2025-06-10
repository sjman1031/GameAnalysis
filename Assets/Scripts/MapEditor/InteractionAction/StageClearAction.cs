using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/StageClearAction")]
public class StageClearAction : InteractionAction
{
    public override bool Execute(GameObject source, GameObject target)
    {
        if(target.tag == "Player")
        {
            GameManager.Instance.SaveStageClear();
            return true;
        }

        return false;
    }
}