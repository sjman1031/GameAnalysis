using Enums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb1; // WASD로 움직일 오브젝트의 Rigidbody2D
    public Rigidbody2D rb2; // 방향키로 움직일 오브젝트의 Rigidbody2D

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public ePlayerState playerState1;
    public ePlayerState playerState2;

    public bool isJumping1 = false;
    public bool isJumping2 = false;

    public PlayerManager[] children;

    public DistanceJoint2D  groundJoint;
    public SpringJoint2D    onAirJoint;
    
    // 임시변수
    float t = 0f;

    private void Start()
    {
        children    = GetComponentsInChildren<PlayerManager>();

        groundJoint = GetComponentInChildren<DistanceJoint2D>();
        onAirJoint  = GetComponentInChildren<SpringJoint2D>();
        
        foreach(PlayerManager child in children)
        {
            child.OnChildCollisionEnter += IsObjectOnGround;
        }
    }

    private void Update()
    {
        MoveObject(rb1, KeyCode.W, KeyCode.A, KeyCode.D, ref playerState1);
        MoveObject(rb2, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, ref playerState2);

        t += Time.fixedDeltaTime;

        if (playerState1 == ePlayerState.Jump || playerState2 == ePlayerState.Jump)
        {
            groundJoint.enabled = false;
            onAirJoint.enabled = true;
        }
        else
        {
            groundJoint.enabled = true;
            onAirJoint.enabled = false;
        }
    }

        void MoveObject(Rigidbody2D rb, KeyCode up, KeyCode left, KeyCode right, ref ePlayerState playerState)
    {
        // rb.velocity 부분이 joint의 물리처리에 영향을 줘서 정상적인 처리가 되지 않아 폐기
        // 이걸 왜 몰랐지 바본가
        //Vector2 moveDirection = Vector2.zero;

        //if (Input.GetKey(left))
        //    moveDirection.x -= 1;
        //if (Input.GetKey(right))
        //    moveDirection.x += 1;

        //if (Input.GetKey(up) && playerState != ePlayerState.Jump && playerState != ePlayerState.OnAir)
        //{
        //    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //    playerState = ePlayerState.Jump;
        //}

        //rb.velocity = new Vector2(moveDirection.normalized.x * moveSpeed, rb.velocity.y); 

        float moveDirction = 0f;
        
        if (Input.GetKey(left))
            moveDirction = -1f;
        if (Input.GetKey(right))
            moveDirction = 1f;

        float desiredSpeed = moveDirction * moveSpeed;
        float currentSpeed = rb.velocity.x;

        float speedDiff = desiredSpeed - currentSpeed;
        float horizontalAcceleration = 10f;
        float forceX = speedDiff * horizontalAcceleration;

        rb.AddForce(new Vector2(forceX, 0f), ForceMode2D.Force);   

        if(Input.GetKey(up) && playerState != ePlayerState.Jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
            playerState = ePlayerState.Jump;    
        }
    }

    private void IsObjectOnGround(Collision2D collision, GameObject gameObject)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") 
        {
            if(gameObject.name == "Lucy")
                playerState1 = ePlayerState.Idle;
            if(gameObject.name == "Paul")
                playerState2 = ePlayerState.Idle;
        }
    }    
}
