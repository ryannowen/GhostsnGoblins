using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainPlatformScript : MonoBehaviour {

    [SerializeField] GameObject childCol = null;
    [SerializeField] private float solidTime = 0f;
    [SerializeField] private float timer = 0f;

    // Start is called before the first frame update
    void Start() {
        if (childCol == null) {
            childCol = gameObject.transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (timer >= 0f) {
            timer -= Time.deltaTime;
        }
            
        if (timer <= 0f) {
            childCol.SetActive(true);
        } else {
            childCol.SetActive(false);
        }   
    }

    void OnTriggerEnter2D(Collider2D collision) {
        timer = solidTime;
    }

    void OnTriggerStay2D(Collider2D collision) {
        timer = solidTime;
    }

    void OnTriggerExit2D(Collider2D collision) {
        timer = solidTime;
    }
}