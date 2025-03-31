using Enums;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb1; // WASD로 움직일 오브젝트의 Rigidbody2D
    public Rigidbody2D rb2; // 방향키로 움직일 오브젝트의 Rigidbody2D

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public ePlayerState playerState1;
    public ePlayerState playerState2;

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
        MoveObject(rb1, KeyCode.W, KeyCode.A, KeyCode.D, ref playerState1);
        MoveObject(rb2, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, ref playerState2);

        //if(playerState1 == ePlayerState.OnAir || playerState2 == ePlayerState.OnAir)
        //{
        //    groundJoint.enabled = false;    
        //    onAirJoint.enabled  = true;
        //}
        //else
        //{
        //    groundJoint.enabled = true;
        //    onAirJoint.enabled  = false;
        //}
    }

    void MoveObject(Rigidbody2D rb, KeyCode up, KeyCode left, KeyCode right, ref ePlayerState playerState)
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(left))
            moveDirection.x -= 1;
        if (Input.GetKey(right))
            moveDirection.x += 1;

        if (Input.GetKey(up) && playerState != ePlayerState.Jump && playerState != ePlayerState.OnAir)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerState = ePlayerState.Jump;
        }

        rb.velocity = new Vector2(moveDirection.normalized.x * moveSpeed, rb.velocity.y);
    }

    private void IsObjectOnGround(Collision2D collision, GameObject gameObject)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") 
        {
            if(gameObject.name == "Red")
                playerState1 = ePlayerState.Idle;
            if(gameObject.name == "Blue")
                playerState2 = ePlayerState.Idle;
        }

    }

    
}
