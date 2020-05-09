using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESpawnReactorType
{
    eOnBeginSpawning,
    eOnSpawn,
    eOnEndSpawning
};

public interface ISpawnReactor
{
    void ReactorReset();
    void ReactorOnBeginSpawning();
    void ReactorOnSpawn(GameObject argSpawnedObject);
    void ReactorOnEndSpawning();
}
