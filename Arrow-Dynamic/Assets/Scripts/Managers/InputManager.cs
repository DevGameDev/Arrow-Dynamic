using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages input in the game.
/// </summary>
public class InputManager : MonoBehaviour
{
    //////////////////////////////////////////////////
    // Public Properties and Methods
    //////////////////////////////////////////////////

    public static InputManager Instance { get; private set; } // Singleton

    public enum InputMapType
    {
        Disabled,
        Gameplay,
        ArrowWheel,
        Menu,
        Instructions
    }

    public void SetInputActionMap(InputMapType type)
    {
        foreach (InputActionMap map in allMaps)
            map.Disable();

        if (type is InputMapType.Gameplay)
            gameplayMap.Enable();
        else if (type is InputMapType.ArrowWheel)
            arrowWheelMap.Enable();
        else if (type is InputMapType.Menu)
            menusMap.Enable();
        else if (type is InputMapType.Instructions)
            instructionsMap.Enable();
    }

    //////////////////////////////////////////////////
    // Private Fields and Methods
    //////////////////////////////////////////////////

    [SerializeField] InputActionAsset inputAsset;
    private InputActionMap gameplayMap;
    private InputActionMap arrowWheelMap;
    private InputActionMap menusMap;
    private InputActionMap instructionsMap;
    private List<InputActionMap> allMaps = new List<InputActionMap>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameplayMap = inputAsset.FindActionMap("Gameplay");
        allMaps.Add(gameplayMap);

        arrowWheelMap = inputAsset.FindActionMap("ArrowWheel");
        allMaps.Add(arrowWheelMap);

        menusMap = inputAsset.FindActionMap("Menus");
        allMaps.Add(menusMap);

        instructionsMap = inputAsset.FindActionMap("Instructions");
        allMaps.Add(instructionsMap);
    }
}
