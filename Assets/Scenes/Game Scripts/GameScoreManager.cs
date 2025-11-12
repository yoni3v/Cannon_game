using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    iHealable playerHealth_system;

    [Header("References")]
    [SerializeField] TextMeshProUGUI Counter;
    Animator CounterAnimator;

    public static GameScoreManager Instance;
    public PinocchioNewsIntegration _pin_sdk;

    int Score;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterKill()
    {
        _pin_sdk.AddScore(1);
        _pin_sdk.RegisterKill();

        if (CounterAnimator == null)
        {
            CounterAnimator = Counter.GetComponent<Animator>();
        }

        //increase the counter
        Score++;
        
        //If the counter is enabled set the value directly
        if (!Counter.gameObject.activeInHierarchy)
        {
            Counter.gameObject.SetActive(true);
        }

        Counter.text = Score.ToString();
        CounterAnimator.Play("Impact");
        Invoke(nameof(DeActivateCounter), 0.5f);

        playerHealth_system = GameObject.FindWithTag("Player").GetComponent<iHealable>();
        playerHealth_system.OnHeal(10);
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