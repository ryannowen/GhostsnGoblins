using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro m_scoreText = null;

    public IEnumerator ActivatePopup(Vector2 argPosition, int argDisplayScore, float argDiplayTime)
    {
        transform.position = argPosition;
        if(null != m_scoreText)
            m_scoreText.text = argDisplayScore.ToString();
        else
            m_scoreText.text = "Txt=NULL";

        yield return new WaitForSeconds(argDiplayTime);

        gameObject.SetActive(false);
    }
}
