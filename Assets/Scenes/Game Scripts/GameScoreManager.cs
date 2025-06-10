using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI Counter;
    Animator CounterAnimator;

    int Score;
    int HighScore;

    public void RegisterKill()
    {
        //Save the information
        if (PlayerPrefs.HasKey(nameof(HighScore)))
        {
            HighScore = PlayerPrefs.GetInt(nameof(HighScore));
        }
        else
        {
            if (Score >= HighScore)
            {
                HighScore = Score;
                PlayerPrefs.SetInt(nameof(HighScore), HighScore);
            }
        }

        if (CounterAnimator == null)
        {
            CounterAnimator = Counter.GetComponent<Animator>();
        }

        Score++;
        Counter.gameObject.SetActive(true);
        Counter.text = "x" + Score.ToString();
        CounterAnimator.Play("Impact");
        Invoke(nameof(DeActivateCounter), 0.5f);
    }

    private void DeActivateCounter()
    {
        Counter.gameObject.SetActive(false);
    }
}