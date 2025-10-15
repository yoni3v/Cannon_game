using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Particles_Selector : MonoBehaviour
{
    //private vars
    Player_Canon_Modded _Player;
    List<GameObject> SpawnedUnits = new List<GameObject>();

    [Header("UI references")]
    [SerializeField] GameObject ImagesHolder;
    [SerializeField] GameObject Particle_UI_prefab;

    [Header("Values")]
    [SerializeField] Particles[] Particle;

    private void Awake()
    {
        _Player = FindAnyObjectByType<Player_Canon_Modded>();

        foreach (var item in Particle)
        {
            //spawning the ui object
            var spawnedUnit = Instantiate(Particle_UI_prefab, ImagesHolder.transform);

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

    private void HandleUI_Click(bool state, Particles asset)
    {
        if (state)
        {
            if (_Player.Impact_Particles.Contains(asset.Particle)) return;
            _Player.Impact_Particles.Add(asset.Particle);
        }
        else
        {
            if (!_Player.Impact_Particles.Contains(asset.Particle)) return;
            _Player.Impact_Particles.Remove(asset.Particle);
        }
    }
}