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
                hit.collider.GetComponent<Boutons>()?.OnClick();
            }
        };
        click.canceled += ctx =>
        {
            clicking = false;
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