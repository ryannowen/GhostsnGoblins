using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private PlayerController.ArmourType m_armourType = PlayerController.ArmourType.Copper;
    [SerializeField] private int m_itemCost = 100;

    public PlayerController.ArmourType GetArmour()
    {
        return m_armourType;
    }

    public int GetCost()
    {
        return m_itemCost;
    }
}
