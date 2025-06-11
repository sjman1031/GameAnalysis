using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/StageClearAction")]
public class StageClearAction : InteractionAction
{
    private GameManager cachedGM;

    public override bool Execute(GameObject source, GameObject target)
    { 

        if(target.tag == "Player")
        {
            if(cachedGM == null)
                cachedGM = GameObject.FindWithTag("GameManager")
                 ?.GetComponent<GameManager>();

            if (cachedGM != null)
            {
                cachedGM.OnMapClear();
                return true;
            }
            else
            {
                Debug.LogWarning("GameManager를 찾을 수 없습니다.");
            }
        }

        return false;
    }
}