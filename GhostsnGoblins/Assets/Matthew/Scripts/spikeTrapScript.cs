using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapScript : MonoBehaviour {

    private bool hasTriggered;
    private Vector2 originalTrapPosition;

    // Start is called before the first frame update
    void Start() {
        originalTrapPosition = transform.position;
        hasTriggered = false;
    }

    void resizeTrapCollider(BoxCollider2D bCol) {
        bCol.size = this.gameObject.GetComponent<SpriteRenderer>().size;
        bCol.offset = new Vector2(0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (!hasTriggered) {
                print("Trap triggered!");
                hasTriggered = true;
                resizeTrapCollider(this.gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
        // Ask if hasTriggered is true and if it is, play the required animation.
        if (hasTriggered) {
            transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up, Time.deltaTime * 12);
        }

    }
}
