
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public Text scoretxt;
    public int X = 0;
    public Text highScore;


    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            X += 200;
            scoretxt.text = X.ToString();

            if (X > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", X);
                highScore.text = X.ToString();
            }
        }
    }
}
