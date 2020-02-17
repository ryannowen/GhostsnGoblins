using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartTrapScript : MonoBehaviour {

    enum t_Type { triggerable, timed };

    [SerializeField] private GameObject dartObj = null;
    [SerializeField] private float dartSpeed = 10;
    [SerializeField] private int dartDamage = 1;
    [SerializeField] private t_Type trapType = t_Type.triggerable;
    [SerializeField] private float shootDelay = 2;

    private bool canSpawnPart;

    // Start is called before the first frame update
    void Start() {
        if (shootDelay < 0.5f){
            shootDelay = 0.5f;
        }

        canSpawnPart = true;
        System_Spawn.instance.CreatePool(dartObj, Mathf.RoundToInt(10 / shootDelay), false);
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (trapType == t_Type.triggerable) {
                if (canSpawnPart) {
                    GameObject tmpDart = System_Spawn.instance.GetObjectFromPool(dartObj);

                    tmpDart.GetComponent<dartScript>().setDartDamage(dartDamage);
                    tmpDart.GetComponent<dartScript>().setDartSpeed(dartSpeed);

                    tmpDart.transform.position = transform.position;
                    tmpDart.transform.rotation = transform.rotation;

                    canSpawnPart = false;
                    StartCoroutine(delayDartSpawn(shootDelay));
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (trapType == t_Type.timed) {
            if (canSpawnPart) {
                GameObject tmpDart = System_Spawn.instance.GetObjectFromPool(dartObj);

                tmpDart.GetComponent<dartScript>().setDartDamage(dartDamage);
                tmpDart.GetComponent<dartScript>().setDartSpeed(dartSpeed);

                tmpDart.transform.position = transform.position;
                tmpDart.transform.rotation = transform.rotation;

                canSpawnPart = false;
                StartCoroutine(delayDartSpawn(shootDelay));
            }
        }
    }

    // Wait time before the trap can spawn another dart.
    private IEnumerator delayDartSpawn(float wTime) {
        yield return new WaitForSeconds(wTime);
        canSpawnPart = true;
    }
}
