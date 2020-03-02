using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemShop : MonoBehaviour
{

    public int cost;
    public int pointNum;

    void Start()
    {
        pointNum = PlayerPrefs.GetInt("Points");
    }
    public void buyItem()
    {
        pointNum -= cost;
    }
}
