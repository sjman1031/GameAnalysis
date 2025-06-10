using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ExtendRopeLengthAction")]
public class ExtendRopeLengthAction : InteractionAction
{
    public override bool Execute(GameObject source, GameObject target)
    {
        GameObject lucyGO = GameObject.Find("Lucy");

        if (lucyGO == null)
        {
            Debug.LogWarning("ExtendLengthAction: Lucy GameObject가 없습니다.");
            return false;
        }

        lucyGO.GetComponent<DistanceJoint2D>().distance += 2f;
        return true;
    }
}