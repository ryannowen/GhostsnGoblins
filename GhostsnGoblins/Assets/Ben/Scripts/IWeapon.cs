using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{

    void Action(GameObject objectFiring, Vector3 argsStartPosition, Vector3 argsDirection);

}
