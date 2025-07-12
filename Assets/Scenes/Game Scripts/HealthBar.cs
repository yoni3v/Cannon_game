using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    Slider slider;
    HealthSystem player_health;
    Image fill_;

    [Header("Color settings")]
    [SerializeField] Color FullHealth_Col;
    [SerializeField] Color LowHealth_Col;

    private void Start()
    {
        slider = GetComponent<Slider>();
        player_health = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();

        if (player_health != null)
        {
            slider.maxValue = player_health.MaxHealth;
            slider.minValue = 0;
            slider.value = player_health.CurrentHealth;

            //assign the events
            player_health.OnDamageEvent.AddListener(UpdateSlider);

            fill_ = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Please assign the [Player] tag to your player in the scene");
        }
    }

    private void UpdateSlider()
    {
        slider.value = player_health.CurrentHealth;

        fill_.color = Color.Lerp(LowHealth_Col, FullHealth_Col, slider.value / slider.maxValue);
    }
}