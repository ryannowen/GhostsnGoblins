using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatformScript : MonoBehaviour {

    [SerializeField] private float fallDelay;

    private float gravityScale;

    // Start is called before the first frame update
    void Start() {
        fallDelay = 0.75f;
        gravityScale = this.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            StartCoroutine(fallingDelay(fallDelay));
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator fallingDelay(float wTime) {
        yield return new WaitForSeconds(wTime);
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }
}
