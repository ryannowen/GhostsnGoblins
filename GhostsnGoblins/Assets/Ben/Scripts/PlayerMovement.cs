using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float m_GravityScale = 4f, m_JumpForce = 7f, m_MovementSpeed = 5f, m_ClimbingSpeed = 3f;
    Vector3 m_DesiredMove = Vector3.zero;
    public bool m_Grounded = false, m_Jump = false, m_Climbing = false, m_MovingLeft = false, m_MovingRight = false;

    public LayerMask m_GroundCheckLayerMask;
    public LayerMask m_LadderCheckLayerMask;

    Rigidbody2D m_Rigidbody;
    CapsuleCollider2D m_PlayerCollider;

    // Start is called before the first frame update
    void Start()
    {


        if (GetComponent<Rigidbody2D>())
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }
        else
        {
            m_Rigidbody = this.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
            RigidBodySetup();
        }

        // Find capsule collider and store a reference to it
        // If there no capsule collider is found, create one and store a reference to it
        if (GetComponent<CapsuleCollider2D>())
        {
            m_PlayerCollider = GetComponent<CapsuleCollider2D>();
        }
        else
        {
            m_PlayerCollider = this.gameObject.AddComponent(typeof(CapsuleCollider2D)) as CapsuleCollider2D;
        }

        // Checks for errors such as variables not being assigned values
        Setup();

    }

    // Update is called once per frame
    void Update()
    {

        GetInput();
        CheckGroundedState();

        CheckClimbingState();
        ManageClimbingSettings();

        CheckForJump();
        m_DesiredMove = GetDesiredMove();

    }

    void FixedUpdate()
    {

        m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, m_DesiredMove, 0.25f);

        if (m_Jump)
            m_Rigidbody.velocity = Vector2.Lerp(m_Rigidbody.velocity, new Vector2(m_Rigidbody.velocity.x, m_JumpForce), 1f);

        m_Jump = false;

    }

    void Setup() 
    {

        if (m_GravityScale == 0)
            m_GravityScale = 4f;

        if (m_JumpForce == 0)
            m_JumpForce = 7f;

        if (m_MovementSpeed == 0)
            m_MovementSpeed = 5f;

    }

    void RigidBodySetup() 
    {

        m_Rigidbody.sharedMaterial = (PhysicsMaterial2D)Resources.Load("PhysicsMaterials/Slippy");
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    void GetInput()
    {

        if (Input.GetKey(KeyCode.A))
            m_MovingLeft = true;
        if (Input.GetKey(KeyCode.D))
            m_MovingRight = true;

        if (Input.GetKeyUp(KeyCode.A))
            m_MovingLeft = false;
        if (Input.GetKeyUp(KeyCode.D))
            m_MovingRight = false;

        // Ladder Movement
        if (m_Climbing) 
        {

            if (Input.GetKey(KeyCode.W))
                m_MovingLeft = true;
            if (Input.GetKey(KeyCode.S))
                m_MovingRight = true;

        }

    }

    Vector3 GetDesiredMove()
    {

        Vector3 velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {

            velocity -= Vector3.right;

        }
        if (Input.GetKey(KeyCode.D))
        {

            velocity += Vector3.right;

        }

        if (m_Climbing)
        {

            if (Input.GetKey(KeyCode.W))
            {

                velocity += Vector3.up;

            }
            if (Input.GetKey(KeyCode.S))
            {

                velocity -= Vector3.up;

            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {

                //velocity -= Vector3.up;

            }

        }

        if (!m_Climbing)
            velocity.y = m_Rigidbody.velocity.y;
        else
            velocity.y *= m_ClimbingSpeed;

        velocity.x *= m_MovementSpeed;

        return velocity;

    }

    void CheckGroundedState()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, (m_PlayerCollider.size.y / 2 + 0.02f) * transform.localScale.x, m_GroundCheckLayerMask);

        //Debug.DrawRay(transform.position, Vector3.down * (m_PlayerCollider.size.y / 2 + 0.02f) * transform.localScale.x, Color.red);

        if (hit)
            m_Grounded = true;
        else
            m_Grounded = false;

    }

    void CheckClimbingState() {

        //RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * m_PlayerCollider.size.y / 2, Vector3.forward, Mathf.Infinity, m_LadderCheckLayerMask);

        //RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, m_PlayerCollider.size, CapsuleDirection2D.Vertical, 0, Vector3.forward, Mathf.Infinity, m_LadderCheckLayerMask);

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, m_PlayerCollider.size, 0, Vector3.forward, Mathf.Infinity, m_LadderCheckLayerMask);

        if (hit)
        {

            if (hit.collider.gameObject.tag == "Ladder")
            {

                m_Climbing = true;

            }
            else
            {

                m_Climbing = false;

            }

        }
        else 
        {

            m_Climbing = false;

        }

    }

    void ManageClimbingSettings() {

        if (m_Climbing)
        {

            m_Rigidbody.gravityScale = 0f;
            m_Grounded = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Jump = true;
                m_Climbing = false;
            }

        }
        else
        {

            m_Rigidbody.gravityScale = 4f;

        }

    }

    void CheckForJump()
    {

        if (Input.GetKey(KeyCode.Space) && m_Grounded)
            m_Jump = true;

    }

}
