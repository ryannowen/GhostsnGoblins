using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempFallingPlatformScript : MonoBehaviour {

    [Tooltip("Does the platform function entirely? If you want the platform to fall, make sure this is enabled.")]
    [SerializeField] private bool enablePlatform = false;

    [Tooltip("The time of delay before the platform falls when collided (in seconds).")]
    [SerializeField] private float fallDelay = 1;

    [Tooltip("Does the platform fade when it falls and resets?")]
    [SerializeField] private bool doesFade = false;

    [Tooltip("The speed of fading (in seconds).")]
    [SerializeField] private float fadeSpeed = 1;

    [Tooltip("The time of delay before the platform respawns to its original position (in seconds).")]
    [SerializeField] private float respawnDelay = 3;

    [Tooltip("Should the platform fall?")]
    [SerializeField] private bool shouldFall = true;

    [Tooltip("Should the platform respawn?")]
    [SerializeField] private bool shouldRespawn = true;

    [Tooltip("The gravity strength of the platform when it falls.")]
    [SerializeField] private float gravScale = 2.5f;

    private SpriteRenderer sRenderer;
    private Rigidbody2D rb2D;

    private Vector2 originalPosition;
    private bool isFalling;
    private float gravityScale;

    // Start is called before the first frame update
    void Start() {
        if (!enablePlatform) {
            return;
        }

        rb2D = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        if (shouldFall) {
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.gravityScale = gravScale;
            rb2D.mass = 200;
        } else {
            rb2D.bodyType = RigidbodyType2D.Static;
        }

        isFalling = false;
        sRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        gravityScale = rb2D.gravityScale;
        rb2D.gravityScale = 0;
        originalPosition = transform.parent.position;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            if (enablePlatform) {
                StartCoroutine(fallingDelay(fallDelay));
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!enablePlatform) {
            return;
        }

        if (isFalling && doesFade) {
            float alphaCol = sRenderer.color.a;

            alphaCol = Mathf.Lerp(alphaCol, 0, Time.fixedDeltaTime * fadeSpeed);
            sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, alphaCol);
        } else if (!isFalling && doesFade) {
            float alphaCol = sRenderer.color.a;

            alphaCol = Mathf.Lerp(alphaCol, 1, Time.fixedDeltaTime * fadeSpeed);
            sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, alphaCol);
        }
    }

    private IEnumerator fallingDelay(float wTime) {
        yield return new WaitForSeconds(wTime);

        isFalling = true;

        if (shouldFall) {
            rb2D.gravityScale = gravityScale;
        }

        if (shouldRespawn) {
            StartCoroutine(respawnPlatform(respawnDelay));
        }

        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private IEnumerator respawnPlatform(float wTime) {
        yield return new WaitForSeconds(wTime);

        rb2D.gravityScale = 0;
        rb2D.velocity = Vector3.zero;
        transform.parent.position = originalPosition;
        isFalling = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
