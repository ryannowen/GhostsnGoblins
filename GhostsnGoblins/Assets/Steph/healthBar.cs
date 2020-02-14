using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{

    public Slider Slider;
    public Image fill;

    public void setMaxHealth(int health)
    {
        Slider.maxValue = health;
        Slider.value = health;
    }
    
    public void setHealth(int health)
    {
        Slider.value = health;
        fill.color = new Color(255,215,0,100);
    }

}
