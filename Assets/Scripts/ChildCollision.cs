using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollision : MonoBehaviour
{
    public event Action<Collision2D, GameObject> OnChildCollisionEnter;
    public ePlayerState playerState;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnChildCollisionEnter?.Invoke(collision, this.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
