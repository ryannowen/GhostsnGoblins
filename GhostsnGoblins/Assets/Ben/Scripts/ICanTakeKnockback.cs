using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanTakeKnockback
{

    void TakeKnockback(Vector3 argsSenderPosition, float argsKnockbackPower);

}
