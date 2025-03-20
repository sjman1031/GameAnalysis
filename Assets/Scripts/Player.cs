using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb1; // WASD로 움직일 오브젝트의 Rigidbody2D
    public Rigidbody2D rb2; // 방향키로 움직일 오브젝트의 Rigidbody2D

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public bool isJumping1 = false;
    public bool isJumping2 = false;

    public ChildCollision[] children;

    public DistanceJoint2D  groundJoint;
    public SpringJoint2D    onAirJoint;

    private void Start()
    {
        children    = GetComponentsInChildren<ChildCollision>();

        groundJoint = GetComponentInChildren<DistanceJoint2D>();
        onAirJoint  = GetComponentInChildren<SpringJoint2D>();
        
        foreach(ChildCollision child in children)
        {
            child.OnChildCollisionEnter += IsObjectOnGround;
        }
    }

    private void Update()
    {
        MoveObject(rb1, KeyCode.W, KeyCode.A, KeyCode.D, ref isJumping1);
        MoveObject(rb2, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, ref isJumping2);

        if(CheckOnAir(rb1) || CheckOnAir(rb2))
        {
            groundJoint.enabled = false;    
            onAirJoint.enabled  = true;
        }
        else
        {
            groundJoint.enabled = true;
            onAirJoint.enabled  = false;
        }
    }

    void MoveObject(Rigidbody2D rb, KeyCode up, KeyCode left, KeyCode right, ref bool isJumping)
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(left))
            moveDirection.x -= 1;
        if (Input.GetKey(right))
            moveDirection.x += 1;

        if (Input.GetKey(up) && isJumping == false)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
        }

        rb.velocity = new Vector2(moveDirection.normalized.x * moveSpeed, rb.velocity.y);
    }

    private bool CheckOnAir(Rigidbody2D rb1)
    {
        return false;
    }

    private void IsObjectOnGround(Collision2D collision, GameObject gameObject)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") 
        {
            if(gameObject.name == "Red")
                isJumping1 = false;
            if(gameObject.name == "Blue")
                isJumping2 = false;
        }
    }
}
