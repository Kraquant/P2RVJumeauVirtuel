using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveOrigin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] float sensitivity;
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] float speed = 0.1f;
    private float rotateDirection;

    private FileMovement moveFile;

    [SerializeField] GameObject OsPlateau8;
    private bool isGrabbing;
    private bool nearTable;
    private bool nearHand;

    void Start()
    {
        InputAction rotateY = actionAsset.FindActionMap("XRI RightHand").FindAction("RotateVertical");
        InputAction grab = actionAsset.FindActionMap("XRI RightHand").FindAction("Grab");

        rotateY.Enable();
        rotateY.performed += OnRotateActivate;
        rotateY.canceled += OnRotateActivate;

        grab.Enable();
        grab.performed += OnGrabActivate;
        grab.canceled += OnGrabRelease;

        nearTable = false;
        nearHand = false;
        isGrabbing = false;

        moveFile = GetComponent<FileMovement>();
    }

    private void FixedUpdate()
    {
        this.transform.Rotate(0, sensitivity * rotateDirection, 0, Space.Self);
        if (isGrabbing)
        {
            Debug.Log("test");
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position + offset, speed);
            moveFile.initPos = this.transform.position;
        }
    }

    private void OnGrabActivate(InputAction.CallbackContext context)
    {
        if (nearHand && moveFile.trajectory != null)
        {
            isGrabbing = true;
            transform.parent = null;
        }   
    }

    private void OnGrabRelease(InputAction.CallbackContext context)
    {
        isGrabbing = false;
        if (nearTable)
        {
            transform.SetParent(OsPlateau8.transform, true);
        }
        else
        {
            transform.SetParent(null, true);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    private void OnRotateActivate(InputAction.CallbackContext context)
    {
        if (moveFile.trajectory == null) return;
        rotateDirection = context.ReadValue<Vector2>().x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TableTrigger")
        {
            nearTable = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, OsPlateau8.transform.up);
            //work = false;
            /*Debug.Log(transform.position);
            transform.SetParent(OsPlateau8.transform, true);
            Debug.Log(transform.position);*/
            //transform.eulerAngles = new Vector3(OsPlateau8.transform.eulerAngles.x, 0, OsPlateau8.transform.eulerAngles.z) ;

        }
        if (other.gameObject.tag == "Hand")
        {
            nearHand = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "TableTrigger")
        {
            nearTable = false;
            /*transform.SetParent(null, true);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);*/
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        if(other.gameObject.tag == "Hand")
        {
            nearHand = false;
        }
        //workend = true;
    }
}
