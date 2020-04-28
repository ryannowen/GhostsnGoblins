using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlateScript : MonoBehaviour {

    [Tooltip("Drag the trap prefabs you wish to be activated when the pressure plate is touched into the list.")]
    [SerializeField] private GameObject[] trapsList = { };

    [Tooltip("Set the amount of time before the pressure plate can be used again (in seconds).")]
    [SerializeField] private float pressurePlateDelay = 3;

    [Tooltip("Set the speed that the pressure plate moves down.")]
    [SerializeField] private float pressurePlateSpeed = 10;

    private bool oneTimeBool = false;
    private bool pressurePlateTriggered = false;
    private Vector2 originalPlatePos;
    private float pressurePlateYSize;

    // Start is called before the first frame update
    void Start() {
        pressurePlateYSize = this.gameObject.GetComponent<SpriteRenderer>().size.y;
        originalPlatePos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!pressurePlateTriggered) {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
            
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
                transform.position = Vector3.Lerp(transform.position, originalPlatePos - ((Vector2)transform.up * pressurePlateYSize), Time.fixedDeltaTime * pressurePlateSpeed);
            } else {
                transform.position = Vector3.Lerp(transform.position, originalPlatePos, Time.fixedDeltaTime * pressurePlateSpeed);
            }
        }
    }

    private IEnumerator resetPressurePlate(float wTime) {
        yield return new WaitForSeconds(wTime);

        pressurePlateTriggered = false;
    }
}
