using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderModded : MonoBehaviour
{
    [Header("Settings")]
    public string Format = "F1";
    public TextMeshProUGUI Label;
    public Slider _ui_slider;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (_ui_slider != null && Label != null)
        {
            Label.text = _ui_slider.value.ToString(Format);
        }
#endif
    }

    private void Start()
    {
        _ui_slider.onValueChanged.AddListener(SliderValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        Label.text = value.ToString(Format);
    }

    private void OnDestroy()
    {
        _ui_slider.onValueChanged.RemoveListener(SliderValueChanged);
    }
}