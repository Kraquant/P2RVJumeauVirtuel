using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private TeleportationProvider provider;
    [SerializeField] private XRRayInteractor rayInteractor;
    private bool isActive;

    void Start()
    {
        InputAction tpActivate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
        InputAction tpCancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");

        tpActivate.Enable();
        tpCancel.Enable();

        tpActivate.performed += OnTeleportationActivate;
        tpCancel.canceled += OnTeleportationCancel;
    }

    private void Update()
    {
        if (!isActive) return; 
    }

    private void OnTeleportationActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        Debug.Log("Activation de la TP");
        isActive = true;
    }

    private void OnTeleportationCancel(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.TryGetHitInfo(out Vector3 pos, out Vector3 norm, out int inlinePos, out bool validTarget);

            if (validTarget)
            {
                TeleportRequest request = new TeleportRequest()
                {
                    destinationPosition = hit.point,
                };
                provider.QueueTeleportRequest(request);
            }
        }

        rayInteractor.enabled = false;
        Debug.Log("Desactivation de la TP");
        isActive = false;
    }
}
