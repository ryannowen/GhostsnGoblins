using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemShop : MonoBehaviour
{
    [SerializeField] private GameObject m_peasantText = null;
    [SerializeField] private GameObject m_purchaseText = null;

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
        }
        else if (!m_peasantText.activeSelf)
        {
            m_peasantText.SetActive(true);
            m_purchaseText.SetActive(false);
            StartCoroutine(FadeSwitch());
        }
    }

    private IEnumerator FadeSwitch()
    {
        yield return m_fadeDelay;
        m_peasantText.SetActive(false);

    }
}
