using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private PlayerInput playerInput;
    private Vector3 curScreenPos;

    Camera cam;
    private bool isDragging;

    private Vector3 WorldPos
    {
        get
        {
            float z = cam.WorldToScreenPoint(transform.position).z;
            return cam.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
        }
    }
    private bool isClickedOn
    {
        get
        {
            Ray ray = cam.ScreenPointToRay(curScreenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform;
            }
            return false;
        }
    }

    private InputAction touchPositionAction;
    private InputAction touchPressAction;


    private void Awake()
    {
        cam = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        touchPositionAction.performed += TouchPosition;
        touchPressAction.performed += TouchPress;
        touchPressAction.canceled += TouchPressCanceled;
    }

    private void OnDisable()
    {
        touchPositionAction.performed -= TouchPosition;
        touchPressAction.performed -= TouchPress;
        touchPressAction.canceled -= TouchPressCanceled;


    }

    private void TouchPress(InputAction.CallbackContext context)
    {
        if (isClickedOn)
        {
            StartCoroutine(Drag());
        }
    }

    private void TouchPressCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;
    }

    private void TouchPosition(InputAction.CallbackContext context)
    {
        curScreenPos = touchPositionAction.ReadValue<Vector2>();
        
    }

    private IEnumerator Drag()
    {
        isDragging = true;
        Vector3 offset = transform.position - WorldPos;

        while (isDragging)
        {
            // dragging
            transform.position = WorldPos + offset;
            yield return null;
        }
        
    }



}
