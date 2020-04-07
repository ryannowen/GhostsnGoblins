using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public string inspectorName = "";
    public GameObject item = null;
    public int spawnAmount = 0;
    public bool shouldPeek = false;
    public bool ignoreAllActiveCheck = false;
    [Range(0, 100)] public int spawnChance = 10;
}