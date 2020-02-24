
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public Text scoretxt;
    public int X = 0;
    public int Y = 0;
    public int Z = 0;
    public Text highScore;
    public Text highScore2;
    public Text highScore3;


    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        highScore2.text = PlayerPrefs.GetInt("HighScore2", 0).ToString();
        highScore3.text = PlayerPrefs.GetInt("HighScore3", 0).ToString();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            X += 200;
            scoretxt.text = X.ToString();

            if (X > PlayerPrefs.GetInt("HighScore", 0))
            {
                Y = PlayerPrefs.GetInt("HighScore", X);
                PlayerPrefs.SetInt("HighScore", X);
                highScore.text = X.ToString();
                PlayerPrefs.SetInt("HighScore2", Y);
            }

            else if (X > PlayerPrefs.GetInt("HighScore2", 0))
            {
                Y = PlayerPrefs.GetInt("HighScore2", X);
                PlayerPrefs.SetInt("HighScore2", X);
                highScore2.text = X.ToString();
                PlayerPrefs.SetInt("HighScore3", Y);

            }

            else if (X > PlayerPrefs.GetInt("HighScore3", 0))
            {
                PlayerPrefs.SetInt("HighScore3", X);
                highScore3.text = X.ToString();
            }

            if (Y > PlayerPrefs.GetInt("highScore2", 0))
            {
                Z = PlayerPrefs.GetInt("HighScore2", Y);
                PlayerPrefs.SetInt("HighScore2", Y);
                highScore2.text = Y.ToString();
                PlayerPrefs.SetInt("HighScore3", Z);
            }

            else if (Y > PlayerPrefs.GetInt("HighScore3", 0))
            {
                PlayerPrefs.SetInt("HighScore3", Y);
                highScore3.text = Y.ToString();
            }
        }
    }
}
