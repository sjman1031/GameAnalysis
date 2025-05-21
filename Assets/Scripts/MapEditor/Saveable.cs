using System.Collections.Generic;
using System;
using UnityEngine;
using Enums;

// 저장 가능한 오브젝트에만 붙힐 컴포넌트
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
                Debug.LogWarning($"[{name}] ConnectionEntry가 비어있습니다.");
                continue;
            }

            entry.action.Execute(gameObject, entry.target.gameObject);
        }
    }
}
