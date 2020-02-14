using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapScript : MonoBehaviour {

	private Vector2 originalTriggerSize;
	private Vector2 originalTriggerOffset;
	private Vector2 originalTrapPosition;

	private bool hasTriggered;

	[SerializeField] private float trapSpeed = 12;
    [SerializeField] private float trapTriggerDelay = 0.5f;
    [SerializeField] private float trapResetDelay = 2;
	[SerializeField] private int spikeTrapDamage = 1;

    // Start is called before the first frame update
    void Start() {
		originalTriggerSize = this.gameObject.GetComponent<BoxCollider2D>().size;
		originalTriggerOffset = this.gameObject.GetComponent<BoxCollider2D>().offset;

        originalTrapPosition = transform.position;
        hasTriggered = false;
    }

    void resizeTrapCollider(BoxCollider2D bCol) {
        bCol.size = this.gameObject.GetComponent<SpriteRenderer>().size;
        bCol.offset = new Vector2(0f, 0f);
    }

	void resetTrapCollider(BoxCollider2D bCol) {
		bCol.size = originalTriggerSize;
		bCol.offset = originalTriggerOffset;
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
			if (!hasTriggered) {
                StartCoroutine(delayTrapActivation(trapTriggerDelay));		
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

    private IEnumerator delayTrapActivation(float wTime) {
        yield return new WaitForSeconds(wTime);

        hasTriggered = true;
        resizeTrapCollider(this.gameObject.GetComponent<BoxCollider2D>());
        StartCoroutine(resetSpikeCollider(trapResetDelay, this.gameObject.GetComponent<BoxCollider2D>()));
    }
}
