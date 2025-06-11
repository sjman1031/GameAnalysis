using UnityEngine;
using System.Collections;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

public enum ePlayerInputType { WASD, ARROW }
public enum ePlayerState { Idle, Jump }
public enum eNetworkState { OffLine, OnLine }

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
#if PHOTON_UNITY_NETWORKING
public class PlayerController : MonoBehaviourPun, IPunObservable
#else
public class PlayerController : MonoBehaviour
#endif
{
    // public Transform arrow; 

    [Header("Input")]
    public ePlayerInputType inputType = ePlayerInputType.WASD;

    [Header("Movement")]
    public float moveSpeed      = 7f;
    public float jumpForce      = 13f;
    public float acceleration   = 12f;
    public float dashSpeed      = 30f;
    public float dashDuration   = 0.2f;
    public bool canDash         = false;
    public KeyCode dashKey      = KeyCode.None;

    [Header("Swing")]
    public float swingThreshold = 0.97f;
    public float swingPower = 10f;
    public float maxAngularVelocity = 200f;
    public bool isSwingActive = false;
    public bool isBelow = false;

    [Header("Joint Options")]
    public DistanceJoint2D distanceJoint;
    public bool isJointHolder = false;

    private bool swingImmune = false;

    public eNetworkState networkState = eNetworkState.OffLine;

    public Rigidbody2D rb;
    public PlayerController otherPlayer;
    public bool onGround = false;

    public ePlayerState playerState = ePlayerState.Idle;
    private bool prevSwinging = false;
    private bool jointInitialized = false;

    private float swingTimer = 0f;
    private const float swingMinTime = 2f;

    private Transform lucy, paul;

#if PHOTON_UNITY_NETWORKING
    private PhotonView pv;
#endif

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();

#if PHOTON_UNITY_NETWORKING
        pv = GetComponent<PhotonView>();
#endif

        playerState = ePlayerState.Idle;
        isJointHolder = (this.name == "Lucy");


        if (otherPlayer == null)
        {
            foreach (var pc in FindObjectsOfType<PlayerController>())
                if (pc != null)
                    if (pc != this) otherPlayer = pc;


            //Debug.Log($"{this.name} otherPlayer: {otherPlayer.name}");
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 120;

        //Debug.Log($"{this.name} position: {transform.position}");
        //Debug.Log($"{this.name} velocity: {rb.velocity}");
    }

    private void Update()
    {
#if PHOTON_UNITY_NETWORKING
        if (!pv.IsMine && pv != null && PhotonNetwork.IsConnected) return;
#endif

        if(otherPlayer == null)
        {
            foreach (var pc in FindObjectsOfType<PlayerController>())
                if (pc != null)
                    if (pc != this) otherPlayer = pc;


            //Debug.Log($"{this.name} otherPlayer: {otherPlayer.name}");
        }

        if (!IsSwingState())
        {
            Move();
            Jump();
            SwingImmune();
        }
        else
            TrySwingBoost();

        if (canDash)
        {
            Vector2 dircetion = Vector2.zero;
            if (inputType == ePlayerInputType.WASD && Input.GetKeyDown(KeyCode.LeftShift))
            {
                dircetion = rb.velocity.normalized;
                StartCoroutine(Dash(dircetion));
            }

            else if (inputType == ePlayerInputType.ARROW && Input.GetKeyDown(KeyCode.RightShift))
            {
                dircetion = rb.velocity.normalized;
                StartCoroutine(Dash(dircetion));
            }
        }

        UpdatePlayerState();
    }


//    private void FixedUpdate()
//    {
//#if PHOTON_UNITY_NETWORKING
//        if (!pv.IsMine && pv != null) return;
//#endif
//        Move();
//        Jump();
//        SwingImmune();
//        //if (isJointHolder)
//        //    UpdateJoint();
//        TrySwingBoost();
//        UpdatePlayerState();
//    }

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
        bool isSwinging = otherPlayer.IsSwingState();

        if (inputType == ePlayerInputType.WASD)
        {
            if(isSwinging && Input.GetKey(KeyCode.S)) 
                rb.isKinematic = true;

            if(isSwinging && Input.GetKeyUp(KeyCode.S))
                rb.isKinematic = false;
        }
        else
        {
            if (isSwinging && Input.GetKey(KeyCode.DownArrow))
                rb.isKinematic = true;

            if (isSwinging && Input.GetKeyUp(KeyCode.DownArrow))
                rb.isKinematic = false;
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
        if (inputType == ePlayerInputType.WASD && Input.GetKeyDown(KeyCode.W))
            jumpPressed = true;
        if (inputType == ePlayerInputType.ARROW && Input.GetKeyDown(KeyCode.UpArrow))
            jumpPressed = true;

        if (jumpPressed && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerState = ePlayerState.Jump;
        }
    }

    private IEnumerator Dash(Vector2 dircetion)
    {
        float originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = dircetion * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravityScale;

        canDash = false;
    }

    //private void UpdateJoint()
    //{
    //    bool tight = IsRopeTight();
    //    bool thisOnAir = !onGround;
    //    bool otherOnAir = !otherPlayer.onGround;
    //    bool someoneOnAir = thisOnAir || otherOnAir;

    //    if (!someoneOnAir)
    //    {
    //        useSpringJoint = false;
    //        EnableDistanceJoint();
    //    }
    //    else 
    //    {
    //        useSpringJoint = true;
    //        EnableSpringJoint();
    //    }
    //}

    //private bool IsRopeTight()
    //{
    //    float jointLength = useSpringJoint ? springJoint.distance : distanceJoint.distance;
    //    float actualLength = Vector2.Distance(rb.position, otherPlayer.rb.position);
    //    return (actualLength / jointLength) > 0.95f;
    //}

    //private void EnableDistanceJoint()
    //{
    //    if(distanceJoint != null && !distanceJoint.enabled)
    //        distanceJoint.enabled = true;
    //    if(springJoint != null && springJoint.enabled)
    //        springJoint.enabled = false;
    //}

    //private void EnableSpringJoint()
    //{
    //    if (springJoint != null && !springJoint.enabled)
    //        springJoint.enabled = true;
    //    if (distanceJoint != null && distanceJoint.enabled)
    //        distanceJoint.enabled = false;
    //}

    //private bool IsSwingState()
    //{
    //    if (otherPlayer == null || distanceJoint == null || !distanceJoint.enabled || distanceJoint.connectedBody == null)
    //        return false;

    //    float jointLength = distanceJoint.distance;
    //    float actualLength = Vector2.Distance(rb.position, otherPlayer.rb.position);
    //    bool ropeTight = (jointLength > 0f) && ((actualLength / jointLength) > ropeTensionThreshold);

    //    bool thisOnAir = !onGround;
    //    bool otherOnAir = !otherPlayer.onGround;

    //    // 둘 다 땅에 있으면 false
    //    if (onGround && otherPlayer.onGround)
    //    {
    //        swingTimer = 0f;
    //        return false;
    //    }

    //    // ropeTight+공중인 상태가 일정 시간 이상이어야 swinging true
    //    if (ropeTight && (thisOnAir || otherOnAir))
    //    {
    //        swingTimer += Time.fixedDeltaTime;
    //        if (swingTimer > swingMinTime)
    //            return true;
    //    }
    //    else
    //    {
    //        swingTimer = 0f;
    //    }
    //    return false;
    //}

    private bool IsSwingState()
    {
        if (isJointHolder)
            return playerState == ePlayerState.Jump
                && otherPlayer.onGround
                && Vector3.Distance(rb.position, otherPlayer.rb.position)
                >= distanceJoint.distance * swingThreshold;
        else
            return playerState == ePlayerState.Jump
                && otherPlayer.onGround
                && Vector3.Distance(rb.position, otherPlayer.rb.position)
                >= otherPlayer.distanceJoint.distance * swingThreshold;           
    }

    private void TrySwingBoost()
    {
        if (otherPlayer == null || otherPlayer.rb == null) return;
        // if (distanceJoint == null || !distanceJoint.enabled || distanceJoint.connectedBody == null) return;

        Vector2 centerPos   = otherPlayer.rb.position;
        Vector2 orbitPos    = rb.position;
        Vector2 r           = orbitPos - centerPos;

        float radius = (distanceJoint != null && distanceJoint.enabled) ? distanceJoint.distance : otherPlayer.distanceJoint.distance;
        if (radius <= 0f) return;
        
        Vector2 rHat = r.normalized;
        
        Vector2 tangentCCW  = new Vector2(-rHat.y, rHat.x);
        Vector2 tangentCW   = new Vector2(rHat.y, -rHat.x);

        
        float swingDir = 0f;
        if(inputType == ePlayerInputType.WASD)
        {
            if (Input.GetKey(KeyCode.A)) swingDir = -1f;
            else if (Input.GetKey(KeyCode.D)) swingDir = 1f;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow)) swingDir = -1f;
            else if (Input.GetKey(KeyCode.LeftArrow)) swingDir = 1f;
        }

        if (swingDir == 0f) return;

        Vector2 chosenTangent = (swingDir > 0f) ? tangentCCW : tangentCW;
        //if (tangentialSpeed >= maxAngularVelocity) return;
        
        float v_tan = Vector2.Dot(rb.velocity, chosenTangent);
        float maxLinearSpeed = maxAngularVelocity * radius;

        if (v_tan > maxLinearSpeed) return;

        rb.AddForce(chosenTangent * swingPower, ForceMode2D.Force);

        //if (otherPlayer == null) return;
        //if (distanceJoint == null || !distanceJoint.enabled || distanceJoint.connectedBody == null) return;

        //float jointLength = distanceJoint.distance;
        //float actualLength = Vector2.Distance(rb.position, otherPlayer.rb.position);
        //bool ropeTight = (jointLength > 0f) && ((actualLength / jointLength) > ropeTensionThreshold);

        //bool thisOnAir = !onGround;
        //bool otherOnAir = !otherPlayer.onGround;
        //bool swing = ropeTight && (thisOnAir || otherOnAir);

        //// "회전 가속" 입력 처리 (←/→ 또는 A/D)
        //float inputDir = 0f;
        //if (inputType == ePlayerInputType.WASD)
        //{
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        rb.gravityScale = 0f;
        //        inputDir = -1f;
        //        Vector2 toCenter = otherPlayer.rb.position - rb.position;
        //        Vector2 tangent = new Vector2(toCenter.y, -toCenter.x).normalized;

        //        float tangentialSpeed = Vector2.Dot(rb.velocity, tangent);

        //        if (tangentialSpeed < maxAngularVelocity)
        //            rb.AddForce(tangent * swingPower * inputDir);
        //    }
        //    if (Input.GetKey(KeyCode.D)) 
        //    {
        //        inputDir = 1f; 
        //        Vector2 toCenter = otherPlayer.rb.position - rb.position;
        //        Vector2 tangent = new Vector2(toCenter.y, -toCenter.x).normalized;

        //        float tangentialSpeed = Vector2.Dot(rb.velocity, tangent);

        //        if (tangentialSpeed < maxAngularVelocity)
        //            rb.AddForce(tangent * swingPower * inputDir); 
        //    }
        //}
        //else
        //{
        //    if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        inputDir = -1f;
        //        Vector2 toCenter = otherPlayer.rb.position - rb.position;
        //        Vector2 tangent = new Vector2(toCenter.y, -toCenter.x).normalized;

        //        float tangentialSpeed = Vector2.Dot(rb.velocity, tangent);

        //        if (tangentialSpeed < maxAngularVelocity)
        //            rb.AddForce(tangent * swingPower * inputDir);
        //    }
        //    if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        inputDir = 1f;
        //        Vector2 toCenter = otherPlayer.rb.position - rb.position;
        //        Vector2 tangent = new Vector2(toCenter.y, -toCenter.x).normalized;

        //        float tangentialSpeed = Vector2.Dot(rb.velocity, tangent);

        //        if (tangentialSpeed < maxAngularVelocity)
        //            rb.AddForce(tangent * swingPower * inputDir);
        //    }
        //}   
    }

    private void UpdatePlayerState()
    {
            if (onGround)
                playerState = ePlayerState.Idle;
            else
                playerState = ePlayerState.Jump; 
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

