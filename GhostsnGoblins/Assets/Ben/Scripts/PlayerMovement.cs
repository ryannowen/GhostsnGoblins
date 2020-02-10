using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, ISpawn
{

    public float m_GravityScale = 4f, m_JumpForce = 7f, m_MovementSpeed = 5f, m_ClimbingSpeed = 3f;
    Vector3 m_DesiredMove = Vector3.zero;
    public bool m_Grounded = false, m_Jump = false, m_Climbing = false;

    public LayerMask m_GroundCheckLayerMask;

    Rigidbody2D m_Rigidbody;
    CapsuleCollider2D m_PlayerCollider;

    // Start is called before the first frame update
    void Start()
    {

        // Find the rigidbody and store a reference to it
        // If no rigidbody is found, create one and store a reference to it
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
        // If no capsule collider is found, create one and store a reference to it
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

        // Check if the player is grounded
        CheckGroundedState();

        // Manage player settings for climbing ladders
        ManageClimbingSettings();

        // Check if the player wants to jump
        if (Input.GetAxis("Jump") > 0 && m_Grounded)
            m_Jump = true;

        // Get the desired movement direction
        m_DesiredMove = GetDesiredMove();

    }

    void FixedUpdate()
    {

        // Lerp the player velocity to the desired movement direction
        m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, m_DesiredMove, 0.25f);

        // Check if the player should jump and apply velocity
        if (m_Jump)
            m_Rigidbody.velocity = Vector2.Lerp(m_Rigidbody.velocity, new Vector2(m_Rigidbody.velocity.x, m_JumpForce), 1f);

        // Set the player jump to false
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

    Vector3 GetDesiredMove()
    {

        Vector3 velocity = Vector3.zero;

        velocity.x += Input.GetAxis("Horizontal");

        if (m_Climbing)
        {

            velocity.y += Input.GetAxis("Vertical");

        }

        velocity.Normalize();

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

        if (hit)
            m_Grounded = true;
        else
            m_Grounded = false;

    }

    void ManageClimbingSettings() {

        // Check if the player is climbing
        if (m_Climbing)
        {

            m_Rigidbody.gravityScale = 0f;
            m_Grounded = false;

            // If the player jumps disable climbing
            if (Input.GetAxis("Jump") > 0)
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

    public void SetClimbingState(bool state)
    {

        m_Climbing = state;

    }

    public void OnSpawn()
    {



    }

    public void OnDeSpawn()
    {



    }

}
