using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Toolbox_UI : MonoBehaviour, IPointerClickHandler
{
    [Header("Components")]
    public RectTransform _icon;
    public RectTransform _toolbox;
    public RectTransform SelectedCursor;
    public RectTransform prefabsHolder;
    public GameObject _tool_prefab;
    bool state = false;

    //items
    public List<item> items = new List<item>();
    List<RectTransform> spawnedUnits = new List<RectTransform>();

    //player values
    Player_Canon_Modded _local_player;

    private void Awake()
    {
        _local_player = FindAnyObjectByType<Player_Canon_Modded>();     //for now since the game is offline for now

        //            [ CALLBACKS ]            \\

        for (int i = 0; i < items.Count; i++)
        {
            var spawnedUnit = Instantiate(_tool_prefab, prefabsHolder).GetComponent<Tool_item>();
            spawnedUnit.SetValues(items[i].logo, items[i].name, i);
            spawnedUnit.OnClick += HandleClickByObserver;

            //store the references for later use
            spawnedUnits.Add(spawnedUnit.GetComponent<RectTransform>());
        }

        HandleClickByObserver(0);
    }

    #region Click Mechanics

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleState();
    }

    private void ToggleState()
    {
        state = !state;
        _toolbox.gameObject.SetActive(state);

        if (state)
        {
            _icon.eulerAngles = new Vector3(0,0,-45);
        }
        else
        {
            _icon.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void HandleClickByObserver(int index)
    {
        _local_player.ChangeProjectile(items[index].Projectile_Object, items[index].speed, items[index].rotation_offset);

        SelectedCursor.anchoredPosition = spawnedUnits[index].anchoredPosition;
    }

    #endregion
}