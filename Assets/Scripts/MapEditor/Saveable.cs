using System.Collections.Generic;
using System;
using UnityEngine;

// ���� ������ ������Ʈ���� ���� ������Ʈ
public class Saveable : MonoBehaviour
{
    public string prefabName;
    public string id;
    public List<ConnectionEntry> connections;

    public bool isSaveable = true;

    private void Reset()
    {
        //var col = GetComponent<Collider>();
        //if(col != null)    
        //    col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        foreach (var entry in connections)
        {
            if (entry.target == null || entry.action == null)
            {
                Debug.LogWarning($"[{name}] ConnectionEntry�� ����ֽ��ϴ�.");
                continue;
            }

            entry.action.Execute(collision.gameObject, entry.target.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        foreach (var entry in connections)
        {
            if (entry.target == null || entry.action == null)
            {
                Debug.LogWarning($"[{name}] ConnectionEntry�� ����ֽ��ϴ�.");
                continue;
            }

            entry.action.Execute(collision.gameObject, entry.target.gameObject);
        }
    }
}
