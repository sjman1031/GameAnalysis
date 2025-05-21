using Enums;
using System;
using UnityEngine;

[Serializable]
public struct ConnectionEntry
{
    [Tooltip("이 오브젝트가 충돌했을 때 실행할 대상 오브젝트")]
    public Saveable target;

    [Tooltip("실행할 액션을 드래그하세요")]
    public InteractionAction action;
}
