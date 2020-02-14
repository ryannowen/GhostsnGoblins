using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartScript : MonoBehaviour, ISpawn {

    [SerializeField] private float dartSpeed;

    // Start is called before the first frame update
    void Start() {
        dartSpeed = 5;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            // Call the TakeDamage function from IDamageable interface here.
            col.gameObject.GetComponent<IDamageable>().TakeDamage(1);

            // Destroy the game object.
            gameObject.SetActive(false);
        } 
    }

    // Update is called once per frame
    void Update() {
        transform.position += (transform.up * dartSpeed) * Time.deltaTime;
    }

    public void OnSpawn() {

    }

    public void OnDeSpawn() {

    }
}
