using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemies_Selector : MonoBehaviour
{
    //private vars
    EnemySpawner Spawner;
    List<GameObject> SpawnedUnits = new List<GameObject>();

    [Header("UI references")]
    [SerializeField] GameObject ImagesHolder;
    [SerializeField] GameObject Enemies_UI_prefab;

    [Header("Values")]
    [SerializeField] Enemy_Asset[] Enemies;

    private void Start()
    {
        Spawner = FindAnyObjectByType<EnemySpawner>();

        foreach (var item in Enemies)
        {
            //spawning the ui object
            var spawnedUnit = Instantiate(Enemies_UI_prefab, ImagesHolder.transform);

            //making the changes to its values
            spawnedUnit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Name;
            spawnedUnit.GetComponent<Image>().sprite = item.Picture == null ? null : item.Picture;
            spawnedUnit.GetComponent<UI_object_Enemy>().OnClick.AddListener(value => HandleUI_Click(value, item));

            //adding it to the list
            SpawnedUnits.Add(spawnedUnit);

            //adding the objects
            HandleUI_Click(true, item);
        }
    }

    private void HandleUI_Click(bool state, Enemy_Asset asset)
    {
        if (state)
        {
            if (Spawner.EnemyObjects.Contains(asset.Prefab)) return;
            Spawner.EnemyObjects.Add(asset.Prefab);
        }
        else
        {
            if (!Spawner.EnemyObjects.Contains(asset.Prefab)) return;
            Spawner.EnemyObjects.Remove(asset.Prefab);
        }
    }
}