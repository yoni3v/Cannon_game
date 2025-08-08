using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cannon_ui : MonoBehaviour
{
    public cannon_asset[] cannons;
    [HideInInspector] public cannon_asset active_cannon;

    [Header("UI references")]
    [SerializeField] GameObject cannons_selecter;
    [SerializeField] GameObject cannon_ui_prefab;

    private void Start()
    {
        foreach (var item in cannons)
        {
            var item_obj = Instantiate(cannon_ui_prefab, cannons_selecter.transform);
            item_obj.GetComponent<Image>().sprite = item.cannon_snapshot;
            item_obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.asset_name;

            var btn = item_obj.GetComponent<Button>();
            btn.onClick.AddListener(() => HandleCannonObj(item));
        }
    }

    private void HandleCannonObj(cannon_asset asset)
    {
        if (active_cannon != asset)
        {
            active_cannon = asset;
            FindAnyObjectByType<Player_Canon_Modded>().ChangeCannonModel(asset.asset_name);
            Debug.Log("Triggered cannon change event");
        }
    }
}