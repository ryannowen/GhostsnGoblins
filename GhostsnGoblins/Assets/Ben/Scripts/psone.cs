using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psone : MonoBehaviour
{

    [SerializeField] private GameObject ChildCollider;

    [SerializeField] private float solidTimer = 0f;
    [SerializeField] private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {

        if (timer >= 0f)
            timer -= Time.deltaTime;

        if (timer <= 0f)
            ChildCollider.SetActive(true);
        else
            ChildCollider.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer = solidTimer;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        timer = solidTimer;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        timer = solidTimer;
    }

}
