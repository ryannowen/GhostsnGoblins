using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weapon : MonoBehaviour
{

    public Sprite weapon1;
    public Sprite weapon2;

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
        if (image.sprite == weapon1)
        {
            image.sprite = weapon2;
        }
        else
        {
            image.sprite = weapon1;
        }
    }
}
