using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject item = null;
    public int spawnAmount = 0;
    public bool spawnState = false;
    public int poolAmount = 0;
    public bool createPool = false;
    public bool ignoreAllActiveCheck = false;


    [Range(0, 100)] public int spawnChance = 0;
}
