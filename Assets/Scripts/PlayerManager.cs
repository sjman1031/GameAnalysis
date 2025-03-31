using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;

                if (_instance == null)
                    Debug.Log("No Player Manager");
            }

            return _instance;
        }    
    }

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
    }

    private ePlayerState _state { get; set; };

}