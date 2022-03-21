using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class Interaction2D : MonoBehaviour
{
    [SerializeField]
    private Camera gameCamera;
    private InputAction click;
    private bool clicking;
    private Collider lastPressed;


    void Awake()
    {
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.started += ctx =>
        {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
            {
                clicking = true;
                lastPressed = hit.collider;
                hit.collider.GetComponent<Boutons>()?.OnClick();
            }
        };
        click.canceled += ctx =>
        {
            clicking = false;
            if (lastPressed != null)
            {
                lastPressed.GetComponent<Boutons>()?.OnStopClick();
                lastPressed = null;
            }
        };
        click.Enable();
    }

    private void Update()
    {
        if (clicking)
        {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
            {
                hit.collider.GetComponent<Boutons>()?.OnLongClick();
            }
        }
    }
}