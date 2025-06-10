using UnityEngine;

public class CursorPositionInWorldSpace : MonoBehaviour
{
    public static CursorPositionInWorldSpace instance;
    public Vector3 output;
    public LayerMask WhatIsPointable;

    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        // For a 3D point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, WhatIsPointable))
        {
            output = hit.point;
        }
    }
}