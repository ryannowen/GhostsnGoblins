using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemShop : MonoBehaviour
{

    public int cost;
    public int pointNum;
    public GameObject moneyText;
    private float timeWhenDisapear;
    private float timeToAppear = 5f;

    void Start()
    {
        pointNum = PlayerPrefs.GetInt("Points");
    }
    public void buyItem()
    {
        if (pointNum >= cost)
        {
            pointNum -= cost;
        }
        else
        {
            moneyText.SetActive(true);
            timeWhenDisapear = Time.time + timeToAppear;
        }

    }

    private void Update()
    {
        if (moneyText.activeSelf == true && (Time.time >= timeWhenDisapear))
        {
            moneyText.SetActive(false);
        }
    }
}
