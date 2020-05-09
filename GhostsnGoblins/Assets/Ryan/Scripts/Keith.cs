using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keith : MonoBehaviour, IDamageable
{
    enum EState
    {
        eIdle,
        eRoll,
        eBeam,
        eLaughing,
        eCount
    }

    [SerializeField] private GameObject[] m_IdlePositions = null;
    [SerializeField] private EState m_state = EState.eRoll;
    [SerializeField] private float m_switchStateDelay = 3.0f;


    [SerializeField] private GameObject[] m_enemySpawners = null;

    [SerializeField] private GameObject[] m_levelDoors = null;
    [SerializeField] private GameObject[] m_rewardSpawners = null;
    [SerializeField] private string m_defeatedSound = "LevelFinished";
    [SerializeField] private string m_damagedSound = "DamageInflictedSound";
    private Animator m_animator;
    private GameObject m_player;

    [SerializeField] private int m_health = 10;
    [SerializeField] private int m_normalSpeed = 10;
    [SerializeField] private int m_rollSpeed = 6;
    [SerializeField] private int m_beamSpeed = 4;
    [SerializeField] private float m_beamStopAttackDelay = 3.0f;

    [SerializeField] private GameObject m_beamGameObject = null;

    private GameObject m_focusedPlayer = null;
    private GameObject m_chosenIdle = null;

    private WaitForSeconds m_delayRandomiseStateSeconds;
    private WaitForSeconds m_delayStopBeamSeconds;


    private bool m_changingState = false;
    private bool m_playerOnRight = false;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_player = Singleton_Game.m_instance.GetPlayer(0);

        m_delayRandomiseStateSeconds = new WaitForSeconds(m_switchStateDelay);
        m_delayStopBeamSeconds = new WaitForSeconds(m_beamStopAttackDelay);

        ChooseIdlePosition();
        ActivateNewState();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_focusedPlayer.transform.position.x - transform.position.x > 0)
            m_playerOnRight = true;
        else
            m_playerOnRight = false;

        if (null != m_animator)
        {
            m_animator.SetBool("isPlayerOnRight", m_playerOnRight);
            m_animator.SetFloat("RollSpeed", m_playerOnRight ? 1 : -1);
        }

        switch (m_state)
        {
            case EState.eIdle:
                transform.position = Vector3.MoveTowards(transform.position, m_chosenIdle.transform.position, Time.deltaTime * m_normalSpeed);

                if(transform.position == m_chosenIdle.transform.position)
                {
                    if(!m_changingState)
                        StartCoroutine(RandomiseStateDelay());
                }
                break;

            case EState.eRoll:
                transform.position = Vector3.MoveTowards(transform.position, m_focusedPlayer.transform.position, Time.deltaTime * m_rollSpeed);

                break;

            case EState.eBeam:
                transform.position = Vector3.MoveTowards(transform.position, m_focusedPlayer.transform.position, Time.deltaTime * m_beamSpeed);

                break;

            case EState.eLaughing:
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EState.eIdle == m_state || EState.eLaughing == m_state)
            return;

        if (18 == collision.gameObject.layer) // player layer
        {

            if (collision.transform.parent.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.transform.parent.gameObject.GetComponent<IDamageable>().TakeDamage(1);
            }


            m_state = EState.eIdle;
            ActivateNewState();
        }
    }

    public void TakeDamage(int argDamage)
    {
        if (EState.eRoll == m_state)
        {
            m_state = EState.eIdle;

            ActivateNewState();
        }
        else if(EState.eIdle == m_state && m_changingState)
        {
            Singleton_Sound.m_instance.PlayAudioClip(m_damagedSound);

            m_health -= 1;

            if (0 > m_health)
                KillEntity();
        }
    }

    public void KillEntity()
    {
        foreach (GameObject levelDoor in m_levelDoors)
        {
            if (null == levelDoor)
                continue;

            LevelDoor door = levelDoor.GetComponent<LevelDoor>();

            if (null != door)
                door.SetDoorOpen(true);
        }

        foreach(GameObject rewardSpawner in m_rewardSpawners)
        {
            if (null == rewardSpawner)
                continue;

            ISpawner spawner = rewardSpawner.GetComponent<ISpawner>();

            if (null != spawner)
                spawner.ForceSpawn();
        }

        Singleton_Sound.m_instance.PlayAudioClip(m_defeatedSound);

        Singleton_Sound.m_instance.fadeOutSound(0.005f);

        gameObject.SetActive(false);
    }

    private void ChooseIdlePosition()
    {
        GameObject newIdle = m_chosenIdle;

        while(newIdle == m_chosenIdle)
        {
            newIdle = m_IdlePositions[Random.Range(0, m_IdlePositions.Length)];
        }

        m_chosenIdle = newIdle;
    }

    private void RandomiseState()
    {
        EState newState = EState.eIdle;
        while (newState == m_state)
        {
            newState = (EState)(Random.Range(0, (int)(EState.eCount)));
        }

        m_state = newState;

        ActivateNewState();
    }

    private IEnumerator RandomiseStateDelay()
    {
        m_changingState = true;
        yield return m_delayRandomiseStateSeconds;
        m_changingState = false;

        RandomiseState();
    }

    private void LaughingAttack()
    {
        foreach(GameObject spawnerObject in m_enemySpawners)
        {
            ISpawner spawner = spawnerObject.GetComponent<ISpawner>();

            if (null != spawner)
                spawner.ForceSpawn();
        }

        Singleton_Sound.m_instance.PlayAudioClip("Comberang");


        StartCoroutine(RandomiseStateDelay());
    }


    private void ActivateNewState()
    {
        switch (m_state)
        {
            case EState.eRoll:
                m_animator.SetBool("isRolling", true);
                m_animator.SetBool("isIdle", false);
                m_animator.SetBool("isBeaming", false);
                m_animator.SetBool("isLaughing", false);

                ChooseIdlePosition();
                break;

            case EState.eBeam:
                m_animator.SetBool("isRolling", false);
                m_animator.SetBool("isIdle", false);
                m_animator.SetBool("isBeaming", true);
                m_animator.SetBool("isLaughing", false);

                StartCoroutine(BeamAttack());
                break;

            case EState.eLaughing:
                m_animator.SetBool("isRolling", false);
                m_animator.SetBool("isIdle", false);
                m_animator.SetBool("isBeaming", false);
                m_animator.SetBool("isLaughing", true);

                LaughingAttack();
                break;

            default:
            case EState.eIdle:
                if (Vector2.Distance(transform.position, Singleton_Game.m_instance.GetPlayer(0).transform.position) < Vector2.Distance(transform.position, Singleton_Game.m_instance.GetPlayer(1).transform.position))
                    m_focusedPlayer = Singleton_Game.m_instance.GetPlayer(0);
                else
                    m_focusedPlayer = Singleton_Game.m_instance.GetPlayer(1);

              
                m_animator.SetBool("isRolling", false);
                m_animator.SetBool("isIdle", true);
                m_animator.SetBool("isBeaming", false);
                m_animator.SetBool("isLaughing", false);
                break;
        }
    }

    private IEnumerator BeamAttack()
    {
        if (null != m_beamGameObject)
        {
            m_beamGameObject.SetActive(true);

            if (m_focusedPlayer.transform.position.x - transform.position.x > 0)
            {
                m_beamGameObject.transform.localPosition = new Vector2(2.2f, 1f) ;
            }
            else
            {
                m_beamGameObject.transform.localPosition = new Vector2(-2.2f, 1);
            }

        }

        yield return m_delayStopBeamSeconds;

        if (m_state == EState.eBeam)
        {

            if (null != m_beamGameObject)
                m_beamGameObject.SetActive(false);

            m_state = EState.eIdle;
            ActivateNewState();
        }
    }

    public void BeamCollision()
    {
        if (null != m_beamGameObject)
            m_beamGameObject.SetActive(false);

        m_state = EState.eIdle;
        ActivateNewState();
    }
}
