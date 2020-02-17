using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatformScript : MonoBehaviour {

    [SerializeField] private float fallDelay = 1;
    [SerializeField] private bool doesFade = false;
    [SerializeField] private float fadeSpeed = 1;
    [SerializeField] private float respawnDelay = 3;

    private SpriteRenderer sRenderer;

    private Vector2 originalPosition;
    private bool isFalling;
    private float gravityScale;

    // Start is called before the first frame update
    void Start() {
        isFalling = false;
        sRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        gravityScale = this.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            StartCoroutine(fallingDelay(fallDelay));
        }
    }

    // Update is called once per frame
    void Update() {
        if (isFalling && doesFade) {
            float alphaCol = sRenderer.color.a;

            alphaCol = Mathf.Lerp(alphaCol, 0, Time.deltaTime * fadeSpeed);
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
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(respawnPlatform(respawnDelay));
    }

    private IEnumerator respawnPlatform(float wTime) {
        yield return new WaitForSeconds(wTime);

        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        this.gameObject.transform.position = originalPosition;
        isFalling = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
