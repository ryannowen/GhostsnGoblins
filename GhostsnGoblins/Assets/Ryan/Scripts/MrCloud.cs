using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrCloud : MonoBehaviour
{
    GameObject m_HUD = null;

    // Start is called before the first frame update
    void Start()
    {
        m_HUD = Singleton_Game.m_instance.GetHUD();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerC = collision.gameObject.GetComponent<PlayerController>();

            if (!playerC.Interacting())
                return;

            HUD hud = m_HUD.GetComponent<HUD>();
            itemShop vitemShop = m_HUD.GetComponent<itemShop>();

            if (vitemShop.CurrentBuyingPlayer() == null)
            {
                playerC.SetUsingGUI(true);
                vitemShop.SetScoreText(Singleton_Game.m_instance.GetScore());
                vitemShop.SetBuyingPlayer(playerC);
                hud.ShowItemShop();
            }
        }
    }
}
