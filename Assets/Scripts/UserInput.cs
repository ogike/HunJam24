using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;
    
    public Vector2 MoveInput { get; private set; }

    public bool MenuButtonPressedThisFrame { get; private set; }
    public bool InteractButtonPressedThisFrame { get; private set; }
    
    public bool DebugMenuButtonPressedThisFrame { get; private set; }
    public bool DebugEnemySpawnPressedThisFrame { get; private set; }
    
    private PlayerInput _playerInput;

    private InputAction _moveAction;
    

    private InputAction _menuAction;
    private InputAction _interactAction;
    
    private InputAction _debugMenuAction;
    private InputAction _debugSpawnAction;
    
    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one UserInput instance in scene!");
            return;
        }
        instance = this;

        _playerInput = GetComponent<PlayerInput>();
        
        _moveAction = _playerInput.actions["Move"];
        _menuAction = _playerInput.actions["MenuOpenClose"];
        _menuAction = _playerInput.actions["Interact"];
        _debugMenuAction = _playerInput.actions["DebugMenu"];
        _debugSpawnAction = _playerInput.actions["DebugSpawnEnemy"];
    }

    private void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();

        MenuButtonPressedThisFrame = _menuAction.WasPressedThisFrame();

        MenuButtonPressedThisFrame = _interactAction.WasPressedThisFrame();
        
        DebugMenuButtonPressedThisFrame = _debugMenuAction.WasPressedThisFrame();
        DebugEnemySpawnPressedThisFrame = _debugSpawnAction.WasPressedThisFrame();
    }
}
