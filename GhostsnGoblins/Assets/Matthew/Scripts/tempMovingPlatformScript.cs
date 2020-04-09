using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempMovingPlatformScript : MonoBehaviour {

	public enum platformType { looping, triggered }

	[Tooltip("Place empty GameObjects in here, the platform will move to the GameObjects' position. Insert each GameObject in the order you wish the platform to traverse.")]
	[SerializeField] private GameObject[] positionArray = null;

	[Tooltip("Platform type, looping or triggered.")]
	[SerializeField] platformType pType = platformType.looping;

	[Tooltip("The amount of time it takes to move a platform to a point in the position array.")]
	[SerializeField] private float platformMoveDelay = 1;

	[Tooltip("Time (in seconds) it takes for the platform to move to a certain position.")]
	[SerializeField] private float platformDestinationTime = 3;

	private mainPlatformScript platformScript = null;
	private Transform platformTransform = null;
	private Vector3 vel;

	private int startPNum = 0;
	private int currentPNum = -1;
	private int endPNum = 0;

	private bool fallingMode = false;
	private bool hasTriggered = false;
	private bool currentlyMoving = false;
	private bool switchMovementDir = false;
	private bool initialTriggerMovement = true;

	private void Awake() {
		platformMoveDelay = Mathf.Abs(platformMoveDelay);
		platformDestinationTime = Mathf.Abs(platformDestinationTime);

		platformScript = transform.parent.gameObject.GetComponent<mainPlatformScript>();
		if (platformScript == null) {
			print("Couldn't find the platform script on the child collider's parent. Please attach one!");
			return;
		}

		if (positionArray == null) {
			gameObject.GetComponent<tempMovingPlatformScript>().enabled = false;
			return;
		}

		platformTransform = transform.parent;
		if (platformTransform == null) {
			if (platformScript.getPrintErrorBool()) {
				print("Couldn't find a parent transform for the platform to function with!");
			}

			gameObject.GetComponent<tempMovingPlatformScript>().enabled = false;
			return;
		}

		if (positionArray.Length > 0) {
			positionArray[startPNum].transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, 0);

			foreach (GameObject posObj in positionArray) {
				posObj.transform.position = new Vector3(posObj.transform.position.x, posObj.transform.position.y, 0);
			}

			platformTransform.position = positionArray[startPNum].transform.position;
			endPNum = (positionArray.Length - 1);
		} else {
			if (platformScript.getPrintErrorBool()) {
				print("Nothing is in the position array. Please attach GameObject points into the position array!");
			}

			gameObject.GetComponent<tempMovingPlatformScript>().enabled = false;
			return;
		}
	}

	private void Start() {
		if (enabled) {
			if (pType == platformType.looping) {
				StartCoroutine(moveP());
			}
		}
	}

    private void OnCollisionEnter2D(Collision2D col) {
        if (enabled) {
            if (col.gameObject.CompareTag("Player")) {
                if (!fallingMode) {
                    col.gameObject.transform.parent = platformTransform;
                }

                if (pType == platformType.triggered) {
                    if (!hasTriggered && !fallingMode) {
                        currentPNum++;
                        hasTriggered = true;
                        StartCoroutine(moveP());
                    }
                }
            }
        }
    }

	private void OnCollisionExit2D(Collision2D col) {
        if (enabled) {
            if (col.gameObject.CompareTag("Player")) {
                col.gameObject.transform.parent = null;
            }
        }
	}

	private void FixedUpdate() {
		if (enabled) {
			switch (pType) {
				case platformType.looping:
					if (!fallingMode) {
						if (currentlyMoving) {
							if (!checkPState(platformTransform.position)) {
								platformTransform.position = Vector3.SmoothDamp(platformTransform.position, positionArray[currentPNum].transform.position, ref vel, platformDestinationTime);
							} else {
								currentlyMoving = false;
								StartCoroutine(moveP());
							}
						} else {
							return;
						}
					} else {
						return;
					}

					break;

				case platformType.triggered:
					if (!fallingMode) {
						if (hasTriggered) {
							if (currentlyMoving) {
								if (!checkPState(platformTransform.position)) {
									platformTransform.position = Vector3.SmoothDamp(platformTransform.position, positionArray[currentPNum].transform.position, ref vel, platformDestinationTime);
								} else {
									currentlyMoving = false;
									StartCoroutine(moveP());
								}
							} else {
								return;
							}
						} else {
							return;
						}
					} else {
						return;
					}

					break;
				default:
					return;
			}
		}
	}

	public bool getFallingMode() {
		return fallingMode;
	}

	public void setFallingMode(bool fallingBool) {
		fallingMode = fallingBool;
	}

	public void setPlatformNumDefault() {
		if (enabled) {
			currentPNum = startPNum;
			platformTransform.position = positionArray[startPNum].transform.position;
		}
	}
	public platformType getPlatformType() {
		return pType;
	}

	private bool checkPState(Vector3 platformPos) {
		if (Vector3.Distance(platformPos, positionArray[currentPNum].transform.position) <= 0.05f) {
			return true;
		}

		return false;
	}

	private IEnumerator moveP() {
		yield return new WaitForSeconds(platformMoveDelay);

		if (!fallingMode) {
			switch (pType) {
				case platformType.looping:
					if (currentPNum <= startPNum) {
						currentPNum = startPNum;
						switchMovementDir = false;
					}

					if (currentPNum >= endPNum) {
						currentPNum = endPNum;
						switchMovementDir = true;
					}

					if (!switchMovementDir) {
						currentPNum++;
					} else {
						currentPNum--;
					}

					currentlyMoving = true;
					break;

				case platformType.triggered:
					if (currentPNum <= startPNum && initialTriggerMovement) {
						currentPNum = startPNum;
						switchMovementDir = false;
						initialTriggerMovement = false;
					} else if (currentPNum <= startPNum && !initialTriggerMovement) {
						currentPNum = -1;
						switchMovementDir = false;
						initialTriggerMovement = true;
						hasTriggered = false;
						currentlyMoving = false;
						break;
					}

					if (currentPNum >= endPNum) {
						currentPNum = endPNum;
						switchMovementDir = true;
					}

					if (!switchMovementDir) {
						currentPNum++;
					} else {
						currentPNum--;
					}

					currentlyMoving = true;
					break;

				default:
					break;
			}
		}
	}
}