using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveOrigin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] float sensitivity = 0.01f;
    [SerializeField] GameObject target;
    private float xDirection;
    private float yDirection;
    private float zDirection;

    public GameObject Plateau8;
    public GameObject Kuka;

    void Start()
    {
        //InputAction moveX = actionAsset.FindActionMap("XRI RightHand").FindAction("MoveOriginX");
        //InputAction moveY = actionAsset.FindActionMap("XRI RightHand").FindAction("MoveOriginY");
        //InputAction moveZ = actionAsset.FindActionMap("XRI RightHand").FindAction("MoveOriginZ");

        //moveX.Enable();
        //moveY.Enable();
        //moveZ.Enable();

        //moveX.performed += OnMoveXActivate;
        //moveX.canceled += OnMoveXActivate;

        //moveY.performed += OnMoveYActivate;
        //moveY.canceled += OnMoveYActivate;

        //moveZ.performed += OnMoveZActivate;
        //moveZ.canceled += OnMoveZActivate;
    }

    private void OnMoveXActivate(InputAction.CallbackContext context)
    {
        Debug.Log("Deplacement sur l'axe x");
        xDirection = context.ReadValue<float>();
    }

    private void OnMoveYActivate(InputAction.CallbackContext context)
    {
        Debug.Log("Deplacement sur l'axe y");
        yDirection = context.ReadValue<float>();
    }

    private void OnMoveZActivate(InputAction.CallbackContext context)
    {
        Debug.Log("Deplacement sur l'axe z");
        zDirection = context.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        target.transform.Translate(Vector3.right * sensitivity * xDirection);
        target.transform.Translate(Vector3.up * sensitivity * yDirection);
        target.transform.Translate(Vector3.forward * sensitivity * zDirection);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject == Plateau8)
        {
            Debug.Log("Bonne collision");
            target.transform.SetParent(Plateau8.transform);
            //target.transform.Rotate(new Vector3(1, 0, 0), 180);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collision quit");
        if (other.gameObject == Plateau8)
        {
            Debug.Log("Bonne collision quit");
            target.transform.SetParent(Kuka.transform);
            target.transform.Rotate(-target.transform.rotation.eulerAngles);
        }
    }
}
