using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformScript : MonoBehaviour {

    enum p_type { loop, reverseLoop, touchOnce, touchReverse };

    [Tooltip("Collection of points (as a GameObject) that the platform travels. Adjust size depending on how many points. The platform will start at the first point supplied.")]
    [SerializeField] private GameObject[] platformPoints = { };

    [Tooltip("Loop - Traverses the points repeatedly. Reverse Loop - Traverses the points and reverses repeatedly. Touch Once - Traverses the points once. Touch Reverse - Traverses the points once, then reverses.")]
    [SerializeField] private p_type platformType = p_type.touchOnce;

    [Tooltip("Time to reach the next listed GameObject in the list (in seconds).")]
    [SerializeField] private float timeToReachPoint = 1;

    [Tooltip("A delay before the platform moves to the next point (in seconds).")]
    [SerializeField] private float moveDelay = 0.2f;

    private bool canMove;
    private bool isReversing;
    private bool oneTimeReverse;
    private Vector3 originalPosition;

    private int platformCount;
    private Vector3 vel;


    // Start is called before the first frame update
    void Start() {
        if (platformPoints.Length == 0) {
            print("No points found for the platform to traverse, please supply GameObjects as points.");
            this.enabled = false;
        } else {
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            oneTimeReverse = false;
            platformCount = 0;
            transform.position = new Vector3(Mathf.Round(platformPoints[0].transform.position.x), Mathf.Round(platformPoints[0].transform.position.y), 0);
            isReversing = false;
            canMove = false;

            if (platformType == p_type.loop || platformType == p_type.reverseLoop) {
                StartCoroutine(enableMoving(0));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            if (platformType == p_type.touchOnce || platformType == p_type.touchReverse) {
                StartCoroutine(enableMoving(moveDelay));
            }
            
            col.gameObject.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            print("I get into this for some reason???");
            col.gameObject.transform.parent = null;
            oneTimeReverse = false;
        }
    }

    void FixedUpdate() {
        if (canMove) {
            switch (platformType) {
                case p_type.touchOnce:
                    if (platformCount == platformPoints.Length) {
                        platformCount = platformPoints.Length;
                        canMove = false;
                        break;
                    }

                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0), ref vel, timeToReachPoint);
                    
                    if (Vector3.Distance(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0)) < 0.2f) {
                        canMove = false;
                        StartCoroutine(platformOnce(moveDelay));
                    }

                    break;

                case p_type.touchReverse:
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0), ref vel, timeToReachPoint * Time.fixedDeltaTime);

                    if (Vector3.Distance(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0)) < 0.2f) {
                        canMove = false;
                        StartCoroutine(platformReverse(moveDelay));
                    }
                    break;

                case p_type.reverseLoop:
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0), ref vel, timeToReachPoint);

                    if (Vector3.Distance(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0)) < 0.2f) {
                        canMove = false;
                        StartCoroutine(platformReverseLoop(moveDelay));
                    }

                    break;

                case p_type.loop:
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0), ref vel, timeToReachPoint);

                    if (Vector3.Distance(transform.position, new Vector3(platformPoints[platformCount].transform.position.x, platformPoints[platformCount].transform.position.y, 0)) < 0.2f) {
                        canMove = false;
                        StartCoroutine(platformLoop(moveDelay));
                    }

                    break;
            }

        }
    }

    private IEnumerator platformOnce(float wTime) {
        yield return new WaitForSeconds(wTime);

        platformCount++;
        canMove = true;
    }

    private IEnumerator platformReverse(float wTime) {
        yield return new WaitForSeconds(wTime);

        if (platformCount < (platformPoints.Length - 1) && !isReversing && !oneTimeReverse) {
            platformCount++;
        } else if (platformCount > 0 && isReversing) {
            platformCount--;
            oneTimeReverse = true;
        }

        if (!isReversing && platformCount == 0) {
            canMove = false;
        } else {
            canMove = true;
        }

        if (platformCount == (platformPoints.Length - 1)) {
            isReversing = true;
        } else if (platformCount == 0) {
            isReversing = false;
        }
    }

    private IEnumerator platformLoop(float wTime) {
        yield return new WaitForSeconds(wTime);
        platformCount++;

        if (platformCount == platformPoints.Length) {
            platformCount = 0;
        }

        canMove = true;
    }

    private IEnumerator platformReverseLoop(float wTime) {
        yield return new WaitForSeconds(wTime);

        if (platformCount < (platformPoints.Length - 1) && !isReversing) {
            platformCount++;
        } else if (platformCount > 0 && isReversing) {
            platformCount--;
        }

        if (platformCount == (platformPoints.Length - 1)) {
            isReversing = true;
        } else if (platformCount == 0) {
            isReversing = false;
        }

        canMove = true;
    }

    private IEnumerator enableMoving(float wTime) {
        yield return new WaitForSeconds(wTime);

        canMove = true;
    }
}