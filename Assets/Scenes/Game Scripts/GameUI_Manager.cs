using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameUI_Manager : MonoBehaviour
{
    [Header("Settings")]
    public float MenuLerpDuration = 5f;
    public float GameLerpDuration = 5f;
    public float LerpingSpeed = 0.7f;

    [Header("References")]
    [SerializeField] Transform MenuCamera;
    [SerializeField] Canvas Menu;
    [SerializeField] Canvas Game;

    //private variables
    Vector3 DefualtCameraPosition, DefualtCameraRotation;
    Camera main_camera;

    private void Start()
    {
        main_camera = Camera.main;
        DefualtCameraPosition = main_camera.transform.position;
        DefualtCameraRotation = main_camera.transform.eulerAngles;

        //Disable all the menus
        Game.enabled = false;
        Menu.enabled = false;

        //Show the menu when the game starts
        ShowMenu();
    }

    private void ShowMenu()
    {
        Time.timeScale = 0;
        Player_Canon_Modded.TakeInput = false;
        StartCoroutine(LerpCameraToMenu(MenuLerpDuration));
    }

    public void ShowGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LerpCameraToGame(GameLerpDuration));
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
#endif
    }

    IEnumerator LerpCameraToMenu(float duration)
    {
        //keeping track of the main camera time
        float _Time = 0;

        while (_Time < duration)
        {
            //adding a unscaled union to Time
            _Time += Time.unscaledDeltaTime * LerpingSpeed;

            //We can lerp the camera here
            float ratio = _Time / duration;
            main_camera.transform.position = Vector3.Lerp(DefualtCameraPosition, MenuCamera.position, ratio);
            main_camera.transform.eulerAngles = Vector3.Lerp(DefualtCameraRotation, MenuCamera.eulerAngles, ratio);

            yield return null;
        }

        Menu.enabled = true;
        Game.enabled = false;
    }

    IEnumerator LerpCameraToGame(float duration)
    {
        //keeping track of the main camera time
        float _Time = 0;
        Menu.enabled = false;

        while (_Time < duration)
        {
            //adding a unscaled union to Time
            _Time += Time.unscaledDeltaTime * LerpingSpeed;

            //We can lerp the camera here
            float ratio = _Time / duration;
            main_camera.transform.position = Vector3.Lerp(MenuCamera.position, DefualtCameraPosition, ratio);
            main_camera.transform.eulerAngles = Vector3.Lerp(MenuCamera.eulerAngles, DefualtCameraRotation, ratio);

            yield return null;
        }

        Player_Canon_Modded.TakeInput = true;
        Game.enabled = true;
    }
}