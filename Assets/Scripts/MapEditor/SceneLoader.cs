using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SceneLoader : MonoBehaviour
{
    const string MAPS_FILE = "MapsData";

    public InteractionAction[] allActions;
    private Dictionary<string, InteractionAction> actionDict;

    void Awake()
    {
        actionDict = allActions.ToDictionary(a => a.name, a => a);
    }

    public void LoadMap(string mapName)
    {
        CleanScene();

        var db = DataManager.Instance.LoadData<MapDataBase>(MAPS_FILE) ?? new MapDataBase();
        var map = db.maps.Find(m => m.mapName == mapName);

        if (map == null)
        {
            Debug.LogWarning($"¸Ê '{mapName}' ¾øÀ½");
            return;
        }

        var spawned = new Dictionary<string, GameObject>();
        foreach (var od in map.objects)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/" + od.prefabName);
            if (prefab == null) continue;

            var go = Instantiate(prefab);
            go.name = od.instanceName;
            go.transform.position    = new Vector3(od.posX, od.posY, od.posZ);
            go.transform.eulerAngles = new Vector3(od.rotX, od.rotY, od.rotZ);
            go.transform.localScale  = new Vector3(od.scaleX, od.scaleY, od.scaleZ);
            go.SetActive(od.isActive);

            var s = go.GetComponent<Saveable>();
            s.id = od.id;
            spawned[od.id] = go;
        }

        foreach (var od in map.objects)
        {
            var s = spawned[od.id].GetComponent<Saveable>();
            s.connections.Clear();
            if (od.connections)
            {
                foreach (var cd in od.connections)
                {
                    if (!spawned.ContainsKey(cd.targetId) || !actionDict.ContainsKey(cd.actionType))
                        continue;

                    s.connections.Add(new ConnectionEntry
                    {
                        target = spawned[cd.targetId].GetComponent<Saveable>(),
                        action = actionDict[cd.actionType]
                    });
                } 
            }
        }

        Debug.Log($"¸Ê '{mapName}' ·Îµå ¿Ï·á");
    }

    private void CleanScene()
    {
        foreach (var s in FindObjectsOfType<Saveable>())
            Destroy(s.gameObject);

        foreach (var grid in FindObjectsOfType<Grid>())
            Destroy(grid.gameObject);
    }
}