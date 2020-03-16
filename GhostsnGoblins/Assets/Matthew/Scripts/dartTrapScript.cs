using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartTrapScript : MonoBehaviour {

    enum t_Type { triggerable, timed, pressurePlated };

    [Tooltip("The object in which the trap fires (should always be dart).")]
    [SerializeField] private GameObject dartObj = null;

    [Tooltip("Triggerable - Trap fired when the player hits the collider. Timed - Fires automatically depending on a given delay. Pressure plated - Should be the option if intended to be controlled by a pressure plate.")]
    [SerializeField] private t_Type trapType = t_Type.triggerable;

    [Tooltip("A delay before an object fires again (in seconds).")]
    [SerializeField] private float shootDelay = 2;

    private bool canSpawnPart; 
    private FireProjectile fProjectile;

    // Start is called before the first frame update
    void Start() {
        fProjectile = this.gameObject.GetComponent<FireProjectile>();
        fProjectile.SetProjectile(dartObj);

        if (shootDelay < 0.5f) {
            shootDelay = 0.5f;
        }

        canSpawnPart = true;

        if (!Singleton_Sound.m_instance.DoesAudioClipExist("DartFire")) {
            Singleton_Sound.m_instance.CreateAudioClip("DartFire", Resources.Load("Sounds/DartFire") as AudioClip);
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (trapType == t_Type.triggerable) {
                activateDartTrap();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (trapType == t_Type.timed) {
            activateDartTrap();
        }
    }

    // Wait time before the trap can spawn another dart.
    private IEnumerator delayDartSpawn(float wTime) {
        yield return new WaitForSeconds(wTime);
        canSpawnPart = true;
    }

    public void activateDartTrap() {
        if (canSpawnPart) {
            Singleton_Sound.m_instance.PlayAudioClip("DartFire");

            fProjectile.Fire(transform.position, transform.up, transform.rotation);
            canSpawnPart = false;
            StartCoroutine(delayDartSpawn(shootDelay));
        }
    }
}