using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapScript : MonoBehaviour {

	private Vector2 originalTriggerSize;
	private Vector2 originalTriggerOffset;
	private Vector2 originalTrapPosition;

	private bool hasTriggered;

	[SerializeField] private float trapSpeed;
	[SerializeField] private int spikeTrapDamage;

    // Start is called before the first frame update
    void Start() {
		originalTriggerSize = this.gameObject.GetComponent<BoxCollider2D>().size;
		originalTriggerOffset = this.gameObject.GetComponent<BoxCollider2D>().offset;

        originalTrapPosition = transform.position;
        hasTriggered = false;
		trapSpeed = 12;
		spikeTrapDamage = 1;
    }

    void resizeTrapCollider(BoxCollider2D bCol) {
        bCol.size = this.gameObject.GetComponent<SpriteRenderer>().size;
        bCol.offset = new Vector2(0f, 0f);
    }

	void resetTrapCollider(BoxCollider2D bCol) {
		bCol.size = originalTriggerSize;
		bCol.offset = originalTriggerOffset;
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			
		}
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
			if (!hasTriggered) {
				hasTriggered = true;
				resizeTrapCollider(this.gameObject.GetComponent<BoxCollider2D>());
				StartCoroutine(resetSpikeCollider(2.0f, this.gameObject.GetComponent<BoxCollider2D>()));
			} else {
				col.gameObject.GetComponent<IDamageable>().TakeDamage(spikeTrapDamage);
			}
		}
    }

    // Update is called once per frame
    void Update() {
        
        // Ask if hasTriggered is true and if it is, play the required animation.
        if (hasTriggered) {
            transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up, Time.deltaTime * trapSpeed);
        } else {
			transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.deltaTime * trapSpeed);
		}

    }

	private IEnumerator resetSpikeCollider(float wTime, BoxCollider2D boxCol) {
		yield return new WaitForSeconds(wTime);
		hasTriggered = false;
		resetTrapCollider(boxCol);
	}
}
