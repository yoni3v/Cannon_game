using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool_item : MonoBehaviour, IPointerClickHandler
{
    [Header("components")]
    [SerializeField] Image logo;
    [SerializeField] TextMeshProUGUI _label;
    public int index;

    public event Action<int> OnClick;

    public void SetValues(Sprite _logo, string name, int index)
    {
        _label.text = name;
        logo.sprite = _logo;
        this.index = index;
    }

    private void OnDestroy()
    {
        OnClick = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(index);
    }
}