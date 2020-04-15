using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBossMusic : MonoBehaviour {

    [SerializeField] private float fadeOutSpeed = 1;
    [SerializeField] private float fadeInSpeed = 50;

    private void OnTriggerEnter2D(Collider2D col) {
        if (enabled) {
            if (col.gameObject.CompareTag("Player")) {
                Singleton_Sound.m_instance.transitionToDifferentSound("BossBattle", fadeOutSpeed, fadeInSpeed, 0.75f);
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
