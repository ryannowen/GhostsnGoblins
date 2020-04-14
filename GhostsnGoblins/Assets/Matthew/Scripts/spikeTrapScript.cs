using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapScript : MonoBehaviour {

    enum t_Type { triggerable, timed, pressurePlated };

    private Vector2 originalTrapPosition;
    private bool hasTriggered;
    private bool isCurrentlyActive = true;
    private bool runCoroutine = false;
    private bool canPlaySound = true;

    [SerializeField] BoxCollider2D triggerCollider = null;

    [Tooltip("Sets the speed of the trap.")]
    [SerializeField] private float trapSpeed = 12;

    [Tooltip("The amount of time before the trap can be triggered again (only used if the trap type is triggerable).")]
    [SerializeField] private float trapActivationDelay = 1;

    [Tooltip("The amount of time to wait before the trap activates (in seconds)")]
    [SerializeField] private float trapTriggerDelay = 0.1f;

    [Tooltip("Reset delay, which is the time before the trap resets (for timed it is the delay for it to go up and down) (in seconds).")]
    [SerializeField] private float trapResetDelay = 2;

    [Tooltip("The amount of damage the spike trap does.")]
    [SerializeField] private int spikeTrapDamage = 1;

    [Tooltip("Timed - automatically moving. Triggered - Has to be collided with to activate. Pressure plated - Should be the option if intended to be controlled by a pressure plate.")]
    [SerializeField] private t_Type trapType = t_Type.triggerable;

    // Start is called before the first frame update
    void Start() {
        originalTrapPosition = transform.position;

        if (trapType == t_Type.timed) {
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
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (!hasTriggered) {
                if (trapType == t_Type.triggerable) {
                    if (isCurrentlyActive) { 
                        isCurrentlyActive = false;

                        if (triggerCollider != null) {
                            triggerCollider.enabled = false;
                        }
                    }
                }
                
                StartCoroutine(delayTrapActivation(trapTriggerDelay));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            if (col.gameObject.GetComponent<IDamageable>() != null) {
                col.gameObject.GetComponent<IDamageable>().TakeDamage(spikeTrapDamage);
            }

            if (col.gameObject.GetComponent<ICanTakeKnockback>() != null) {
                col.gameObject.GetComponent<ICanTakeKnockback>().TakeKnockback(col.transform.position, 20);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        switch (trapType) {
            case t_Type.timed:
                if (hasTriggered) {
                    if (canPlaySound) {
                        Singleton_Sound.m_instance.PlayAudioClipOneShot("Spike", 0.2f);
                        canPlaySound = false;
                    }

                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up - new Vector2(0, 0.3f), Time.fixedDeltaTime * trapSpeed);
                    StartCoroutine(resetSpikeTrap(trapResetDelay));
                } else {

                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.fixedDeltaTime * trapSpeed);
                    StartCoroutine(delayTrapActivation(trapResetDelay));
                }
                break;

            case t_Type.triggerable:

                // Ask if hasTriggered is true and if it is, play the required animation.
                if (hasTriggered) {
                    if (canPlaySound) {
                        Singleton_Sound.m_instance.PlayAudioClip("Spike", 0.25f);
                        canPlaySound = false;
                    }

                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up - new Vector2(0, 0.3f), Time.fixedDeltaTime * trapSpeed);

                    if (Vector2.Distance((Vector2)transform.position, (Vector2)transform.up - new Vector2(0, 0.3f)) < 0.05f && !isCurrentlyActive && runCoroutine) {
                        runCoroutine = false;
                        StartCoroutine(resetSpikeTrap(trapResetDelay));
                    }
                } else {

                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.fixedDeltaTime * trapSpeed);

                    if ((Vector2)transform.position == (Vector2)originalTrapPosition && !isCurrentlyActive && runCoroutine) {
                        runCoroutine = false;
                        StartCoroutine(delayTrapEnabling(trapActivationDelay));
                    }
                }
                
                break;

            case t_Type.pressurePlated:
                if (hasTriggered) {
                    if (canPlaySound) {
                        Singleton_Sound.m_instance.PlayAudioClip("Spike", 0.25f);
                        canPlaySound = false;
                    }

                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition + (Vector2)transform.up, Time.fixedDeltaTime * trapSpeed);
                    StartCoroutine(resetSpikeTrap(trapResetDelay));
                } else {
                    transform.position = Vector3.Lerp(transform.position, originalTrapPosition, Time.fixedDeltaTime * trapSpeed);
                }
                break;
        }
    }

    private IEnumerator resetSpikeTrap(float wTime) {
        yield return new WaitForSeconds(wTime);

        hasTriggered = false;
        canPlaySound = true;
        runCoroutine = true;
    }

    private IEnumerator delayTrapActivation(float wTime) {
        yield return new WaitForSeconds(wTime);

        hasTriggered = true;
        canPlaySound = true;
        runCoroutine = true;
    }

    private IEnumerator delayTrapEnabling(float wTime) {
        yield return new WaitForSeconds(wTime);

        isCurrentlyActive = true;

        if (triggerCollider != null) {
            triggerCollider.enabled = true;
        }
    }
}
