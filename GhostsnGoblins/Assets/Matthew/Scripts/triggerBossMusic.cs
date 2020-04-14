using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBossMusic : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col) {
        if (enabled) {
            if (col.gameObject.CompareTag("Player")) {
                Singleton_Sound.m_instance.transitionToDifferentSound("BossBattle", 0.5f, 1, 0.75f);
                enabled = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
