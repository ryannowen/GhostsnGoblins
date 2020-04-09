using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempFallingPlatformScript : MonoBehaviour {

	[Tooltip("Should the platform fade when falling?")]
	[SerializeField] private bool doesFade = false;

	[Tooltip("The speed of fading (opaque to transparent) in seconds.")]
	[SerializeField] private float fadeSpeed = 2;

	[Tooltip("Should the platform fall?")]
	[SerializeField] private bool shouldFall = true;

	[Tooltip("The delay before falling is applied to the platform in seconds.")]
	[SerializeField] private float fallDelay = 2;

	[Tooltip("Should the platform respawn once fallen?")]
	[SerializeField] private bool doesRespawn = true;

	[Tooltip("The delay before the platform reappears (in seconds).")]
	[SerializeField] private float respawnDelay = 3;

	private mainPlatformScript platformScript = null;
	private tempMovingPlatformScript movingPlatformScript = null;

	private Transform platformTransform = null;
	private Rigidbody2D platformRb2D = null;
	private SpriteRenderer spriteRend = null;

	private Vector3 initialPosition;
	private bool isFalling = false;

	private void Awake() {
		
		movingPlatformScript = gameObject.GetComponent<tempMovingPlatformScript>();
		if (movingPlatformScript == null) {
			print("Couldn't find the moving platform script on the child collider. Please attach one!");
			return;
		}

		platformScript = transform.parent.gameObject.GetComponent<mainPlatformScript>();
		if (platformScript == null) {
			print("Couldn't find the platform script on the child collider's parent. Please attach one!");
			movingPlatformScript.enabled = false;
			return;
		}

		platformTransform = transform.parent;
		if (platformTransform == null) {
			print("Couldn't find a parent transform for the platform to function with!");
			movingPlatformScript.enabled = false;
			return;
		}

		initialPosition = new Vector3(platformTransform.position.x, platformTransform.position.y, 0);

		platformRb2D = platformTransform.gameObject.GetComponent<Rigidbody2D>();
		if (platformRb2D == null) {
			print("Couldn't find a rigidbody2d attached to the parent platform. Please attach one.");
			return;
		}

		spriteRend = platformTransform.gameObject.GetComponent<SpriteRenderer>();
		if (spriteRend == null) {
			print("Couldn't find a sprite renderer attached to the parent platform. Please attach one.");
			return;
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
        if (enabled) {
            if (col.gameObject.CompareTag("Player")) {
                col.gameObject.transform.parent = platformTransform;
                StartCoroutine(makePlatformFall());
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
		float alphaCol = spriteRend.color.a;

		if (isFalling && doesFade) {
			alphaCol = Mathf.Lerp(alphaCol, 0, Time.fixedDeltaTime * fadeSpeed);
			spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alphaCol);
		} else if (!isFalling && doesFade) {
			alphaCol = Mathf.Lerp(alphaCol, 1, Time.fixedDeltaTime * fadeSpeed);
			spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alphaCol);
		}
	}

	private IEnumerator makePlatformFall() {
		yield return new WaitForSeconds(fallDelay);

        if (!movingPlatformScript.getFallingMode()) {
			isFalling = true;
			movingPlatformScript.setFallingMode(true);
			platformScript.setTimer(99999999);
			platformScript.toggleTriggerCollider(false);

			if (shouldFall) {
				platformRb2D.bodyType = RigidbodyType2D.Dynamic;
			}

			if (doesRespawn) {
				StartCoroutine(respawnPlatform());
			}
		}
	}

	private IEnumerator respawnPlatform() {
		yield return new WaitForSeconds(respawnDelay);

		if (movingPlatformScript.getFallingMode()) {
			platformRb2D.velocity = Vector3.zero;
			platformRb2D.bodyType = RigidbodyType2D.Kinematic;

			if (movingPlatformScript.enabled == true) {
				movingPlatformScript.setPlatformNumDefault();
			} else {
				platformTransform.position = initialPosition;
			}
			
			isFalling = false;
			movingPlatformScript.setFallingMode(false);

			StartCoroutine(enablePlatform());
		}
	}

	private IEnumerator enablePlatform() {
		while (spriteRend.color.a <= 0.99f) {
			yield return null;
		}

		platformScript.toggleTriggerCollider(true);
		platformScript.setTimer(0);
	}
}