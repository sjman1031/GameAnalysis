using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    private PlayerManager() { }

    public static PlayerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new PlayerManager();
            }

            return _instance;
        }    
    }

    private void Awake()
    {
        DataManager.Instance.LoadDatas("asd");
    }

    private ePlayerState _state { get; set; }

}