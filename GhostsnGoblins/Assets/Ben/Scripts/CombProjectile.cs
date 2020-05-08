using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombProjectile : MonoBehaviour
{

    [SerializeField] float m_TargetDetectionSize = 0.1f;
    GameObject m_FiredFrom = null;
    bool m_HasReachedTarget = false;
    Vector3 m_StartPos = Vector3.zero;
    Vector3 m_TargetPos = Vector3.zero;
    float m_ProjectileSpeed = 5f;
    Rigidbody2D m_Rigidbody = null;
    AudioSource m_AudioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // If comb has reached its target
        if (Vector3.Distance(transform.position, m_TargetPos) <= m_TargetDetectionSize)
        {
            m_HasReachedTarget = true;
        }
        else
        {
            m_Rigidbody.velocity = Vector3.zero;
            Vector3 targetDirection = m_TargetPos - m_StartPos;
            m_Rigidbody.AddForce(targetDirection.normalized * m_ProjectileSpeed, ForceMode2D.Impulse);
        }

        if (m_HasReachedTarget)
        {
            m_Rigidbody.velocity = Vector3.zero;
            Vector3 returnDirection = m_FiredFrom.transform.position - transform.position;
            m_Rigidbody.AddForce(returnDirection.normalized * m_ProjectileSpeed, ForceMode2D.Impulse);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            if (m_HasReachedTarget)
            {
                m_AudioSource.Stop();
                m_AudioSource.loop = false;
                m_AudioSource = null;
                this.gameObject.SetActive(false);
            }

        }

    }

    public void SetPathInfo(GameObject argsFiredFrom, Vector3 argsTargetPosition, float argsProjectileSpeed)
    {

        m_HasReachedTarget = false;
        m_FiredFrom = argsFiredFrom;
        m_StartPos = m_FiredFrom.transform.position;
        m_TargetPos = argsTargetPosition;
        m_ProjectileSpeed = argsProjectileSpeed;

    }

    public void SetAudioInfo(AudioSource argsAudioSource)
    {
        m_AudioSource = argsAudioSource;
    }

    public void StopAudioSource()
    {
        m_AudioSource.Stop();
    }

}
