using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingingAxeTrap : MonoBehaviour {

    private Rigidbody2D rb2D;

    [Tooltip("A rotation cap for the left side of the axe, ideal number for this is -0.4.")]
    [Range(-1, 1)] [SerializeField] private float leftRotationCap = -0.4f;

    [Tooltip("A rotation cap for the left side of the axe, ideal number for this is 0.4.")]
    [Range(-1, 1)] [SerializeField] private float rightRotationCap = 0.4f;

    [Tooltip("The velocity threshold of the axe (ideal number is 120).")]
    [SerializeField] private float velThresh = 120;

    [Tooltip("The amount of damage the axe inflicts to the player.")]
    [SerializeField] private int axeDamage = 1;

    // Start is called before the first frame update
    void Start() {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.angularVelocity = velThresh;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<IDamageable>().TakeDamage(axeDamage);
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
