using System.Collections;
using UnityEngine;

public abstract class InteractionAction : ScriptableObject
{
    public abstract void Execute(GameObject source, GameObject target);
}
