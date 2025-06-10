using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/StageClearAction")]
public class StageClearAction : InteractionAction
{
    public override void Execute(GameObject source, GameObject target)
    {
        if(target.tag == "Player")
        {
            GameManager.Instance.SaveStageClear();
        }
    }
}