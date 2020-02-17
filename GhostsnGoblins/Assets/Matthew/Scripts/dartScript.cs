using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartScript : MonoBehaviour, ISpawn {

    private float dartSpeed = 5;
    private int dartDamage = 1;

    // Start is called before the first frame update
    void Start() {

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
        
    }

    void FixedUpdate() {
        transform.position += (transform.up * dartSpeed) * Time.fixedDeltaTime;
    }

    public void OnSpawn() {

    }

    public void OnDeSpawn() {

    }

    public void setDartSpeed(float dSpeed) {
        dartSpeed = dSpeed;
    }

    public void setDartDamage(int dDamage) {
        dartDamage = dDamage;
    }
}
