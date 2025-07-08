using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI Counter;
    Animator CounterAnimator;

    public static GameScoreManager Instance;
    BalanceWeb webInstance;

    int Score;

    private void Awake()
    {
        Instance = this;
        webInstance = FindAnyObjectByType<BalanceWeb>();
    }

    public void RegisterKill()
    {
        if (CounterAnimator == null)
        {
            CounterAnimator = Counter.GetComponent<Animator>();
        }

        //increase the counter
        Score++;
        webInstance.AppendBalance(1);
        
        //If the counter is enabled set the value directly
        if (!Counter.gameObject.activeInHierarchy)
        {
            Counter.gameObject.SetActive(true);
        }

        Counter.text = Score.ToString();
        CounterAnimator.Play("Impact");
        Invoke(nameof(DeActivateCounter), 0.5f);
    }

    private void DeActivateCounter()
    {
        Counter.gameObject.SetActive(false);
    }

    public int getScore()
    {
        return Score;
    }
}