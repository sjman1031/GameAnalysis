using Enums;
using System;
using UnityEngine;

[Serializable]
public struct ConnectionEntry
{
    [Tooltip("�� ������Ʈ�� �浹���� �� ������ ��� ������Ʈ")]
    public Saveable target;

    [Tooltip("������ �׼��� �巡���ϼ���")]
    public InteractionAction action;
}
