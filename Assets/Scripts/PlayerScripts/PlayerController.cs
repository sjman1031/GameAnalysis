using UnityEngine;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

public enum ePlayerInputType { WASD, ARROW }
public enum ePlayerState { Idle, Jump, OnAir, Dash }

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
#if PHOTON_UNITY_NETWORKING
    , IPunObservable
#endif
{
    [Header("Input")]
    public ePlayerInputType inputType = ePlayerInputType.WASD;

    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 13f;
    public float acceleration = 12f;

    [Header("Swing")]
    public float swingTorque = 500f;
    public float maxAngularVelocity = 3000000f;
    public float ropeTensionThreshold = 0.97f; // 스윙 판정 임계값
    public bool isSwinging = false;

    [Header("Joint Options")]
    public bool useSpringJoint = false;
    public SpringJoint2D springJoint;
    public DistanceJoint2D distanceJoint;
    public bool isJointHolder = false;

    private bool swingImmune = false;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerController otherPlayer;
    [HideInInspector] public bool onGround = false;

    public ePlayerState playerState = ePlayerState.Idle;
    private bool prevSwinging = false;
    private bool jointInitialized = false;

    private float swingTimer = 0f;
    private const float swingMinTime = 2f;

#if PHOTON_UNITY_NETWORKING
    private PhotonView pv;
#endif

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();

#if PHOTON_UNITY_NETWORKING
        pv = GetComponent<PhotonView>();
#endif

        playerState = ePlayerState.Idle;
        isJointHolder = (this.name == "Lucy");
    }

    private void Start()
    {
        onGround = true;

        Debug.Log($"{this.name} position: {transform.position}");
        Debug.Log($"{this.name} velocity: {rb.velocity}");

        if (otherPlayer == null)
        {
            foreach (var pc in FindObjectsOfType<PlayerController>())
                if (pc != null)
                    if (pc != this) otherPlayer = pc;


            Debug.Log($"{this.name} otherPlayer: {otherPlayer.name}");
        }
    }

    private void FixedUpdate()
    {
#if PHOTON_UNITY_NETWORKING
        if (!pv.IsMine && pv != null) return;
#endif
        bool amIGrounded = onGround;
        bool otherGrounded = otherPlayer != null && otherPlayer.onGround;
        bool bothOnGround = amIGrounded && otherGrounded;
        bool bothInAir = !amIGrounded && !otherGrounded;

        bool swinging = isSwinging; 

        if (bothOnGround)
        {
            rb.freezeRotation = true;
            rb.MoveRotation(0f);
            rb.angularVelocity = 0f;
        }
        else if (bothInAir)
        {
            rb.freezeRotation = true;
            rb.MoveRotation(0f);
            rb.angularVelocity = 0f;
        }
        else if (amIGrounded && !otherGrounded)
        {
            rb.freezeRotation = true;
            rb.MoveRotation(0f);
            rb.angularVelocity = 0f;
        }
        else if (!amIGrounded && otherGrounded)
        {
            if (swinging)
            {
                rb.freezeRotation = false;
                ClampRotation();
            }
            else
            {
                rb.freezeRotation = true;
                rb.MoveRotation(0f);
                rb.angularVelocity = 0f;
            }
        }


        if (springJoint != null && distanceJoint != null)
        {
            if (isSwinging)
            {
                if (!springJoint.enabled)
                {
                    CopyJointParameters(distanceJoint, springJoint);
                    springJoint.enabled = true;
                    distanceJoint.enabled = false;
                }
            }
            else
            {
                if (!distanceJoint.enabled)
                {
                    CopyJointParameters(springJoint, distanceJoint);
                    distanceJoint.enabled = true;
                    springJoint.enabled = false;
                }
            }
        }

        Move();
        Jump();
        SwingImmune();
        //if (isJointHolder)
        //    UpdateJoint();
        TrySwingBoost();
        UpdatePlayerState();
    }

    private void CopyJointParameters(Joint2D from, Joint2D to)
    {
        if (from == null || to == null) return;

        to.connectedBody = from.connectedBody;
        if (from is DistanceJoint2D djFrom && to is SpringJoint2D sjTo)
        {
            sjTo.distance = djFrom.distance;
            sjTo.anchor = djFrom.anchor;
            sjTo.connectedAnchor = djFrom.connectedAnchor;
        }
        else if (from is SpringJoint2D sjFrom && to is DistanceJoint2D djTo)
        {
            djTo.distance = sjFrom.distance;
            djTo.anchor = sjFrom.anchor;
            djTo.connectedAnchor = sjFrom.connectedAnchor;
        }
    }

    private void SwingImmune()
    {
        bool isSwinging = IsSwingState();
        bool isDownPressed =
            (inputType == ePlayerInputType.WASD && Input.GetKey(KeyCode.S)) ||
            (inputType == ePlayerInputType.ARROW && Input.GetKey(KeyCode.DownArrow));

        if (isSwinging && isDownPressed)
        {
            if(!swingImmune)
                swingImmune = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            if(swingImmune)
            {
                swingImmune = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    private void Move()
    {
        float moveDir = 0f;
        if (inputType == ePlayerInputType.WASD)
        {
            if (Input.GetKey(KeyCode.A)) moveDir = -1f;
            if (Input.GetKey(KeyCode.D)) moveDir = 1f;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveDir = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) moveDir = 1f;
        }

        float desiredSpeed = moveDir * moveSpeed;
        float speedDiff = desiredSpeed - rb.velocity.x;
        float forceX = speedDiff * acceleration;
        rb.AddForce(new Vector2(forceX, 0f), ForceMode2D.Force);
    }

    private void Jump()
    {
        bool jumpPressed = false;
        if (inputType == ePlayerInputType.WASD && Input.GetKey(KeyCode.W))
            jumpPressed = true;
        if (inputType == ePlayerInputType.ARROW && Input.GetKey(KeyCode.UpArrow))
            jumpPressed = true;

        if (jumpPressed && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerState = ePlayerState.Jump;
        }
    }

    private void UpdateJoint()
    {
        bool tight = IsRopeTight();
        bool thisOnAir = !onGround;
        bool otherOnAir = !otherPlayer.onGround;
        bool someoneOnAir = thisOnAir || otherOnAir;

        if (!someoneOnAir)
        {
            useSpringJoint = false;
            EnableDistanceJoint();
        }
        else 
        {
            useSpringJoint = true;
            EnableSpringJoint();
        }
    }

    private bool IsRopeTight()
    {
        float jointLength = useSpringJoint ? springJoint.distance : distanceJoint.distance;
        float actualLength = Vector2.Distance(rb.position, otherPlayer.rb.position);
        return (actualLength / jointLength) > 0.95f;
    }

    private void EnableDistanceJoint()
    {
        if(distanceJoint != null && !distanceJoint.enabled)
            distanceJoint.enabled = true;
        if(springJoint != null && springJoint.enabled)
            springJoint.enabled = false;
    }

    private void EnableSpringJoint()
    {
        if (springJoint != null && !springJoint.enabled)
            springJoint.enabled = true;
        if (distanceJoint != null && distanceJoint.enabled)
            distanceJoint.enabled = false;
    }

    private bool IsSwingState()
    {
        if (otherPlayer == null || distanceJoint == null || !distanceJoint.enabled || distanceJoint.connectedBody == null)
            return false;

        float jointLength = distanceJoint.distance;
        float actualLength = Vector2.Distance(rb.position, otherPlayer.rb.position);
        bool ropeTight = (jointLength > 0f) && ((actualLength / jointLength) > ropeTensionThreshold);

        bool thisOnAir = !onGround;
        bool otherOnAir = !otherPlayer.onGround;

        // 둘 다 땅에 있으면 false
        if (onGround && otherPlayer.onGround)
        {
            swingTimer = 0f;
            return false;
        }

        // ropeTight+공중인 상태가 일정 시간 이상이어야 swinging true
        if (ropeTight && (thisOnAir || otherOnAir))
        {
            swingTimer += Time.fixedDeltaTime;
            if (swingTimer > swingMinTime)
                return true;
        }
        else
        {
            swingTimer = 0f;
        }
        return false;
    }

    private void TrySwingBoost()
    {
        if (otherPlayer == null) return;
        if (distanceJoint == null || !distanceJoint.enabled || distanceJoint.connectedBody == null) return;

        float jointLength = distanceJoint.distance;
        float actualLength = Vector2.Distance(rb.position, otherPlayer.rb.position);
        bool ropeTight = (jointLength > 0f) && ((actualLength / jointLength) > ropeTensionThreshold);

        bool thisOnAir = !onGround;
        bool otherOnAir = !otherPlayer.onGround;
        bool swing = ropeTight && (thisOnAir || otherOnAir);

        // "회전 가속" 입력 처리 (←/→ 또는 A/D)
        float inputDir = 0f;
        if (inputType == ePlayerInputType.WASD)
        {
            if (Input.GetKey(KeyCode.A)) inputDir = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir = 1f;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) inputDir = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) inputDir = 1f;
        }

        // 스윙 중 & 방향키 입력 중이면 매 프레임마다 토크 가속
        if (swing && Mathf.Abs(inputDir) > 0.1f)
        {
            float torque = swingTorque * inputDir;
            rb.AddTorque(torque, ForceMode2D.Force); // Force(=연속), Impulse(=짧게)
                                                     // 속도 제한
            if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxAngularVelocity;
        }
    }

    private void UpdatePlayerState()
    {
        if (onGround)
            playerState = ePlayerState.Idle;
        else if (playerState != ePlayerState.Jump)
            playerState = ePlayerState.OnAir;
    }

    private void ClampRotation()
    {
        float angle = NormalizeAngle(rb.rotation);
        if(angle > 90f)
            angle = Mathf.LerpAngle(angle, 90f, Time.fixedDeltaTime * 5f);
        else if(angle < -90f)
            angle = Mathf.LerpAngle(angle, -90f, Time.fixedDeltaTime * 5f);

        rb.MoveRotation(angle);
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < 180f) angle += 360f;
        return angle;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}

