using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public string inspectorName = "";
    public GameObject item = null;
    public int spawnAmount = 0;
    [Range(0, 100)] public int spawnChance = 10;
    [Space]
    public bool shouldPeek = false;
    public bool ignoreAllActiveCheck = false;
    [Space]
    public GameObject[] spawnReactors = null;
}