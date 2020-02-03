using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, Interface_Damage
{

    public Rigidbody2D rb;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        TakeDamage();
    }

    public void TakeDamage()
    {

        print ("hi");

    }

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.D)) {

            rb.AddForce(transform.right * speed, ForceMode2D.Impulse);

        }
        if (Input.GetKey(KeyCode.A))
        {

            rb.AddForce(-transform.right * speed, ForceMode2D.Impulse);

        }

    }
}
