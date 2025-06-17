using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI Counter;
    Animator CounterAnimator;

    int Score;

    public void RegisterKill()
    {
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

        Counter.text = "x" + Score.ToString();
        CounterAnimator.Play("Impact");
        Invoke(nameof(DeActivateCounter), 0.5f);
    }

    private void DeActivateCounter()
    {
        Counter.gameObject.SetActive(false);
    }
}