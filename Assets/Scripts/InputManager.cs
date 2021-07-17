using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputActions inputActions;

    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        inputActions = new InputActions();

        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        } else
        {
            _instance = this;
        }
    }
       
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return inputActions.Player.Jump.triggered;
    }

    public bool AttackedThisFrame()
    {
        return inputActions.Player.LeftAttack.triggered;
    }

    public bool ZoomedThisFrame()
    {
        return inputActions.Player.RightAttack.triggered;
    }

    public bool PowerUpTriggeredThisFrame()
    {
        return inputActions.Player.PowerUps.triggered;
    }
    
    public bool PickUpBallTriggeredThisFrame()
    {
        return inputActions.Player.PickUpBall.triggered;
    }
    
    public bool DropUpBallTriggeredThisFrame()
    {
        return inputActions.Player.DropBall.triggered;
    }
}
