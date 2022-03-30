using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

// Detection de clics sur les boutons du pendant en mode d'interaction 2D
// Auteur : Luc Pares

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
            // Lorsque l'on detecte un clic dans le collider d'un bouton, on appelle la fonction de clic bref dans Boutons.cs
            if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
            {
                clicking = true;
                lastPressed = hit.collider;
                hit.collider.GetComponent<Boutons>()?.OnClick();
            }
        };
        // Une fois que le clic s'arrete, la fonction de fin de clic
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
            // Si le clic se prolonge, la fonction de clic long
            if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
            {
                hit.collider.GetComponent<Boutons>()?.OnLongClick();
            }
        }
    }
}