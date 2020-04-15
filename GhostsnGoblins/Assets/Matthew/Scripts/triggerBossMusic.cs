using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBossMusic : MonoBehaviour {

    [Tooltip("This is the amount of time each increment of volume decreases on the main audio (in seconds).")]
    [SerializeField] private float fadeOutTimeDelay = 0.02f;

    [Tooltip("This is the amount of time each increment of volume increases on the main audio (in seconds).")]
    [SerializeField] private float fadeInTimeDelay = 0.02f;

    private void OnTriggerEnter2D(Collider2D col) {
        if (enabled) {
            if (col.gameObject.CompareTag("Player")) {
                Singleton_Sound.m_instance.transitionToDifferentSound("BossBattle", fadeOutTimeDelay, fadeInTimeDelay, 0.75f);
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
