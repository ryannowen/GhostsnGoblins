using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartScript : MonoBehaviour, ISpawn {

    private float dartSpeed;

    // Start is called before the first frame update
    void Start() {
        dartSpeed = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            // Take health from the player here.


            // Destroy the game object.
            gameObject.SetActive(false);
        } 
    }

    // Update is called once per frame
    void Update() {
        transform.position += (transform.up * dartSpeed);
    }

    public void OnSpawn() {

    }

    public void OnDeSpawn() {

    }
}
