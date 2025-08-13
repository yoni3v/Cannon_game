using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_object_Enemy : MonoBehaviour, IPointerClickHandler
{
    public bool state;
    [SerializeField] GameObject added_status;
    public UnityEvent<bool> OnClick;

    private void Start()
    {
        added_status.SetActive(state);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        state = !state;
        OnClick?.Invoke(state);
        added_status.SetActive(state);
    }
}