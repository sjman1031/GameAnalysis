using System.Collections;
using UnityEngine;

public abstract class InteractionAction : ScriptableObject
{
    public abstract bool Execute(GameObject source, GameObject target);
}
