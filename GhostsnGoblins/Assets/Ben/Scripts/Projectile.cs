using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Vector2 m_Direction = new Vector2(1,0);

    // Start is called before the first frame update
    void Start()
    {

        m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        m_Rigidbody.AddForce(m_Direction, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public void SetDirection(Vector2 argsNewDirection)
    {

        m_Direction = argsNewDirection;

    }

}
