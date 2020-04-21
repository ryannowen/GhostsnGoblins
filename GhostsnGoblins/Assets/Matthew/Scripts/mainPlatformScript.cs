using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Dear Matthew:
 * 
 * When you wrote your platform code, only God and you knew how it worked,
 * because they were insane.
 * Now after you've rewrote them atleast once, only God knows sit!
 * 
 * Therefore, if you're trying to optimize or fix the routines and
 * you fail (most surely), please increase the counter
 * as a warning for next time you attempt it
 * 
 * long long int total_hours_wasted_doing_more_platforms:  567
 * 
 */

public class mainPlatformScript : MonoBehaviour {

    [Tooltip("Platform settings are now stored in the 'ChildCollider' GameObject in the platform!")]
    [SerializeField] BoxCollider2D childCol;

    [Tooltip("Platform settings are now stored in the 'ChildCollider' GameObject in the platform!")]
    [SerializeField] private float solidTime = 0f;

    [Tooltip("Platform settings are now stored in the 'ChildCollider' GameObject in the platform!")]
    [SerializeField] private float timer = 0f;

    [Tooltip("Should error printing be enabled?")]
    [SerializeField] private bool printErrors = false;

    private BoxCollider2D triggerCollider;

    // Start is called before the first frame update
    void Awake() {
        triggerCollider = gameObject.GetComponent<BoxCollider2D>();
        if (triggerCollider == null) {
            if (printErrors) {
                print("Couldn't find a collider attached to the main platform. Please attach one!");
            }
            
            Destroy(gameObject);
            return;
        }

        if (childCol == null) {
            childCol = gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        }

        if(null != childCol)
        {
            childCol.GetComponent<SpriteRenderer>().size = GetComponent<SpriteRenderer>().size;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (timer >= 0f) {
            timer -= Time.deltaTime;
        }

        if (timer <= 0f)
        {
            childCol.enabled = true;
        }
        else
        {
            childCol.enabled = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player"))
        {
            if (timer > solidTime)
            {
                return;
            }
            else
            {
                timer = solidTime;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player"))
        {
            if (timer > solidTime)
            {
                return;
            }
            else
            {
                timer = solidTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player"))
        {
            if (timer > solidTime)
            {
                return;
            }
            else
            {
                timer = solidTime;
            }
        }
    }

    public void setTimer(float timeValue) {
        timer = timeValue;
    }

    public float getTimer() {
        return timer;
    }

    public bool getPrintErrorBool() {
        return printErrors;
    }

    public void toggleTriggerCollider(bool triggerColBool) {
        if (triggerCollider != null) {
            triggerCollider.enabled = triggerColBool;
        }
    }
}