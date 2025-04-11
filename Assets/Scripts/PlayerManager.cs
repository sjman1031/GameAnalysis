using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private void Awake()
    {
        //if(Instance == null) Instance = this;
        //else Destroy(gameObject);
    }
   

    public ePlayerState playerState { get; set; }
    
    public event Action<Collision2D, GameObject> OnChildCollisionEnter;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnChildCollisionEnter?.Invoke(collision, this.gameObject);
    }
}
