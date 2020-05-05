using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Exterminate : MonoBehaviour, ISpawnReactor
{
    [SerializeField] private GameObject m_itemToSpawn = null;
    [SerializeField] private GameObject m_spawnLocation = null;
    [SerializeField] private string m_soundToPlayWhenItemSpawns = "";

    private List<GameObject> m_exterminateTargets = new List<GameObject>();

    [SerializeField] private GameObject m_levelDoor = null;

    // Update is called once per frame
    void Update()
    {
        if(m_exterminateTargets.Count > 0)
        {
            bool exterminatedEnemies = true;

            foreach (GameObject target in m_exterminateTargets)
            {
                if (target.activeSelf)
                {
                    exterminatedEnemies = false;
                    break;
                }
            }

            if(exterminatedEnemies)
            {
                Singleton_Sound.m_instance.fadeOutSound(0.005f);
                Singleton_Sound.m_instance.PlayAudioClipOneShot(m_soundToPlayWhenItemSpawns, 0.4f);

                if (null != m_itemToSpawn)
                {
                    GameObject spawnedItem = System_Spawn.instance.GetObjectFromPool(m_itemToSpawn, true);
                    spawnedItem.transform.position = m_spawnLocation.transform.position;
                }

                m_exterminateTargets.Clear();

                if(null != m_levelDoor)
                {
                    LevelDoor door = m_levelDoor.GetComponent<LevelDoor>();

                    if(null != door)
                        door.SetDoorOpen(true);
                }
            }
        }
    }
    public void ReactorOnBeginSpawning() { }

    public void ReactorOnSpawn(GameObject argSpawnedObject)
    {
        m_exterminateTargets.Add(argSpawnedObject);
    }

    public void ReactorOnEndSpawning() { }
}
