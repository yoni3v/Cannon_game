using UnityEngine;

/// <summary>
/// Ensures camera maintains transparent background for WebGL overlay
/// CRITICAL for seeing news content behind the game
/// </summary>
public class TransparentBackgroundManager : MonoBehaviour
{
    [Header("Transparency Settings")]
    [Tooltip("Continuously enforce transparent background")]
    public bool maintainTransparency = true;

    [Tooltip("The camera to make transparent (usually Main Camera)")]
    public Camera targetCamera;

    [Header("Debug")]
    public bool showDebugInfo = true;

    private Color transparentColor = new Color(0, 0, 0, 0);

    void Start()
    {
        // Find main camera if not assigned
        if (targetCamera == null)
        {
            targetCamera = Camera.main;

            if (targetCamera == null)
            {
                Debug.LogError("[TransparentBackground] No camera found! Please assign a camera.");
                return;
            }
        }

        // Set transparent background immediately
        SetTransparentBackground();

        if (showDebugInfo)
        {
            Debug.Log($"[TransparentBackground] Camera: {targetCamera.name}");
            Debug.Log($"[TransparentBackground] Background set to transparent (RGBA: 0,0,0,0)");
        }
    }

    void Update()
    {
        if (!maintainTransparency || targetCamera == null) return;

        // Continuously check and enforce transparency
        // (In case something else tries to change it)
        if (targetCamera.backgroundColor.a != 0 || targetCamera.clearFlags != CameraClearFlags.SolidColor)
        {
            SetTransparentBackground();

            if (showDebugInfo)
            {
                Debug.LogWarning("[TransparentBackground] Camera background was changed! Restoring transparency...");
            }
        }
    }

    /// <summary>
    /// Set camera to transparent background
    /// </summary>
    private void SetTransparentBackground()
    {
        if (targetCamera == null) return;

        // Set clear flags to solid color
        targetCamera.clearFlags = CameraClearFlags.SolidColor;

        // Set background to fully transparent
        targetCamera.backgroundColor = transparentColor;
    }

    /// <summary>
    /// Manually trigger transparency setup (can be called from other scripts)
    /// </summary>
    public void ForceTransparency()
    {
        SetTransparentBackground();
        Debug.Log("[TransparentBackground] Transparency forced");
    }

    /// <summary>
    /// Check if camera is currently transparent
    /// </summary>
    public bool IsTransparent()
    {
        if (targetCamera == null) return false;

        return targetCamera.clearFlags == CameraClearFlags.SolidColor &&
               targetCamera.backgroundColor.a == 0;
    }
}