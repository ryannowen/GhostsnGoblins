using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformScript : MonoBehaviour {

    enum p_Direction { up, down, left, right };

    enum p_Type { loop, touchReverse, touchOnce };

    [SerializeField] private float timeToReachDestination = 1;
    [SerializeField] private int distance = 10;
    [SerializeField] private float movingDelay = 1;
    [SerializeField] private p_Direction platformDirection = p_Direction.left;
    [SerializeField] private p_Type platformMovementType = p_Type.touchOnce;

    private bool canMove;
    private bool isReversing = false;

    //private float interpolateValue = 0.0f;

    private Vector2 startPos;
    private Vector2 endPos;
    private Vector3 vel;

    // Start is called before the first frame update
    void Start() {
        if (platformMovementType == p_Type.loop) {
            canMove = true;
        } else {
            canMove = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.transform.parent = transform;

            if (platformMovementType == p_Type.touchOnce || platformMovementType == p_Type.touchReverse) {
                StartCoroutine(enableMoving(movingDelay));
            }
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.transform.parent = null;
        }
    }

    void FixedUpdate() {
        if (canMove) {

            switch (platformMovementType) {
                case p_Type.loop:
                    if (!isReversing) {
                        transform.position = Vector3.SmoothDamp((Vector2)transform.position, endPos, ref vel, timeToReachDestination);

                        switch (platformDirection) {
                            case p_Direction.up:
                                if (transform.position.y >= endPos.y) {
                                    StartCoroutine(reverseMoving(movingDelay));
                                }

                                break;

                            case p_Direction.down:
                                if (transform.position.y <= endPos.y) {
                                    StartCoroutine(reverseMoving(movingDelay));
                                }

                                break;

                            case p_Direction.left:
                                if (transform.position.x <= endPos.x) {
                                    StartCoroutine(reverseMoving(movingDelay));
                                }

                                break;

                            case p_Direction.right:
                                if (transform.position.x >= endPos.x) {
                                    StartCoroutine(reverseMoving(movingDelay));
                                }

                                break;
                        }

                    } else if (isReversing) {
                        transform.position = Vector3.SmoothDamp((Vector2)transform.position, endPos, ref vel, timeToReachDestination);

                        switch (platformDirection) {
                            case p_Direction.up:
                                if (transform.position.y >= endPos.y) {
                                    StartCoroutine(enableMoving(movingDelay));
                                }

                                break;

                            case p_Direction.down:
                                if (transform.position.y <= endPos.y) {
                                    StartCoroutine(enableMoving(movingDelay));
                                }

                                break;

                            case p_Direction.left:
                                if (transform.position.x <= endPos.x) {
                                    StartCoroutine(enableMoving(movingDelay));
                                }

                                break;

                            case p_Direction.right:
                                if (transform.position.x >= endPos.x) {
                                    StartCoroutine(enableMoving(movingDelay));
                                }

                                break;
                        }
                    }

                    break;

                case p_Type.touchOnce:
                    transform.position = Vector3.SmoothDamp((Vector2)transform.position, endPos, ref vel, timeToReachDestination);

                    //transform.position = Vector2.Lerp(transform.position, endPos, platformSpeed * Time.fixedDeltaTime);

                    /*if (Vector2.Distance(transform.position, endPos) >= distance / 10) {
                        Vector2 vec = endPos - (Vector2)transform.position;
                        vec.Normalize();
                        transform.position = (Vector2)transform.position + ((vec * platformSpeed) * Time.deltaTime);
                    } else {
                        transform.position = Vector2.Lerp(transform.position, endPos, platformSpeed * Time.deltaTime);
                    }*/

                    break;

                case p_Type.touchReverse:

                    break;
            }
        }
    }

    private IEnumerator reverseMoving(float wTime) {
        yield return new WaitForSeconds(wTime);

        startPos = transform.position;

        switch (platformDirection) {
            case p_Direction.up:
                endPos = transform.TransformPoint((Vector2)(-transform.up * distance));
                break;

            case p_Direction.down:
                endPos = transform.TransformPoint((Vector2)(transform.up * distance));
                break;

            case p_Direction.left:
                endPos = transform.TransformPoint((Vector2)(transform.right * distance));
                break;

            case p_Direction.right:
                endPos = transform.TransformPoint((Vector2)(-transform.right * distance));
                break;
        }

        isReversing = true;
    }

    private IEnumerator enableMoving(float wTime) {
        yield return new WaitForSeconds(wTime);

        startPos = transform.position;

        switch(platformDirection) {
            case p_Direction.up:
                endPos = transform.TransformPoint((Vector2)(transform.up * distance));
                break;

            case p_Direction.down:
                endPos = transform.TransformPoint((Vector2)(-transform.up * distance));
                break;

            case p_Direction.left:
                endPos = transform.TransformPoint((Vector2)(-transform.right * distance));
                break;

            case p_Direction.right:
                endPos = transform.TransformPoint((Vector2)(transform.right * distance));
                break;
        }

        isReversing = false;
        canMove = true;
    }
}
