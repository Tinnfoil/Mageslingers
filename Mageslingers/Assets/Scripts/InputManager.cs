using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    public bool IsControlSet;

    public bool Fire;
    public bool AltFire;
    public Vector2 Move;
    public Vector2 Look;
    public bool Sprinting;
    public bool Jump;

    public bool IsCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "KeyboardMouse";
        }
    }
    [Header("Movement Settings")]
    public bool analogMovement;
    // Start is called before the first frame update
    void Start()
    {
        BindInput();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (IsControlSet)
            {
                UnbindInput();
            }
            else
            {
                BindInput();
            }
        }
    }

    public void OnFire(InputAction.CallbackContext value)
    {
        Fire = value.performed;
    }
    public void OnAltFire(InputAction.CallbackContext value)
    {
        AltFire = value.performed;
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        //Debug.Log("OnMove:" + value.ReadValue<Vector2>());
        Move = value.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext value)
    {
        //Debug.Log("OnLook:" + value.ReadValue<Vector2>());
        Look = value.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext value)
    {
        //Debug.Log("OnJump:" + value.performed);
        Jump = value.performed;
    }
    public void OnSprint(InputAction.CallbackContext value)
    {
        // Debug.Log("OnSprint:" + value.performed);
        Sprinting = value.performed;

    }

    public void BindInput()
    {
        if (IsControlSet) return;

        InputAction Fire = playerInput.currentActionMap.FindAction("Fire");
        Fire.performed += OnFire; Fire.canceled += OnFire;
        InputAction AltFire = playerInput.currentActionMap.FindAction("AltFire");
        AltFire.performed += OnAltFire; AltFire.canceled += OnAltFire;
        InputAction Move = playerInput.currentActionMap.FindAction("Move");
        Move.performed += OnMove; Move.canceled += OnMove;
        InputAction Look = playerInput.currentActionMap.FindAction("Look");
        Look.performed += OnLook; Look.canceled += OnLook;
        InputAction Jump = playerInput.currentActionMap.FindAction("Jump");
        Jump.performed += OnJump;
        InputAction Sprint = playerInput.currentActionMap.FindAction("Sprint");
        Sprint.performed += OnSprint; Sprint.canceled += OnSprint;

        IsControlSet = true;
    }

    public void UnbindInput()
    {
        if (!IsControlSet) return;
        Move = default(Vector2);
        Look = default(Vector2);
        Jump = default(bool);
        Sprinting = default(bool);

        playerInput.currentActionMap.FindAction("Move").performed -= OnMove; playerInput.currentActionMap.FindAction("Move").canceled -= OnMove;
        playerInput.currentActionMap.FindAction("Look").performed -= OnLook; playerInput.currentActionMap.FindAction("Look").canceled -= OnLook;
        playerInput.currentActionMap.FindAction("Jump").performed -= OnJump;
        playerInput.currentActionMap.FindAction("Sprint").performed -= OnSprint; playerInput.currentActionMap.FindAction("Sprint").canceled -= OnSprint;
        playerInput.currentActionMap.FindAction("Fire").performed -= OnFire; playerInput.currentActionMap.FindAction("Fire").canceled -= OnFire;
        playerInput.currentActionMap.FindAction("AltFire").performed -= OnAltFire; playerInput.currentActionMap.FindAction("AltFire").canceled -= OnAltFire;
        IsControlSet = false;
    }

    private void OnDisable()
    {
        UnbindInput();
        playerInput.actions = null;
    }
}
