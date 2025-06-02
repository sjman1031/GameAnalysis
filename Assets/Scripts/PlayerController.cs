using Enums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb1; // WASD�� ������ ������Ʈ�� Rigidbody2D
    public Rigidbody2D rb2; // ����Ű�� ������ ������Ʈ�� Rigidbody2D

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public ePlayerState playerState1;
    public ePlayerState playerState2;

    public bool isJumping1 = false;
    public bool isJumping2 = false;

    public PlayerManager[] children;

    public SpringJoint2D lucyJoint;
    public SpringJoint2D paulJoint;
    
    // �ӽú���
    float t = 0f;

    private void Start()
    {
        children    = GetComponentsInChildren<PlayerManager>();
        
        foreach(PlayerManager child in children)
        {
            child.OnChildCollisionEnter += IsObjectOnGround;
        }
    }

    private void Update()
    {
        MoveObject(rb1, KeyCode.W, KeyCode.A, KeyCode.D, lucyJoint, ref playerState1);
        MoveObject(rb2, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, paulJoint, ref playerState2);

        t += Time.fixedDeltaTime;

        //if (playerState1 == ePlayerState.Jump || playerState2 == ePlayerState.Jump)
        //{
        //    groundJoint.enabled = false;
        //    onAirJoint.enabled = true;
        //}
        //else
        //{
        //    groundJoint.enabled = true;
        //    onAirJoint.enabled = false;
        //}
    }

    void MoveObject(Rigidbody2D rb, KeyCode up, KeyCode left, KeyCode right, SpringJoint2D joint, ref ePlayerState playerState)
    {
        // rb.velocity �κ��� joint�� ����ó���� ������ �༭ �������� ó���� ���� �ʾ� ���
        // �̰� �� ������ �ٺ���
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
            float jointYforce = (joint.connectedBody.velocity - rb.velocity).y;
            float correctedJump = jumpForce - jointYforce * 0.5f; // 0.5f�� �������
            correctedJump = Mathf.Clamp(correctedJump, jumpForce, jumpForce);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
            playerState = ePlayerState.Jump;        
        }
    }

    private void IsObjectOnGround(Collision2D collision, GameObject gameObject)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") 
        {
            if (gameObject.name == "Lucy")
            {
                playerState1 = ePlayerState.Idle;
                Debug.Log("Lucy On Ground");
            }
            if (gameObject.name == "Pual")
            {
                playerState2 = ePlayerState.Idle;
                Debug.Log("Pual On Ground");
            }
        }
    }    
}
