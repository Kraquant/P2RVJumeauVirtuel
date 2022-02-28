using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;

    void Start()
    {
        InputAction tpActivate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
        InputAction tpCancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");

        tpActivate.Enable();
        tpCancel.Enable();

        tpActivate.performed += OnTeleportationActivate;
        tpCancel.canceled += OnTeleportationCancel;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTeleportationActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        Debug.Log("Activation de la TP");
    }

    private void OnTeleportationCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        Debug.Log("Desactivation de la TP");
    }
}
