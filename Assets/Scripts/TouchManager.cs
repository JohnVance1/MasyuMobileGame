using System;
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

    private Node currentNode;

    Camera cam;
    private bool isDragging;



    public delegate bool AddEdge(Node n1, Node n2);
    public static event AddEdge addEdge;


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
            Node node = GetSelectedNode();
            if (node != null)
            {
                currentNode = node;
                Debug.Log($"Current Node - X: {currentNode.x} Y: {currentNode.y}");
                return true;
            }
            return false;
        }
    }

    private Node isNewNode
    {
        get
        {
            Node newNode = GetSelectedNode(); 
            if(newNode != null)
            {
                if(newNode != currentNode)
                {
                    return newNode;
                }          

            }
            else
            {
                isDragging = false;
            }
            return null;
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
            Node tempNode = isNewNode;
            if (tempNode != null)
            {
                addEdge?.Invoke(currentNode, tempNode);
                currentNode = tempNode;
                Debug.Log($"Current Node - X: {currentNode.x} Y: {currentNode.y}");
            }
            //transform.position = WorldPos + offset;
            yield return null;
        }
        
    }

    private Node GetSelectedNode()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(curScreenPos), Vector2.zero);
        if(hit.transform == null)
        {
            return null;
        }
        if (hit.transform.GetComponent<Node>() != null)
        {            
            return hit.transform.GetComponent<Node>();
        }
        return null;
    }



}
