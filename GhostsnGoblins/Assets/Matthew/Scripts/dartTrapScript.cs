using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartTrapScript : MonoBehaviour {

    [SerializeField] private GameObject dartObj;
    [SerializeField] private bool canSpawnPart;

    // Start is called before the first frame update
    void Start() {
        canSpawnPart = true;
        System_Spawn.instance.CreatePool(dartObj, 5, false);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (canSpawnPart) {
                GameObject tmpDart = System_Spawn.instance.GetObjectFromPool(dartObj);

                tmpDart.transform.position = transform.position;
                tmpDart.transform.rotation = transform.rotation;

                delayDartSpawn(3.0f);
                canSpawnPart = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Wait time before the trap can spawn another dart.
    private IEnumerator delayDartSpawn(float wTime) {
        yield return new WaitForSeconds(wTime);
        canSpawnPart = true;
    }
}
