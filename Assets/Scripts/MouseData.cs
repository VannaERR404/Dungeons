using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseData : MonoBehaviour
{
    Mouse mouse => Mouse.current;
    [SerializeField] private Camera cam;
    public LayerMask layerMask;

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerMask);
        if(hit.collider != null)
        {
        }
    }
}