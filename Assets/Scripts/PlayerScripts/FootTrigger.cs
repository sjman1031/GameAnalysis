using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    public PlayerController owner;

    private void Awake()
    {
        owner = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Head"))
            owner.onGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Head"))
            owner.onGround = false;
    }
}