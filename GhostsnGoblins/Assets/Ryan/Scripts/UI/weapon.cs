using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weapon : MonoBehaviour
{

    public Sprite weapon1;
    public Sprite weapon2;
    public Sprite weapon3;
    public Sprite weapon4;
    public Sprite weapon5;
    public Sprite weapon6;
    public int weaponNum = 1;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (image.sprite == null)
            image.sprite = weapon1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            weaponSwitch();
        }
    }

    void weaponSwitch()
    {
        if (weaponNum < 6)
            weaponNum += 1;
        else
            weaponNum = 1;

        switch (weaponNum)
        {
            case 1:
                image.sprite = weapon1;
                break;

            case 2:
                image.sprite = weapon2;
                break;

            case 3:
                image.sprite = weapon3;
                break;

            case 4:
                image.sprite = weapon4;
                break;

            case 5:
                image.sprite = weapon5;
                break;

            case 6:
                image.sprite = weapon6;
                break;
        }
    }
}
