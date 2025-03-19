using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb1; // WASD�� ������ ������Ʈ�� Rigidbody2D
    public Rigidbody2D rb2; // ����Ű�� ������ ������Ʈ�� Rigidbody2D

    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    public bool isGrounded1 = false;
    public bool isGrounded2 = false;

    private void Update()
    {
        MoveObject(rb1, KeyCode.W, KeyCode.A, KeyCode.D, ref isGrounded1);
        MoveObject(rb2, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, ref isGrounded2);
    }

    void MoveObject(Rigidbody2D rb, KeyCode up, KeyCode left, KeyCode right, ref bool isGrounded)
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(left))
            moveDirection.x -= 1;
        if (Input.GetKey(right))
            moveDirection.x += 1;

        rb.velocity = moveDirection.normalized * moveSpeed;

        if (Input.GetKey(up) && isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ٴ� �浹 ����
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                // �浹 ������ ������Ʈ �Ʒ����̸� �ٴ� �浹�� ����
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (contact.point.y < collision.otherCollider.transform.position.y)
                    {
                        if (collision.otherCollider.gameObject == rb1.gameObject)
                            isGrounded1 = true;
                        else if (collision.otherCollider.gameObject == rb2.gameObject)
                            isGrounded2 = true;
                    }
                }
            }
        }
    }

}
