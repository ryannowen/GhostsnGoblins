using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingingAxeTrap : MonoBehaviour {

    private Rigidbody2D rb2D;
    private float leftRotationCap;
    private float rightRotationCap;
    private float velThresh;

    private int axeDamage;

    // Start is called before the first frame update
    void Start() {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        leftRotationCap = -0.4f;
        rightRotationCap = 0.4f;
        velThresh = 120;
        axeDamage = 0;

        rb2D.angularVelocity = velThresh;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            // Player should fling back and take damage here.

        }
    }

    // Update is called once per frame
    void Update() {

        // Check whether the objects current rotation matches the angular boundaries, along with the angular velocity and if so, inverse its angular velocity.
        if (transform.rotation.z > 0 && transform.rotation.z < rightRotationCap && (rb2D.angularVelocity > 0) && rb2D.angularVelocity < velThresh) {
            rb2D.angularVelocity = velThresh;
        } else if (transform.rotation.z < 0 && transform.rotation.z > leftRotationCap && (rb2D.angularVelocity < 0) && rb2D.angularVelocity > velThresh * -1) {
            rb2D.angularVelocity = velThresh * -1;
        }
    }
}
