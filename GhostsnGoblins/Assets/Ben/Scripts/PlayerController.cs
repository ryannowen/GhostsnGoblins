using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKillable, IDamageable
{

    [SerializeField] private int playerHealth = 2;
    [SerializeField] private int armourPoints = 3;

    GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {
     
        

    }

    public void TakeDamage(int amount)
    {

        playerHealth -= amount;

    }

    public void KillEntity()
    {

        gameObject.SetActive(false);

    }

}
