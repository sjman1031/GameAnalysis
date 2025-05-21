using System.Collections.Generic;
using System;
using UnityEngine;
using Enums;

// ���� ������ ������Ʈ���� ���� ������Ʈ
public class Saveable :MonoBehaviour
{
    public string prefabName;
    public string id;
    public List<ConnectionEntry> connections;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach(var entry in connections)
        {
            if (entry.target == null || entry.action == null)
            {
                Debug.LogWarning($"[{name}] ConnectionEntry�� ����ֽ��ϴ�.");
                continue;
            }

            entry.action.Execute(gameObject, entry.target.gameObject);
        }
    }
}
