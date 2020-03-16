using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainPlatformScript : MonoBehaviour {

    [Tooltip("Platform settings are now stored in the 'ChildCollider' GameObject in the platform!")]
    [SerializeField] BoxCollider2D childCol;

    [Tooltip("Platform settings are now stored in the 'ChildCollider' GameObject in the platform!")]
    [SerializeField] private float solidTime = 0f;

    [Tooltip("Platform settings are now stored in the 'ChildCollider' GameObject in the platform!")]
    [SerializeField] private float timer = 0f;

    // Start is called before the first frame update
    void Start() {
        if (childCol == null) {
            childCol = gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (timer >= 0f) {
            timer -= Time.deltaTime;
        }
            
        if (timer <= 0f) {
            childCol.enabled = true;
        } else {
            childCol.enabled = false;
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