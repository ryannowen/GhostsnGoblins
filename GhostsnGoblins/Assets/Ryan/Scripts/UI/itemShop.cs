using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemShop : MonoBehaviour
{
    [SerializeField] private GameObject m_peasantText = null;
    [SerializeField] private GameObject m_purchaseText = null;
    [SerializeField] private TMPro.TextMeshProUGUI m_scoreText = null;

    private PlayerController m_buyingPlayer = null;
    private float m_disappearDelaySeconds = 2.0f;

    private WaitForSeconds m_fadeDelay = null;

    private void Start()
    {
        m_fadeDelay = new WaitForSeconds(m_disappearDelaySeconds);
    }

    public void buyItem(GameObject argItem)
    {
        int currentScore = Singleton_Game.m_instance.GetScore();
        Item itemData = argItem.GetComponent<Item>();
        int itemCost = itemData.GetCost();

        if (currentScore - itemCost >= 0)
        {
            m_purchaseText.SetActive(true);
            m_peasantText.SetActive(false);
            Singleton_Game.m_instance.AddScore(-itemCost);

            if (null != m_buyingPlayer)
            {
                m_buyingPlayer.SetArmourType(itemData.GetArmour());
                m_buyingPlayer.SetArmourPoints((int)itemData.GetArmour());
            }
            else
                Debug.LogWarning("Couldn't buy item because buying player is null");
        }
        else if (!m_peasantText.activeSelf)
        {
            m_peasantText.SetActive(true);
            m_purchaseText.SetActive(false);
            StartCoroutine(FadeSwitch());
        }
    }

    public void SetCurrentPlayerUsingGUIFasle()
    {
        m_buyingPlayer.SetUsingGUI(false);
        m_buyingPlayer = null;
    }

    public PlayerController CurrentBuyingPlayer() 
    {
        return m_buyingPlayer;
    }

    private IEnumerator FadeSwitch()
    {
        yield return m_fadeDelay;
        m_peasantText.SetActive(false);
    }

    public void SetBuyingPlayer(PlayerController argPlayerController)
    {
        m_buyingPlayer = argPlayerController;
    }

    public void SetScoreText(int argScore)
    {
        if(null == m_scoreText)
        {
            Debug.LogWarning("Cannot set itemshop score text because it is null");
            return;
        }


        m_scoreText.text = "Score: " + argScore;
    }
}
