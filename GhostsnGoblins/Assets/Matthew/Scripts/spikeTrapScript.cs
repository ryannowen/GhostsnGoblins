using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapScript : MonoBehaviour {

    enum t_Type { triggerable, timed, pressurePlated };

	private Vector2 originalTriggerSize;
	private Vector2 originalTriggerOffset;
	private Vector2 originalTrapPosition;

	private bool hasTriggered;

    [Tooltip("Sets the speed of the trap.")]
    [SerializeField] private float trapSpeed = 12;

    [Tooltip("Reset delay, which is the time before the trap resets (for timed it is the delay for it to go up and down) (in seconds).")]
    [SerializeField] private float trapResetDelay = 2;

    [Tooltip("The amount of damage the spike trap does.")]
    [SerializeField] private int spikeTrapDamage = 1;

    [Tooltip("Timed - automatically moving. Triggered - Has to be collided with to activate. Pressure plated - Should be the option if intended to be controlled by a pressure plate.")]
    [SerializeField] private t_Type trapType = t_Type.triggerable;

    // Start is called before the first frame update
    void Start() {
        originalTriggerSize = this.gameObject.GetComponent<BoxCollider2D>().size;
        originalTriggerOffset = this.gameObject.GetComponent<BoxCollider2D>().offset;
        originalTrapPosition = transform.position;

        if (trapType == t_Type.timed) {
            resizeTrapCollider(this.gameObject.GetComponent<BoxCollider2D>());
            hasTriggered = true;
        } else if (trapType == t_Type.triggerable) {
            hasTriggered = false;
        } else if (trapType == t_Type.pressurePlated) {
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        }
    }

    public void activateSpikeTrap() {
        hasTriggered = true;
        StartCoroutine(resetSpikeCollider(trapResetDelay, this.gameObject.GetComponent<BoxCollider2D>()));
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
                StartCoroutine(delayTrapActivation(trapResetDelay));		
			} else { 
				col.gameObject.GetComponent<IDamageable>().TakeDamage(spikeTrapDamage);
			}
		}
    }

    // Update is called once per frame
    void FixedUpdate() {
        switch(trapType) {
            case t_Type.timed:
                if (hasTriggered) {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up, Time.fixedDeltaTime * trapSpeed);
                    StartCoroutine(resetSpikeCollider(trapResetDelay, this.gameObject.GetComponent<BoxCollider2D>()));
                } else {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.fixedDeltaTime * trapSpeed);
                    StartCoroutine(delayTrapActivation(trapResetDelay));
                }
                break;

            case t_Type.triggerable:

                // Ask if hasTriggered is true and if it is, play the required animation.
                if (hasTriggered) {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up, Time.fixedDeltaTime * trapSpeed);
                } else {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.fixedDeltaTime * trapSpeed);
                }
                break;

            case t_Type.pressurePlated:
                if (hasTriggered) {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up, Time.fixedDeltaTime * trapSpeed);
                } else {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.fixedDeltaTime * trapSpeed);
                }
                break;

        } 
    }

	private IEnumerator resetSpikeCollider(float wTime, BoxCollider2D boxCol) {
		yield return new WaitForSeconds(wTime);

		hasTriggered = false;

        if (trapType == t_Type.triggerable) {
            resetTrapCollider(boxCol);
        }
	}

    private IEnumerator delayTrapActivation(float wTime) {
        yield return new WaitForSeconds(wTime);

        hasTriggered = true;

        if (trapType == t_Type.triggerable){
            resizeTrapCollider(this.gameObject.GetComponent<BoxCollider2D>());
            StartCoroutine(resetSpikeCollider(trapResetDelay, this.gameObject.GetComponent<BoxCollider2D>()));
        }
    }
}
