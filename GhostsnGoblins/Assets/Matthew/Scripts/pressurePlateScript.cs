using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlateScript : MonoBehaviour {

    [Tooltip("Drag the trap prefabs you wish to be activated when the pressure plate is touched into the list.")]
    [SerializeField] private GameObject[] trapsList = { };

    [Tooltip("Set the amount of time before the pressure plate can be used again (in seconds).")]
    [SerializeField] private float pressurePlateDelay = 3;

    private bool oneTimeBool = false;
    private bool pressurePlateTriggered = false;
    private Vector2 originalPlatePos;

    // Start is called before the first frame update
    void Start() {
        originalPlatePos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (!pressurePlateTriggered) {
            if (col.gameObject.tag == "Player") {
            
                for (int i = 0; i < trapsList.Length; i++) {
                    switch (trapsList[i].tag) {
                        case "SpikeTrap":
                            trapsList[i].GetComponent<spikeTrapScript>().activateSpikeTrap();
                            break;

                        case "DartTrap":
                            trapsList[i].GetComponent<dartTrapScript>().activateDartTrap();
                            break;
                    }
                }

                
                pressurePlateTriggered = true;
                oneTimeBool = true;
                StartCoroutine(resetPressurePlate(pressurePlateDelay));
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (oneTimeBool) {
            if (pressurePlateTriggered) {
                transform.position = Vector3.Lerp(transform.position, originalPlatePos - ((Vector2)transform.up * 0.17f), Time.fixedDeltaTime * 10);
            } else {
                transform.position = Vector3.Lerp(transform.position, originalPlatePos + ((Vector2)transform.up * 0.17f), Time.fixedDeltaTime * 10);
            }
        }
    }

    private IEnumerator resetPressurePlate(float wTime) {
        yield return new WaitForSeconds(wTime);

        pressurePlateTriggered = false;
    }
}
